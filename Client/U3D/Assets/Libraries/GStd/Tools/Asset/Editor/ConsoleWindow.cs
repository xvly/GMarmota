using System;
using System.Reflection;
using UnityEditor;

namespace GStd.Editor
{
	// Token: 0x020000E1 RID: 225
	public sealed class ConsoleWindow : Singleton<ConsoleWindow>
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0001E054 File Offset: 0x0001C254
		public ConsoleWindow()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type type = assembly.GetType("UnityEditor.ConsoleWindow");
			FieldInfo field = type.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
			this.object_0 = field.GetValue(null);
			FieldInfo field2 = type.GetField("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
			this.object_1 = field2.GetValue(this.object_0);
			this.fieldInfo_0 = field2.FieldType.GetField("row", BindingFlags.Instance | BindingFlags.Public);
			Type type2 = assembly.GetType("UnityEditorInternal.LogEntries");
			this.methodInfo_0 = type2.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
			this.methodInfo_1 = type2.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
			Type type3 = assembly.GetType("UnityEditorInternal.LogEntry");
			this.object_2 = Activator.CreateInstance(type3);
			this.fieldInfo_1 = type3.GetField("condition", BindingFlags.Instance | BindingFlags.Public);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0001E13C File Offset: 0x0001C33C
		public string CurrentCondition
		{
			get
			{
				int num = (int)this.fieldInfo_0.GetValue(this.object_1);
				this.methodInfo_0.Invoke(null, new object[]
				{
					num,
					this.object_2
				});
				return this.fieldInfo_1.GetValue(this.object_2) as string;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000037CE File Offset: 0x000019CE
		public void Clear()
		{
			this.methodInfo_1.Invoke(null, null);
		}

		// Token: 0x040004D7 RID: 1239
		private object object_0;

		// Token: 0x040004D8 RID: 1240
		private object object_1;

		// Token: 0x040004D9 RID: 1241
		private FieldInfo fieldInfo_0;

		// Token: 0x040004DA RID: 1242
		private MethodInfo methodInfo_0;

		// Token: 0x040004DB RID: 1243
		private MethodInfo methodInfo_1;

		// Token: 0x040004DC RID: 1244
		private object object_2;

		// Token: 0x040004DD RID: 1245
		private FieldInfo fieldInfo_1;
	}
}
