namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class PrefabPool : Singleton<PrefabPool>
    {
        private Dictionary<AssetID, PrefabCache> dictionary_0 = new Dictionary<AssetID, PrefabCache>();
        private Dictionary<GameObject, PrefabCache> dictionary_1 = new Dictionary<GameObject, PrefabCache>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("PrefabPool");
        private RepeatedTimer repeatedTimer_0;

        public PrefabPool()
        {
            this.repeatedTimer_0 = RepeatedTimer.Repeat(12.5f, 10f, new Action(this.method_1));
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
        }

        public void ClearAllUnused()
        {
            this.dictionary_0.RemoveAll<AssetID, PrefabCache>(new Func<AssetID, PrefabCache, bool>(this.method_2));
        }

        ~PrefabPool()
        {
            this.repeatedTimer_0.Dispose();
        }

        public void Free(GameObject prefab)
        {
            if (prefab == null)
            {
                logger_0.LogError("Try to free a null Prefab.");
            }
            else
            {
                PrefabCache cache;
                if (!this.dictionary_1.TryGetValue(prefab, out cache))
                {
                    object[] args = new object[] { prefab.name };
                    logger_0.LogWarning("Try to free an instance {0} not allocated by this pool.", args);
                }
                else
                {
                    cache.method_3();
                }
            }
        }

        public WaitLoadPrefab Load(AssetID assetID)
        {
            PrefabCache cache;
            if (this.dictionary_0.TryGetValue(assetID, out cache))
            {
                cache.method_2();
                return new WaitLoadPrefab(cache);
            }
            cache = new PrefabCache(this.dictionary_1);
            cache.method_4(assetID);
            cache.method_2();
            this.dictionary_0.Add(assetID, cache);
            return new WaitLoadPrefab(cache);
        }

        public void Load(AssetID assetID, Action<GameObject> complete)
        {
            Scheduler.RunCoroutine(this.method_0(assetID, complete));
        }

        [DebuggerHidden]
        private IEnumerator method_0(AssetID assetID_0, Action<GameObject> action_0)
        {
            return new Class65 { assetID_0 = assetID_0, action_0 = action_0, assetID_1 = assetID_0, action_1 = action_0, prefabPool_0 = this };
        }

        private void method_1()
        {
            this.dictionary_0.RemoveAll<AssetID, PrefabCache>(new Func<AssetID, PrefabCache, bool>(this.method_3));
        }

        [CompilerGenerated]
        private bool method_2(AssetID assetID_0, PrefabCache prefabCache_0)
        {
            if (string.IsNullOrEmpty(prefabCache_0.Error))
            {
                if (prefabCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                GameObject key = prefabCache_0.method_6();
                if (key != null)
                {
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                    this.dictionary_1.Remove(key);
                }
            }
            return true;
        }

        [CompilerGenerated]
        private bool method_3(AssetID assetID_0, PrefabCache prefabCache_0)
        {
            if (string.IsNullOrEmpty(prefabCache_0.Error))
            {
                if (prefabCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                float num = prefabCache_0.LastFreeTime + prefabCache_0.ReleaseAfterFree;
                if (Time.realtimeSinceStartup <= num)
                {
                    return false;
                }
                GameObject key = prefabCache_0.method_6();
                if (key != null)
                {
                    this.dictionary_1.Remove(key);
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                }
            }
            return true;
        }

        public IDictionary<AssetID, PrefabCache> Caches
        {
            get
            {
                return this.dictionary_0;
            }
        }

        public float SweepLeftTime
        {
            get
            {
                return this.repeatedTimer_0.LeftTime;
            }
        }

        public float SweepTime
        {
            get
            {
                return this.repeatedTimer_0.RepeatTime;
            }
        }

        [CompilerGenerated]
        private sealed class Class65 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal Action<GameObject> action_0;
            internal Action<GameObject> action_1;
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal object object_0;
            internal PrefabPool prefabPool_0;
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
                        this.waitLoadPrefab_0 = this.prefabPool_0.Load(this.assetID_0);
                        this.object_0 = this.waitLoadPrefab_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadPrefab_0.Error))
                        {
                            this.action_0(this.waitLoadPrefab_0.LoadedObject);
                            break;
                        }
                        PrefabPool.logger_0.LogError(this.waitLoadPrefab_0.Error);
                        this.action_0(null);
                        break;

                    default:
                        goto Label_009C;
                }
                this.int_0 = -1;
            Label_009C:
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
    }
}

