using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GStd.Asset{
	public static class AssetManager
	{
		private static AssetLoader assetLoader;

		// setup
#if UNITY_EDITOR
		private const string PREF_SIMULATE = "absimulate";

        [MenuItem("GStd/AssetBundle/Simulate", false, 0)]
        private static void Simulate()
        {
            Menu.SetChecked("GStd/AssetBundle/Simulate", true);
            EditorPrefs.SetBool(PREF_SIMULATE, true);
        }

        [MenuItem("GStd/AssetBundle/Simulate", true, 0)]
        private static bool CheckSimulate()
        {
            Menu.SetChecked("GStd/AssetBundle/Simulate", EditorPrefs.GetBool(PREF_SIMULATE));
            return true;
        }

        [MenuItem("GStd/AssetBundle/No Simulate", false, 1)]
        private static void NoSimulate()
        {
            Menu.SetChecked("GStd/AssetBundle/Simulate", false);
            EditorPrefs.SetBool(PREF_SIMULATE, false);
        }

        [MenuItem("GStd/AssetBundle/No Simulate", true, 1)]
        private static bool CheckNoSimulate()
        {
            Menu.SetChecked("GStd/AssetBundle/No Simulate", !EditorPrefs.GetBool(PREF_SIMULATE));
            return true;
        }
		private static bool IsSimulateAssetBundle()
		{
			return EditorPrefs.GetBool(PREF_SIMULATE);
		}

		private static void SetupSimulateLoader()
		{
			assetLoader = new AssetLoaderSimulate();
		}
#endif

		public static void Setup()
		{
#if UNITY_EDITOR
				if (IsSimulateAssetBundle())
					SetupSimulateLoader();
				else
					SetupABLoader();
#else
				SetupABLoader();
#endif

            gameObjectPool = new GameObjectPool();
		}

		private static void SetupABLoader()
		{
			assetLoader = new AssetLoaderAB();
		}

		// asset bundle
		public struct LoadItem
		{
			public string assetBundleName;
			public string assetName;
			public Object inst;
		}

		private static List<LoadItem> loaded = new List<LoadItem>();

		public static T LoadAsset<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			foreach(var item in loaded)
			{
				if (item.assetBundleName == assetBundleName && item.assetName == assetName)
					return item.inst as T;
			}

			var ret = assetLoader.LoadAsset<T>(assetBundleName, assetName);
			loaded.Add(new LoadItem(){assetBundleName=assetBundleName, assetName=assetName, inst=ret}); 

			return ret;
		}

		public static UnityEngine.Object LoadAsset(string assetBundleName, string assetName, System.Type type)
		{
			return assetLoader.LoadAsset(assetBundleName, assetName, type);
		}

		public static void LoadLevel(string assetBundleName, string sceneName, LoadSceneMode loadMode, System.Action complete, System.Action<float> progress)
		{
			Scheduler.RunCoroutine(assetLoader.LoadLevel(assetBundleName, sceneName, loadMode, complete, progress));
		}

		public static bool IsAssetBundleCache(string assetBundleName)
		{
			return false;	
		}

		public static void UnloadAssetBundle(string assetBundleName)
		{
			
		}
	
		// pool
		private static GameObjectPool gameObjectPool;

		public static GameObject Spawn(string assetBundleName, string assetName)
		{
			return gameObjectPool.Spawn(assetBundleName, assetName);
		}
		public static GameObject Spawn(string assetBundleName, string assetName, Vector3 position, Vector3 rotation, Transform parent)
		{
			return gameObjectPool.Spawn(assetBundleName, assetName);
		}

		public static GameObject Spawn(GameObject prefab)
		{
			return gameObjectPool.Spawn(prefab, Vector3.zero, Quaternion.identity, null);
		}

		public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
		{
			return gameObjectPool.Spawn(prefab, position, rotation, parent);
		}

		public static void Despawn(GameObject inst)
		{
			gameObjectPool.Despawn(inst);
		}

		public static void Clear()
		{

		}
	}
}

