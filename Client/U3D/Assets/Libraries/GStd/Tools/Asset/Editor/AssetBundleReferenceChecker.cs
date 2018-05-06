using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
namespace GStd.Editor
{
    public class AssetBundleReferenceChecker : EditorWindow
    {

        private string assetBundleName;

        private UnityEngine.Object[] objs;

        public void Awake()
        {

        }

        void OnEnable()
        {

        }

        Vector2 scrollView;
        void OnGUI()
        {
            this.assetBundleName = EditorGUILayout.TextField("asset bundle name", assetBundleName);

            if (GUILayout.Button("find"))
            {
                if (string.IsNullOrEmpty(this.assetBundleName))
                {
                    return;
                }

                List<UnityEngine.Object> objList = new List<UnityEngine.Object>();

                var guids = AssetDatabase.FindAssets("t:object", new string[] { "Assets/Game" });
                using (ProgressIndicator progress = new ProgressIndicator("...", guids.Length))
                {
                    foreach (var guid in guids)
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        var deps = AssetDatabase.GetDependencies(path);

                        if (progress.Show(path + "(" + deps.Length + ")"))
                        {
                            Debug.Log("break");
                            return;
                        }

                        foreach (var dep in deps)
                        {
                            var depAsset = AssetImporter.GetAtPath(dep);
                            if (depAsset != null && depAsset.assetBundleName == this.assetBundleName)
                            {
                                var assetObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                                if (!objList.Contains(assetObj))
                                    objList.Add(assetObj);
                            }
                        }

                        progress.Next();
                    }
                }

                this.objs = objList.ToArray();
            }

            if (this.objs != null)
            {
                this.scrollView = GUILayout.BeginScrollView(this.scrollView);
                foreach (var obj in this.objs)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(AssetDatabase.GetAssetPath(obj), GUILayout.Width(this.position.width / 2));
                    EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), false, GUILayout.Width(this.position.width / 2 - 10));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
        }

        [MenuItem("Window/GStd/AssetBundle/ReferenceChecker &G")]
        static void ShowWindow()
        {
            AssetBundleReferenceChecker window = EditorWindow.GetWindow<AssetBundleReferenceChecker>();
        }
    }

}

