namespace GStd
{
    using System;
    using UnityEngine;

    public abstract class WaitLoadObject : WaitLoadAsset
    {
        protected WaitLoadObject()
        {
        }

        public abstract UnityEngine.Object GetObject();
        public T GetObject<T>() where T: UnityEngine.Object
        {
            return (this.GetObject() as T);
        }
    }
}

