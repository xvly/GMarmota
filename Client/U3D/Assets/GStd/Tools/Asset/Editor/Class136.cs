using System;
using System.Reflection;
using UnityEditor;

namespace ns1
{
	// Token: 0x020000F0 RID: 240
	internal static class Class136
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
		internal static string[] smethod_0(string string_0)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type type = assembly.GetType("UnityEditor.OSUtil");
			MethodInfo method = type.GetMethod("GetDefaultApps");
			return (string[])method.Invoke(null, new object[]
			{
				string_0
			});
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0001F10C File Offset: 0x0001D30C
		internal static string smethod_1(string string_0)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type type = assembly.GetType("UnityEditor.OSUtil");
			MethodInfo method = type.GetMethod("GetAppFriendlyName");
			return (string)method.Invoke(null, new object[]
			{
				string_0
			});
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001F158 File Offset: 0x0001D358
		internal static string smethod_2(string string_0)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type type = assembly.GetType("UnityEditor.OSUtil");
			MethodInfo method = type.GetMethod("GetDefaultAppPath");
			return (string)method.Invoke(null, new object[]
			{
				string_0
			});
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x0001F1A4 File Offset: 0x0001D3A4
		internal static string smethod_3()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(EditorWindow));
			Type type = assembly.GetType("UnityEditor.OSUtil");
			MethodInfo method = type.GetMethod("GetDefaultCachePath");
			return (string)method.Invoke(null, null);
		}
	}
}
