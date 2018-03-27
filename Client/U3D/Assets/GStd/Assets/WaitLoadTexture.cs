namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class WaitLoadTexture : CustomYieldInstruction
    {
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private Texture texture_0;
        private TextureCache textureCache_0;

        internal WaitLoadTexture(TextureCache textureCache_1)
        {
            this.textureCache_0 = textureCache_1;
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
                if (!string.IsNullOrEmpty(this.textureCache_0.Error))
                {
                    this.Error = this.textureCache_0.Error;
                    return false;
                }
                if (this.textureCache_0.method_5())
                {
                    this.LoadedObject = this.textureCache_0.method_6();
                    return false;
                }
                return true;
            }
        }

        public Texture LoadedObject
        {
            [CompilerGenerated]
            get
            {
                return this.texture_0;
            }
            [CompilerGenerated]
            private set
            {
                this.texture_0 = value;
            }
        }
    }
}

