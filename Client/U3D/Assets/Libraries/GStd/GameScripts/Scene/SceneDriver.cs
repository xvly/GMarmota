using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneDriver : MonoBehaviour {

	[SerializeField]
	private GameObject prefab;

	[SerializeField]
	private LayerMask walkableLayer;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float rotDuration = 0.5f;

	private GameObject inst;
    private Animator instAnimator;

	// Use this for initialization
    void Awake()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, 1000, walkableLayer))
        {
            this.transform.position = hit.point;
        }
    }

	void Start () {
		inst = GameObject.Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform);

        this.instAnimator = inst.GetComponent<Animator>();
	}

    private Camera mainCamera;

    private Vector3? destination;

    private void HandleInput()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (Input.GetMouseButtonDown(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            var sceneHits = Physics.RaycastAll(
                ray, Mathf.Infinity, walkableLayer);
            if (sceneHits.Length > 0)
            {
                var hit = sceneHits[0];
                destination = hit.point;

                this.transform.DOLookAt(
                    new Vector3(this.destination.Value.x, this.transform.position.y, this.destination.Value.z), 
                    this.rotDuration);
            }
        }
        else
        {
            var foward = mainCamera.transform.forward;    
            foward = new Vector3(foward.x, 0, foward.z);

            var right = mainCamera.transform.right;
            right = new Vector3(right.x, 0, right.z);

            Vector3 dir = Vector3.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                dir = dir + foward;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                dir = dir - foward;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                dir = dir - right;   
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                dir = dir + right;   
            }

            if (dir != Vector3.zero)
            {
                dir = dir.normalized;
                var checkPos = this.transform.position + dir;
                RaycastHit hit;
                if (Physics.Raycast(checkPos + new Vector3(0,10,0), Vector3.down, out hit, 100, walkableLayer))
                    destination = hit.point;

                this.transform.DOLookAt(
                    new Vector3(this.destination.Value.x, this.transform.position.y, this.destination.Value.z), 
                    0.5f);
            }
        }
    }

    void Translate()
    {
        if (this.destination == null || this.destination.Value == this.transform.position)
            return;

        this.instAnimator.SetInteger("status", 1);

        // Debug.Log("!! do lookat " + this.destination.Value);

        // this.transform.DOLookAt(
        // new Vector3(this.destination.Value.x, this.transform.position.y, this.destination.Value.z), 
        // this.rotDuration);

        // move
        var offset = this.destination.Value - this.transform.position;
        var dir = offset.normalized;
        var step = dir * this.moveSpeed * Time.deltaTime;
        if (step.magnitude > offset.magnitude)
        {
            this.transform.position = this.destination.Value;
            this.destination = null;
            this.instAnimator.SetInteger("status", 0);
        }
        else
        {
            this.transform.position += step;
        }
    }

	// Update is called once per frame
	void Update () {
        this.HandleInput();
        this.Translate();
	}

	
}
