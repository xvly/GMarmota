namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class WaitLoadPrefab : CustomYieldInstruction
    {
        [CompilerGenerated]
        private GameObject gameObject_0;
        private PrefabCache prefabCache_0;
        [CompilerGenerated]
        private string string_0;

        internal WaitLoadPrefab(PrefabCache prefabCache_1)
        {
            this.prefabCache_0 = prefabCache_1;
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

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if (!string.IsNullOrEmpty(this.prefabCache_0.Error))
                {
                    this.Error = this.prefabCache_0.Error;
                    return false;
                }
                if (this.prefabCache_0.method_5())
                {
                    this.LoadedObject = this.prefabCache_0.method_6();
                    return false;
                }
                return true;
            }
        }

        public GameObject LoadedObject
        {
            [CompilerGenerated]
            get
            {
                return this.gameObject_0;
            }
            [CompilerGenerated]
            private set
            {
                this.gameObject_0 = value;
            }
        }
    }
}

