using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GStd;
using GStd.Asset;

public class TestPool : MonoBehaviour {

	public Object origin;
	public string assetBundleName;
	public string assetName;
	public float spawnSpeed = 1;
	public float despawnSpeed = 3;

	/// <summary>
	/// Called when the script is loaded or a value is changed in the
	/// inspector (Called in the editor only).
	/// </summary>
	void OnValidate()
	{
		if (origin != null && (string.IsNullOrEmpty(assetBundleName) || string.IsNullOrEmpty(assetName)))
		{
			var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(origin));
			assetBundleName = importer.assetBundleName;
			assetName = origin.name;
		}
		else if (origin == null && (!string.IsNullOrEmpty(assetBundleName) || !string.IsNullOrEmpty(assetName)))
		{
			var assets = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			// Debug.Log("!! assets " + assets.Length);
			// foreach(var asset in assets)
			// {
			// 	Debug.Log("!! asset = " + asset);
			// }
			this.origin = AssetDatabase.LoadAssetAtPath<Object>(assets[0]);
		}

	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		AssetManager.SetupSimulateLoader();
	}

	// Use this for initialization
	void Start () {
		this.StartCoroutine(Spawner());
		this.StartCoroutine(Despawner());
	}

	List<Object> insts = new List<Object>();

	IEnumerator Spawner()
	{
		while(true)
		{
			yield return new WaitForSeconds(this.spawnSpeed);

			var inst = AssetManager.SpawnGameObject(this.assetBundleName, this.assetName);	
			inst.transform.position = new Vector3(Random.Range(-10,10), 0, Random.Range(-10,10));
			this.insts.Add(inst);	
		}
	}

	IEnumerator Despawner()
	{
		while(true)
		{
			yield return new WaitForSeconds(this.despawnSpeed);
			if (this.insts.Count > 0)
			{
				AssetManager.DespawnGameObject(this.insts[0] as GameObject);
				this.insts.RemoveAt(0);
			}
		}
	}
}
