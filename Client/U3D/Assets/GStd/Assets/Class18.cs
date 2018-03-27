namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal sealed class Class18 : WaitLoadLevel
    {
        private AsyncOperation asyncOperation_0;
        private Class9 class9_0;
        private LoadSceneMode loadSceneMode_0;
        private string string_1;
        private string string_2;

        internal Class18(string string_3, params object[] args)
        {
            base.Error = string.Format(string_3, args);
        }

        internal Class18(Class9 class9_1, string string_3, string string_4, LoadSceneMode loadSceneMode_1)
        {
            this.class9_0 = class9_1;
            this.string_1 = string_3;
            this.string_2 = Path.GetFileNameWithoutExtension(string_4);
            this.loadSceneMode_0 = loadSceneMode_1;
        }

        internal override bool vmethod_0()
        {
            Class7 class2 = this.class9_0.method_4(this.string_1);
            if (class2 == null)
            {
                return true;
            }
            if (!string.IsNullOrEmpty(class2.method_1()))
            {
                base.Error = class2.method_1();
                return false;
            }
            this.asyncOperation_0 = SceneManager.LoadSceneAsync(this.string_2, this.loadSceneMode_0);
            return false;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if ((this.asyncOperation_0 == null) && (base.Error != null))
                {
                    return false;
                }
                return !((this.asyncOperation_0 != null) && this.asyncOperation_0.isDone);
            }
        }
    }
}

