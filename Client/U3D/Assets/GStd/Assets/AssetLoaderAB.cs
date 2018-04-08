using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset
{
	public class AssetLoaderAB : IAssetLoader {
		public T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object
		{
			Debug.Log("!! AssetLoaderAB loadobject");
			return null;
		}		
	}

}
