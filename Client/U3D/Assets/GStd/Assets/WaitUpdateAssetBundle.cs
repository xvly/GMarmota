namespace GStd
{
    using ns0;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    public sealed class WaitUpdateAssetBundle : CustomYieldInstruction
    {
        private Class5 class5_0;
        private string string_0;
        private WaitForSecondsRealtime waitForSecondsRealtime_0;

        internal WaitUpdateAssetBundle()
        {
        }

        internal WaitUpdateAssetBundle(Class5 class5_1)
        {
            this.class5_0 = class5_1;
        }

        internal WaitUpdateAssetBundle(string string_1)
        {
            this.string_0 = string_1;
        }

        internal WaitUpdateAssetBundle(WaitForSecondsRealtime waitForSecondsRealtime_1)
        {
            this.waitForSecondsRealtime_0 = waitForSecondsRealtime_1;
        }

        public int BytesDownloaded
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return 0;
                }
                if (this.class5_0 == null)
                {
                    return 0;
                }
                return this.class5_0.method_7();
            }
        }

        public int ContentLength
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return 0;
                }
                if (this.class5_0 == null)
                {
                    return 0;
                }
                return this.class5_0.method_8();
            }
        }

        public int DownloadSpeed
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return 0;
                }
                if (this.class5_0 == null)
                {
                    return 0;
                }
                return this.class5_0.method_10();
            }
        }

        public string Error
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return (!this.waitForSecondsRealtime_0.keepWaiting ? "Simulate update failed" : string.Empty);
                }
                if (!string.IsNullOrEmpty(this.string_0))
                {
                    return this.string_0;
                }
                if (this.class5_0 != null)
                {
                    return this.class5_0.method_2();
                }
                return null;
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification="Reviewed. Suppression is OK here.")]
        public override bool keepWaiting
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return this.waitForSecondsRealtime_0.keepWaiting;
                }
                return (((this.Error == null) && (this.class5_0 != null)) && this.class5_0.method_13());
            }
        }

        public float Progress
        {
            get
            {
                if (this.waitForSecondsRealtime_0 != null)
                {
                    return 0f;
                }
                if (this.class5_0 == null)
                {
                    return 0f;
                }
                return this.class5_0.method_9();
            }
        }
    }
}

