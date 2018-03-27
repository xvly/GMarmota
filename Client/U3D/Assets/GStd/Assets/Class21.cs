namespace ns0
{
    using GStd;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using UnityEditor;
    using UnityEngine.SceneManagement;

    internal sealed class Class21 : WaitLoadLevel
    {
        internal Class21(string string_1, string string_2, LoadSceneMode loadSceneMode_0)
        {
            string[] assetPathsFromAssetBundleAndAssetName = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(string_1, Path.GetFileNameWithoutExtension(string_2));
            if (assetPathsFromAssetBundleAndAssetName.Length == 0)
            {
                base.Error = string.Format("There is no scene with name \"{0}\" in \"{1}\"", string_2, string_1);
            }
            else if (loadSceneMode_0 == LoadSceneMode.Additive)
            {
                EditorApplication.LoadLevelAdditiveInPlayMode(assetPathsFromAssetBundleAndAssetName[0]);
            }
            else
            {
                EditorApplication.LoadLevelInPlayMode(assetPathsFromAssetBundleAndAssetName[0]);
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
                return false;
            }
        }
    }
}

