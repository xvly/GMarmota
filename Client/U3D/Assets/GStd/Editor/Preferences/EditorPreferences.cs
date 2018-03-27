using System;
using ns1;
using UnityEditor;

namespace GStd.Editor
{
    // Token: 0x020000EF RID: 239
    public sealed class EditorPreferences
    {
        // Token: 0x06000411 RID: 1041 RVA: 0x00003A90 File Offset: 0x00001C90
        [PreferenceItem("GStd Tools")]
        static void OnGUI()
        {
            TextEditorTool.GUI();
            EditorGUILayout.Space();
            Class179.GUI();
            EditorGUILayout.Space();
            Class132.GUI();
        }
    }
}
