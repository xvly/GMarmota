using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public static class AssetManager
	{
		private static AssetLoader assetLoader;

		#if UNITY_EDITOR
		public static void SetupSimulateLoader()
		{
			assetLoader = new AssetLoaderSimulate();
		}
		#endif

		public static void SetupABLoader()
		{
			assetLoader = new AssetLoaderAB();
		}

		public static T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			return assetLoader.LoadObject<T>(assetBundleName, assetName);
		}

		public static UnityEngine.Object LoadObject(string assetBundleName, string assetName, System.Type type)
		{
			return assetLoader.LoadObject(assetBundleName, assetName, type);
		}

		public static bool IsAssetBundleCache(string assetBundleName)
		{
			return false;	
		}

		public static void UnloadAssetBundle(string assetBundleName)
		{

		}
	}
}

