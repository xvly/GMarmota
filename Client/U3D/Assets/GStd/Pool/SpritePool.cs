namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class SpritePool : Singleton<SpritePool>
    {
        private Dictionary<AssetID, SpriteCache> dictionary_0 = new Dictionary<AssetID, SpriteCache>();
        private Dictionary<Sprite, SpriteCache> dictionary_1 = new Dictionary<Sprite, SpriteCache>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("SpritePool");
        private RepeatedTimer repeatedTimer_0;

        public SpritePool()
        {
            this.repeatedTimer_0 = RepeatedTimer.Repeat(17.5f, 10f, new Action(this.method_1));
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
        }

        public void ClearAllUnused()
        {
            this.dictionary_0.RemoveAll<AssetID, SpriteCache>(new Func<AssetID, SpriteCache, bool>(this.method_2));
        }

        ~SpritePool()
        {
            this.repeatedTimer_0.Dispose();
        }

        public void Free(Sprite sprite)
        {
            if (sprite == null)
            {
                logger_0.LogError("Try to free a null Sprite.");
            }
            else
            {
                SpriteCache cache;
                if (!this.dictionary_1.TryGetValue(sprite, out cache))
                {
                    object[] args = new object[] { sprite.name };
                    logger_0.LogWarning("Try to free an instance {0} not allocated by this pool.", args);
                }
                else
                {
                    cache.method_3();
                }
            }
        }

        public WaitLoadSprite Load(AssetID assetID)
        {
            SpriteCache cache;
            if (this.dictionary_0.TryGetValue(assetID, out cache))
            {
                cache.method_2();
                return new WaitLoadSprite(cache);
            }
            cache = new SpriteCache(this.dictionary_1);
            cache.method_4(assetID);
            cache.method_2();
            this.dictionary_0.Add(assetID, cache);
            return new WaitLoadSprite(cache);
        }

        public void Load(AssetID assetID, Action<Sprite> complete)
        {
            Scheduler.RunCoroutine(this.method_0(assetID, complete));
        }

        [DebuggerHidden]
        private IEnumerator method_0(AssetID assetID_0, Action<Sprite> action_0)
        {
            return new Class69 { assetID_0 = assetID_0, action_0 = action_0, assetID_1 = assetID_0, action_1 = action_0, spritePool_0 = this };
        }

        private void method_1()
        {
            this.dictionary_0.RemoveAll<AssetID, SpriteCache>(new Func<AssetID, SpriteCache, bool>(this.method_3));
        }

        [CompilerGenerated]
        private bool method_2(AssetID assetID_0, SpriteCache spriteCache_0)
        {
            if (string.IsNullOrEmpty(spriteCache_0.Error))
            {
                if (spriteCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                Sprite key = spriteCache_0.method_6();
                if (key != null)
                {
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                    this.dictionary_1.Remove(key);
                }
            }
            return true;
        }

        [CompilerGenerated]
        private bool method_3(AssetID assetID_0, SpriteCache spriteCache_0)
        {
            if (string.IsNullOrEmpty(spriteCache_0.Error))
            {
                if (spriteCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                float num = spriteCache_0.LastFreeTime + spriteCache_0.ReleaseAfterFree;
                if (Time.realtimeSinceStartup <= num)
                {
                    return false;
                }
                Sprite key = spriteCache_0.method_6();
                if (key != null)
                {
                    this.dictionary_1.Remove(key);
                    Resources.UnloadAsset(key);
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                }
            }
            return true;
        }

        public IDictionary<AssetID, SpriteCache> Caches
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
        private sealed class Class69 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal Action<Sprite> action_0;
            internal Action<Sprite> action_1;
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal object object_0;
            internal SpritePool spritePool_0;
            internal WaitLoadSprite waitLoadSprite_0;

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
                        this.waitLoadSprite_0 = this.spritePool_0.Load(this.assetID_0);
                        this.object_0 = this.waitLoadSprite_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadSprite_0.Error))
                        {
                            this.action_0(this.waitLoadSprite_0.LoadedObject);
                            break;
                        }
                        SpritePool.logger_0.LogError(this.waitLoadSprite_0.Error);
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

