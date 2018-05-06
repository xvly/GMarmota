namespace GStd.Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    [CustomEditor(typeof(I18NConfig))]

    public class I18NConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            var languageProperty = this.serializedObject.FindProperty("language");
            EditorGUILayout.PropertyField(languageProperty);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}