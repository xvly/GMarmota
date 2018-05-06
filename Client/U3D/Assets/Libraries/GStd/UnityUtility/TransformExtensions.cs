using System;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
namespace GStd
{
    public static class TransformExtensions
    {
        public static Transform FindRecursively(this Transform transform, string name)
        {
            for (int i=0; i<transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {
                    return child;
                }

                var ret = child.FindRecursively(name);
                if (ret != null)
                    return ret;
            }

            return null;
        }

        public static Transform[] FindAllRecursively(this Transform transform, string name)
        {
            List<Transform> ret = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name == name)
                {
                    ret.Add(child);
                }

                ret.AddRange(child.FindAllRecursively(name));
            }

            return ret.ToArray();
        }

        public static string RootPath(this Transform transform)
        {
            string path = transform.name;
            var parent = transform.parent;
            while(parent != null)
            {
                path = parent + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }
}
