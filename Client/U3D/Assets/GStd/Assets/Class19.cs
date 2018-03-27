namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using UnityEngine.SceneManagement;

    internal sealed class Class19 : WaitLoadLevel
    {
        private bool bool_0;
        private Class9 class9_0;
        private LoadSceneMode loadSceneMode_0;
        private string string_1;
        private string string_2;

        internal Class19(Class9 class9_1, string string_3, string string_4, LoadSceneMode loadSceneMode_1)
        {
            this.class9_0 = class9_1;
            this.string_1 = string_3;
            this.string_2 = Path.GetFileNameWithoutExtension(string_4);
            this.loadSceneMode_0 = loadSceneMode_1;
            this.bool_0 = false;
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
            SceneManager.LoadScene(this.string_2, this.loadSceneMode_0);
            this.bool_0 = true;
            return false;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                return ((base.Error == null) && !this.bool_0);
            }
        }
    }
}

