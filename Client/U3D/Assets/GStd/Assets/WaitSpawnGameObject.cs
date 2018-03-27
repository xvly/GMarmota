namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class WaitSpawnGameObject : CustomYieldInstruction
    {
        private bool bool_0;
        private GameObject gameObject_0;
        private GameObjectCache gameObjectCache_0;
        private InstantiateQueue instantiateQueue_0;
        private int int_0;
        [CompilerGenerated]
        private string string_0;

        public WaitSpawnGameObject(GameObjectCache cache, InstantiateQueue instantiateQueue, int instantiatePriority)
        {
            this.gameObjectCache_0 = cache;
            this.instantiateQueue_0 = instantiateQueue;
            this.int_0 = instantiatePriority;
        }

        [CompilerGenerated]
        private void method_0(GameObject gameObject_1)
        {
            this.gameObject_0 = gameObject_1;
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

        public GameObject Instance
        {
            get
            {
                if (!this.gameObjectCache_0.method_0())
                {
                    return null;
                }
                if ((this.instantiateQueue_0 == null) && (this.gameObject_0 == null))
                {
                    this.gameObject_0 = this.gameObjectCache_0.method_8(null);
                }
                return this.gameObject_0;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if (!string.IsNullOrEmpty(this.gameObjectCache_0.Error))
                {
                    this.Error = this.gameObjectCache_0.Error;
                    return false;
                }
                if (!this.gameObjectCache_0.method_0())
                {
                    return true;
                }
                if (this.instantiateQueue_0 != null)
                {
                    if (!this.bool_0)
                    {
                        this.bool_0 = true;
                        this.gameObjectCache_0.method_9(this.instantiateQueue_0, this.int_0, new Action<GameObject>(this.method_0));
                    }
                    if (this.gameObject_0 == null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

