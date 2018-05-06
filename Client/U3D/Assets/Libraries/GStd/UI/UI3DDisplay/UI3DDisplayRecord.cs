using UnityEngine;

namespace GStd {
    public sealed class UI3DDisplayRecord : MonoBehaviour
    {
        private int layer;
        private bool visible;
        private Renderer attachRenderer;
        private UI3DDisplayCamera displayCamera;

        internal void Initialize(Renderer renderer, UI3DDisplayCamera camera)
        {
            this.attachRenderer = renderer;
            this.displayCamera = camera;
            this.layer = renderer.gameObject.layer;
            //this.visible = renderer.enabled;
        }

        private static bool IsParentOf(Transform obj, Transform parent)
        {
            if (obj == parent)
            {
                return true;
            }

            if (obj.parent == null)
            {
                return false;
            }

            return IsParentOf(obj.parent, parent);
        }

        private void OnTransformParentChanged()
        {
            if (this.displayCamera == null || 
                !IsParentOf(this.transform, this.displayCamera.transform))
            {
                GameObject.Destroy(this);
            }
        }

        private void OnDestroy()
        {
            //if (this.attachRenderer != null)
            //{
            //    this.attachRenderer.enabled = this.visible;
            //}
            this.gameObject.layer = this.layer;
        }
    }
}
