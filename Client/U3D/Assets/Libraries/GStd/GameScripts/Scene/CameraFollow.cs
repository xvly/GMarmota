using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using GStd.Editor;
#endif

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour {

	[SerializeField]
	private Transform target;

	[SerializeField]
	private float followSpeed;

	[SerializeField]
	private Vector3 offset;
	[SerializeField]
	private Vector3 lookAtOffset;

	[SerializeField]
	private AnimationCurve elastic;

	private void FollowPosition(Vector3 position, bool isImmediate=false)
	{
		var targetPosition = position + this.offset;

		if (this.transform.position == targetPosition)
			return;

		if (isImmediate)
		{
			this.transform.position = targetPosition;
		}
		else
		{
			var offset = targetPosition - this.transform.position;
			var distance = offset.magnitude;
			if (distance < 0.01f)
			{
				this.transform.position = targetPosition;
			}
			else
			{
				var step = this.elastic.Evaluate(distance) * distance * (this.followSpeed * Time.deltaTime);
				if (step > distance)
					this.transform.position = targetPosition;
				else
					this.transform.position += step * offset;
			}
		}

		this.transform.LookAt(position + this.lookAtOffset);
	}

	private void LateUpdate()
	{
		if (this.target == null)
			return;

#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			this.offset = this.transform.position - this.target.position;
			this.lookAtOffset = this.lookAtPoint.transform.position - this.target.position;
			this.transform.LookAt(this.lookAtPoint.transform);
			return;
		}
#endif

		this.FollowPosition(this.target.transform.position);
	}

	#if UNITY_EDITOR

	private GameObject lookAtPoint;
	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	void OnEnable()
	{
		if (this.lookAtPoint == null)
		{
			this.lookAtPoint = new GameObject("look at point");
			this.lookAtPoint.hideFlags = HideFlags.HideInInspector | HideFlags.DontSave;
			if (this.target != null)
				this.lookAtPoint.transform.position = this.target.transform.position;
			IconManager.SetIcon(this.lookAtPoint, IconManager.LabelIcon.Red);
		}
	}

	/// <summary>
	/// This function is called when the behaviour becomes disabled or inactive.
	/// </summary>
	void OnDisable()
	{
		if (this.lookAtPoint != null)
		{
			if (Application.isPlaying)
				GameObject.Destroy(this.lookAtPoint);
			else
				GameObject.DestroyImmediate(this.lookAtPoint);
			this.lookAtPoint = null;
		}
	}
	/// <summary>
	/// Reset is called when the user hits the Reset button in the Inspector's
	/// context menu or when adding the component the first time.
	/// </summary>
	void Reset()
	{
		// check target
		if (Application.isPlaying)
		{
			if (target == null)
			{
				Debug.LogWarning("not set target");
				return;
			}	
		}
		else
		{
			var driver = GameObject.FindObjectOfType<SceneDriver>();
			if (driver == null)
			{
				Debug.LogWarning("not find sceneDriver");
				return;
			}

			target = driver.transform;
		}
	}

	/// <summary>
	/// Called when the script is loaded or a value is changed in the
	/// inspector (Called in the editor only).
	/// </summary>
	void OnValidate()
	{
		if (this.target != null)
		{
			this.FollowPosition(this.target.position);

			if (this.lookAtPoint != null)
				this.lookAtPoint.transform.position = this.target.position + this.lookAtOffset;
		}
	}
	#endif
}
