using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEditor;

namespace ns1
{
	// Token: 0x020000EB RID: 235
	internal static class Class132
	{
		// Token: 0x06000404 RID: 1028 RVA: 0x00003A70 File Offset: 0x00001C70
		internal static bool smethod_0()
		{
			return !string.IsNullOrEmpty(Class132.class131_0.method_0());
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00003A84 File Offset: 0x00001C84
		internal static void GUI()
		{
			Class132.class131_0.method_1();
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0001ED30 File Offset: 0x0001CF30
		internal static void smethod_2(string string_0, string string_1, Action<string> action_0)
		{
			Class132.Class133 @class = new Class132.Class133();
			@class.action_0 = action_0;
			string string_2 = Class132.class131_0.method_0();
			string string_3 = string_1 + string_0;
			Class132.smethod_3(string_2, string_3, new Action<string>(@class.method_0));
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0001ED70 File Offset: 0x0001CF70
		private static void smethod_3(string string_0, string string_1, Action<string> action_0)
		{
			Class132.Class134 @class = new Class132.Class134();
			@class.action_0 = action_0;
			using (Process process = Process.Start(new ProcessStartInfo(string_0, string_1)
			{
				CreateNoWindow = true,
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				StandardOutputEncoding = Encoding.UTF8,
				StandardErrorEncoding = Encoding.UTF8
			}))
			{
				process.OutputDataReceived += @class.method_0;
				process.ErrorDataReceived += @class.method_1;
				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();
				process.Close();
			}
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x0001EE24 File Offset: 0x0001D024
		[InitializeOnLoadMethod]
		private static void smethod_4()
		{
			string[] string_ = new string[]
			{
				"./Tools/luacheck.exe"
			};
			Class132.class131_0 = new Class131("Nirvana.External.LuaCheck", "LuaCheck", string_);
		}

		// Token: 0x04000519 RID: 1305
		private static Class131 class131_0;

		// Token: 0x020000EC RID: 236
		[CompilerGenerated]
		private sealed class Class133
		{
			// Token: 0x0600040A RID: 1034 RVA: 0x0001EE58 File Offset: 0x0001D058
			internal void method_0(string string_0)
			{
				if (string_0.StartsWith("Checking ") && string_0.EndsWith(" OK"))
				{
					return;
				}
				if (string_0.StartsWith("Checking ") && string_0.EndsWith(" warning"))
				{
					return;
				}
				if (string_0.StartsWith("Checking ") && string_0.EndsWith(" warnings"))
				{
					return;
				}
				if (string_0.StartsWith("Checking ") && string_0.EndsWith(" error"))
				{
					return;
				}
				if (string_0.StartsWith("Checking ") && string_0.EndsWith(" errors"))
				{
					return;
				}
				if (string_0.StartsWith("Total: ") && string_0.EndsWith(" file"))
				{
					return;
				}
				this.action_0(string_0);
			}

			// Token: 0x0400051A RID: 1306
			internal Action<string> action_0;
		}

		// Token: 0x020000ED RID: 237
		[CompilerGenerated]
		private sealed class Class134
		{
			// Token: 0x0600040C RID: 1036 RVA: 0x0001EF14 File Offset: 0x0001D114
			internal void method_0(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					string text = e.Data.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						this.action_0(text);
					}
				}
			}

			// Token: 0x0600040D RID: 1037 RVA: 0x0001EF14 File Offset: 0x0001D114
			internal void method_1(object sender, DataReceivedEventArgs e)
			{
				if (!string.IsNullOrEmpty(e.Data))
				{
					string text = e.Data.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						this.action_0(text);
					}
				}
			}

			// Token: 0x0400051B RID: 1307
			internal Action<string> action_0;
		}
	}
}
