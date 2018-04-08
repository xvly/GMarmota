using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public class AssetLoaderSimulate : IAssetLoader {
		public T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			Debug.Log("!! call assetloader simulate");
			return null;
		}		
	}
}
