namespace GStd.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    using System.Text.RegularExpressions;
    using System;
    using System.Collections.Generic;
    //using GStd;

    public static class AssetTools {

        [MenuItem("Assets/GStd/Disable prewarm")]
        static void DiablePrewarm()
        {
            var objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach (var obj in objs)
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    var go = obj as GameObject;
                    var pses = go.GetComponentsInChildren<ParticleSystem>(true);
                    foreach (var ps in pses)
                    {
                        if (ps.main.prewarm)
                        {
                            var psmain = ps.main;
                            psmain.prewarm = false;
                            EditorUtility.SetDirty(obj);

                            Debug.Log("disable prewarm:" + obj.name + "," + ps.gameObject.name);
                        }
                    }
                }
            }
            AssetDatabase.SaveAssets();

            Debug.Log("disable prewarm finished");
        }

        [MenuItem("Assets/GStd/Check Atlas")]
        static void CheckAtlas()
        {
            var guids = AssetDatabase.FindAssets("t:prefab", new string[]{"Assets/Game/UIs/Views"});
            foreach(var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                var flag = path.Remove(0, "Assets/Game/UIs/Views/".Length);
                flag = flag.Substring(0, flag.IndexOf("/"));

                var depPaths = AssetDatabase.GetDependencies(path);
                foreach (var depPath in depPaths)
                {
                    if (depPath.StartsWith("Assets/Game/UIs/Views/") && !depPath.EndsWith(".prefab")) 
                    {
                        var depFlag = depPath.Remove(0, "Assets/Game/UIs/Views/".Length);
                        depFlag = depFlag.Substring(0, depFlag.IndexOf("/"));

                        if (depFlag != flag)
                        {
                            Debug.Log("!!<color=#ffff00> " + path + "</color> -> <color=#00ff00>" + depPath + "</color>");

                        }
                    }
                }
            }

        }

        [MenuItem("Assets/GStd/Disable shadow")]
        static void DisableShadow()
        {
            var objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach (var obj in objs)
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    var go = obj as GameObject;
                    var renderers = go.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                    {
                        if (renderer.shadowCastingMode != UnityEngine.Rendering.ShadowCastingMode.Off)
                        {
                            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                            EditorUtility.SetDirty(obj);

                            Debug.Log("disable shadowCastingMode: " + obj.name + "," + renderer.gameObject.name);
                        }

                        if (renderer.receiveShadows)
                        {
                            renderer.receiveShadows = false;
                            EditorUtility.SetDirty(obj);

                            Debug.Log("disable receiveShadows: " + obj.name + "," + renderer.gameObject.name);
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();

            Debug.Log("disable shadow finished");
        }

        [MenuItem("Assets/GStd/Disable UI raycast")]
        static void DisableRaycast()
        {
            var objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach (var obj in objs)
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    var go = obj as GameObject;
                    var raycasts = go.GetComponentsInChildren<UnityEngine.UI.Graphic>();
                    foreach (var raycast in raycasts)
                    {
                        if (raycast.raycastTarget)
                        {
                            raycast.raycastTarget = false;
                            EditorUtility.SetDirty(obj);

                            Debug.Log("disable ui raycast " + obj.name + "," + raycast.gameObject.name);
                        }
                    }
                }
            }

            AssetDatabase.SaveAssets();

            Debug.Log("disable ui raycast finished");
        }

        [MenuItem("Assets/GStd/Set missing reference to none")]
        static void SetMissingReferenceToNone()
        {
            var objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach (var obj in objs)
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    CheckMissing(obj as GameObject, typeof(MonoBehaviour));
                }
            }

            AssetDatabase.SaveAssets();
        }

        static void CheckMissing(GameObject go, System.Type type)
        {
            bool isAnyChange = false;

            var components = go.GetComponentsInChildren(type, true);
            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.Log("component missing ");
                    continue;
                }

                SerializedObject so = new SerializedObject(component);
                bool isSoChange = false;
                var sp = so.GetIterator();
                while (sp.Next(true))
                {
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            sp.objectReferenceInstanceIDValue = 0;
                            isSoChange = true;

                            Debug.Log("set missing reference to none: " +
                                go.name + "," +
                                component.gameObject + "." + component.name);
                        }
                    }
                }

                if (isSoChange)
                {
                    so.ApplyModifiedProperties();
                    isAnyChange = true;
                }
            }

            if (isAnyChange)
            {
                EditorUtility.SetDirty(go);
            }
        }

        public static bool FixMissingScripts(GameObject go)
        {
            SerializedObject so = new SerializedObject(go);
            var properties = so.FindProperty("m_Component");
            var components = go.GetComponents<Component>();
            int propertyIndex = 0;
            bool isAnyChange = false;
            foreach(var component in components)
            {
                if (component == null)
                {
                    Debug.Log("fix missing scripts " + go.name + "," + propertyIndex);
                    properties.DeleteArrayElementAtIndex(propertyIndex);
                    isAnyChange = true;
                }
                propertyIndex++;
            }

            if (isAnyChange)
                so.ApplyModifiedProperties();

            return isAnyChange;
        }

        [MenuItem("Assets/Tools/remove missing scripts")]
        static void RemoveMissingScripts()
        {
            var objs = Selection.GetFiltered<UnityEngine.GameObject>(SelectionMode.DeepAssets);
            foreach(var obj in objs)
            {
                if (!(obj is GameObject))
                    continue;

                var tfs = obj.GetComponentsInChildren<Transform>(true);
                foreach(var tf in tfs)
                {
                    FixMissingScripts(tf.gameObject);
                }
            }

            AssetDatabase.Refresh();
        }

        #region material
        [MenuItem("Assets/GStd/RM Default-Material")]
        static void RMDefaultMaterial()
        {
            UnityEngine.Object[] UnityAssets = AssetDatabase.LoadAllAssetsAtPath("Resources/unity_builtin_extra");
            foreach (var asset in UnityAssets)
            {
                if (asset.name == "Default-Material")
                {
                    if (EditorUtility.DisplayDialog("确认信息", "确认删除资源:" + asset.name + "?", "删除"))
                    {
                        UnityEngine.Object.DestroyImmediate(asset, true);
                    }
                    break;
                }
            }

            Debug.Log("opreate finished");
        }


        /// <summary>
        /// clear material unused properties
        /// </summary>
        [MenuItem("Assets/GStd/ClearMaterialUnusedProperties", false)]
        public static void ClearMaterialUnusedProperties()
        {
            var materials = Selection.GetFiltered<Material>(SelectionMode.DeepAssets);
            foreach(var material in materials)
                ClearMaterialUnusedProperties(material, false);
            AssetDatabase.SaveAssets();

            Debug.Log("finished");
        }

        static void ClearMaterialUnusedProperties(Material mat, bool isSaveImmediate = true)
        {
            SerializedObject matInfo = new SerializedObject(mat);
            SerializedProperty propArr = matInfo.FindProperty("m_SavedProperties");

            propArr.Next(true);
            do
            {
                if (!propArr.isArray) continue;
                for (int i = propArr.arraySize - 1; i >= 0; --i)
                {
                    var p1 = propArr.GetArrayElementAtIndex(i);
                    if (p1.isArray)
                    {
                        for (int ii = p1.arraySize - 1; ii >= 0; --ii)
                        {
                            var p2 = p1.GetArrayElementAtIndex(ii);
                            var val = p2.FindPropertyRelative("first");
                            if (!mat.HasProperty(val.stringValue))
                            {
                                Debug.Log("remove " + mat.name + "," + val.stringValue);
                                p1.DeleteArrayElementAtIndex(ii);
                            }
                        }
                    }
                    else
                    {
                        var val = p1.FindPropertyRelative("first");
                        if (!mat.HasProperty(val.stringValue))
                        {
                            Debug.Log("remove " + mat.name + "," + val.stringValue);
                            propArr.DeleteArrayElementAtIndex(i);
                        }
                    }
                }
            } while (propArr.Next(false));

            matInfo.ApplyModifiedProperties();
            //Resources.UnloadAsset(mat);
            if (isSaveImmediate)
                AssetDatabase.SaveAssets();
        }

        #endregion

        #region asset bundle
        [MenuItem("Assets/GStd/Check AssetBundle Cross reference")]
        static void CheckAssetBundleCrossReference()
        {
            List<string> abList = new List<string>(AssetDatabase.GetAllAssetBundleNames());
            Dictionary<string, string[]> depDict = new Dictionary<string, string[]>();
            for (int i = 0; i < abList.Count; i++)
            {
                var ab = abList[i];
                var deps = AssetDatabase.GetAssetBundleDependencies(ab, false);
                if (deps != null && deps.Length > 0)
                {
                    depDict[ab] = deps;
                }
                else
                {
                    abList.RemoveAt(i);
                    --i;
                }
            }

            for (int i = 0; i < abList.Count; i++)
            {
                var ab = abList[i];

                string[] deps = null;
                if (depDict.TryGetValue(ab, out deps))
                {
                    foreach (var dep in deps)
                    {
                        string[] depdeps = null;
                        if (depDict.TryGetValue(dep, out depdeps))
                        {
                            foreach (var depdep in depdeps)
                            {
                                if (depdep == ab)
                                {
                                    Debug.Log("cross reference: " + ab + " <-> " + dep);
                                }
                            }
                        }
                    }
                    depDict.Remove(ab);
                }
            }

            Debug.Log("Check AssetBundle Cross reference done");
        }
        #endregion

        #region text
        [MenuItem("Assets/GStd/UI/Fix ttf(Not Dynamic)")]
        static void FindErrorTtfText()
        {
            var objs = Selection.GetFiltered<UnityEngine.Object>(SelectionMode.DeepAssets);
            foreach(var obj in objs)
            {
                var prefab = obj as GameObject;
                if (prefab == null)
                    continue;

                bool isAnyChange = false;
                var textComponents = prefab.GetComponentsInChildren<UnityEngine.UI.Text>(true);
                foreach(var text in textComponents)
                {
                    var font = text.font;
                    if (!font.dynamic)
                    {
                        if (text.fontStyle != FontStyle.Normal)
                        {
                            Debug.Log("style error, " + obj.name + "," + text.gameObject.name);
                            text.fontStyle = FontStyle.Normal;

                            isAnyChange = true;
                        }

                        if (text.fontSize != 0)
                        {
                            Debug.Log("size error, " + obj.name + "," + text.gameObject.name);
                            text.fontSize = 0;

                            isAnyChange = true;
                        }
                    }
                }

                if (isAnyChange)
                {
                    EditorUtility.SetDirty(obj);
                }
            }

            AssetDatabase.SaveAssets();
        }
        #endregion
    }

}

