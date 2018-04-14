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
	}

}
