using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFastSort : MonoBehaviour {

	void Awake()
	{
		int count= 1000;
		int[] data = new int[count];
		for (int i=0; i<count; i++)
		{
			data[i] = Random.Range(0, count);
		}

		var nowTicks = System.DateTime.Now.Ticks;
		InsertSort(data);
		var newTicks = System.DateTime.Now.Ticks;
		Debug.Log("insert sort : " + (newTicks - nowTicks));

		nowTicks = newTicks;
		BubbleSort(data);
		newTicks = System.DateTime.Now.Ticks;
		Debug.Log("bobble sort : " + (newTicks - nowTicks));
	}

	void PrintDatas(int[] datas)
	{
		string text = "";
		foreach(var v in datas)
			text += v + " ";
		Debug.Log(text);
	}

	void InsertSort(int[] datas)
	{
		int[] tempDatas = new int[datas.Length];
		for (int i=0; i<datas.Length; i++)
		{
			int j=i-1;
			var iValue = datas[i];
			while(j>=0 && tempDatas[j] > iValue)
			{
				tempDatas[j+1] = tempDatas[j];
				j--;
			}

			tempDatas[j+1] = iValue;
		}
	}

	void BubbleSort(int[] datas)
	{
		for (int i=0; i<datas.Length; i++)
		{
			for (int j=1; j < datas.Length-i; j++)
			{
				if (datas[j-1] > datas[j])
				{
					var temp = datas[j-1];
					datas[j-1] = datas[j];
					datas[j] = temp;
				}
			}
		}
	}

	void _QuickSort(int[] datas)
	{

	}

	void QuickSort(int[] datas)
	{ 
		if (datas.Length == 0)
			return;

		var keyValue = datas[0];
		if (datas.Length%2 != 0)
		{
			int[] lDatas = new int[datas.Length/2];
			int[] rDatas = new int[datas.Length/2 + 1];
		}
		else
		{
			int[] lDatas = new int[datas.Length/2];
			int[] rDatas = new int[datas.Length/2];
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
