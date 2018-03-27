namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    internal sealed class Class25 : WaitLoadObject
    {
        private float float_0;
        private UnityEngine.Object object_0;

        internal Class25(string string_1, params object[] args)
        {
            base.Error = string.Format(string_1, args);
        }

        internal Class25(UnityEngine.Object object_1, float float_1)
        {
            this.object_0 = object_1;
            this.float_0 = float_1;
        }

        public override UnityEngine.Object GetObject()
        {
            return this.object_0;
        }

        internal override bool vmethod_0()
        {
            return false;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                this.float_0 -= Time.unscaledDeltaTime;
                return (this.float_0 > 0f);
            }
        }
    }
}

