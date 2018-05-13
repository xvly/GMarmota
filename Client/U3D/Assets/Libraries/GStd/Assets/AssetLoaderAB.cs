using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

		public override IEnumerator LoadLevel(string assetBundleName, string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadMode, System.Action complete, System.Action<float> progress)
		{
			Debug.Log("!! load level 1" + assetBundleName + "," + sceneName + "," + loadMode);
			yield return new WaitForSeconds(1);

			Debug.Log("!! load level 2" );
		}
	}
}
