namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class Class22 : WaitLoadObject
    {
        [CompilerGenerated]
        private AssetBundleRequest assetBundleRequest_0;
        [CompilerGenerated]
        private Class9 class9_0;
        private string string_1;
        private string string_2;
        private System.Type type_0;

        internal Class22(string string_3, params object[] args)
        {
            base.Error = string.Format(string_3, args);
        }

        internal Class22(Class9 class9_1, string string_3, string string_4, System.Type type_1)
        {
            this.method_1(class9_1);
            this.string_1 = string_3;
            this.string_2 = string_4;
            this.type_0 = type_1;
        }

        public override UnityEngine.Object GetObject()
        {
            if ((this.method_2() != null) && this.method_2().isDone)
            {
                return this.method_2().asset;
            }
            return null;
        }

        [CompilerGenerated]
        internal Class9 method_0()
        {
            return this.class9_0;
        }

        [CompilerGenerated]
        private void method_1(Class9 class9_1)
        {
            this.class9_0 = class9_1;
        }

        [CompilerGenerated]
        protected AssetBundleRequest method_2()
        {
            return this.assetBundleRequest_0;
        }

        [CompilerGenerated]
        private void method_3(AssetBundleRequest assetBundleRequest_1)
        {
            this.assetBundleRequest_0 = assetBundleRequest_1;
        }

        protected string method_4()
        {
            return this.string_1;
        }

        internal override bool vmethod_0()
        {
            Class7 class2 = this.method_0().method_4(this.string_1);
            if (class2 == null)
            {
                return true;
            }
            if (class2.method_1() != null)
            {
                base.Error = class2.method_1();
                return false;
            }
            this.method_3(class2.method_3().LoadAssetAsync(this.string_2, this.type_0));
            return false;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if ((this.method_2() == null) && (base.Error != null))
                {
                    return false;
                }
                return !((this.method_2() != null) && this.method_2().isDone);
            }
        }
    }
}

