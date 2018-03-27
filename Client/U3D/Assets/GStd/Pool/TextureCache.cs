namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class TextureCache
    {
        [CompilerGenerated]
        private bool bool_0;
        private float float_0 = -1f;
        private float float_1 = 60f;
        private float float_2;
        private IDictionary<Texture, TextureCache> idictionary_0;
        private int int_0;
        private static GStd.Logger logger_0 = LogSystem.GetLogger("TextureCache");
        [CompilerGenerated]
        private string string_0;
        private Texture texture_0;

        internal TextureCache(IDictionary<Texture, TextureCache> idictionary_1)
        {
            this.idictionary_0 = idictionary_1;
        }

        internal float method_0()
        {
            return this.float_1;
        }

        internal void method_1(float float_3)
        {
            this.float_1 = float_3;
            this.float_2 = this.float_1;
        }

        internal void method_2()
        {
            this.int_0++;
        }

        internal void method_3()
        {
            this.int_0--;
            this.float_0 = Time.realtimeSinceStartup;
        }

        internal void method_4(AssetID assetID_0)
        {
            Scheduler.RunCoroutine(this.method_7(assetID_0));
        }

        internal bool method_5()
        {
            return (this.texture_0 != null);
        }

        internal Texture method_6()
        {
            return this.texture_0;
        }

        [DebuggerHidden]
        private IEnumerator method_7(AssetID assetID_0)
        {
            return new Class71 { assetID_0 = assetID_0, assetID_1 = assetID_0, textureCache_0 = this };
        }

        private Texture2D method_8(UnityEngine.TextAsset textAsset_0)
        {
            byte[] bytes = textAsset_0.bytes;
            Texture2D textured = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!textured.LoadImage(bytes, true))
            {
                return null;
            }
            textured.wrapMode = TextureWrapMode.Clamp;
            return textured;
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

        public float LastFreeTime
        {
            get
            {
                return this.float_0;
            }
        }

        public int ReferenceCount
        {
            get
            {
                return this.int_0;
            }
        }

        public float ReleaseAfterFree
        {
            get
            {
                return this.float_2;
            }
        }

        public bool TextAsset
        {
            [CompilerGenerated]
            get
            {
                return this.bool_0;
            }
            [CompilerGenerated]
            private set
            {
                this.bool_0 = value;
            }
        }

        [CompilerGenerated]
        private sealed class Class71 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal UnityEngine.Object object_0;
            internal object object_1;
            internal TextAsset textAsset_0;
            internal TextureCache textureCache_0;
            internal WaitLoadObject waitLoadObject_0;

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
                        this.waitLoadObject_0 = AssetManager.LoadObject(this.assetID_0, typeof(UnityEngine.Object));
                        this.object_1 = this.waitLoadObject_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadObject_0.Error))
                        {
                            this.object_0 = this.waitLoadObject_0.GetObject();
                            this.textureCache_0.texture_0 = this.object_0 as Texture;
                            if (this.textureCache_0.texture_0 == null)
                            {
                                this.textAsset_0 = this.object_0 as TextAsset;
                                if (this.textAsset_0 != null)
                                {
                                    this.textureCache_0.texture_0 = this.textureCache_0.method_8(this.textAsset_0);
                                    Resources.UnloadAsset(this.textAsset_0);
                                    this.textureCache_0.TextAsset = true;
                                }
                            }
                            if (this.textureCache_0.texture_0 == null)
                            {
                                this.textureCache_0.Error = string.Format("This asset: {0} is not a Texture", this.assetID_0);
                            }
                            else
                            {
                                this.textureCache_0.float_2 = this.textureCache_0.method_0();
                                if (this.textureCache_0.idictionary_0.ContainsKey(this.textureCache_0.texture_0))
                                {
                                    object[] args = new object[] { this.assetID_0 };
                                    TextureCache.logger_0.LogWarning("The texture {0} has been loaded.", args);
                                    this.textureCache_0.idictionary_0[this.textureCache_0.texture_0] = this.textureCache_0;
                                }
                                else
                                {
                                    this.textureCache_0.idictionary_0.Add(this.textureCache_0.texture_0, this.textureCache_0);
                                }
                                this.int_0 = -1;
                            }
                            break;
                        }
                        this.textureCache_0.Error = this.waitLoadObject_0.Error;
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
                    return this.object_1;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.object_1;
                }
            }
        }
    }
}

