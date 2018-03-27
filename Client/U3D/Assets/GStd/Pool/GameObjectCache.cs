namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngine.Assertions;

    public sealed class GameObjectCache
    {
        private bool bool_0;
        private bool bool_1;
        private Dictionary<GameObject, GameObjectCache> dictionary_0;
        private float float_0 = 60f;
        private float float_1;
        private GameObject gameObject_0;
        private int int_0;
        private int int_1 = 0x10;
        private int int_2;
        private List<CacheItem> list_0 = new List<CacheItem>();
        private PoolStrategy poolStrategy_0;
        [CompilerGenerated]
        private string string_0;

        internal GameObjectCache(Dictionary<GameObject, GameObjectCache> dictionary_1)
        {
            this.dictionary_0 = dictionary_1;
        }

        internal bool method_0()
        {
            return (this.gameObject_0 != null);
        }

        internal float method_1()
        {
            return this.float_0;
        }

        internal void method_10(GameObject gameObject_1)
        {
            this.int_0--;
            if (this.list_0.Count >= this.int_2)
            {
                UnityEngine.Object.Destroy(gameObject_1);
            }
            else
            {
                CacheItem item = new CacheItem {
                    CacheObject = gameObject_1,
                    LastFreeTime = Time.realtimeSinceStartup
                };
                this.list_0.Add(item);
            }
        }

        internal void method_11()
        {
            this.list_0.RemoveAll(new Predicate<CacheItem>(this.method_14));
        }

        internal void method_12()
        {
            foreach (CacheItem item in this.list_0)
            {
                UnityEngine.Object.Destroy(item.CacheObject);
            }
            this.list_0.Clear();
        }

        [DebuggerHidden]
        private IEnumerator method_13(AssetID assetID_0)
        {
            return new Class60 { assetID_0 = assetID_0, assetID_1 = assetID_0, gameObjectCache_0 = this };
        }

        [CompilerGenerated]
        private bool method_14(CacheItem cacheItem_0)
        {
            UnityEngine.Assertions.Assert.IsNotNull<GameObject>(cacheItem_0.CacheObject);
            float num = cacheItem_0.LastFreeTime + this.float_1;
            if (Time.realtimeSinceStartup > num)
            {
                UnityEngine.Object.Destroy(cacheItem_0.CacheObject);
                return true;
            }
            return false;
        }

        internal void method_2(float float_2)
        {
            this.float_0 = float_2;
            if (this.poolStrategy_0 != null)
            {
                this.float_1 = this.poolStrategy_0.InstanceReleaseAfterFree;
            }
            else
            {
                this.float_1 = this.float_0;
            }
        }

        internal int method_3()
        {
            return this.int_1;
        }

        internal void method_4(int int_3)
        {
            this.int_1 = int_3;
            if (this.poolStrategy_0 != null)
            {
                this.int_2 = this.poolStrategy_0.InstancePoolCount;
            }
            else
            {
                this.int_2 = this.int_1;
            }
        }

        internal void method_5(GameObject gameObject_1)
        {
            this.gameObject_0 = gameObject_1;
            this.poolStrategy_0 = this.gameObject_0.GetComponent<PoolStrategy>();
            if (this.poolStrategy_0 != null)
            {
                this.float_1 = this.poolStrategy_0.InstanceReleaseAfterFree;
                this.int_2 = this.poolStrategy_0.InstancePoolCount;
            }
            else
            {
                this.float_1 = this.method_1();
                this.int_2 = this.method_3();
            }
        }

        internal void method_6(AssetID assetID_0)
        {
            Scheduler.RunCoroutine(this.method_13(assetID_0));
        }

        internal void method_7()
        {
            if (this.gameObject_0 != null)
            {
                if (this.bool_1)
                {
                    Singleton<PrefabPool>.Instance.Free(this.gameObject_0);
                }
                this.gameObject_0 = null;
            }
        }

        internal GameObject method_8(Transform transform_0)
        {
            this.int_0++;
            if (this.list_0.Count > 0)
            {
                int index = this.list_0.Count - 1;
                CacheItem item = this.list_0[index];
                this.list_0.RemoveAt(index);
                GameObject cacheObject = item.CacheObject;
                UnityEngine.Assertions.Assert.IsNotNull<GameObject>(cacheObject);
                cacheObject.SetActive(true);
                cacheObject.transform.SetParent(transform_0);
                cacheObject.transform.localPosition = this.gameObject_0.transform.localPosition;
                cacheObject.transform.localRotation = this.gameObject_0.transform.localRotation;
                cacheObject.transform.localScale = this.gameObject_0.transform.localScale;
                this.dictionary_0.Add(cacheObject, this);
                return cacheObject;
            }
            GameObject key = UnityEngine.Object.Instantiate<GameObject>(this.gameObject_0, transform_0);
            this.dictionary_0.Add(key, this);
            return key;
        }

        internal void method_9(InstantiateQueue instantiateQueue_0, int int_3, Action<GameObject> action_0)
        {
            Class61 class2 = new Class61 {
                action_0 = action_0,
                gameObjectCache_0 = this
            };
            this.int_0++;
            if (this.list_0.Count > 0)
            {
                int index = this.list_0.Count - 1;
                CacheItem item = this.list_0[index];
                this.list_0.RemoveAt(index);
                GameObject cacheObject = item.CacheObject;
                UnityEngine.Assertions.Assert.IsNotNull<GameObject>(cacheObject);
                cacheObject.SetActive(true);
                cacheObject.transform.localPosition = this.gameObject_0.transform.localPosition;
                cacheObject.transform.localRotation = this.gameObject_0.transform.localRotation;
                cacheObject.transform.localScale = this.gameObject_0.transform.localScale;
                this.dictionary_0.Add(cacheObject, this);
                class2.action_0(cacheObject);
            }
            else
            {
                instantiateQueue_0.method_0(this.gameObject_0, int_3, new Action<GameObject>(class2.method_0));
            }
        }

        public int CacheCount
        {
            get
            {
                return this.list_0.Count;
            }
        }

        public string Error
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            private set
            {
                this.string_0 = value;
            }
        }

        public int InstancePoolCount
        {
            get
            {
                return this.int_2;
            }
        }

        public IEnumerable<CacheItem> Instances
        {
            get
            {
                return this.list_0;
            }
        }

        public bool Loading
        {
            get
            {
                return this.bool_0;
            }
        }

        public float ReleaseAfterFree
        {
            get
            {
                return this.float_1;
            }
        }

        public int SpawnCount
        {
            get
            {
                return this.int_0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CacheItem
        {
            public GameObject CacheObject;
            public float LastFreeTime;
        }

        [CompilerGenerated]
        private sealed class Class60 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal GameObjectCache gameObjectCache_0;
            internal int int_0;
            internal object object_0;
            internal WaitLoadPrefab waitLoadPrefab_0;

            [DebuggerHidden]
            public void Dispose()
            {
                this.int_0 = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.int_0;
                this.int_0 = -1;
                switch (num)
                {
                    case 0:
                        this.gameObjectCache_0.bool_0 = true;
                        this.waitLoadPrefab_0 = Singleton<PrefabPool>.Instance.Load(this.assetID_0);
                        this.object_0 = this.waitLoadPrefab_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadPrefab_0.Error))
                        {
                            this.gameObjectCache_0.method_5(this.waitLoadPrefab_0.LoadedObject);
                            this.gameObjectCache_0.bool_1 = true;
                            this.gameObjectCache_0.bool_0 = false;
                            this.int_0 = -1;
                            break;
                        }
                        this.gameObjectCache_0.Error = this.waitLoadPrefab_0.Error;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.object_0;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.object_0;
                }
            }
        }

        [CompilerGenerated]
        private sealed class Class61
        {
            internal Action<GameObject> action_0;
            internal GameObjectCache gameObjectCache_0;

            internal void method_0(GameObject gameObject_0)
            {
                this.gameObjectCache_0.dictionary_0.Add(gameObject_0, this.gameObjectCache_0);
                this.action_0(gameObject_0);
            }
        }
    }
}

