namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal sealed class Class20 : WaitLoadLevel
    {
        private AsyncOperation asyncOperation_0;
        private float float_0;

        internal Class20(string string_1, string string_2, LoadSceneMode loadSceneMode_0, float float_1)
        {
            this.float_0 = float_1;
            string[] assetPathsFromAssetBundleAndAssetName = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(string_1, Path.GetFileNameWithoutExtension(string_2));
            if (assetPathsFromAssetBundleAndAssetName.Length == 0)
            {
                base.Error = string.Format("There is no scene with name \"{0}\" in \"{1}\"", string_2, string_1);
            }
            else if (loadSceneMode_0 == LoadSceneMode.Additive)
            {
                this.asyncOperation_0 = EditorApplication.LoadLevelAdditiveAsyncInPlayMode(assetPathsFromAssetBundleAndAssetName[0]);
            }
            else
            {
                this.asyncOperation_0 = EditorApplication.LoadLevelAsyncInPlayMode(assetPathsFromAssetBundleAndAssetName[0]);
            }
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
                return ((this.float_0 > 0f) || ((this.asyncOperation_0 != null) && !this.asyncOperation_0.isDone));
            }
        }
    }
}

