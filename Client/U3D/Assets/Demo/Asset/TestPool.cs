using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GStd;
using GStd.Asset;

[ExecuteInEditMode]
public class TestPool : MonoBehaviour {

	public GameObject prefab;
	public string assetBundleName;
	public string assetName;
	public float spawnSpeed = 1;
	public float despawnSpeed = 3;

	public Transform root;

	#if UNITY_EDITOR

	/// <summary>
	/// Called when the script is loaded or a value is changed in the
	/// inspector (Called in the editor only).
	/// </summary>
	void OnValidate()
	{
		if (prefab != null)
		{
			var importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(prefab));
			assetBundleName = importer.assetBundleName;
			assetName = prefab.name;
		}
		else if (!string.IsNullOrEmpty(assetBundleName) || !string.IsNullOrEmpty(assetName))
		{
			var assets = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
			this.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assets[0]);
		}
	}
	#endif

	public GUIStyle style;

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(0,0, Screen.width/3, Screen.height), style);
		GUILayout.Label("spawn:" + this.spawnSpeed, style);
		this.spawnSpeed = GUILayout.HorizontalSlider(this.spawnSpeed, 0.01f, 5);

		GUILayout.Label("dspawn:" + this.despawnSpeed, style);
		this.despawnSpeed = GUILayout.HorizontalSlider(this.despawnSpeed, 0.01f, 5);
		
		GUILayout.Label("inst count:" + this.insts.Count, style);

		GUILayout.EndArea();
	}

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		if (!Application.isPlaying)
			return;

		AssetManager.Setup();
	}

	// Use this for initialization
	void Start () {
		if (!Application.isPlaying)
			return;
		this.StartCoroutine(Spawner());
		this.StartCoroutine(Despawner());
	}

	List<GameObject> insts = new List<GameObject>();

	IEnumerator Spawner()
	{
		while(true)
		{
			yield return new WaitForSeconds(this.spawnSpeed);

			//var inst = AssetManager.Spawn(this.assetBundleName, this.assetName);	
			var inst = AssetManager.Spawn(this.prefab, Vector3.zero, Quaternion.identity, root);	
			inst.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

			if (!this.insts.Exists((x) => {return x == inst;}))
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
				AssetManager.Despawn(this.insts[0]);
				this.insts.RemoveAt(0);
			}
		}
	}

	Vector3 offset = new Vector3(0, -1, 0);

	void Update()
	{
		var deltaTime = Time.deltaTime;
		foreach(var inst in this.insts)
		{
			inst.transform.position += offset * deltaTime;
		}
	}
}
