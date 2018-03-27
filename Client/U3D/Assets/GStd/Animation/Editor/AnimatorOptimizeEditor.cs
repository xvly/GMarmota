
namespace GStd.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using UnityEditorInternal;
    using System;
    using System.Text.RegularExpressions;

    [CustomEditor(typeof(AnimatorOptimize))]
    public class AnimatorOptimizeEditor : Editor
    {
        List<GameObject> prefabs = new List<GameObject>();
        
        SerializedProperty args;
        ReorderableList argsList;

        private string nameRegex = "";

        public void OnEnable()
        {
            // args
            this.args = this.serializedObject.FindProperty("args");
            this.argsList = new ReorderableList(
                this.serializedObject, this.args);
            this.argsList.drawHeaderCallback +=
                rect => GUI.Label(rect, "args");
            this.argsList.elementHeight = EditorGUIUtility.singleLineHeight;
            this.argsList.drawElementCallback +=
                (rect, index, isActive, isFocused) =>
                {
                    DrawArgs(this.args, rect, index, isActive, isFocused);
                };
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            this.argsList.DoLayoutList();
            this.serializedObject.ApplyModifiedProperties();

            GUILayout.BeginHorizontal();
            GUILayout.Label("regex:");
            this.nameRegex = EditorGUILayout.TextArea(this.nameRegex);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Find"))
            {
                this.FindPrefabs();
            }

            if (GUILayout.Button("Optimize"))
            {
                Optimize();
            }

            if (GUILayout.Button("Deoptimize"))
            {
                Deoptimize();
            }

            if (prefabs != null)
            {
                foreach (var prefab in prefabs)
                {
                    EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
                }
            }
        }

        void DrawArgs(
            SerializedProperty property,
            Rect rect,
            int index,
            bool isActive,
            bool isFocused)
        {
            var element = this.args.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element);
        }

        void FindPrefabs()
        {
            var path = AssetDatabase.GetAssetPath(this.target);
            if (string.IsNullOrEmpty(path))
                return;

            var dir = Path.GetDirectoryName(path);
            // find prefabs
            prefabs = new List<GameObject>();
            var guids = AssetDatabase.FindAssets("t:prefab", new string[] { dir });
            foreach (var guid in guids)
            {
                var objPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objPath);
                var animator = prefab.GetComponentInChildren<Animator>(true);
                if (animator == null || !Regex.IsMatch(prefab.name, nameRegex))
                    continue;

                prefabs.Add(prefab);
            }
        }

        string[] MatchBones(Transform tf, string[] checkBones)
        {
            List<string> existBone = new List<string>();
            foreach(var bone in checkBones)
            {
                if (tf.FindRecursively(bone) != null)
                    existBone.Add(bone);
            }
            return existBone.ToArray();
        }

        void Optimize()
        {
            if (prefabs == null || prefabs.Count == 0)
            {
                Debug.LogError("Optimize, prefabs is not valid or count equal to zero");
                return;
            }
            
            string[] args = (this.target as AnimatorOptimize).args;
            foreach (var prefab in prefabs)
            {
                var inst = GameObject.Instantiate(prefab);

                var matchBones = this.MatchBones(inst.transform, args);

                var smr = inst.GetComponentInChildren<SkinnedMeshRenderer>();
                var smrBones = smr.bones;

                Dictionary<string, List<Transform>> restoreBones = new Dictionary<string, List<Transform>>();
                List<string> optimizeBones = new List<string>();
                foreach (var matchBone in matchBones)
                {
                    bool isFind = false;
                    // smr bones
                    foreach (var smrBone in smrBones)
                    {
                        if (matchBone == smrBone.name)
                        {
                            optimizeBones.Add(matchBone);
                            isFind = true;
                            break;
                        }
                    }

                    // not found, temp save
                    if (!isFind)
                    {
                        // 
                        var tfMatchBones = inst.transform.FindAllRecursively(matchBone);
                        if (tfMatchBones.Length == 0)
                        {
                            Debug.LogError(string.Format("<color=#ffff00>{0}</color> transform not found, perhaps multi times", matchBone));
                            continue;
                        }

                        // 
                        foreach(var tfMatchBone in tfMatchBones)
                        {
                            var tfMatchBoneParent = tfMatchBone.parent;
                            if (tfMatchBoneParent == inst.transform) // if the parent is inst self, it can be optimize
                            {
                                if (!optimizeBones.Contains(matchBone))
                                    optimizeBones.Add(matchBone);
                            }
                            else
                            {
                                // if the parent is the restore one
                                bool isSmrBoneParent = false;
                                foreach (var smrBone in smrBones)
                                {
                                    if (tfMatchBoneParent == smrBone)
                                    {
                                        isSmrBoneParent = true;
                                        break;
                                    }
                                }

                                if (!isSmrBoneParent)
                                {
                                    Debug.LogWarning(string.Format("the bone <color=#ffff00>{0}</color> parent <color=#ffff00>{1}</color> can not match, maybe lost", matchBone, tfMatchBoneParent.name));
                                    continue;
                                }

                                //
                                tfMatchBone.SetParent(null);
                                if (!restoreBones.ContainsKey(tfMatchBoneParent.name))
                                {
                                    restoreBones.Add(tfMatchBoneParent.name, new List<Transform>(new Transform[] { tfMatchBone }));
                                }
                                else
                                {
                                    restoreBones[tfMatchBoneParent.name].Add(tfMatchBone);
                                }

                                if (!optimizeBones.Contains(tfMatchBoneParent.name))
                                {
                                    optimizeBones.Add(tfMatchBoneParent.name);
                                }
                            }
                        }
                    }
                }

                AnimatorUtility.OptimizeTransformHierarchy(inst, optimizeBones.ToArray());

                // restore
                foreach(var kvRestoreBone in restoreBones)
                {
                    var parent = inst.transform.FindRecursively(kvRestoreBone.Key);
                    foreach(var restoreTransform in kvRestoreBone.Value)
                    {
                        restoreTransform.SetParent(parent);
                    }
                }

                PrefabUtility.ReplacePrefab(inst, prefab);
                GameObject.DestroyImmediate(inst);
            }

            FindPrefabs();
        }

        void Deoptimize()
        {
            if (prefabs == null || prefabs.Count == 0)
                return;

            foreach (var prefab in prefabs)
            {
                var inst = GameObject.Instantiate(prefab);
                AnimatorUtility.DeoptimizeTransformHierarchy(inst);
                PrefabUtility.ReplacePrefab(inst, prefab);
                GameObject.DestroyImmediate(inst);
            }

            FindPrefabs();
        }
    }

}

