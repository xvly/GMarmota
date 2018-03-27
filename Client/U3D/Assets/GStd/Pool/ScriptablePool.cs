namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class ScriptablePool : Singleton<ScriptablePool>
    {
        private Dictionary<AssetID, ScriptableCache> dictionary_0 = new Dictionary<AssetID, ScriptableCache>();
        private Dictionary<ScriptableObject, ScriptableCache> dictionary_1 = new Dictionary<ScriptableObject, ScriptableCache>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("ScriptablePool");
        private RepeatedTimer repeatedTimer_0;

        public ScriptablePool()
        {
            this.repeatedTimer_0 = RepeatedTimer.Repeat(15f, 10f, new Action(this.method_1));
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
        }

        public void ClearAllUnused()
        {
            this.dictionary_0.RemoveAll<AssetID, ScriptableCache>(new Func<AssetID, ScriptableCache, bool>(this.method_2));
        }

        ~ScriptablePool()
        {
            this.repeatedTimer_0.Dispose();
        }

        public void Free(ScriptableObject obj)
        {
            if (obj == null)
            {
                logger_0.LogError("Try to free a null ScriptableObject.");
            }
            else
            {
                ScriptableCache cache;
                if (!this.dictionary_1.TryGetValue(obj, out cache))
                {
                    object[] args = new object[] { obj.name };
                    logger_0.LogWarning("Try to free an instance {0} not allocated by this pool.", args);
                }
                else
                {
                    cache.method_3();
                }
            }
        }

        public WaitLoadScriptable Load(AssetID assetID)
        {
            ScriptableCache cache;
            if (this.dictionary_0.TryGetValue(assetID, out cache))
            {
                cache.method_2();
                return new WaitLoadScriptable(cache);
            }
            cache = new ScriptableCache(this.dictionary_1);
            cache.method_4(assetID);
            cache.method_2();
            this.dictionary_0.Add(assetID, cache);
            return new WaitLoadScriptable(cache);
        }

        public void Load(AssetID assetID, Action<ScriptableObject> complete)
        {
            Scheduler.RunCoroutine(this.method_0(assetID, complete));
        }

        [DebuggerHidden]
        private IEnumerator method_0(AssetID assetID_0, Action<ScriptableObject> action_0)
        {
            return new Class67 { assetID_0 = assetID_0, action_0 = action_0, assetID_1 = assetID_0, action_1 = action_0, scriptablePool_0 = this };
        }

        private void method_1()
        {
            this.dictionary_0.RemoveAll<AssetID, ScriptableCache>(new Func<AssetID, ScriptableCache, bool>(this.method_3));
        }

        [CompilerGenerated]
        private bool method_2(AssetID assetID_0, ScriptableCache scriptableCache_0)
        {
            if (string.IsNullOrEmpty(scriptableCache_0.Error))
            {
                if (scriptableCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                ScriptableObject key = scriptableCache_0.method_6();
                if (key != null)
                {
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                    this.dictionary_1.Remove(key);
                }
            }
            return true;
        }

        [CompilerGenerated]
        private bool method_3(AssetID assetID_0, ScriptableCache scriptableCache_0)
        {
            if (string.IsNullOrEmpty(scriptableCache_0.Error))
            {
                if (scriptableCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                float num = scriptableCache_0.LastFreeTime + scriptableCache_0.ReleaseAfterFree;
                if (Time.realtimeSinceStartup <= num)
                {
                    return false;
                }
                ScriptableObject key = scriptableCache_0.method_6();
                if (key != null)
                {
                    this.dictionary_1.Remove(key);
                    Resources.UnloadAsset(key);
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                }
            }
            return true;
        }

        public IDictionary<AssetID, ScriptableCache> Caches
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
        private sealed class Class67 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal Action<ScriptableObject> action_0;
            internal Action<ScriptableObject> action_1;
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal object object_0;
            internal ScriptablePool scriptablePool_0;
            internal WaitLoadScriptable waitLoadScriptable_0;

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
                        this.waitLoadScriptable_0 = this.scriptablePool_0.Load(this.assetID_0);
                        this.object_0 = this.waitLoadScriptable_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadScriptable_0.Error))
                        {
                            this.action_0(this.waitLoadScriptable_0.LoadedObject);
                            break;
                        }
                        ScriptablePool.logger_0.LogError(this.waitLoadScriptable_0.Error);
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

