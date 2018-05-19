using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneDriver : MonoBehaviour {

	[SerializeField]
	private GameObject prefab;

    [SerializeField]
    private LayerMask walkableLayer;
    
    private MoveableObject moveableObject;
    private Camera mainCamera;

	void Start () {
		var inst = GameObject.Instantiate(prefab, this.transform.position, this.transform.rotation, this.transform);

        this.moveableObject = this.gameObject.GetComponent<MoveableObject>();
        if (this.moveableObject == null)
        {
            this.moveableObject = this.gameObject.AddComponent<MoveableObject>();
            this.moveableObject.walkableLayer = this.walkableLayer;    
        }
	}

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
                this.moveableObject.MoveTo(hit.point);
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
                    this.moveableObject.MoveTo(hit.point);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        this.HandleInput();
	}
}
