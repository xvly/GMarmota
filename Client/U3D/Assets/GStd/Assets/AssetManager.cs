using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public static class AssetManager
	{
		private static IAssetLoader assetLoader;

		public static void SetupSimulateLoader()
		{
			assetLoader = new AssetLoaderSimulate();
		}

		public static void SetupABLoader()
		{
			assetLoader = new AssetLoaderAB();
		}

		public static T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			return assetLoader.LoadObject<T>(assetBundleName, assetName);
		}

		public static bool IsAssetBundleCache(string assetBundleName)
		{
			return false;	
		}
	}
}

