using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GStd
{
    [RequireComponent(typeof(Camera))]
    public class CameraLayerCullDistances : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private CameraLayerCullDistancesData[] datas;

        void ApplyLayer()
        {
            float[] distances = new float[32];
            foreach (var v in datas)
                distances[v.layer.ToInt()] = v.distance;
            this.camera.layerCullDistances = distances;
        }

        void OnEnable()
        {
            this.ApplyLayer();
        }

#if UNITY_EDITOR
        void Reset()
        {
            this.camera = GetComponent<Camera>();

            // default set
            CameraLayerCullDistancesData data = new CameraLayerCullDistancesData();
            data.layer = 1 << LayerMask.NameToLayer("SceneSmall");
            data.distance = 50f;
            this.datas = new CameraLayerCullDistancesData[] { data };

            this.ApplyLayer();
        }

        void OnValidate()
        {
            this.ApplyLayer();
        }
#endif
    }

    [Serializable]
    public struct CameraLayerCullDistancesData
    {
        public LayerMask layer;
        public float distance;
    }
}


