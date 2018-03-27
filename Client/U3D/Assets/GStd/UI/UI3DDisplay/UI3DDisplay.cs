using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GStd {
    [AddComponentMenu("GStd/UI/Control/UI3D Display"), ExecuteInEditMode]
    public sealed class UI3DDisplay : MonoBehaviour, IEventSystemHandler, IDragHandler
    {
        [SerializeField, Tooltip("The display image.")]
        private RawImage displayImage;
        [SerializeField, Tooltip("The display camera.")]
        private Camera displayCamera;
        [SerializeField, Tooltip("The display resolution X.")]
        private int resolutionX = 512;
        [SerializeField, Tooltip("The display resolution y.")]
        private int resolutionY = 512;
        [SerializeField, Tooltip("The display layer.")]
        private LayerMask displayLayer = 1024;
        [SerializeField, Tooltip("The drag speed.")]
        private float dragSpeed = 10f;
        [SerializeField, Tooltip("The offset for the display object.")]
        private Vector3 displayOffset = Vector3.zero;
        [SerializeField, Tooltip("The rotation for the display object.")]
        private Vector3 displayRotation = Vector3.zero;
        [SerializeField, Tooltip("The scale for the display object.")]
        private Vector3 displayScale = Vector3.one;
        [SerializeField, Tooltip("this transform will auto fit scale.")]
        private Transform fitTransform;

        private float dragAngle;
        //private int oldLayer;
        private UI3DDisplayCamera ui3dCamera;
        private RenderTexture renderTexture;
        private GameObject targetObject;
        public float DragAngle
        {
            set
            {
                dragAngle = value;
            }
            get
            {
                return dragAngle;
            }
        }

        public void Display(GameObject displayObject, Camera lookCamera, int new_field_of_view)
        {
            this.targetObject = displayObject;

            if (this.enabled &&
                this.renderTexture == null)
            {
                this.renderTexture = RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 16);
                this.displayImage.texture = this.renderTexture;
                this.displayCamera.targetTexture = this.renderTexture;
            }
            
            this.displayCamera.cullingMask = this.displayLayer.value;
            this.ui3dCamera.DisplayLayer = this.displayLayer.ToInt();

            //this.oldLayer = this.targetObject.layer;
            //this.targetObject.SetLayerRecursively(this.displayLayer.ToInt());

            this.displayCamera.enabled = true;
            this.displayCamera.gameObject.SetActive(true);

            this.ui3dCamera.DisplayObject = this.targetObject;

            this.displayImage.enabled = true;
            Transform transform = this.targetObject.transform;
            if (this.fitTransform != null)
            {
                this.fitTransform.localScale = Vector3.one;
                transform.SetParent(this.fitTransform, true);
            }
            else
            {
                transform.SetParent(base.transform, true);
            }

            transform.SetPositionAndRotation(
                Vector3.zero, Quaternion.identity);
            transform.localScale = Vector3.one;

            this.displayCamera.transform.SetPositionAndRotation(
                lookCamera.transform.position, 
                lookCamera.transform.rotation);
            this.displayCamera.orthographic = lookCamera.orthographic;
            this.displayCamera.orthographicSize = lookCamera.orthographicSize;
            if (new_field_of_view > 0)
            {
                this.displayCamera.fieldOfView = new_field_of_view;
            }
            else
            {
                this.displayCamera.fieldOfView = lookCamera.fieldOfView;
            }
            this.displayCamera.nearClipPlane = lookCamera.nearClipPlane;
            this.displayCamera.farClipPlane = lookCamera.farClipPlane;
            this.displayCamera.orthographic = lookCamera.orthographic;
            this.displayCamera.orthographicSize = lookCamera.orthographicSize;

            transform.SetPositionAndRotation(
                this.displayOffset, 
                Quaternion.Euler(this.displayRotation) * Quaternion.AngleAxis(this.dragAngle, transform.up));
            transform.localScale = this.displayScale;

            if (this.fitTransform != null)
            {
                var lossyScale = this.fitTransform.lossyScale;
                this.fitTransform.localScale = new Vector3(
                    1f/lossyScale.x * this.transform.localScale.x, 
                    1f/lossyScale.y*this.transform.localScale.y, 
                    1f/lossyScale.z*this.transform.localScale.z);
            }
        }

        public void ClearDisplay()
        {
            this.ui3dCamera.DisplayObject = null;
            //this.targetObject.SetLayerRecursively(this.oldLayer); // TODO: 这样还原，如果子对象有不同的layer就会有问题
            this.targetObject = null;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.targetObject && this.dragSpeed > 0f)
	        {
                float x = eventData.delta.x;
                float num = -this.dragSpeed * x * Time.deltaTime;
                this.dragAngle += num;
                Transform transform = this.targetObject.transform;
                Vector3 up = transform.up;
                transform.rotation = Quaternion.Euler(this.displayRotation) * Quaternion.AngleAxis(this.dragAngle, up);
            }
        }

        private void Awake()
        {
            if (this.displayImage == null)
            {
                this.displayImage = base.GetComponent<RawImage>();
            }
            if (this.displayImage != null)
            {
                this.displayImage.enabled = false;
            }
            if (this.displayCamera != null)
            {
                this.displayCamera.enabled =false;
                this.ui3dCamera = this.displayCamera.GetOrAddComponent<UI3DDisplayCamera>();
            }
        }

        private void OnEnable()
        {
            if (this.targetObject != null &&
                this.renderTexture == null) 
            {
                this.renderTexture = RenderTexture.GetTemporary(this.resolutionX, this.resolutionY, 16);
                this.displayImage.texture = this.renderTexture;
                this.displayCamera.targetTexture = this.renderTexture;
            }
        }

        private void OnDisable()
        {
            if (this.renderTexture != null)
            {
                this.displayCamera.targetTexture = null;
                this.displayImage.texture = null;

                RenderTexture.ReleaseTemporary(this.renderTexture);
                this.renderTexture = null;
            }
        }

        private void OnDestroy()
        {
            if (this.renderTexture != null)
	        {
                this.displayCamera.targetTexture = null;
                this.displayImage.texture = null;

                RenderTexture.ReleaseTemporary(this.renderTexture);
                this.renderTexture = null;
            }
        }

    }

}


