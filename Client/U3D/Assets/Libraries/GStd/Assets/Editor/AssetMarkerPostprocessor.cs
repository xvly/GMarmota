using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GStd.Editor.Asset{
	
	class AssetMarkerPostprocessor : AssetPostprocessor
	{
		static bool IsIgnore(string path)
		{
			return path.EndsWith(".cs");
		}

		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (string str in importedAssets)
			{
				if (IsIgnore(str))
					continue;
				// Debug.Log("Reimported Asset: " + str + ",");
				AssetMarkerEditor.ProcessAddMarker(str);
				AssetMarkerEditor.ProcessAsset(str);
			}
			foreach (string str in deletedAssets)
			{
				if (IsIgnore(str))
					continue;
				// Debug.Log("Deleted Asset: " + str);
				AssetMarkerEditor.ProcessDelMarker(str);
			}

			for (int i = 0; i < movedAssets.Length; i++)
			{
				if (IsIgnore(movedFromAssetPaths[i]))
					continue;
				// Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
				AssetMarkerEditor.ProcessDelMarker(movedFromAssetPaths[i]);
				AssetMarkerEditor.ProcessAddMarker(movedAssets[i]);
				AssetMarkerEditor.ProcessAsset(movedAssets[i]);
			}
		}
	}
	
}

