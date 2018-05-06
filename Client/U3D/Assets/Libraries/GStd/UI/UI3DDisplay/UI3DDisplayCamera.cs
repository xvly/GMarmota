using System.Collections.Generic;
//using GStd;
using UnityEngine;

namespace GStd {
    public class UI3DDisplayCamera : MonoBehaviour
    {
        private GameObject displayObject;
        private Dictionary<Transform, RendererItem> cache = 
            new Dictionary<Transform, RendererItem>();

        private static int _disableLayer =-1;
        private static int _enableLayer =-1;

        protected static int DisableLayer
        {
            get
            {
                if (_disableLayer == -1)
                    _disableLayer =LayerMask.NameToLayer("UI");
                return _disableLayer;
            }
        }

        protected static int EnableLayer
        {
            get{
                if (_enableLayer == -1)
                    _enableLayer = LayerMask.NameToLayer("UI3D");
                return _enableLayer;
            }
        }
        
        internal GameObject DisplayObject
        {
            get
            {
                return this.displayObject;
            }

            set
            {
                if (this.displayObject != null)
                {
                    ResetRenderer(this.DisplayObject.transform);
                }

                this.displayObject = value;
            }
        }
        internal int DisplayLayer { get; set; }

        private static void ResetRenderer(Transform transform)
        {
            var renderer = transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                var record = renderer.GetComponent<UI3DDisplayRecord>();
                if (record == null)
                {
                    GameObject.Destroy(record);
                }
            }

            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                ResetRenderer(child);
            }
        }

        private void UpdateCache(Transform transform)
        {
            RendererItem item;
            if (!this.cache.TryGetValue(transform, out item))
            {
                item = new RendererItem();
                var renderer = transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    var record = transform.GetComponent<UI3DDisplayRecord>();
                    record = renderer.gameObject.AddComponent<UI3DDisplayRecord>();
                    record.hideFlags = HideFlags.DontSave;
                    record.Initialize(renderer, this);
                    record.gameObject.layer = this.DisplayLayer;

                    item.Renderer = renderer;
                    item.Record = record;
                }

                this.cache.Add(transform, item);
            }

            item.IsExisted = true;
            for (int i = 0; i < transform.childCount; ++i)
            {
                var child = transform.GetChild(i);
                this.UpdateCache(child);
            }
        }

        private void OnPreCull()
        {
            if (this.DisplayObject != null)
            {
                foreach (var kv in this.cache)
                {
                    kv.Value.IsExisted = false;
                }

                this.UpdateCache(this.DisplayObject.transform);
                //this.cache.RemoveAll((k, v) => !v.IsExisted);

                foreach (var kv in this.cache)
                {
                    if (kv.Value.Renderer != null)
                    {
                        kv.Value.Renderer.gameObject.layer = EnableLayer;
                    }
                }
            }
        }

        private void OnPostRender()
        {
            if (this.DisplayObject != null)
            {
                foreach (var kv in this.cache)
                {
                    if (kv.Value.Renderer != null)
                    {
                        kv.Value.Renderer.gameObject.layer = DisableLayer;
                    }
                }
            }
        }

        private void OnDisable()
        {
            if (this.DisplayObject != null)
            {
                ResetRenderer(this.DisplayObject.transform);
            }
        }

        private class RendererItem
        {
            public bool IsExisted;
            public Renderer Renderer;
            public UI3DDisplayRecord Record;
        }
    }
}
