namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class WaitLoadSprite : CustomYieldInstruction
    {
        [CompilerGenerated]
        private Sprite sprite_0;
        private SpriteCache spriteCache_0;
        [CompilerGenerated]
        private string string_0;

        internal WaitLoadSprite(SpriteCache spriteCache_1)
        {
            this.spriteCache_0 = spriteCache_1;
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
                if (!string.IsNullOrEmpty(this.spriteCache_0.Error))
                {
                    this.Error = this.spriteCache_0.Error;
                    return false;
                }
                if (this.spriteCache_0.method_5())
                {
                    this.LoadedObject = this.spriteCache_0.method_6();
                    return false;
                }
                return true;
            }
        }

        public Sprite LoadedObject
        {
            [CompilerGenerated]
            get
            {
                return this.sprite_0;
            }
            [CompilerGenerated]
            private set
            {
                this.sprite_0 = value;
            }
        }
    }
}

