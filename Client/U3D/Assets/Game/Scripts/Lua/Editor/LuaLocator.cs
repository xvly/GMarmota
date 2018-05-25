using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using GStd.Editor;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Reflection;

public static class LuaLocator
{
    private static int lastOpenInstanceID;
    private static int lastOpenLine;


    /// <summary>
    /// https://docs.unity3d.com/ScriptReference/Callbacks.OnOpenAssetAttribute.html
    /// Return true if you handled the opening of the asset or false if an external tool should open it.
    /// 
    /// OnOpenAssetAttribute has an option to provide an order index in the callback, starting at 0. 
    /// This is useful if you have more than one OnOpenAssetAttribute callback, and you would like them to be called in a certain order.
    /// Callbacks are called in order, starting at zero.
    /// </summary>
    /// <param name="instanceID"></param>
    /// <param name="line">nstructs the text editor to go to that line</param>
    /// <returns></returns>
    [OnOpenAsset(0)]
    private static bool OnOpenAssetLog(int instanceID, int line)
    {
        if (lastOpenInstanceID == instanceID && lastOpenLine == line)
            return false;

        lastOpenInstanceID = instanceID;
        lastOpenLine = line;

        // 点击Project asset，判断文件后缀即可。
        if (line == -1)
        {
            var path = AssetDatabase.GetAssetPath(instanceID);
            if (path.EndsWith(".lua", System.StringComparison.OrdinalIgnoreCase))
            {
                //return TextEditorTool.OpenText(path, 0, 0);
                return false;
            }
        }

        // 点击日志，判断日志内容，分析出lua文件路径和行号
        // var log = ConsoleWindowSelectedLog;
        // if (log != null)
        // {
        //     var match = Regex.Match(log, @"\[(.*.lua):(.*?)\]:");
        //     if (match.Success && match.Groups.Count > 2)
        //     {
        //         var matchPath = Path.Combine("Assets/Game/Lua", match.Groups[1].Value);
        //         var matchLine = int.Parse(match.Groups[2].Value);

        //         //return TextEditorTool.OpenText(matchPath, matchLine, 0);

        //         var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(matchPath);
        //         lastOpenInstanceID = asset.GetInstanceID();
        //         lastOpenLine = matchLine;
        //         return AssetDatabase.OpenAsset(asset, matchLine);
        //     }
        // }
        

        // 不做任何处理
        return false;
    }

    static string ConsoleWindowSelectedLog
    {
        get
        {
            // 获取日志窗口
            var editorWindowAssembly = Assembly.GetAssembly(typeof(UnityEditor.EditorWindow));
            if (editorWindowAssembly == null)
                return null;

            var consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
                return null;

            var consoleWindowField = consoleWindowType.GetField("ms_ConsoleWindow",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (consoleWindowField == null)
                return null;

            var consoleWindowInst = consoleWindowField.GetValue(null);
            if (consoleWindowInst == null)
                return null;

            // 日志窗口处于选中状态，返回日志内容
            if ((object)UnityEditor.EditorWindow.focusedWindow == consoleWindowInst)
            {
                var listViewState = editorWindowAssembly.GetType("UnityEditor.ListViewState");
                if (listViewState == null)
                    return null;

                var listViewField = consoleWindowType.GetField("m_ListView",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (listViewField == null)
                    return null;

                var consoleWindowListView = listViewField.GetValue(consoleWindowInst);
                if (consoleWindowListView == null)
                    return null;

                var activeText = consoleWindowType.GetField("m_ActiveText",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (activeText == null)
                    return null;

                return activeText.GetValue(consoleWindowInst).ToString();
            }

            return null;
        }
    }
}
