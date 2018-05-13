using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GStd{
	public static class Scheduler
	{
		public static void Delay(float time, System.Action callback)
		{
			CheckAndInit();
		}

		public static List<Action> frameListeners{
			get;
			private set;
		}

		public static void AddFrameListener(Action callback)
		{
			frameListeners.Add(callback);
		}

		public static Coroutine RunCoroutine(IEnumerator coroutine)
		{
			CheckAndInit();
			return _coroutine.StartCoroutine(coroutine);
		}

		private static GameObject inst;
		private static GCoroutineBehaviour _coroutine;

		private static void CheckAndInit()
		{
			if (_coroutine == null && Application.isPlaying)
			{
				var inst = new GameObject("GStd.Coroutine");
				_coroutine = inst.AddComponent<GCoroutineBehaviour>();
				UnityEngine.Object.DontDestroyOnLoad(inst);
			}
		}
	}

	public class GCoroutineBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			frameListeners = Scheduler.frameListeners;
		}

		public void RunCoroutine()
		{
		}

		private List<Action> frameListeners;

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		void Update()
		{
			if (this.frameListeners == null)
				return;

			foreach(var action in this.frameListeners)
			{
				action();
			}
		}
	}
}

