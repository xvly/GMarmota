using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GStd;

[CustomEditor(typeof(ClippingGroup))]
public class ClippingGroupEditor : Editor{

    SerializedProperty range;
    SerializedProperty camera;
    SerializedProperty roots;
    SerializedProperty groups;

    void OnEnable()
    {
        this.range = this.serializedObject.FindProperty("range");
        this.camera = this.serializedObject.FindProperty("camera");
        this.roots = this.serializedObject.FindProperty("roots");
        this.groups = this.serializedObject.FindProperty("groups");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.range, new GUIContent("range"));
        EditorGUILayout.PropertyField(this.camera, new GUIContent("camera"));
        EditorGUILayout.PropertyField(this.roots, new GUIContent("roots"), true);
        this.serializedObject.ApplyModifiedProperties();

        var groups = (this.target as ClippingGroup).groups;
        if (groups != null && groups.Count > 0)
        {
            EditorGUILayout.LabelField(new GUIContent("1renderer count:"), new GUIContent(groups.Count.ToString()));
            foreach (var g in groups)
            {
                GUILayout.Label(g.Key.ToString());
                foreach (var data in g.Value.renderers)
                {
                    EditorGUILayout.ObjectField(data, typeof(Renderer));
                }
            }
        }
        else
        {
            GUILayout.Label("groups is empty");
        }
        
        
        //EditorGUILayout.PropertyField(this.serializedObject.FindProperty("range"));
    }
}
