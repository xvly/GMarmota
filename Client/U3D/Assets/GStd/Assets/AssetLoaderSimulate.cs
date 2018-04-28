#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GStd.Asset{
	public class AssetLoaderSimulate : AssetLoader {
		public override T LoadObject<T>(string assetBundleName, string assetName)
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

		public override Object LoadObject(string assetBundleName, string assetName, System.Type type)
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
	}
}

#endif