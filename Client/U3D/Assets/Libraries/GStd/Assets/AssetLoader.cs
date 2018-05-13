using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GStd.Asset{
	public abstract class AssetLoader
	{
		public abstract T LoadAsset<T>(string assetBundleName, string assetName) where T:UnityEngine.Object;
		public abstract Object LoadAsset(string assetBundleName, string assetName, System.Type type);
		public abstract IEnumerator LoadLevel(string assetBundleName, string sceneName, LoadSceneMode loadMode, System.Action complete, System.Action<float> progress);
	}
}

