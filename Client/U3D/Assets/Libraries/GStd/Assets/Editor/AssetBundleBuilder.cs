using UnityEngine;
using UnityEditor;

namespace GStd.Editor.Asset
{
    public static class AssetBundleBuilder
    {
		private static bool CheckSwitchPlatform(BuildTarget target)
		{
			#if UNITY_ANDROID
			if (target != BuildTarget.Android)
				return true;
			#elif UNITY_IOS
			if (target != BuildTarget.iOS)
				return true;
			#else
			if (target != BuildTarget.StandaloneWindows && target != BuildTarget.StandaloneWindows64)
				return true; 
			#endif

			return false;
		}

		[MenuItem("GStd/AssetBundle/Build/PC", false, 2)]
		private static void BuildPC()
		{
			if (CheckSwitchPlatform(BuildTarget.StandaloneWindows) && !EditorUtility.DisplayDialog("提示", "将触发切换平台", "确定", "取消"))
				return;

            string outputPath = Application.dataPath + "/../AssetBundle/PC/AssetBundle";
			if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
			BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
		}

        [MenuItem("GStd/AssetBundle/Build/Android", false, 3)]
        private static void BuildAndroid()
        {
            if (CheckSwitchPlatform(BuildTarget.Android) && !EditorUtility.DisplayDialog("提示", "将触发切换平台", "确定", "取消"))
                return;

            string outputPath = Application.dataPath + "/../AssetBundle/Android/AssetBundle";
            if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.Android);
        }

        [MenuItem("GStd/AssetBundle/Build/iOS", false, 4)]
        private static void BuildiOS()
        {
            if (CheckSwitchPlatform(BuildTarget.iOS) && !EditorUtility.DisplayDialog("提示", "将触发切换平台", "确定", "取消"))
                return;

            string outputPath = Application.dataPath + "/../AssetBundle/iOS/AssetBundle";
            if (!System.IO.Directory.Exists(outputPath))
                System.IO.Directory.CreateDirectory(outputPath);
            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.iOS);
        }
    }
}