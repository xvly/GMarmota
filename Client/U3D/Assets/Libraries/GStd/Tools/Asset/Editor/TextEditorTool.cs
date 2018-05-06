using System;
using System.Diagnostics;
using System.IO;
using ns1;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace GStd.Editor
{
	public static class TextEditorTool
	{
		public static bool OpenText(string path, int row, int column)
		{
			string fullPath = Path.GetFullPath(path);
			if (!File.Exists(fullPath))
			{
				Debug.LogErrorFormat("The file {0} is not exitsed.", new object[]
				{
					path
				});
				return false;
			}
			if (TextEditorTool.int_0 == 0 && !string.IsNullOrEmpty(TextEditorTool.class131_0.method_0()))
			{
				string arguments = string.Format("\"{0}\":{1}", fullPath, row);
				Process.Start(TextEditorTool.class131_0.method_0(), arguments);
				return true;
			}
			if (TextEditorTool.int_0 == 1 && !string.IsNullOrEmpty(TextEditorTool.class131_1.method_0()))
			{
				string arguments2 = string.Format("--goto \"{0}\":{1}:{2}", fullPath, row, column);
				Process.Start(TextEditorTool.class131_1.method_0(), arguments2);
				return true;
			}
			Object @object = AssetDatabase.LoadAssetAtPath<Object>(path);
			return AssetDatabase.OpenAsset(@object, row);
		}

		internal static void GUI()
		{
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Text Editor:", new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			TextEditorTool.int_0 = EditorGUILayout.Popup(TextEditorTool.int_0, TextEditorTool.string_0, new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				EditorPrefs.SetInt("GStd.TextEditor.ActiveIndex", TextEditorTool.int_0);
			}
			EditorGUILayout.EndHorizontal();
			int num = TextEditorTool.int_0;
			if (num != 0)
			{
				if (num == 1)
				{
					TextEditorTool.class131_1.method_1();
				}
			}
			else
			{
				TextEditorTool.class131_0.method_1();
			}
		}

		[InitializeOnLoadMethod]
		private static void smethod_1()
		{
			string[] string_ = new string[]
			{
				"C:\\Sublime Text 3\\sublime_text.exe",
				"C:\\Program Files\\Sublime Text 3\\sublime_text.exe",
				"C:\\Program Files (x86)\\Sublime Text 3\\sublime_text.exe",
				"D:\\Sublime Text 3\\sublime_text.exe",
				"D:\\Program Files\\Sublime Text 3\\sublime_text.exe",
				"D:\\Program Files (x86)\\Sublime Text 3\\sublime_text.exe",
				"E:\\Sublime Text 3\\sublime_text.exe",
				"E:\\Program Files\\Sublime Text 3\\sublime_text.exe",
				"E:\\Program Files (x86)\\Sublime Text 3\\sublime_text.exe",
				"F:\\Sublime Text 3\\sublime_text.exe",
				"F:\\Program Files\\Sublime Text 3\\sublime_text.exe",
				"F:\\Program Files (x86)\\Sublime Text 3\\sublime_text.exe"
			};
			TextEditorTool.class131_0 = new Class131("GStd.External.SublimeText", "Sublime Text", string_);
			string_ = new string[]
			{
				"C:\\Program Files\\Microsoft VS Code\\Code.exe",
				"C:\\Program Files (x86)\\Microsoft VS Code\\Code.exe",
				"D:\\Program Files\\Microsoft VS Code\\Code.exe",
				"D:\\Program Files (x86)\\Microsoft VS Code\\Code.exe",
				"E:\\Program Files\\Microsoft VS Code\\Code.exe",
				"E:\\Program Files (x86)\\Microsoft VS Code\\Code.exe",
				"F:\\Program Files\\Microsoft VS Code\\Code.exe",
				"F:\\Program Files (x86)\\Microsoft VS Code\\Code.exe"
			};
			TextEditorTool.class131_1 = new Class131("GStd.External.VisualStudioCode", "Visual Studio Code", string_);
			TextEditorTool.int_0 = EditorPrefs.GetInt("GStd.TextEditor.ActiveIndex", 0);
		}

		private static Class131 class131_0;

		private static Class131 class131_1;

		private static int int_0;

		private static string[] string_0 = new string[]
		{
			"Sublime Text",
			"Visual Studio Code"
		};
	}
}
