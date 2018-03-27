#if false

using GStd.Editor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(FontMaker), true)]
internal sealed class FontMakerEditor⁬ : Editor
{
    [CompilerGenerated]
    private sealed class MyC
    {
        internal SerializedProperty sp;
        internal FontMakerEditor⁬ maker;
        internal void RR(Rect rect, int num, bool flag, bool flag2)
        {
            this.maker.CalcElement(sp, rect, num, flag, flag2);
        }
    }

    [CompilerGenerated]
    private SerializedProperty atlasName;
    [CompilerGenerated]
    private SerializedProperty atlasPadding;
    [CompilerGenerated]
    private SerializedProperty atlasSize;

    private SerializedProperty fonts;

    private ReorderableList fontsList;

    private Dictionary<string, ReorderableList> dictA = new Dictionary<string, ReorderableList>();

    [CompilerGenerated]
    private static ReorderableList.HeaderCallbackDelegate delegateA;

    public override void OnInspectorGUI()
    {
        base.serializedObject.Update();
        EditorGUILayout.PropertyField(this.atlasName, new GUILayoutOption[0]);
        this.fontsList.DoLayoutList();
        EditorGUILayout.PropertyField(this.atlasPadding, new GUIContent("padding"));
        EditorGUILayout.PropertyField(this.atlasSize, new GUIContent("size"));

        base.serializedObject.ApplyModifiedProperties();

        FontMaker fontMaker = (FontMaker)this.target;
        if (GUILayout.Button("Build", new GUILayoutOption[0]))
        {
            fontMaker.Build(this.atlasPadding.intValue, this.atlasSize.intValue);
        }
    }

    private void OnEnable()
    {
        if (this.target == null)
            return;

        SerializedObject serializedObject = base.serializedObject;
        this.atlasName = serializedObject.FindProperty("atlasName");
        this.atlasPadding = serializedObject.FindProperty("atlasPadding");
        this.atlasSize = serializedObject.FindProperty("atlasSize");
        this.fonts = serializedObject.FindProperty("fonts");
        this.fontsList = new ReorderableList(serializedObject, this.fonts);

        ReorderableList arg_6D_0 = this.fontsList;
        if (delegateA == null)
            delegateA = new ReorderableList.HeaderCallbackDelegate(DrawHeader);
        arg_6D_0.drawHeaderCallback = delegateA;
        this.fontsList.elementHeightCallback = new ReorderableList.ElementHeightCallbackDelegate(this.ElementHight);
        this.fontsList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(this.DrawElement);
    }

    [CompilerGenerated]
    private static void DrawHeader(Rect position)
    {
        GUI.Label(position, "Fonts:");
    }

    private float CalcHight(SerializedProperty serializedProperty, int num)
    {
        SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(num);
        if (!arrayElementAtIndex.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        SerializedProperty serializedProperty2 = arrayElementAtIndex.FindPropertyRelative("glyphs");
        if (serializedProperty2.arraySize > 0)
        {
            return (4.75f + (float)serializedProperty2.arraySize) * EditorGUIUtility.singleLineHeight;
        }
        return 5.75f * EditorGUIUtility.singleLineHeight;
    }

    private void CalcElement(SerializedProperty serializedProperty, Rect rect, int num, bool flag, bool flag2)
    {
        SerializedProperty arrayElementAtIndex = serializedProperty.GetArrayElementAtIndex(num);
        rect.x += 10f;
        rect.width -= 10f;
        Rect rect2 = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
        arrayElementAtIndex.isExpanded = EditorGUI.Foldout(rect2, arrayElementAtIndex.isExpanded, new GUIContent(arrayElementAtIndex.displayName));
        if (!arrayElementAtIndex.isExpanded)
        {
            return;
        }
        SerializedProperty serializedProperty2 = arrayElementAtIndex.FindPropertyRelative("fontName");
        SerializedProperty serializedProperty3 = arrayElementAtIndex.FindPropertyRelative("glyphs");
        rect2.y += 1.25f * EditorGUIUtility.singleLineHeight;
        EditorGUI.PropertyField(rect2, serializedProperty2, new GUIContent("Font Name:"));
        rect2.y += EditorGUIUtility.singleLineHeight;
        rect2.height = rect.y - EditorGUIUtility.singleLineHeight;
        ReorderableList reorderableList = this.DrawList(serializedProperty3);
        reorderableList.DoList(rect2);
    }

    private ReorderableList DrawList(SerializedProperty sp)
    {
        MyC cc = new MyC();
        cc.sp = sp;
        cc.maker = this;
        ReorderableList reorderableList;
        if (this.dictA.TryGetValue(cc.sp.propertyPath, out reorderableList))
        {
            return reorderableList;
        }
        reorderableList = new ReorderableList(cc.sp.serializedObject, cc.sp);
        reorderableList.drawHeaderCallback = new ReorderableList.HeaderCallbackDelegate(this.DrawRect);
        reorderableList.elementHeight = EditorGUIUtility.singleLineHeight;
        reorderableList.drawElementCallback = new ReorderableList.ElementCallbackDelegate(cc.RR);
        this.dictA.Add(cc.sp.propertyPath, reorderableList);
        return reorderableList;
    }

    private void DrawRect(Rect rect)
    {

        Rect position = new Rect(rect.x + 10f, rect.y, rect.width / 4f, rect.height);
        Rect position2 = new Rect(rect.x + 10f + rect.width / 4f, rect.y, rect.width / 4f, rect.height);
        Rect position3 = new Rect(rect.x + 10f + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
        GUI.Label(position, "Character:");
        GUI.Label(position2, "Code:");
        GUI.Label(position3, "Image:");
    }

    [CompilerGenerated]
    private float ElementHight(int num)
    {
        return this.CalcHight(this.fonts, num);
    }

    [CompilerGenerated]
    private void DrawElement(Rect rect, int num, bool flag, bool flag2)
    {
        this.CalcElement(this.fonts, rect, num, flag, flag2);
    }
}

#endif
