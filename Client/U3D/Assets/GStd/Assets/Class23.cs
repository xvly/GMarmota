namespace ns0
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.Assertions;

    internal sealed class Class23 : Class22
    {
        internal Class23(Class9 class9_1, string string_3, string string_4, System.Type type_1) : base(class9_1, string_3, string_4, type_1)
        {
        }

        internal override bool vmethod_0()
        {
            base.vmethod_0();
            if (base.Error != null)
            {
                base.method_0().method_10(base.method_4());
                return false;
            }
            if ((base.method_2() != null) && base.method_2().isDone)
            {
                AssetBundleManifest manifest = base.GetObject<AssetBundleManifest>();
                base.method_0().method_10(base.method_4());
                Assert.IsNotNull<AssetBundleManifest>(manifest);
                base.method_0().method_3(manifest);
                return false;
            }
            return true;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if ((base.method_2() == null) && (base.Error != null))
                {
                    return false;
                }
                return !((base.method_2() != null) && base.method_2().isDone);
            }
        }
    }
}

