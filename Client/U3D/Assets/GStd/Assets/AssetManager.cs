using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ns0;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GStd
{
    // Token: 0x0200001C RID: 28
    public static class AssetManager
    {
        // Token: 0x14000001 RID: 1
        // (add) Token: 0x060000B0 RID: 176 RVA: 0x000028C9 File Offset: 0x00000AC9
        // (remove) Token: 0x060000B1 RID: 177 RVA: 0x000028D6 File Offset: 0x00000AD6
        public static event Action<string> DownloadStartEvent
        {
            add
            {
                AssetManager.class6_0.method_0(value);
            }
            remove
            {
                AssetManager.class6_0.method_1(value);
            }
        }

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x060000B2 RID: 178 RVA: 0x000028E3 File Offset: 0x00000AE3
        // (remove) Token: 0x060000B3 RID: 179 RVA: 0x000028F0 File Offset: 0x00000AF0
        public static event Action<string> DownloadFinishEvent
        {
            add
            {
                AssetManager.class6_0.method_2(value);
            }
            remove
            {
                AssetManager.class6_0.method_3(value);
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x060000B4 RID: 180 RVA: 0x000028FD File Offset: 0x00000AFD
        // (set) Token: 0x060000B5 RID: 181 RVA: 0x00002904 File Offset: 0x00000B04
        //public static AssetSimulator Simulator { get; private set; } = new AssetSimulator();

            static AssetManager()
        {
            Simulator = new AssetSimulator();
        }

        public static AssetSimulator Simulator
        {
            get;
            private set;
        }
        

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x060000B6 RID: 182 RVA: 0x0000290C File Offset: 0x00000B0C
        public static string CachePath
        {
            get
            {
                return AssetManager.class27_0.method_0();
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x060000B7 RID: 183 RVA: 0x00002918 File Offset: 0x00000B18
        // (set) Token: 0x060000B8 RID: 184 RVA: 0x00002924 File Offset: 0x00000B24
        public static string DownloadingURL
        {
            get
            {
                return AssetManager.class28_0.method_0();
            }
            set
            {
                AssetManager.class28_0.method_1(value);
            }
        }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x060000B9 RID: 185 RVA: 0x00002931 File Offset: 0x00000B31
        // (set) Token: 0x060000BA RID: 186 RVA: 0x0000293D File Offset: 0x00000B3D
        public static string AssetVersion
        {
            get
            {
                return AssetManager.class28_0.method_2();
            }
            set
            {
                AssetManager.class28_0.method_3(value);
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x060000BB RID: 187 RVA: 0x0000294A File Offset: 0x00000B4A
        // (set) Token: 0x060000BC RID: 188 RVA: 0x00002956 File Offset: 0x00000B56
        public static bool IgnoreHashCheck
        {
            get
            {
                return AssetManager.class28_0.method_4();
            }
            set
            {
                AssetManager.class28_0.method_5(value);
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x060000BD RID: 189 RVA: 0x00002963 File Offset: 0x00000B63
        // (set) Token: 0x060000BE RID: 190 RVA: 0x0000296F File Offset: 0x00000B6F
        public static string[] ActiveVariants
        {
            get
            {
                return AssetManager.class9_0.method_0();
            }
            set
            {
                AssetManager.class9_0.method_1(value);
            }
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x060000BF RID: 191 RVA: 0x0000297C File Offset: 0x00000B7C
        public static AssetBundleManifest Manifest
        {
            get
            {
                return AssetManager.class9_0.method_2();
            }
        }

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x060000C0 RID: 192 RVA: 0x00002988 File Offset: 0x00000B88
        public static bool HasManifest
        {
            get
            {
                return AssetManager.Simulator.SimulateAssetBundle || AssetManager.Manifest != null;
            }
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x000029A3 File Offset: 0x00000BA3
        public static void DrawAssetBundles()
        {
            AssetManager.class9_0.method_15();
        }

        // Token: 0x060000C2 RID: 194 RVA: 0x000029AF File Offset: 0x00000BAF
        public static void ClearCache()
        {
            AssetManager.class27_0.method_4();
        }

        // Token: 0x060000C3 RID: 195 RVA: 0x0000A780 File Offset: 0x00008980
        public static IEnumerator Dispose()
        {
            AssetManager.class28_0.method_14();
            AssetManager.class9_0.method_3(null);
            AssetManager.list_0.Clear();
            if (AssetManager.func_0 == null)
            {
                AssetManager.func_0 = new Func<bool>(AssetManager.smethod_9);
            }
            return new WaitUntil(AssetManager.func_0);
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x000029BB File Offset: 0x00000BBB
        public static string LoadVersion()
        {
            return AssetManager.class27_0.method_1();
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x000029C7 File Offset: 0x00000BC7
        public static void SaveVersion(string version)
        {
            AssetManager.class27_0.method_2(version);
        }

        // Token: 0x060000C6 RID: 198 RVA: 0x0000A7D0 File Offset: 0x000089D0
        public static WaitLoadAsset LoadLocalManifest(string manifestAssetBundleName)
        {
            Scheduler.FrameEvent += AssetManager.smethod_0;
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_2();
            }
            if (!AssetManager.class9_0.method_5(manifestAssetBundleName))
            {
                AssetManager.logger_0.LogError("Can not load local manifest: {0}", new object[]
                {
                    manifestAssetBundleName
                });
                return null;
            }
            Class23 @class = new Class23(AssetManager.class9_0, manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
            AssetManager.list_0.Add(@class);
            return @class;
        }

        // Token: 0x060000C7 RID: 199 RVA: 0x0000A854 File Offset: 0x00008A54
        public static WaitLoadAsset LoadRemoteManifest(string manifestAssetBundleName)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_2();
            }
            if (!AssetManager.class9_0.method_6(manifestAssetBundleName))
            {
                AssetManager.logger_0.LogError("Can not load local manifest: {0}", new object[]
                {
                    manifestAssetBundleName
                });
                return null;
            }
            Class23 @class = new Class23(AssetManager.class9_0, manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
            AssetManager.list_0.Add(@class);
            return @class;
        }

        // Token: 0x060000C8 RID: 200 RVA: 0x0000A8C8 File Offset: 0x00008AC8
        public static bool IsVersionCached(string bundleName)
        {
            Hash128 assetBundleHash = AssetManager.Manifest.GetAssetBundleHash(bundleName);
            return AssetManager.class27_0.method_3(bundleName, assetBundleHash);
        }

        // Token: 0x060000C9 RID: 201 RVA: 0x000029D4 File Offset: 0x00000BD4
        public static bool IsVersionCached(string bundleName, Hash128 hash)
        {
            return AssetManager.class27_0.method_3(bundleName, hash);
        }

        // Token: 0x060000CA RID: 202 RVA: 0x0000A8F0 File Offset: 0x00008AF0
        public static WaitLoadFileInfo LoadFileInfo()
        {
            string downloadingURL = AssetManager.DownloadingURL;
            Assert.IsFalse(string.IsNullOrEmpty(downloadingURL));
            string string_ = downloadingURL + "/file_info.txt";
            return AssetManager.assetBundleFileInfo_0.method_2(string_);
        }

        // Token: 0x060000CB RID: 203 RVA: 0x0000A928 File Offset: 0x00008B28
        public static WaitUpdateAssetBundle UpdateBundle(string bundleName)
        {
            Hash128 assetBundleHash = AssetManager.Manifest.GetAssetBundleHash(bundleName);
            return AssetManager.class28_0.method_6(bundleName, assetBundleHash);
        }

        // Token: 0x060000CC RID: 204 RVA: 0x000029E2 File Offset: 0x00000BE2
        public static WaitUpdateAssetBundle UpdateBundle(string bundleName, Hash128 hash)
        {
            return AssetManager.class28_0.method_6(bundleName, hash);
        }

        // Token: 0x060000CD RID: 205 RVA: 0x000029F0 File Offset: 0x00000BF0
        public static AssetBundle LoadBundleLocal(string assetBundleName)
        {
            return AssetManager.class9_0.method_7(assetBundleName);
        }

        // Token: 0x060000CE RID: 206 RVA: 0x000029FD File Offset: 0x00000BFD
        public static UnityEngine.Object LoadObjectLocal(AssetID assetID, Type assetType)
        {
            return AssetManager.LoadObjectLocal(assetID.BundleName, assetID.AssetName, assetType);
        }

        // Token: 0x060000CF RID: 207 RVA: 0x0000A950 File Offset: 0x00008B50
        public static UnityEngine.Object LoadObjectLocal(string assetBundleName, string assetName, Type assetType)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_4(assetBundleName, assetName, assetType);
            }
            AssetBundle assetBundle = AssetManager.class9_0.method_7(assetBundleName);
            if (assetBundle == null)
            {
                return null;
            }
            return assetBundle.LoadAsset(assetName, assetType);
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00002A13 File Offset: 0x00000C13
        public static WaitLoadObject LoadObject(AssetID assetID, Type assetType)
        {
            return AssetManager.LoadObject(assetID.BundleName, assetID.AssetName, assetType);
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00002A29 File Offset: 0x00000C29
        public static WaitLoadObject LoadObjectSync(AssetID assetID, Type assetType)
        {
            return AssetManager.LoadObjectSync(assetID.BundleName, assetID.AssetName, assetType);
        }

        // Token: 0x060000D2 RID: 210 RVA: 0x0000A998 File Offset: 0x00008B98
        public static WaitLoadObject LoadObject(string assetBundleName, string assetName, Type assetType)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_5(assetBundleName, assetName, assetType);
            }
            if (AssetManager.class9_0.method_8(assetBundleName))
            {
                Class22 @class = new Class22(AssetManager.class9_0, assetBundleName, assetName, assetType);
                AssetManager.list_0.Add(@class);
                return @class;
            }
            string string_ = string.Format("Load asset bundle {0}:{1} failed.", assetBundleName, assetName);
            return new Class22(string_, new object[0]);
        }

        // Token: 0x060000D3 RID: 211 RVA: 0x0000AA04 File Offset: 0x00008C04
        public static WaitLoadObject LoadObjectSync(string assetBundleName, string assetName, Type assetType)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_6(assetBundleName, assetName, assetType);
            }
            if (AssetManager.class9_0.method_8(assetBundleName))
            {
                Class22 @class = new Class22(AssetManager.class9_0, assetBundleName, assetName, assetType);
                AssetManager.list_0.Add(@class);
                return @class;
            }
            string string_ = string.Format("Load asset bundle {0}:{1} failed.", assetBundleName, assetName);
            return new Class22(string_, new object[0]);
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x00002A3F File Offset: 0x00000C3F
        public static WaitLoadLevel LoadLevel(AssetID assetID, LoadSceneMode loadMode)
        {
            return AssetManager.LoadLevel(assetID.BundleName, assetID.AssetName, loadMode);
        }

        // Token: 0x060000D5 RID: 213 RVA: 0x00002A55 File Offset: 0x00000C55
        public static WaitLoadLevel LoadLevelSync(AssetID assetID, LoadSceneMode loadMode)
        {
            return AssetManager.LoadLevelSync(assetID.BundleName, assetID.AssetName, loadMode);
        }

        // Token: 0x060000D6 RID: 214 RVA: 0x0000AA70 File Offset: 0x00008C70
        public static WaitLoadLevel LoadLevel(string assetBundleName, string sceneName, LoadSceneMode loadMode)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_7(assetBundleName, sceneName, loadMode);
            }
            if (AssetManager.class9_0.method_8(assetBundleName))
            {
                Class18 @class = new Class18(AssetManager.class9_0, assetBundleName, sceneName, loadMode);
                AssetManager.list_0.Add(@class);
                return @class;
            }
            return new Class18("Load Level {0}:{1} failed.", new object[]
            {
                assetBundleName,
                sceneName
            });
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x0000AADC File Offset: 0x00008CDC
        public static WaitLoadLevel LoadLevelSync(string assetBundleName, string sceneName, LoadSceneMode loadMode)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return AssetManager.Simulator.method_8(assetBundleName, sceneName, loadMode);
            }
            if (AssetManager.class9_0.method_8(assetBundleName))
            {
                Class19 @class = new Class19(AssetManager.class9_0, assetBundleName, sceneName, loadMode);
                AssetManager.list_0.Add(@class);
                return @class;
            }
            return new Class18("Load Level {0}:{1} failed.", new object[]
            {
                assetBundleName,
                sceneName
            });
        }

        // Token: 0x060000D8 RID: 216 RVA: 0x00002A6B File Offset: 0x00000C6B
        public static string[] GetBundlesWithoutCached(string assetBundleName)
        {
            if (AssetManager.Simulator.SimulateAssetBundle)
            {
                return new string[0];
            }
            return AssetManager.class9_0.method_13(assetBundleName);
        }

        // Token: 0x060000D9 RID: 217 RVA: 0x00002A8B File Offset: 0x00000C8B
        public static bool UnloadAsseBundle(string assetBundle)
        {
            return AssetManager.class9_0.method_9(assetBundle);
        }

        // Token: 0x060000DA RID: 218 RVA: 0x00002A98 File Offset: 0x00000C98
        private static void smethod_0()
        {
            AssetManager.class28_0.method_15();
            AssetManager.class9_0.method_14();
            List<WaitLoadAsset> list = AssetManager.list_0;
            if (AssetManager.predicate_0 == null)
            {
                AssetManager.predicate_0 = new Predicate<WaitLoadAsset>(AssetManager.smethod_10);
            }
            list.RemoveAll(AssetManager.predicate_0);
        }

        // Token: 0x060000DB RID: 219 RVA: 0x00002AD6 File Offset: 0x00000CD6
        public static void LoadRemoteManifest(string manifestAssetBundleName, Action<string> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_2(manifestAssetBundleName, complete));
        }

        // Token: 0x060000DC RID: 220 RVA: 0x00002AE5 File Offset: 0x00000CE5
        public static void LoadFileInfo(Action<string, AssetBundleFileInfo> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_1(complete));
        }

        // Token: 0x060000DD RID: 221 RVA: 0x00002AF3 File Offset: 0x00000CF3
        public static void UpdateBundle(string bundleName, AssetManager.UpdateDelegate update, Action<string> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_3(bundleName, update, complete));
        }

        // Token: 0x060000DE RID: 222 RVA: 0x00002B03 File Offset: 0x00000D03
        public static void UpdateBundle(string bundleName, Hash128 hash, AssetManager.UpdateDelegate update, Action<string> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_4(bundleName, hash, update, complete));
        }

        // Token: 0x060000DF RID: 223 RVA: 0x00002B14 File Offset: 0x00000D14
        public static void LoadObject(AssetID assetID, Type assetType, Action<UnityEngine.Object> complete)
        {
            AssetManager.LoadObject(assetID.BundleName, assetID.AssetName, assetType, complete);
        }

        // Token: 0x060000E0 RID: 224 RVA: 0x00002B2B File Offset: 0x00000D2B
        public static void LoadObjectSync(AssetID assetID, Type assetType, Action<UnityEngine.Object> complete)
        {
            AssetManager.LoadObjectSync(assetID.BundleName, assetID.AssetName, assetType, complete);
        }

        // Token: 0x060000E1 RID: 225 RVA: 0x00002B42 File Offset: 0x00000D42
        public static void LoadObject(string assetBundleName, string assetName, Type assetType, Action<UnityEngine.Object> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_5(assetBundleName, assetName, assetType, complete));
        }

        // Token: 0x060000E2 RID: 226 RVA: 0x00002B53 File Offset: 0x00000D53
        public static void LoadObjectSync(string assetBundleName, string assetName, Type assetType, Action<UnityEngine.Object> complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_6(assetBundleName, assetName, assetType, complete));
        }

        // Token: 0x060000E3 RID: 227 RVA: 0x00002B64 File Offset: 0x00000D64
        public static void LoadLevel(AssetID assetID, LoadSceneMode loadMode, Action complete)
        {
            AssetManager.LoadLevel(assetID.BundleName, assetID.AssetName, loadMode, complete);
        }

        // Token: 0x060000E4 RID: 228 RVA: 0x00002B7B File Offset: 0x00000D7B
        public static void LoadLevelSync(AssetID assetID, LoadSceneMode loadMode, Action complete)
        {
            AssetManager.LoadLevelSync(assetID.BundleName, assetID.AssetName, loadMode, complete);
        }

        // Token: 0x060000E5 RID: 229 RVA: 0x00002B92 File Offset: 0x00000D92
        public static void LoadLevel(string assetBundleName, string levelName, LoadSceneMode loadMode, Action complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_7(assetBundleName, levelName, loadMode, complete));
        }

        // Token: 0x060000E6 RID: 230 RVA: 0x00002BA3 File Offset: 0x00000DA3
        public static void LoadLevelSync(string assetBundleName, string levelName, LoadSceneMode loadMode, Action complete)
        {
            Scheduler.RunCoroutine(AssetManager.smethod_8(assetBundleName, levelName, loadMode, complete));
        }

        // Token: 0x060000E7 RID: 231 RVA: 0x0000AB48 File Offset: 0x00008D48
        private static IEnumerator smethod_1(Action<string, AssetBundleFileInfo> action_0)
        {
            WaitLoadFileInfo waitLoadFileInfo = AssetManager.LoadFileInfo();
            yield return waitLoadFileInfo;
            // return 1; // TODO: check
            action_0(waitLoadFileInfo.Error, waitLoadFileInfo.FileInfo);
            yield break;
        }

        // Token: 0x060000E8 RID: 232 RVA: 0x0000AB6C File Offset: 0x00008D6C
        private static IEnumerator smethod_2(string string_0, Action<string> action_0)
        {
            WaitLoadAsset waitLoadAsset = AssetManager.LoadRemoteManifest(string_0);
            yield return waitLoadAsset;
            // return 1; // TODO: check
            action_0(waitLoadAsset.Error);
            yield break;
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x0000AB9C File Offset: 0x00008D9C
        private static IEnumerator smethod_3(string string_0, AssetManager.UpdateDelegate updateDelegate_0, Action<string> action_0)
        {
            WaitUpdateAssetBundle waitUpdateAssetBundle = AssetManager.UpdateBundle(string_0);
            if (!string.IsNullOrEmpty(waitUpdateAssetBundle.Error))
            {
                action_0(waitUpdateAssetBundle.Error);
                yield break;
            }

            Action value = new Action(() =>
            {
                updateDelegate_0(waitUpdateAssetBundle.Progress, waitUpdateAssetBundle.DownloadSpeed, waitUpdateAssetBundle.BytesDownloaded, waitUpdateAssetBundle.ContentLength);
            });

            Scheduler.FrameEvent += value;
            yield return waitUpdateAssetBundle;
            // return 1; // TODO: check
            Scheduler.FrameEvent -= value;
            action_0(waitUpdateAssetBundle.Error);
            yield break;
        }

        // Token: 0x060000EA RID: 234 RVA: 0x0000ABDC File Offset: 0x00008DDC
        private static IEnumerator smethod_4(string string_0, Hash128 hash128_0, AssetManager.UpdateDelegate updateDelegate_0, Action<string> action_0)
        {
            WaitUpdateAssetBundle waitUpdateAssetBundle = AssetManager.UpdateBundle(string_0, hash128_0);
            if (!string.IsNullOrEmpty(waitUpdateAssetBundle.Error))
            {
                action_0(waitUpdateAssetBundle.Error);
                yield break;
            }
            Action value = new Action(() => {
                updateDelegate_0(waitUpdateAssetBundle.Progress, waitUpdateAssetBundle.DownloadSpeed, waitUpdateAssetBundle.BytesDownloaded, waitUpdateAssetBundle.ContentLength);
            });
            Scheduler.FrameEvent += value;
            yield return waitUpdateAssetBundle;
            // return 1; // TODO: check
            Scheduler.FrameEvent -= value;
            action_0(waitUpdateAssetBundle.Error);
            yield break;
        }

        // Token: 0x060000EB RID: 235 RVA: 0x0000AC28 File Offset: 0x00008E28
        private static IEnumerator smethod_5(string string_0, string string_1, Type type_0, Action<UnityEngine.Object> action_0)
        {
            WaitLoadObject waitLoadObject = AssetManager.LoadObject(string_0, string_1, type_0);
            yield return waitLoadObject;
            // return 1; // TODO: check
            if (string.IsNullOrEmpty(waitLoadObject.Error))
            {
                goto IL_77;
            }
            AssetManager.logger_0.LogError(waitLoadObject.Error);
            IL_77:
            action_0(waitLoadObject.GetObject());
            yield break;
        }

        // Token: 0x060000EC RID: 236 RVA: 0x0000AC74 File Offset: 0x00008E74
        private static IEnumerator smethod_6(string string_0, string string_1, Type type_0, Action<UnityEngine.Object> action_0)
        {
            WaitLoadObject waitLoadObject = AssetManager.LoadObjectSync(string_0, string_1, type_0);
            yield return waitLoadObject;
            // return 1; // TODO: check
            if (string.IsNullOrEmpty(waitLoadObject.Error))
            {
                goto IL_77;
            }
            AssetManager.logger_0.LogError(waitLoadObject.Error);
            IL_77:
            action_0(waitLoadObject.GetObject());
            yield break;
        }

        // Token: 0x060000ED RID: 237 RVA: 0x0000ACC0 File Offset: 0x00008EC0
        private static IEnumerator smethod_7(string string_0, string string_1, LoadSceneMode loadSceneMode_0, Action action_0)
        {
            WaitLoadLevel waitLoadLevel = AssetManager.LoadLevel(string_0, string_1, loadSceneMode_0);
            yield return waitLoadLevel;
            // return 1; // TODO: check
            if (string.IsNullOrEmpty(waitLoadLevel.Error))
            {
                goto IL_77;
            }
            AssetManager.logger_0.LogError(waitLoadLevel.Error);
            IL_77:
            action_0();
            yield break;
        }

        // Token: 0x060000EE RID: 238 RVA: 0x0000AD0C File Offset: 0x00008F0C
        private static IEnumerator smethod_8(string string_0, string string_1, LoadSceneMode loadSceneMode_0, Action action_0)
        {
            WaitLoadLevel waitLoadLevel = AssetManager.LoadLevelSync(string_0, string_1, loadSceneMode_0);
            yield return waitLoadLevel;
            // return 1; // TODO: check
            if (string.IsNullOrEmpty(waitLoadLevel.Error))
            {
                goto IL_77;
            }
            AssetManager.logger_0.LogError(waitLoadLevel.Error);
            IL_77:
            action_0();
            yield break;
        }

        // Token: 0x060000EF RID: 239 RVA: 0x00002BB4 File Offset: 0x00000DB4
        [CompilerGenerated]
        private static bool smethod_9()
        {
            if (!AssetManager.class9_0.method_11())
            {
                Scheduler.FrameEvent -= AssetManager.smethod_0;
                AssetManager.class9_0.method_12();
                return true;
            }
            return false;
        }

        // Token: 0x060000F0 RID: 240 RVA: 0x00002BE0 File Offset: 0x00000DE0
        [CompilerGenerated]
        private static bool smethod_10(WaitLoadAsset waitLoadAsset_0)
        {
            return !waitLoadAsset_0.vmethod_0();
        }

        // Token: 0x0400006A RID: 106
        private static Logger logger_0 = LogSystem.GetLogger("AssetManager");

        // Token: 0x0400006B RID: 107
        private static Class27 class27_0 = new Class27();

        // Token: 0x0400006C RID: 108
        private static Class6 class6_0 = new Class6();

        // Token: 0x0400006D RID: 109
        private static Class28 class28_0 = new Class28(AssetManager.class27_0, AssetManager.class6_0);

        // Token: 0x0400006E RID: 110
        private static Class9 class9_0 = new Class9(AssetManager.class28_0);

        // Token: 0x0400006F RID: 111
        private static AssetBundleFileInfo assetBundleFileInfo_0 = new AssetBundleFileInfo();

        // Token: 0x04000070 RID: 112
        private static List<WaitLoadAsset> list_0 = new List<WaitLoadAsset>();

        // Token: 0x04000071 RID: 113
        [CompilerGenerated]
        private static AssetSimulator assetSimulator_0;

        // Token: 0x04000072 RID: 114
        [CompilerGenerated]
        private static Func<bool> func_0;

        // Token: 0x04000073 RID: 115
        [CompilerGenerated]
        private static Predicate<WaitLoadAsset> predicate_0;

        // Token: 0x0200001D RID: 29
        // (Invoke) Token: 0x060000F2 RID: 242
        public delegate void UpdateDelegate(float progress, int downloadSpeed, int bytesDownloaded, int contentLength);
    }
}
