using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GStd
{
    public class ClippingGroup : MonoBehaviour
    {
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private float range = 50;
        [SerializeField]
        private GameObject[] roots;

        [SerializeField]
        private Renderer[] renderers;

        [SerializeField]
        public Dictionary<Vector3, ClippingGroupData> groups = new Dictionary<Vector3, ClippingGroupData>();

        private Vector3 _lastPosition;

        void Awake()
        {
            _lastPosition = Vector3.zero;
        }

        void Update()
        {
            if (this.camera == null)
               // || this.groups.Count == 0)
                return;

            if (this._lastPosition != Vector3.zero
                && Vector3.Distance(this.camera.transform.position, this._lastPosition) < 3f)
                return;
            this._lastPosition = this.camera.transform.position;

            this.CheckAndEnable();
        }

        int count;
        void CheckAndEnable()
        {
            count = 0;

            //foreach (var kv in this.groups)
            //{
            //    var vPos = this.camera.WorldToViewportPoint(kv.Key);
            //    if (vPos.x > 0 && vPos.x < 1f
            //        && vPos.y > 0 && vPos.y < 1f
            //        && vPos.z > 0
            //        && Vector3.Distance(kv.Key, this._lastPosition) < this.range)
            //    {
            //        if (!kv.Value.isEnable)
            //            kv.Value.SetEnable(true);

            //        count++;
            //    }
            //    else
            //    {
            //        if (kv.Value.isEnable)
            //            this.groups[kv.Key].SetEnable(false);
            //    }
            //}

            foreach (var renderer in this.renderers)
            {
                var rdPos = renderer.transform.position;
                var vPos = this.camera.WorldToViewportPoint(rdPos);
                if (vPos.x > 0 && vPos.x < 1f
                    && vPos.y > 0 && vPos.y < 1f
                    && vPos.z > 0
                    && Vector3.Distance(rdPos, this._lastPosition) < this.range)
                {
                    renderer.enabled = true;
                    count++;
                }
                else
                {
                    renderer.enabled = false;
                }
            }
        }

        void OnGUI()
        {
            GUILayout.Label("clipping active " + this.groups.Count + "," + this.renderers.Length + "," + count);
        }

#if UNITY_EDITOR
        void Reset()
        {
            // default set
            if (this.camera == null && Camera.main != null)
                this.camera = Camera.main;

            if (this.roots == null || this.roots.Length == 0)
                this.roots = new GameObject[] { this.gameObject };

            if (this.renderers == null || this.renderers.Length == 0)
            {
                this.renderers = this.GetComponentsInChildren<Renderer>();
            }

            this.OnValidate();
        }

        private void OnValidate()
        {
            //bool isAnyRootChange = false;
            this.groups.Clear();

            // find renderers
            foreach (var root in this.roots)
            {
                var renderers = root.GetComponentsInChildren<Renderer>(); // need check active?

                foreach (var renderer in renderers)
                {
                    if (!renderer.enabled)
                        continue;

                    // 
                    var rdPosition = renderer.transform.position;
                    bool isAnyMatch = false;
                    foreach (var point in this.groups.Keys)
                    {
                        if (Vector3.Distance(rdPosition, point) < 1) // 
                        {
                            this.groups[point].renderers.Add(renderer);
                            isAnyMatch = true;
                            break;
                        }
                    }

                    if (!isAnyMatch)
                        this.groups.Add(rdPosition, new ClippingGroupData(true, new List<Renderer>() { renderer }));
                }
            }

            if (Application.isPlaying)
                CheckAndEnable();
        }
#endif
    }

    [Serializable]
    public class ClippingGroupData
    {
        public ClippingGroupData(bool isEnable, List<Renderer> renderers)
        {
            this.isEnable = isEnable;
            this.renderers = renderers;
        }

        [SerializeField]
        public bool isEnable;
        [SerializeField]
        public List<Renderer> renderers;

        public void SetEnable(bool isEnable)
        {
            this.isEnable = isEnable;
            foreach (var renderer in this.renderers)
            {
                renderer.enabled = isEnable;
            }
        }
    }
}
