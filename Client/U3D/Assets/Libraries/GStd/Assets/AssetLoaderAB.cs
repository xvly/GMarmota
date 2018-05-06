using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GStd.Asset
{
	public class AssetLoaderAB : AssetLoader {
		private string localDirectory;
		private AssetBundleManifest manifest;
		
		public AssetLoaderAB()
		{
			this.LoadManifest();
		}

		public void UpdateAssetBundle()
		{

		}

		private void LoadManifest()
		{
#if UNITY_EDITOR
			this.localDirectory = Application.dataPath + "/../AssetBundle/PC/AssetBundle";
#else
			this.localDirectory = Application.streamingAssetsPath;
#endif

			string localManifestPath = Path.Combine(this.localDirectory, "AssetBundle");
			var assetBundle = AssetBundle.LoadFromFile(localManifestPath);
			this.manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		}

		public override T LoadAsset<T>(string assetBundleName, string assetName) 
		{
			var fullAssetBundlePath = Path.Combine(this.localDirectory, assetBundleName);
			var ab = AssetBundle.LoadFromFile(fullAssetBundlePath);
			return ab.LoadAsset<T>(assetName);
		}		

		public override Object LoadAsset(string assetBundleName, string assetName, System.Type type) 
		{
			var fullAssetBundlePath = Path.Combine(this.localDirectory, assetBundleName);
			var ab = AssetBundle.LoadFromFile(fullAssetBundlePath);
			return ab.LoadAsset(assetName, type);
		}	
	}
}
