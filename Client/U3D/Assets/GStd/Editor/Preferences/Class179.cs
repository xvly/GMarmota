using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using GStd;
using UnityEditor;
using UnityEngine;

namespace ns1
{
	// Token: 0x0200011D RID: 285
	internal static class Class179
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x00003D0F File Offset: 0x00001F0F
		internal static bool smethod_0()
		{
			return !string.IsNullOrEmpty(Class179.class131_0.method_0());
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00003D23 File Offset: 0x00001F23
		internal static void GUI()
		{
			Class179.class131_0.method_1();
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001F6EC File Offset: 0x0001D8EC
		internal static Class179.Class180 smethod_2(Texture2D[] texture2D_0, int int_0, int int_1, string string_0)
		{
			bool flag = false;
			string[] array = new string[texture2D_0.Length];
			for (int i = 0; i < texture2D_0.Length; i++)
			{
				Texture2D texture2D = texture2D_0[i];
				if (!flag && texture2D.HasAlpha())
				{
					flag = true;
				}
				string assetPath = AssetDatabase.GetAssetPath(texture2D);
				array[i] = Path.GetFullPath(assetPath);
			}
			return Class179.smethod_3(array, int_0, int_1, string_0, flag);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x0001F740 File Offset: 0x0001D940
		internal static Class179.Class180 smethod_3(string[] string_0, int int_0, int int_1, string string_1, bool bool_0)
		{
			string text = string_1 + "{n}.tga";
			string text2 = string_1 + "{n}.xml";
			text = Path.GetFullPath(text);
			text2 = Path.GetFullPath(text2);
			string directoryName = Path.GetDirectoryName(text);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string directoryName2 = Path.GetDirectoryName(text2);
			if (!Directory.Exists(directoryName2))
			{
				Directory.CreateDirectory(directoryName2);
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in string_0)
			{
				stringBuilder.Append(value);
				stringBuilder.Append(" ");
			}
			string text3;
			if (bool_0)
			{
				text3 = "RGBA8888";
			}
			else
			{
				text3 = "RGB888";
			}
			string string_2 = Class179.class131_0.method_0();
			string string_3 = string.Format("--texture-format tga --format xml --algorithm MaxRects --maxrects-heuristics Best --size-constraints POT --force-squared --pack-mode Best --enable-rotation --trim-mode Trim --padding 0 --multipack --opt {0} --max-width {1} --max-height {2} --sheet {3} --data {4} {5}", new object[]
			{
				text3,
				int_0,
				int_1,
				text,
				text2,
				stringBuilder.ToString()
			});
			Class179.smethod_5(string_2, string_3, string.Empty);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(string_1);
			string[] files = Directory.GetFiles(directoryName, string.Format("{0}*.xml", fileNameWithoutExtension));
			return Class179.smethod_4(files, bool_0);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x0001F868 File Offset: 0x0001DA68
		private static Class179.Class180 smethod_4(string[] string_0, bool bool_0)
		{
			Class179.Class180 @class = new Class179.Class180();
			@class.method_1(bool_0);
			foreach (string filename in string_0)
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				XmlElement xmlElement = xmlDocument["TextureAtlas"];
				if (xmlElement == null)
				{
					return null;
				}
				Class179.Class181 class2 = new Class179.Class181();
				class2.method_3(int.Parse(xmlElement.Attributes["width"].Value));
				class2.method_5(int.Parse(xmlElement.Attributes["height"].Value));
				class2.method_1(xmlElement.Attributes["imagePath"].Value);
				@class.method_3(class2);
				foreach (object obj in xmlElement.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					Class179.Class182 class3 = new Class179.Class182();
					class3.method_1(class2);
					class3.method_3(xmlNode.Attributes["n"].Value);
					class3.method_5(int.Parse(xmlNode.Attributes["x"].Value));
					class3.method_7(int.Parse(xmlNode.Attributes["y"].Value));
					class3.method_9(int.Parse(xmlNode.Attributes["w"].Value));
					class3.method_11(int.Parse(xmlNode.Attributes["h"].Value));
					XmlAttribute xmlAttribute = xmlNode.Attributes["pX"];
					class3.method_13((xmlAttribute == null) ? 0f : float.Parse(xmlAttribute.Value));
					XmlAttribute xmlAttribute2 = xmlNode.Attributes["pY"];
					class3.method_15((xmlAttribute2 == null) ? 0f : float.Parse(xmlAttribute2.Value));
					XmlAttribute xmlAttribute3 = xmlNode.Attributes["oX"];
					class3.method_17((xmlAttribute3 == null) ? 0f : float.Parse(xmlAttribute3.Value));
					XmlAttribute xmlAttribute4 = xmlNode.Attributes["oY"];
					class3.method_19((xmlAttribute4 == null) ? 0f : float.Parse(xmlAttribute4.Value));
					XmlAttribute xmlAttribute5 = xmlNode.Attributes["oW"];
					class3.method_21((xmlAttribute5 == null) ? 0f : float.Parse(xmlAttribute5.Value));
					XmlAttribute xmlAttribute6 = xmlNode.Attributes["oH"];
					class3.method_23((xmlAttribute6 == null) ? 0f : float.Parse(xmlAttribute6.Value));
					XmlAttribute xmlAttribute7 = xmlNode.Attributes["r"];
					class3.method_25(xmlAttribute7 != null && xmlAttribute7.Value == "y");
					@class.method_2(class3);
				}
			}
			return @class;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001FBAC File Offset: 0x0001DDAC
		private static void smethod_5(string string_0, string string_1, string string_2)
		{
			using (Process process = Process.Start(new ProcessStartInfo(string_0, string_1)
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8,
				WorkingDirectory = string_2
			}))
			{
				Process process2 = process;
				if (Class179.dataReceivedEventHandler_0 == null)
				{
					Class179.dataReceivedEventHandler_0 = new DataReceivedEventHandler(Class179.smethod_7);
				}
				process2.OutputDataReceived += Class179.dataReceivedEventHandler_0;
				Process process3 = process;
				if (Class179.dataReceivedEventHandler_1 == null)
				{
					Class179.dataReceivedEventHandler_1 = new DataReceivedEventHandler(Class179.smethod_8);
				}
				process3.ErrorDataReceived += Class179.dataReceivedEventHandler_1;
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();
				process.Close();
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001FC7C File Offset: 0x0001DE7C
		[InitializeOnLoadMethod]
		private static void smethod_6()
		{
			string[] string_ = new string[]
			{
				"C:\\Program Files\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"C:\\Program Files (x86)\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"D:\\Program Files\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"D:\\Program Files (x86)\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"E:\\Program Files\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"E:\\Program Files (x86)\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"F:\\Program Files\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe",
				"F:\\Program Files (x86)\\CodeAndWeb\\TexturePacker\\bin\\TexturePacker.exe"
			};
			Class179.class131_0 = new Class131("Nirvana.External.TexturePacker", "TexturePacker", string_);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0001FCE8 File Offset: 0x0001DEE8
		[CompilerGenerated]
		private static void smethod_7(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null)
			{
				return;
			}
			string text = e.Data.Trim();
			if (!string.IsNullOrEmpty(text))
			{
				UnityEngine.Debug.Log(text);
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0001FD18 File Offset: 0x0001DF18
		[CompilerGenerated]
		private static void smethod_8(object sender, DataReceivedEventArgs e)
		{
			if (e.Data == null)
			{
				return;
			}
			string text = e.Data.Trim();
			if (!string.IsNullOrEmpty(text))
			{
                UnityEngine.Debug.LogError(text);
			}
		}

		// Token: 0x0400052C RID: 1324
		private static Class131 class131_0;

		// Token: 0x0400052D RID: 1325
		[CompilerGenerated]
		private static DataReceivedEventHandler dataReceivedEventHandler_0;

		// Token: 0x0400052E RID: 1326
		[CompilerGenerated]
		private static DataReceivedEventHandler dataReceivedEventHandler_1;

		// Token: 0x0200011E RID: 286
		internal class Class180
		{
			// Token: 0x06000462 RID: 1122 RVA: 0x00003D52 File Offset: 0x00001F52
			[CompilerGenerated]
			public bool method_0()
			{
				return this.bool_0;
			}

			// Token: 0x06000463 RID: 1123 RVA: 0x00003D5A File Offset: 0x00001F5A
			[CompilerGenerated]
			public void method_1(bool bool_1)
			{
				this.bool_0 = bool_1;
			}

			// Token: 0x06000464 RID: 1124 RVA: 0x00003D63 File Offset: 0x00001F63
			public void method_2(Class179.Class182 class182_0)
			{
				this.dictionary_0.Add(class182_0.method_2(), class182_0);
			}

			// Token: 0x06000465 RID: 1125 RVA: 0x00003D77 File Offset: 0x00001F77
			public void method_3(Class179.Class181 class181_0)
			{
				this.list_0.Add(class181_0);
			}

			// Token: 0x06000466 RID: 1126 RVA: 0x00003D85 File Offset: 0x00001F85
			public Class179.Class182 method_4(string string_0)
			{
				return this.dictionary_0[string_0];
			}

			// Token: 0x06000467 RID: 1127 RVA: 0x00003D93 File Offset: 0x00001F93
			public IEnumerable<Class179.Class181> method_5()
			{
				return this.list_0;
			}

			// Token: 0x0400052F RID: 1327
			private Dictionary<string, Class179.Class182> dictionary_0 = new Dictionary<string, Class179.Class182>(StringComparer.Ordinal);

			// Token: 0x04000530 RID: 1328
			private List<Class179.Class181> list_0 = new List<Class179.Class181>();

			// Token: 0x04000531 RID: 1329
			[CompilerGenerated]
			private bool bool_0;
		}

		// Token: 0x0200011F RID: 287
		internal class Class181
		{
			// Token: 0x06000469 RID: 1129 RVA: 0x00003D9B File Offset: 0x00001F9B
			[CompilerGenerated]
			internal string method_0()
			{
				return this.string_0;
			}

			// Token: 0x0600046A RID: 1130 RVA: 0x00003DA3 File Offset: 0x00001FA3
			[CompilerGenerated]
			internal void method_1(string string_1)
			{
				this.string_0 = string_1;
			}

			// Token: 0x0600046B RID: 1131 RVA: 0x00003DAC File Offset: 0x00001FAC
			[CompilerGenerated]
			internal int method_2()
			{
				return this.int_0;
			}

			// Token: 0x0600046C RID: 1132 RVA: 0x00003DB4 File Offset: 0x00001FB4
			[CompilerGenerated]
			internal void method_3(int int_2)
			{
				this.int_0 = int_2;
			}

			// Token: 0x0600046D RID: 1133 RVA: 0x00003DBD File Offset: 0x00001FBD
			[CompilerGenerated]
			internal int method_4()
			{
				return this.int_1;
			}

			// Token: 0x0600046E RID: 1134 RVA: 0x00003DC5 File Offset: 0x00001FC5
			[CompilerGenerated]
			internal void method_5(int int_2)
			{
				this.int_1 = int_2;
			}

			// Token: 0x04000532 RID: 1330
			[CompilerGenerated]
			private string string_0;

			// Token: 0x04000533 RID: 1331
			[CompilerGenerated]
			private int int_0;

			// Token: 0x04000534 RID: 1332
			[CompilerGenerated]
			private int int_1;
		}

		// Token: 0x02000120 RID: 288
		internal class Class182
		{
			// Token: 0x06000470 RID: 1136 RVA: 0x00003DCE File Offset: 0x00001FCE
			[CompilerGenerated]
			internal Class179.Class181 method_0()
			{
				return this.class181_0;
			}

			// Token: 0x06000471 RID: 1137 RVA: 0x00003DD6 File Offset: 0x00001FD6
			[CompilerGenerated]
			internal void method_1(Class179.Class181 class181_1)
			{
				this.class181_0 = class181_1;
			}

			// Token: 0x06000472 RID: 1138 RVA: 0x00003DDF File Offset: 0x00001FDF
			[CompilerGenerated]
			internal string method_2()
			{
				return this.string_0;
			}

			// Token: 0x06000473 RID: 1139 RVA: 0x00003DE7 File Offset: 0x00001FE7
			[CompilerGenerated]
			internal void method_3(string string_1)
			{
				this.string_0 = string_1;
			}

			// Token: 0x06000474 RID: 1140 RVA: 0x00003DF0 File Offset: 0x00001FF0
			[CompilerGenerated]
			internal int method_4()
			{
				return this.int_0;
			}

			// Token: 0x06000475 RID: 1141 RVA: 0x00003DF8 File Offset: 0x00001FF8
			[CompilerGenerated]
			internal void method_5(int int_4)
			{
				this.int_0 = int_4;
			}

			// Token: 0x06000476 RID: 1142 RVA: 0x00003E01 File Offset: 0x00002001
			[CompilerGenerated]
			internal int method_6()
			{
				return this.int_1;
			}

			// Token: 0x06000477 RID: 1143 RVA: 0x00003E09 File Offset: 0x00002009
			[CompilerGenerated]
			internal void method_7(int int_4)
			{
				this.int_1 = int_4;
			}

			// Token: 0x06000478 RID: 1144 RVA: 0x00003E12 File Offset: 0x00002012
			[CompilerGenerated]
			internal int method_8()
			{
				return this.int_2;
			}

			// Token: 0x06000479 RID: 1145 RVA: 0x00003E1A File Offset: 0x0000201A
			[CompilerGenerated]
			internal void method_9(int int_4)
			{
				this.int_2 = int_4;
			}

			// Token: 0x0600047A RID: 1146 RVA: 0x00003E23 File Offset: 0x00002023
			[CompilerGenerated]
			internal int method_10()
			{
				return this.int_3;
			}

			// Token: 0x0600047B RID: 1147 RVA: 0x00003E2B File Offset: 0x0000202B
			[CompilerGenerated]
			internal void method_11(int int_4)
			{
				this.int_3 = int_4;
			}

			// Token: 0x0600047C RID: 1148 RVA: 0x00003E34 File Offset: 0x00002034
			[CompilerGenerated]
			internal float method_12()
			{
				return this.float_0;
			}

			// Token: 0x0600047D RID: 1149 RVA: 0x00003E3C File Offset: 0x0000203C
			[CompilerGenerated]
			internal void method_13(float float_6)
			{
				this.float_0 = float_6;
			}

			// Token: 0x0600047E RID: 1150 RVA: 0x00003E45 File Offset: 0x00002045
			[CompilerGenerated]
			internal float method_14()
			{
				return this.float_1;
			}

			// Token: 0x0600047F RID: 1151 RVA: 0x00003E4D File Offset: 0x0000204D
			[CompilerGenerated]
			internal void method_15(float float_6)
			{
				this.float_1 = float_6;
			}

			// Token: 0x06000480 RID: 1152 RVA: 0x00003E56 File Offset: 0x00002056
			[CompilerGenerated]
			internal float method_16()
			{
				return this.float_2;
			}

			// Token: 0x06000481 RID: 1153 RVA: 0x00003E5E File Offset: 0x0000205E
			[CompilerGenerated]
			internal void method_17(float float_6)
			{
				this.float_2 = float_6;
			}

			// Token: 0x06000482 RID: 1154 RVA: 0x00003E67 File Offset: 0x00002067
			[CompilerGenerated]
			internal float method_18()
			{
				return this.float_3;
			}

			// Token: 0x06000483 RID: 1155 RVA: 0x00003E6F File Offset: 0x0000206F
			[CompilerGenerated]
			internal void method_19(float float_6)
			{
				this.float_3 = float_6;
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x00003E78 File Offset: 0x00002078
			[CompilerGenerated]
			internal float method_20()
			{
				return this.float_4;
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x00003E80 File Offset: 0x00002080
			[CompilerGenerated]
			internal void method_21(float float_6)
			{
				this.float_4 = float_6;
			}

			// Token: 0x06000486 RID: 1158 RVA: 0x00003E89 File Offset: 0x00002089
			[CompilerGenerated]
			internal float method_22()
			{
				return this.float_5;
			}

			// Token: 0x06000487 RID: 1159 RVA: 0x00003E91 File Offset: 0x00002091
			[CompilerGenerated]
			internal void method_23(float float_6)
			{
				this.float_5 = float_6;
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00003E9A File Offset: 0x0000209A
			[CompilerGenerated]
			internal bool method_24()
			{
				return this.bool_0;
			}

			// Token: 0x06000489 RID: 1161 RVA: 0x00003EA2 File Offset: 0x000020A2
			[CompilerGenerated]
			internal void method_25(bool bool_1)
			{
				this.bool_0 = bool_1;
			}

			// Token: 0x04000535 RID: 1333
			[CompilerGenerated]
			private Class179.Class181 class181_0;

			// Token: 0x04000536 RID: 1334
			[CompilerGenerated]
			private string string_0;

			// Token: 0x04000537 RID: 1335
			[CompilerGenerated]
			private int int_0;

			// Token: 0x04000538 RID: 1336
			[CompilerGenerated]
			private int int_1;

			// Token: 0x04000539 RID: 1337
			[CompilerGenerated]
			private int int_2;

			// Token: 0x0400053A RID: 1338
			[CompilerGenerated]
			private int int_3;

			// Token: 0x0400053B RID: 1339
			[CompilerGenerated]
			private float float_0;

			// Token: 0x0400053C RID: 1340
			[CompilerGenerated]
			private float float_1;

			// Token: 0x0400053D RID: 1341
			[CompilerGenerated]
			private float float_2;

			// Token: 0x0400053E RID: 1342
			[CompilerGenerated]
			private float float_3;

			// Token: 0x0400053F RID: 1343
			[CompilerGenerated]
			private float float_4;

			// Token: 0x04000540 RID: 1344
			[CompilerGenerated]
			private float float_5;

			// Token: 0x04000541 RID: 1345
			[CompilerGenerated]
			private bool bool_0;
		}
	}
}
