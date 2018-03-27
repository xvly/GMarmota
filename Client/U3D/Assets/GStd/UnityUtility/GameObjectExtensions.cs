using System;
using UnityEngine;
using UnityEngine.Assertions;
namespace GStd
{
    public static class GameObjectExtensions
    {
        public static Component GetOrAddComponent(this GameObject obj, Type type)
        {
            Component component = obj.GetComponent(type);
            if (component == null)
            {
                component = obj.gameObject.AddComponent(type);
            }
            return component;
        }
        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            T t = obj.GetComponent<T>();
            if (t == null)
            {
                t = obj.gameObject.AddComponent<T>();
            }
            return t;
        }
        public static bool HasComponent(this GameObject obj, Type type)
        {
            return obj.GetComponent(type) != null;
        }
        public static bool HasComponent<T>(this GameObject obj) where T : Component
        {
            return obj.GetComponent<T>() != null;
        }
        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;
            foreach (Transform transform in obj.transform)
            {
                transform.gameObject.SetLayerRecursively(layer);
            }
        }
        public static T GetComponentInParentHard<T>(this GameObject obj) where T : Component
        {
            Assert.IsNotNull<GameObject>(obj);
            Transform transform = obj.transform;
            while (transform != null)
            {
                T component = transform.GetComponent<T>();
                if (component != null)
                {
                    return component;
                }
                transform = transform.parent;
            }
            return (T)((object)null);
        }

        public static void EnableAllRenderer(this GameObject obj, bool isEnable)
        {
            var renderers = obj.GetComponentsInChildren<Renderer>();
            foreach(var renderer in renderers)
            {
                renderer.enabled = isEnable;
            }
        }
    }
}
