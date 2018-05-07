using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GStd.Editor.Asset
{
    [CustomEditor(typeof(AssetMarker))]
    public class AssetMarkerEditor : UnityEditor.Editor
    {
        private AssetMarker marker;

        private Dictionary<string, string[]> assets;

        private bool isForceFoldout = true;

        private SerializedProperty bundleName;
        private SerializedProperty includeRegex;
        private SerializedProperty excludeRegex;

        private static Dictionary<string, AssetMarker> assetMarkers = null;

        private static void FindAssetMarkers()
        {
            if (assetMarkers != null)
                return;

            assetMarkers = new Dictionary<string, AssetMarker>();
            var guids = AssetDatabase.FindAssets("t:AssetMarker");
            foreach(var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                assetMarkers.Add(path, AssetDatabase.LoadAssetAtPath<AssetMarker>(path));
            }
        }

        public static void ProcessAddMarker(string path)
        {
            FindAssetMarkers();

            var guid = AssetDatabase.AssetPathToGUID(path);

            if (assetMarkers.ContainsKey(guid))
                return;
                
            var marker = AssetDatabase.LoadAssetAtPath<AssetMarker>(path);
            if (marker != null)
            {
                assetMarkers.Add(guid, marker);
                Debug.Log(string.Format("add marker , path={0}, guid={1}", path, guid));
            }
        }

        public static void ProcessDelMarker(string path)
        {
            FindAssetMarkers();

            var guid = AssetDatabase.AssetPathToGUID(path);
            if (!assetMarkers.ContainsKey(guid))
                return;

            assetMarkers.Remove(guid);
            Debug.Log(string.Format("remove marker , path={0}, guid={1}", path, guid));
        }

        public static void ProcessAsset(string path)
        {
            FindAssetMarkers();

            foreach(var kv in assetMarkers)
            {
                var markerDir = Path.GetDirectoryName(kv.Key);
                if (!path.StartsWith(markerDir) || path[markerDir.Length+1] != '\\')
                    continue;

                if (kv.Value.IsNeedMark(path))
                {
                    kv.Value.Mark(path);
                    break;
                }
            }
        }

        [MenuItem("Assets/GStd/Bundle/Mark All")]
        public static void BundleMarkAll()
        {
            FindAssetMarkers();

            List<AssetMarker> list = new List<AssetMarker>();
            foreach(var kv in assetMarkers)
                list.Add(kv.Value);
                
            foreach(var marker in list)
                marker.MarkAll();
        }

        [MenuItem("Assets/GStd/Bundle/Clear All")]
        public static void BundleClearAll()
        {
            var abs = AssetDatabase.GetAllAssetBundleNames();
            foreach(var ab in abs)
                AssetDatabase.RemoveAssetBundleName(ab, true);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/GStd/Bundle/Remark all")]
        public static void BundleRemarkAll()
        {
            BundleClearAll();
            BundleMarkAll();
        }

        [MenuItem("Assets/GStd/Bundle/Clear folder")]
        public static void BundleClearFolder()
        {
            var assets = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
            foreach(var asset in assets)
            {
                var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
                if (importer == null)
                {
                    Debug.Log("!! invaild path " + asset.name);
                    continue;
                }

                bool isNeedSave = false;
                if (importer.assetBundleName != "")
                {
                    importer.assetBundleName = "";
                    isNeedSave = true;
                }

                if (importer.assetBundleVariant != "")
                {
                    importer.assetBundleVariant = "";
                    isNeedSave = true;
                }

                if (isNeedSave)
                    importer.SaveAndReimport();
            }
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            this.marker = this.target as AssetMarker;
            this.bundleName = this.serializedObject.FindProperty("bundleName");
            this.includeRegex = this.serializedObject.FindProperty("include");
            this.excludeRegex = this.serializedObject.FindProperty("exclude");
            this.assets = this.marker.FindAssets();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(this.bundleName);

            EditorGUILayout.PropertyField(this.includeRegex);
            EditorGUILayout.PropertyField(this.excludeRegex);

            if (GUILayout.Button("Mark"))
                this.marker.MarkAll();

            if (GUILayout.Button("Refresh asset"))
                this.assets = this.marker.FindAssets();

            this.GUIAssets();
            this.serializedObject.ApplyModifiedProperties();
        }

        Vector2 assetScrollView;
        private void GUIAssets()
        {
            if (this.assets == null)
                return;
                
            EditorGUILayout.Space();

            assetScrollView = EditorGUILayout.BeginScrollView(assetScrollView);
            // folder
            foreach (var kv in this.assets)
            {
                if (kv.Key != "")
                {
                    foreach (var assetPath in kv.Value)
                    {
                        var bundleName = this.marker.GetMarkBundleName(kv.Key, assetPath);
                        EditorGUILayout.LabelField(string.Format("[{0}]{1}", bundleName, assetPath));
                    }
                }
            }

            // top directory
            foreach (var kv in this.assets)
            {
                if (kv.Key == "")
                {
                    foreach (var assetPath in kv.Value)
                    {
                        var bundleName = this.marker.GetMarkBundleName("", assetPath);
                        EditorGUILayout.LabelField(string.Format("[{0}]{1}", bundleName, assetPath));
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }
}