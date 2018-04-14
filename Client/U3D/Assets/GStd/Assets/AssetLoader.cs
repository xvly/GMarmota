using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public abstract class AssetLoader
	{
		public abstract T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object;
	}
}

