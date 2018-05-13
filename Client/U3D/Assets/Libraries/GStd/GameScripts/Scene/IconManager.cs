#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GStd.Editor
{
	// Token: 0x020000E9 RID: 233
	public static class IconManager
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x0001FF60 File Offset: 0x0001E160
		public static void SetIcon(GameObject obj, IconManager.LabelIcon icon)
		{
			if (IconManager.guicontent_0 == null)
			{
				IconManager.guicontent_0 = IconManager.smethod_1("sv_label_", string.Empty, 0, 8);
			}
			if (icon == IconManager.LabelIcon.None)
			{
				IconManager.RemoveIcon(obj);
			}
			else
			{
				IconManager.smethod_0(obj, IconManager.guicontent_0[(int)icon].image as Texture2D);
			}
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x0001FFB0 File Offset: 0x0001E1B0
		public static void SetIcon(GameObject obj, IconManager.Icon icon)
		{
			if (IconManager.guicontent_1 == null)
			{
				IconManager.guicontent_1 = IconManager.smethod_1("sv_icon_dot", "_pix16_gizmo", 0, 16);
			}
			if (icon == IconManager.Icon.None)
			{
				IconManager.RemoveIcon(obj);
			}
			else
			{
				IconManager.smethod_0(obj, IconManager.guicontent_1[(int)icon].image as Texture2D);
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00003B04 File Offset: 0x00001D04
		public static void RemoveIcon(GameObject obj)
		{
			IconManager.smethod_0(obj, null);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00020000 File Offset: 0x0001E200
		private static void smethod_0(GameObject gameObject_0, Texture2D texture2D_0)
		{
			Type typeFromHandle = typeof(EditorGUIUtility);
			MethodInfo method = typeFromHandle.GetMethod("SetIconForObject", BindingFlags.Static | BindingFlags.NonPublic);
			method.Invoke(null, new object[]
			{
				gameObject_0,
				texture2D_0
			});
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0002003C File Offset: 0x0001E23C
		private static GUIContent[] smethod_1(string string_0, string string_1, int int_0, int int_1)
		{
			GUIContent[] array = new GUIContent[int_1];
			for (int i = 0; i < int_1; i++)
			{
				string text = string_0 + (int_0 + i) + string_1;
				array[i] = EditorGUIUtility.IconContent(text);
			}
			return array;
		}

		// Token: 0x04000532 RID: 1330
		private static GUIContent[] guicontent_0;

		// Token: 0x04000533 RID: 1331
		private static GUIContent[] guicontent_1;

		// Token: 0x020000EA RID: 234
		public enum LabelIcon
		{
			// Token: 0x04000535 RID: 1333
			None = -1,
			// Token: 0x04000536 RID: 1334
			Gray,
			// Token: 0x04000537 RID: 1335
			Blue,
			// Token: 0x04000538 RID: 1336
			Teal,
			// Token: 0x04000539 RID: 1337
			Green,
			// Token: 0x0400053A RID: 1338
			Yellow,
			// Token: 0x0400053B RID: 1339
			Orange,
			// Token: 0x0400053C RID: 1340
			Red,
			// Token: 0x0400053D RID: 1341
			Purple
		}

		// Token: 0x020000EB RID: 235
		public enum Icon
		{
			// Token: 0x0400053F RID: 1343
			None = -1,
			// Token: 0x04000540 RID: 1344
			CircleGray,
			// Token: 0x04000541 RID: 1345
			CircleBlue,
			// Token: 0x04000542 RID: 1346
			CircleTeal,
			// Token: 0x04000543 RID: 1347
			CircleGreen,
			// Token: 0x04000544 RID: 1348
			CircleYellow,
			// Token: 0x04000545 RID: 1349
			CircleOrange,
			// Token: 0x04000546 RID: 1350
			CircleRed,
			// Token: 0x04000547 RID: 1351
			CirclePurple,
			// Token: 0x04000548 RID: 1352
			DiamondGray,
			// Token: 0x04000549 RID: 1353
			DiamondBlue,
			// Token: 0x0400054A RID: 1354
			DiamondTeal,
			// Token: 0x0400054B RID: 1355
			DiamondGreen,
			// Token: 0x0400054C RID: 1356
			DiamondYellow,
			// Token: 0x0400054D RID: 1357
			DiamondOrange,
			// Token: 0x0400054E RID: 1358
			DiamondRed,
			// Token: 0x0400054F RID: 1359
			DiamondPurple
		}
	}
}

#endif