using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd {
    public class Billboard3D : MonoBehaviour
    {
        public enum BILLBOARD_TYPE
        {
            BILLBOARD,
            VERTICAL,
            HORIZONTAL
        }

        [SerializeField]
        private Transform target;
        [SerializeField]
        private BILLBOARD_TYPE billboardType;

        [SerializeField]
        private bool IsSyncTranslate;
        [SerializeField]
        private Vector3 Offset;
        [SerializeField]
        private Vector3 Rotation;

        void OnEnable()
        {
            if (this.target == null)
            {
                Debug.LogWarning("[Billboard3D]not set target, auto set the MainCamera");

                this.target = Camera.main.transform;
                if (this.target == null)
                {
                    Debug.LogError("[Billboard3D]not set target, not found MainCamera neither");
                    this.enabled = false;
                    return;
                }
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Update();
        }
#endif

        void Update()
        {
            //this.transform.LookAt(this.target);
            this.transform.forward = -this.target.forward;
            switch (this.billboardType)
            {
                case BILLBOARD_TYPE.VERTICAL:
                    {
                        var tmp = this.transform.rotation.eulerAngles;
                        tmp.x = 0;
                        this.transform.rotation = Quaternion.Euler(tmp);
                    }
                    break;
                case BILLBOARD_TYPE.HORIZONTAL:
                    {
                        var tmp = this.transform.rotation.eulerAngles;
                        tmp.z = 0;
                        this.transform.rotation = Quaternion.Euler(tmp);
                    }
                    break;
            }

            if (IsSyncTranslate)
            {
                var position = this.target.position + 
                    this.target.forward * Offset.z + 
                    this.target.up * Offset.y + 
                    this.target.right * Offset.x;
                var rotation = this.transform.rotation * Quaternion.Euler(Rotation);
                this.transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
}

