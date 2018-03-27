namespace GStd
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Networking;

    public sealed class WaitLoadFileInfo : CustomYieldInstruction
    {
        private AssetBundleFileInfo assetBundleFileInfo_0;
        private AsyncOperation asyncOperation_0;
        [CompilerGenerated]
        private string string_0;
        private UnityWebRequest unityWebRequest_0;

        internal WaitLoadFileInfo(AssetBundleFileInfo assetBundleFileInfo_1)
        {
            this.assetBundleFileInfo_0 = assetBundleFileInfo_1;
        }

        internal WaitLoadFileInfo(AssetBundleFileInfo assetBundleFileInfo_1, UnityWebRequest unityWebRequest_1, AsyncOperation asyncOperation_1)
        {
            this.assetBundleFileInfo_0 = assetBundleFileInfo_1;
            this.unityWebRequest_0 = unityWebRequest_1;
            this.asyncOperation_0 = asyncOperation_1;
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

        public AssetBundleFileInfo FileInfo
        {
            get
            {
                return this.assetBundleFileInfo_0;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if (!this.assetBundleFileInfo_0.method_0())
                {
                    if (!this.asyncOperation_0.isDone)
                    {
                        return true;
                    }
                    this.assetBundleFileInfo_0.method_3();
                    if (this.unityWebRequest_0.isNetworkError)
                    {
                        this.Error = this.unityWebRequest_0.error;
                        return false;
                    }
                    if (this.unityWebRequest_0.responseCode >= 400L)
                    {
                        this.Error = string.Format("Http error code: {0}", this.unityWebRequest_0.responseCode);
                        return false;
                    }
                    string content = DownloadHandlerBuffer.GetContent(this.unityWebRequest_0);
                    if (!this.assetBundleFileInfo_0.method_4(content))
                    {
                        this.Error = "Parse file info file failed.";
                    }
                }
                return false;
            }
        }
    }
}

