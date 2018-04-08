﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GStd.Asset{
	public class AssetLoaderSimulate : IAssetLoader {
		public T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			var assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			if (assetPaths.Length == 0)
			{
				Debug.LogError(string.Format("simulate assetBundle:{0} asset:{1} failed", assetBundleName, assetName));
				return null;
			}
			else
				return AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
		}		
	}
}
