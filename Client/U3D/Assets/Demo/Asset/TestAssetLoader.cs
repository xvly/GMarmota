using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GStd;
using GStd.Asset;

public class TestAssetLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		if (GUILayout.Button("random switch scene"))
		{
			this.StartCoroutine(RandomSwitchScene());
		}
	}

	IEnumerator RandomSwitchScene()
	{
		List<string[]> levels = new List<string[]>(){
			new string[]{"scenes/map/linshan01", "LinShan01_Main"},
			new string[]{"scenes/map/renducheng01", "RenDuCheng01_Main"},
			new string[]{"scenes/map/feishenjinjie", "feishenjinjie_Main"}
		};
		while(true)
		{
			var index = Random.Range(0,3);
			var level = levels[index];
			Debug.Log("!! random " + index);
			AssetManager.LoadLevel(level[0], level[1], UnityEngine.SceneManagement.LoadSceneMode.Single, null, null);
			yield return new WaitForSeconds(5);
		}
	}
}
