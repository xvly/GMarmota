using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public static class AssetManager
	{
		private static AssetLoader assetLoader;

		// setup
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

		// asset bundle
		public struct LoadItem
		{
			public string assetBundleName;
			public string assetName;
			public Object inst;
		}

		private static List<LoadItem> loaded = new List<LoadItem>();

		public static T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			foreach(var item in loaded)
			{
				if (item.assetBundleName == assetBundleName && item.assetName == assetName)
					return item.inst as T;
			}

			var ret = assetLoader.LoadObject<T>(assetBundleName, assetName);
			loaded.Add(new LoadItem(){assetBundleName=assetBundleName, assetName=assetName, inst=ret}); 

			return ret;
		}

		public static GameObject LoadPrefab(string assetBundleName, string assetName)
		{


			return assetLoader.LoadObject<GameObject>(assetBundleName, assetName);
		}

		public static bool IsAssetBundleCache(string assetBundleName)
		{
			return false;	
		}

		public static void UnloadAssetBundle(string assetBundleName)
		{
			
		}
 
		public static UnityEngine.Object LoadObject(string assetBundleName, string assetName, System.Type type)
		{
			return assetLoader.LoadObject(assetBundleName, assetName, type);
		}

		// pool
		private static GameObjectPool gameObjectPool = new GameObjectPool();

		public static GameObject SpawnGameObject(string assetBundleName, string assetName)
		{
			return gameObjectPool.Spawn(assetBundleName, assetName);
		}
		public static GameObject SpawnGameObject(string assetBundleName, string assetName, Vector3 position, Vector3 rotation, Transform parent)
		{
			return gameObjectPool.Spawn(assetBundleName, assetName);
		}

		public static GameObject SpawnGameObject(GameObject gameObject, Vector3 position, Vector3 rotation, Transform parent)
		{
			return null;
		}

		public static void DespawnGameObject(GameObject inst)
		{
			gameObjectPool.Despawn(inst);
		}

		// public static AudioClip Spawn(string assetBundleName, string assetName)
		// {
		// 	return null;
		// }

		public static void Despawn(UnityEngine.Object obj)
		{

		}

		public static void ClearPool()
		{

		}
	}
}

