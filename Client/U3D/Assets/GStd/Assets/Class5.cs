namespace ns0
{
    using GStd;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Networking;

    internal sealed class Class5
    {
        [CompilerGenerated]
        private AssetBundle assetBundle_0;
        private AssetBundleCreateRequest assetBundleCreateRequest_0;
        [CompilerGenerated]
        private bool bool_0;
        private Class27 class27_0;
        private Class6 class6_0;
        private Class8 class8_0;
        private const float float_0 = 0.05f;
        private const float float_1 = 0.5f;
        private float float_2;
        private float float_3;
        private float float_4;
        private Hash128 hash128_0;
        private static GStd.Logger logger_0 = LogSystem.GetLogger("AssetBundleLoadTask");
        private string string_0;
        private string string_1;
        [CompilerGenerated]
        private string string_2;
        private ulong ulong_0;
        private ulong ulong_1;
        private ulong ulong_2;
        private UnityWebRequest unityWebRequest_0;

        internal Class5(Class27 class27_1, AssetBundleCreateRequest assetBundleCreateRequest_1, string string_3)
        {
            this.method_12(false);
            this.class27_0 = class27_1;
            this.assetBundleCreateRequest_0 = assetBundleCreateRequest_1;
            this.string_1 = string_3;
        }

        internal Class5(Class27 class27_1, AssetBundleCreateRequest assetBundleCreateRequest_1, string string_3, Hash128 hash128_1)
        {
            this.method_12(false);
            this.class27_0 = class27_1;
            this.assetBundleCreateRequest_0 = assetBundleCreateRequest_1;
            this.string_1 = string_3;
            this.hash128_0 = hash128_1;
        }

        internal Class5(bool bool_1, Class6 class6_1, string string_3, Class27 class27_1, string string_4)
        {
            this.method_12(bool_1);
            this.class6_0 = class6_1;
            this.string_0 = string_3;
            this.class27_0 = class27_1;
            this.string_1 = string_4;
            this.method_19(0.05f);
        }

        internal Class5(bool bool_1, Class6 class6_1, string string_3, Class27 class27_1, string string_4, Hash128 hash128_1)
        {
            this.method_12(bool_1);
            this.class6_0 = class6_1;
            this.string_0 = string_3;
            this.class27_0 = class27_1;
            this.string_1 = string_4;
            this.hash128_0 = hash128_1;
            this.method_19(0.05f);
        }

        [CompilerGenerated]
        internal AssetBundle method_0()
        {
            return this.assetBundle_0;
        }

        [CompilerGenerated]
        private void method_1(AssetBundle assetBundle_1)
        {
            this.assetBundle_0 = assetBundle_1;
        }

        internal int method_10()
        {
            return (int) this.ulong_1;
        }

        [CompilerGenerated]
        internal bool method_11()
        {
            return this.bool_0;
        }

        [CompilerGenerated]
        internal void method_12(bool bool_1)
        {
            this.bool_0 = bool_1;
        }

        internal bool method_13()
        {
            return (this.unityWebRequest_0 != null);
        }

        internal void method_14()
        {
            Assert.IsNull<UnityWebRequest>(this.unityWebRequest_0);
            if (this.hash128_0.isValid)
            {
                this.class8_0 = this.class27_0.method_6(this.string_1, this.hash128_0);
            }
            else
            {
                this.class8_0 = this.class27_0.method_5(this.string_1);
            }
            this.unityWebRequest_0 = UnityWebRequest.Get(this.string_0);
            this.unityWebRequest_0.Send();
        }

        internal bool method_15()
        {
            if (this.unityWebRequest_0 != null)
            {
                this.method_16();
            }
            else
            {
                this.method_18();
            }
            return (((this.method_2() == null) && (!this.method_11() || this.method_13())) && (this.method_0() == null));
        }

        private void method_16()
        {
            ulong num = this.unityWebRequest_0.downloadedBytes - this.ulong_0;
            this.ulong_0 = this.unityWebRequest_0.downloadedBytes;
            this.float_2 = this.unityWebRequest_0.downloadProgress;
            this.method_20(num);
            if (this.unityWebRequest_0.isNetworkError)
            {
                if (this.class8_0 != null)
                {
                    try
                    {
                        this.class8_0.method_3();
                    }
                    catch (Exception exception)
                    {
                        logger_0.LogError(exception);
                    }
                    finally
                    {
                        this.class8_0 = null;
                    }
                }
                this.method_3(this.unityWebRequest_0.error);
                this.class6_0.method_7(this);
                this.unityWebRequest_0 = null;
            }
            else if (this.unityWebRequest_0.responseCode >= 400L)
            {
                if (this.class8_0 != null)
                {
                    try
                    {
                        this.class8_0.method_3();
                    }
                    catch (Exception exception2)
                    {
                        logger_0.LogError(exception2);
                    }
                    finally
                    {
                        this.class8_0 = null;
                    }
                }
                this.method_3(this.unityWebRequest_0.responseCode.ToString());
                this.class6_0.method_7(this);
                this.unityWebRequest_0 = null;
            }
            else if (this.unityWebRequest_0.isDone)
            {
                if (this.class8_0 != null)
                {
                    try
                    {
                        byte[] data = this.unityWebRequest_0.downloadHandler.data;
                        this.class8_0.method_1(data, data.Length);
                        this.class8_0.method_2();
                    }
                    catch (Exception exception3)
                    {
                        this.method_3(exception3.Message);
                        this.class6_0.method_7(this);
                        this.class8_0 = null;
                        return;
                    }
                    finally
                    {
                        this.class8_0 = null;
                    }
                }
                this.method_17();
                this.class6_0.method_7(this);
                this.unityWebRequest_0 = null;
            }
        }

        private void method_17()
        {
            if (!this.method_11())
            {
                if (this.class27_0 != null)
                {
                    if (this.hash128_0.isValid)
                    {
                        Assert.IsNotNull<Class27>(this.class27_0);
                        this.assetBundleCreateRequest_0 = this.class27_0.method_12(this.string_1, this.hash128_0, false);
                        if (this.assetBundleCreateRequest_0 == null)
                        {
                            this.method_3(string.Format("The asset at: {0} with hash {1} is not in the cache.", this.unityWebRequest_0.url, this.hash128_0));
                        }
                    }
                    else
                    {
                        Assert.IsNotNull<Class27>(this.class27_0);
                        this.assetBundleCreateRequest_0 = this.class27_0.method_11(this.string_1);
                        if (this.assetBundleCreateRequest_0 == null)
                        {
                            this.method_3(string.Format("The asset at: {0} is not in the cache.", this.unityWebRequest_0.url, this.hash128_0));
                        }
                    }
                }
                else
                {
                    AssetBundle content = DownloadHandlerAssetBundle.GetContent(this.unityWebRequest_0);
                    if (content != null)
                    {
                        this.method_1(content);
                    }
                    else
                    {
                        this.method_3(string.Format("The asset at: {0} is not an asset bundle.", this.unityWebRequest_0.url));
                    }
                }
            }
        }

        private void method_18()
        {
            if ((this.assetBundleCreateRequest_0 != null) && this.assetBundleCreateRequest_0.isDone)
            {
                if (this.assetBundleCreateRequest_0.assetBundle != null)
                {
                    this.method_1(this.assetBundleCreateRequest_0.assetBundle);
                }
                else if (this.class27_0 != null)
                {
                    if (this.hash128_0.isValid)
                    {
                        this.class27_0.method_8(this.string_1, this.hash128_0);
                        this.method_3(string.Format("The asset at: {0} with hash {1} in cache is not an asset bundle.", this.string_1, this.hash128_0));
                    }
                    else
                    {
                        this.class27_0.method_7(this.string_1);
                        this.method_3(string.Format("The asset at: {0} in cache is not an asset bundle.", this.string_1));
                    }
                }
                else
                {
                    this.method_3(string.Format("The asset at: {0} with hash {1} is not an asset bundle.", this.string_1, this.hash128_0));
                }
            }
        }

        private void method_19(float float_5)
        {
            this.float_3 = float_5;
            this.float_4 = 0f;
            this.ulong_2 = 0L;
        }

        [CompilerGenerated]
        internal string method_2()
        {
            return this.string_2;
        }

        private void method_20(ulong ulong_3)
        {
            this.float_3 -= Time.unscaledDeltaTime;
            this.float_4 += Time.unscaledDeltaTime;
            this.ulong_2 += ulong_3;
            if (this.float_3 <= 0f)
            {
                this.ulong_1 = (ulong) (((float) this.ulong_2) / this.float_4);
                if (this.ulong_1 == 0)
                {
                    this.method_19(0.05f);
                }
                else
                {
                    this.method_19(0.5f);
                }
            }
        }

        [CompilerGenerated]
        private void method_3(string string_3)
        {
            this.string_2 = string_3;
        }

        internal string method_4()
        {
            return this.string_1;
        }

        internal Hash128 method_5()
        {
            return this.hash128_0;
        }

        internal UnityWebRequest method_6()
        {
            return this.unityWebRequest_0;
        }

        internal int method_7()
        {
            return (int) this.ulong_0;
        }

        internal int method_8()
        {
            return 0;
        }

        internal float method_9()
        {
            return this.float_2;
        }
    }
}

