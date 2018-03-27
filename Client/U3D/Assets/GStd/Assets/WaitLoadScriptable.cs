namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class WaitLoadScriptable : CustomYieldInstruction
    {
        private ScriptableCache scriptableCache_0;
        [CompilerGenerated]
        private ScriptableObject scriptableObject_0;
        [CompilerGenerated]
        private string string_0;

        public WaitLoadScriptable(ScriptableCache cache)
        {
            this.scriptableCache_0 = cache;
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
                if (!string.IsNullOrEmpty(this.scriptableCache_0.Error))
                {
                    this.Error = this.scriptableCache_0.Error;
                    return false;
                }
                if (this.scriptableCache_0.method_5())
                {
                    this.LoadedObject = this.scriptableCache_0.method_6();
                    return false;
                }
                return true;
            }
        }

        public ScriptableObject LoadedObject
        {
            [CompilerGenerated]
            get
            {
                return this.scriptableObject_0;
            }
            [CompilerGenerated]
            private set
            {
                this.scriptableObject_0 = value;
            }
        }
    }
}

