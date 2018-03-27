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

    [CreateAssetMenu(
            fileName = "@UGUISplitPackingTag",
            menuName = "GStd/UGUISplitPackingTag")]
    public class UGUISplitPackingTag : ScriptableObject
    {
        [SerializeField]
        private string baseName;
        [SerializeField]
        private int groupSize;
    }

    [CustomEditor(typeof(UGUISplitPackingTag))]
    public class UGUISplitPackingTagEditor : Editor
    {
        private SerializedProperty _baseName;
        private SerializedProperty _groupSize;

        private Texture[] _textures;
        private int[] _textureSizeList;

        public void OnEnable()
        {
            this._baseName = this.serializedObject.FindProperty("baseName");
            this._groupSize = this.serializedObject.FindProperty("groupSize");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this._baseName);
            EditorGUILayout.PropertyField(this._groupSize);

            if (GUILayout.Button("Find Texture"))
                this.FindTexture();

            if (GUILayout.Button("Split"))
                this.Split();

            if (this._textureSizeList != null)
            {
                for (int i = 0; i < this._textureSizeList.Length; i++)
                    EditorGUILayout.IntField(this._textureSizeList[i]);
            }

            if (this._textures != null)
            {
                for (int i = 0; i < this._textures.Length; i++)
                    EditorGUILayout.ObjectField(this._textures[i], typeof(Texture), false);
            }

            this.serializedObject.ApplyModifiedProperties();
        }
        
        
        private void FindTexture()
        {
            var path = AssetDatabase.GetAssetPath(this.target);
            if (string.IsNullOrEmpty(path))
                return;

            var dir = Path.GetDirectoryName(path);

            List<Texture> textureList = new List<Texture>();
            List<int> sizeList = new List<int>();
            var guids = AssetDatabase.FindAssets("t:texture", new string[] { dir });
            foreach (var guid in guids)
            {
                var objPath = AssetDatabase.GUIDToAssetPath(guid);
                var texture = AssetDatabase.LoadAssetAtPath<Texture>(objPath);

                
                if (!sizeList.Contains(texture.width))
                    sizeList.Add(texture.width);

                textureList.Add(texture);
            }

            this._textures = textureList.ToArray();
            this._textureSizeList = sizeList.ToArray();
        }

        private void Split()
        {
            if (this._textures.Length == 0)
            {
                EditorUtility.DisplayDialog("Error", "NO texture selected", "ok");
                return;
            }

            var baseName = this._baseName.stringValue;
            if (baseName == "")
            {
                EditorUtility.DisplayDialog("Error", "BaseName is empty", "ok");
                return;
            }

            int groupSize = this._groupSize.intValue;
            if (groupSize <= 0)
            {
                EditorUtility.DisplayDialog("Error", "GroupSize is invalid", "ok");
                return;
            }

            var textureLength = this._textures.Length;
            ////int cellSize = textureLength / groupSize;
            for (int i = 0; i < textureLength; i++)
            {
                TextureImporter ti = TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(this._textures[i])) as TextureImporter;
                var tagName = string.Format("{0}{1}", baseName, i / groupSize);
                if (ti.spritePackingTag != tagName)
                {
                    ti.spritePackingTag = tagName;
                    ti.SaveAndReimport();
                }
            }
        }
    }

}

