namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class TexturePool : Singleton<TexturePool>
    {
        private Dictionary<AssetID, TextureCache> dictionary_0 = new Dictionary<AssetID, TextureCache>();
        private Dictionary<Texture, TextureCache> dictionary_1 = new Dictionary<Texture, TextureCache>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("TexturePool");
        private RepeatedTimer repeatedTimer_0;

        public TexturePool()
        {
            this.repeatedTimer_0 = RepeatedTimer.Repeat(20f, 10f, new Action(this.method_1));
        }

        public void Clear()
        {
            this.dictionary_0.Clear();
            this.dictionary_1.Clear();
        }

        public void ClearAllUnused()
        {
            this.dictionary_0.RemoveAll<AssetID, TextureCache>(new Func<AssetID, TextureCache, bool>(this.method_2));
        }

        ~TexturePool()
        {
            this.repeatedTimer_0.Dispose();
        }

        public void Free(Texture texture)
        {
            if (texture == null)
            {
                logger_0.LogError("Try to free a null Texture.");
            }
            else
            {
                TextureCache cache;
                if (!this.dictionary_1.TryGetValue(texture, out cache))
                {
                    object[] args = new object[] { texture.name };
                    logger_0.LogWarning("Try to free an instance {0} not allocated by this pool.", args);
                }
                else
                {
                    cache.method_3();
                }
            }
        }

        public WaitLoadTexture Load(AssetID assetID)
        {
            TextureCache cache;
            if (this.dictionary_0.TryGetValue(assetID, out cache))
            {
                cache.method_2();
                return new WaitLoadTexture(cache);
            }
            cache = new TextureCache(this.dictionary_1);
            cache.method_4(assetID);
            cache.method_2();
            this.dictionary_0.Add(assetID, cache);
            return new WaitLoadTexture(cache);
        }

        public void Load(AssetID assetID, Action<Texture> complete)
        {
            Scheduler.RunCoroutine(this.method_0(assetID, complete));
        }

        [DebuggerHidden]
        private IEnumerator method_0(AssetID assetID_0, Action<Texture> action_0)
        {
            return new Class70 { assetID_0 = assetID_0, action_0 = action_0, assetID_1 = assetID_0, action_1 = action_0, texturePool_0 = this };
        }

        private void method_1()
        {
            this.dictionary_0.RemoveAll<AssetID, TextureCache>(new Func<AssetID, TextureCache, bool>(this.method_3));
        }

        [CompilerGenerated]
        private bool method_2(AssetID assetID_0, TextureCache textureCache_0)
        {
            if (string.IsNullOrEmpty(textureCache_0.Error))
            {
                if (textureCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                Texture key = textureCache_0.method_6();
                if (key != null)
                {
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                    this.dictionary_1.Remove(key);
                }
            }
            return true;
        }

        [CompilerGenerated]
        private bool method_3(AssetID assetID_0, TextureCache textureCache_0)
        {
            if (string.IsNullOrEmpty(textureCache_0.Error))
            {
                if (textureCache_0.ReferenceCount != 0)
                {
                    return false;
                }
                float num = textureCache_0.LastFreeTime + textureCache_0.ReleaseAfterFree;
                if (Time.realtimeSinceStartup <= num)
                {
                    return false;
                }
                Texture key = textureCache_0.method_6();
                if (key != null)
                {
                    this.dictionary_1.Remove(key);
                    if (!textureCache_0.TextAsset)
                    {
                        Resources.UnloadAsset(key);
                    }
                    AssetManager.UnloadAsseBundle(assetID_0.BundleName);
                }
            }
            return true;
        }

        public IDictionary<AssetID, TextureCache> Caches
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
        private sealed class Class70 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal Action<Texture> action_0;
            internal Action<Texture> action_1;
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal object object_0;
            internal TexturePool texturePool_0;
            internal WaitLoadTexture waitLoadTexture_0;

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
                        this.waitLoadTexture_0 = this.texturePool_0.Load(this.assetID_0);
                        this.object_0 = this.waitLoadTexture_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadTexture_0.Error))
                        {
                            this.action_0(this.waitLoadTexture_0.LoadedObject);
                            break;
                        }
                        TexturePool.logger_0.LogError(this.waitLoadTexture_0.Error);
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

