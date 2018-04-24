using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset
{
	public class AssetLoaderAB : AssetLoader {
		public override T LoadObject<T>(string assetBundleName, string assetName) 
		{
			Debug.Log("!! AssetLoaderAB loadobject");
			return null;
		}		

		public override Object LoadObject(string assetBundleName, string assetName, System.Type type) 
		{
			Debug.Log("!! AssetLoaderAB loadobject");
			return null;
		}		
	}

}
