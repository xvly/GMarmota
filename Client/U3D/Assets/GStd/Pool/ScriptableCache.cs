namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class ScriptableCache
    {
        private float float_0 = -1f;
        private float float_1 = 60f;
        private float float_2;
        private IDictionary<ScriptableObject, ScriptableCache> idictionary_0;
        private int int_0;
        private static GStd.Logger logger_0 = LogSystem.GetLogger("ScriptableCache");
        private ScriptableObject scriptableObject_0;
        [CompilerGenerated]
        private string string_0;

        internal ScriptableCache(IDictionary<ScriptableObject, ScriptableCache> idictionary_1)
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
            return (this.scriptableObject_0 != null);
        }

        internal ScriptableObject method_6()
        {
            return this.scriptableObject_0;
        }

        [DebuggerHidden]
        private IEnumerator method_7(AssetID assetID_0)
        {
            return new Class66 { assetID_0 = assetID_0, assetID_1 = assetID_0, scriptableCache_0 = this };
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

        [CompilerGenerated]
        private sealed class Class66 : IEnumerator<object>, IEnumerator, IDisposable
        {
            internal AssetID assetID_0;
            internal AssetID assetID_1;
            internal int int_0;
            internal object object_0;
            internal ScriptableCache scriptableCache_0;
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
                        this.waitLoadObject_0 = AssetManager.LoadObject(this.assetID_0, typeof(ScriptableObject));
                        this.object_0 = this.waitLoadObject_0;
                        this.int_0 = 1;
                        return true;

                    case 1:
                        if (string.IsNullOrEmpty(this.waitLoadObject_0.Error))
                        {
                            this.scriptableCache_0.scriptableObject_0 = this.waitLoadObject_0.GetObject() as ScriptableObject;
                            if (this.scriptableCache_0.scriptableObject_0 == null)
                            {
                                this.scriptableCache_0.Error = string.Format("The asset {0} is not a ScriptableObject.", this.assetID_0);
                            }
                            else
                            {
                                this.scriptableCache_0.float_2 = this.scriptableCache_0.method_0();
                                if (this.scriptableCache_0.idictionary_0.ContainsKey(this.scriptableCache_0.scriptableObject_0))
                                {
                                    object[] args = new object[] { this.assetID_0 };
                                    ScriptableCache.logger_0.LogWarning("The scriptable object {0} has been loaded.", args);
                                    this.scriptableCache_0.idictionary_0[this.scriptableCache_0.scriptableObject_0] = this.scriptableCache_0;
                                }
                                else
                                {
                                    this.scriptableCache_0.idictionary_0.Add(this.scriptableCache_0.scriptableObject_0, this.scriptableCache_0);
                                }
                                this.int_0 = -1;
                            }
                            break;
                        }
                        this.scriptableCache_0.Error = this.waitLoadObject_0.Error;
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
    }
}

