namespace GStd
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class WaitLoadAsset : CustomYieldInstruction
    {
        [CompilerGenerated]
        private string string_0;

        protected WaitLoadAsset()
        {
        }

        internal abstract bool vmethod_0();

        public string Error
        {
            [CompilerGenerated]
            get
            {
                return this.string_0;
            }
            [CompilerGenerated]
            protected set
            {
                this.string_0 = value;
            }
        }
    }
}

