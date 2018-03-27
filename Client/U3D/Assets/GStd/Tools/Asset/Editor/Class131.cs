using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ns1
{
	// Token: 0x020000E5 RID: 229
	internal sealed class Class131
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0000387D File Offset: 0x00001A7D
		internal Class131(string string_4, string string_5, string[] string_6)
		{
			this.string_0 = string_4;
			this.string_1 = string_5;
			this.string_2 = string_6;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001E324 File Offset: 0x0001C524
		internal string method_0()
		{
			if (string.IsNullOrEmpty(this.string_3) && EditorPrefs.HasKey(this.string_0))
			{
				string @string = EditorPrefs.GetString(this.string_0);
				if (File.Exists(@string))
				{
					this.string_3 = @string;
				}
				else
				{
					EditorPrefs.DeleteKey(this.string_0);
				}
			}
			if (string.IsNullOrEmpty(this.string_3))
			{
				this.method_3();
			}
			return this.string_3;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001E38C File Offset: 0x0001C58C
		internal void method_1()
		{
			string text = this.method_0();
			string text2 = string.Empty;
			string text3 = this.string_1;
			string text4 = Class136.smethod_1(text);
			if (text3 == text4)
			{
				text2 = string.Format("{0}:", text3);
			}
			else
			{
				text2 = string.Format("{0}: {1}", text3, text4);
			}
			EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
			EditorGUILayout.LabelField(text2, new GUILayoutOption[]
			{
				GUILayout.Width(225f)
			});
			if (GUILayout.Button("Search...", new GUILayoutOption[0]))
			{
				this.method_3();
			}
			if (GUILayout.Button("Browse...", new GUILayoutOption[0]))
			{
				string text5 = EditorUtility.OpenFilePanel("Browse for application", string.Empty, ".exe");
				if (text5.Length != 0)
				{
					this.method_2(text5);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.LabelField(text, new GUILayoutOption[0]);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000389A File Offset: 0x00001A9A
		private void method_2(string string_4)
		{
			if (!File.Exists(string_4))
			{
				return;
			}
			this.string_3 = string_4;
			EditorPrefs.SetString(this.string_0, string_4);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001E460 File Offset: 0x0001C660
		private void method_3()
		{
			foreach (string text in this.string_2)
			{
				if (File.Exists(text))
				{
					this.string_3 = text;
					EditorPrefs.SetString(this.string_0, text);
				}
			}
		}

		// Token: 0x040004E3 RID: 1251
		private string string_0;

		// Token: 0x040004E4 RID: 1252
		private string string_1;

		// Token: 0x040004E5 RID: 1253
		private string[] string_2;

		// Token: 0x040004E6 RID: 1254
		private string string_3;
	}
}
