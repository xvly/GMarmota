//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using LuaInterface;
using GStd;
using GStd.Asset;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// The lua bundle loader.
/// </summary>
public class LuaBundleLoader : LuaFileUtils
{
#if UNITY_IOS
    private const string AssetBundlePrefix = "lua/";
    private const string AssetPrefxi = "Assets/Game/LuaBundle/";
#else
    private const string AssetBundlePrefix = "luajit/";
    private const string AssetPrefxi = "Assets/Game/LuaBundleJit/";
#endif
    private Dictionary<string, string> lookup =
        new Dictionary<string, string>(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="LuaBundleLoader"/> class.
    /// </summary>
    public LuaBundleLoader()
    {
        LuaFileUtils.instance = this;
        this.beZip = false;
    }

    /// <summary>
    /// Setup the lua load table.
    /// </summary>
    public void SetupLoadTable()
    {
#if UNITY_EDITOR
        // if (AssetManager.Simulator.SimulateAssetBundle)
        // {
        //     return;
        // }
#endif

        // var manifest = AssetManager.Manifest;
        // if (manifest == null)
        // {
        //     Debug.LogError("SetupLoadTable failed: no asset bundle manifest.");
        //     return;
        // }

        // this.lookup.Clear();
        // var bundles = manifest.GetAllAssetBundles();
        // Assert.IsNotNull(bundles);
        // foreach (var bundle in bundles)
        // {
        //     if (bundle.StartsWith(AssetBundlePrefix))
        //     {
        //         var assetBundle = AssetManager.LoadBundleLocal(bundle);
        //         if (assetBundle == null)
        //         {
        //             Debug.LogWarningFormat(
        //                 "The bundle {0} is not existed.", bundle);
        //             continue;
        //         }

        //         var assetNames = assetBundle.GetAllAssetNames();
        //         foreach (var assetName in assetNames)
        //         {
        //             this.lookup.Add(assetName, bundle);
        //         }
        //     }
        // }
    }

    /// <summary>
    /// Prune all lua bundles.
    /// </summary>
    public void PruneLuaBundles()
    {
        var bundles = new HashSet<string>();
        foreach (var kv in this.lookup)
        {
            bundles.Add(kv.Value);
        }

        // foreach (var b in bundles)
        // {
        //     AssetManager.UnloadAsseBundle(b);
        // }
    }

    /// <inheritdoc/>
    public override byte[] ReadFile(string fileName)
    {
#if !UNITY_EDITOR
        return ReadAssetBundleFile(fileName);
#else
        return base.ReadFile(fileName);
#endif
    }

    private byte[] ReadAssetBundleFile(string fileName)
    {
        if (!fileName.EndsWith(".lua"))
        {
            fileName += ".lua";
        }

        if (!fileName.EndsWith(".bytes"))
        {
            fileName += ".bytes";
        }

        var filePath = AssetPrefxi + fileName;
        filePath = filePath.ToLower();
        var bundleName = string.Empty;
        if (!this.lookup.TryGetValue(filePath, out bundleName))
        {
            Debug.LogErrorFormat(
                "Load lua file failed: {0}, bundle is not existed.",
                fileName);
            return null;
        }

        var textAsset = AssetManager.LoadObject<TextAsset>(bundleName, filePath);        

        // var textAsset = AssetManager.LoadObjectLocal(
        //     bundleName, filePath, typeof(TextAsset)) as TextAsset;
        if (textAsset == null)
        {
            Debug.LogErrorFormat(
                "Load lua file failed: {0}, can not load asset fomr bundle.",
                fileName);
            return null;
        }

        var buffer = textAsset.bytes;
        Resources.UnloadAsset(textAsset);
        // AssetManager.UnloadAsseBundle(bundleName);
        return buffer;
    }
}
