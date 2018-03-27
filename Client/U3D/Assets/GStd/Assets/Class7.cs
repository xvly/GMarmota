namespace ns0
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal sealed class Class7
    {
        [CompilerGenerated]
        private AssetBundle assetBundle_0;
        private Class26 class26_0;
        [CompilerGenerated]
        private Class7[] class7_0;
        private const float float_0 = 30f;
        private float float_1;
        private int int_0;
        [CompilerGenerated]
        private string string_0;

        internal Class7(Class26 class26_1)
        {
            this.float_1 = 30f;
            this.class26_0 = class26_1;
        }

        internal Class7(AssetBundle assetBundle_1)
        {
            this.float_1 = 30f;
            this.method_4(assetBundle_1);
        }

        internal int method_0()
        {
            return this.int_0;
        }

        [CompilerGenerated]
        internal string method_1()
        {
            return this.string_0;
        }

        internal void method_10()
        {
            if ((this.int_0 > 0) && (--this.int_0 <= 0))
            {
                this.float_1 = 30f;
            }
        }

        internal void method_11()
        {
            if (this.method_3() != null)
            {
                this.method_3().Unload(false);
            }
            if (this.method_5() != null)
            {
                foreach (Class7 class2 in this.method_5())
                {
                    class2.method_10();
                }
            }
        }

        internal bool method_12()
        {
            if (this.int_0 <= 0)
            {
                this.float_1 -= Time.unscaledDeltaTime;
                if (this.float_1 <= 0f)
                {
                    this.method_11();
                    return true;
                }
            }
            return false;
        }

        [CompilerGenerated]
        internal void method_2(string string_1)
        {
            this.string_0 = string_1;
        }

        [CompilerGenerated]
        internal AssetBundle method_3()
        {
            return this.assetBundle_0;
        }

        [CompilerGenerated]
        private void method_4(AssetBundle assetBundle_1)
        {
            this.assetBundle_0 = assetBundle_1;
        }

        [CompilerGenerated]
        internal Class7[] method_5()
        {
            return this.class7_0;
        }

        [CompilerGenerated]
        internal void method_6(Class7[] class7_1)
        {
            this.class7_0 = class7_1;
        }

        internal float method_7()
        {
            return this.float_1;
        }

        internal bool method_8()
        {
            if (this.class26_0.keepWaiting)
            {
                return false;
            }
            if (this.class26_0.method_0() != null)
            {
                this.method_2(this.class26_0.method_0());
            }
            else
            {
                this.method_4(this.class26_0.method_1());
            }
            return true;
        }

        internal void method_9()
        {
            this.int_0++;
        }
    }
}

