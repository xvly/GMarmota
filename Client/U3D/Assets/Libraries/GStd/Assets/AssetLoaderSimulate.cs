#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GStd.Asset{
	public class AssetLoaderSimulate : AssetLoader {
		public override T LoadAsset<T>(string assetBundleName, string assetName)
		{
			var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			if (assetPaths.Length == 0)
			{
				Debug.LogError(string.Format("simulate assetBundle:\"{0}\" asset:\"{1}\" failed", assetBundleName, assetName));
				return null;
			}
			else
				return AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
		}

		public override Object LoadAsset(string assetBundleName, string assetName, System.Type type)
		{
			var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			if (assetPaths.Length == 0)
			{
				Debug.LogError(string.Format("simulate assetBundle:{0} asset:{1} failed", assetBundleName, assetName));
				return null;
			}
			else
				return AssetDatabase.LoadAssetAtPath(assetPaths[0], type);
		}

		public override IEnumerator LoadLevel(string assetBundleName, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadMode, System.Action complete, System.Action<float> progressCallback)
		{
			var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, sceneName);
			if (assetPaths.Length == 0)
			{
				Debug.LogError(string.Format("simulate assetBundle:\"{0}\" asset:\"{1}\" failed", assetBundleName, sceneName));
				yield break;
			}

			var assetPath = assetPaths[0];
			var asyncOperation = EditorApplication.LoadLevelAsyncInPlayMode(assetPath);
			while(asyncOperation.isDone)
			{
				if (progressCallback != null)
					progressCallback(asyncOperation.progress);

				yield return null;
			}

			if (complete != null)
				complete();
		}
	}
}

#endif