using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd.Asset{
	public interface IAssetLoader
	{
		T LoadObject<T>(string assetBundleName, string assetName) where T:UnityEngine.Object;
	}
}

