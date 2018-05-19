using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableObject : MonoBehaviour {
	public LayerMask walkableLayer;
    public float moveSpeed = 5f;
    public float rotDuration = 0.5f;

    private Animator animator;
    private Vector3? destination;

	void Awake()
    {
        
		this.animator = this.GetComponentInChildren<Animator>(true);
        if (this.animator == null)
        {
            Debug.LogError("moveable script not find animator component");
            this.enabled = false;
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1000, walkableLayer))
        {
            this.transform.position = hit.point;
        }
    }

	public void MoveTo(Vector3 destination)
	{
		this.destination = destination;
		this.transform.DOLookAt(
			new Vector3(this.destination.Value.x, this.transform.position.y, this.destination.Value.z), 
			this.rotDuration);
	}

	private void Translate()
	{
		if (this.destination == null || this.destination.Value == this.transform.position)
            return;

        this.animator.SetInteger("status", 1);

        // move
        var offset = this.destination.Value - this.transform.position;
        var dir = offset.normalized;
        var step = dir * this.moveSpeed * Time.deltaTime;
        if (step.magnitude > offset.magnitude)
        {
            this.transform.position = this.destination.Value;
            this.destination = null;
            this.animator.SetInteger("status", 0);
        }
        else
        {
            this.transform.position += step;
        }
	}

	private void Update()
	{
		this.Translate();
	}

}
