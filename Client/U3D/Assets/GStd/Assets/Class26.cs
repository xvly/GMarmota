namespace ns0
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    internal sealed class Class26 : CustomYieldInstruction
    {
        private Class5 class5_0;
        private WaitForSecondsRealtime waitForSecondsRealtime_0;

        internal Class26(Class5 class5_1)
        {
            this.class5_0 = class5_1;
        }

        internal Class26(WaitForSecondsRealtime waitForSecondsRealtime_1)
        {
            this.waitForSecondsRealtime_0 = waitForSecondsRealtime_1;
        }

        internal string method_0()
        {
            if (this.waitForSecondsRealtime_0 != null)
            {
                return (!this.waitForSecondsRealtime_0.keepWaiting ? "Simulate load failed" : string.Empty);
            }
            return this.class5_0.method_2();
        }

        internal AssetBundle method_1()
        {
            if (this.waitForSecondsRealtime_0 != null)
            {
                return null;
            }
            return this.class5_0.method_0();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return this.waitForSecondsRealtime_0.keepWaiting;
                }
                return ((this.method_0() == null) && (this.method_1() == null));
            }
        }
    }
}

