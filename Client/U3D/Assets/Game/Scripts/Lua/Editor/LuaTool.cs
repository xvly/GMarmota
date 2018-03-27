//------------------------------------------------------------------------------
// This file is part of MistLand project in GStd.
// Copyright © 2016-2016 GStd Technology Co., Ltd.
// All Right Reserved.
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using GStd.Editor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The lua tool used to build data for lua.
/// </summary>
public static class LuaTool
{
    [MenuItem("GStd/Lua/Build Bundle[Lua and jit]")]
    public static void BuildLuaAndJITBundle()
    {
        BuidLuaBundleImpl();
        BuidLuaBundleJitImpl();
    }

    //[MenuItem("GStd/Lua/Rebuild Bundle[Lua and jit]")]
    //public static void ReBuildLuaAndJITBundle()
    //{
    //    BuidLuaBundleImpl(true);
    //    BuidLuaBundleJitImpl(true);
    //}

    /// <summary>
    /// Compile the lua source file into bundle.
    /// </summary>
    [MenuItem("GStd/Lua/Build Bundle")]
    public static void BuidLuaBundle()
    {
        BuidLuaBundleImpl();
    }

    /// <summary>
    /// Compile the lua source file into bundle with jit.
    /// </summary>
    [MenuItem("GStd/Lua/Build Bundle Jit")]
    public static void BuidLuaBundleJit()
    {
        BuidLuaBundleJitImpl();
    }

    private static void ReadFilelist(string path, out Dictionary<string, string> filelist)
    {
        filelist = new Dictionary<string, string>();
        if (!File.Exists(path))
            return;

        using (StreamReader sr = new StreamReader(path, Encoding.UTF8))
        {
            if (sr != null)
            {
                string lineContent = sr.ReadLine();
                while (lineContent != null)
                {
                    var contents = lineContent.Split(',');
                    if (contents.Length == 2)
                    {
                        var absolutePath = Path.Combine(Application.dataPath, contents[0]);
                        if (File.Exists(absolutePath))
                            filelist[contents[0]] = contents[1];
                        else
                            UnityEngine.Debug.LogWarning("read file list, file " + absolutePath + " not found!");
                    }
                    lineContent = sr.ReadLine();
                }
            }
        }
    }

    private static void SaveFilelist(string path, Dictionary<string, string> filelist)
    {
        if (filelist == null)
            return;

        using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
        {
            foreach(var kv in filelist)
            {
                sw.WriteLine(string.Format("{0},{1}", kv.Key, kv.Value));
            }
        }
    }

    private static void BuidLuaBundleImpl()
    {
        var bundlePath = Path.Combine(
            Application.dataPath, "Game/LuaBundle");
        var luaFiles = Directory.GetFiles(
                LuaConst.luaDir, "*.lua", SearchOption.AllDirectories);
        var toluaFiles = Directory.GetFiles(
            LuaConst.toluaDir, "*.lua", SearchOption.AllDirectories);
        var filelistPath = Path.Combine(bundlePath, "filelist.csv");

        using (var progress = new ProgressIndicator("Build Bundle for lua.", luaFiles.Length + toluaFiles.Length + 1))
        {
            if (progress.Show("Start build bundle for lua."))
            {
                return;
            }

            Dictionary<string, string> filelist;
            ReadFilelist(filelistPath, out filelist);

            var sourceTable = new Dictionary<string, bool>(
                StringComparer.Ordinal);
            if (CompileLuaBytesFiles(
                LuaConst.luaDir,
                luaFiles,
                bundlePath,
                sourceTable,
                progress,
                filelist))
            {
                return;
            }

            if (CompileLuaBytesFiles(
                LuaConst.toluaDir,
                toluaFiles,
                bundlePath,
                sourceTable,
                progress,
                filelist))
            {
                return;
            }

            if (RemoveDeletedSource(bundlePath, sourceTable, progress, filelist))
            {
                return;
            }

            SaveFilelist(filelistPath, filelist);

            AssetDatabase.Refresh();
        }
    }

    private static void BuidLuaBundleJitImpl()
    {
        var bundlePath = Path.Combine(
            Application.dataPath, "Game/LuaBundleJit");
        var luaFiles = Directory.GetFiles(
                LuaConst.luaDir, "*.lua", SearchOption.AllDirectories);
        var toluaFiles = Directory.GetFiles(
            LuaConst.toluaDir, "*.lua", SearchOption.AllDirectories);
        var filelistPath = Path.Combine(bundlePath, "filelist.csv");

        using (var progress = new ProgressIndicator("Build Bundle for lua jit.", luaFiles.Length + toluaFiles.Length + 1))
        {
            if (progress.Show("Start build bundle for lua jit."))
            {
                return;
            }

            Dictionary<string, string> filelist;
            ReadFilelist(filelistPath, out filelist);

            var sourceTable = new Dictionary<string, bool>(
                StringComparer.Ordinal);
            if (CompileLuaJitBytesFiles(
                LuaConst.luaDir,
                luaFiles,
                bundlePath,
                sourceTable,
                progress,
                filelist))
            {
                return;
            }

            if (CompileLuaJitBytesFiles(
                LuaConst.toluaDir,
                toluaFiles,
                bundlePath,
                sourceTable,
                progress,
                filelist))
            {
                return;
            }

            if (RemoveDeletedSource(
                bundlePath,
                sourceTable,
                progress,
                filelist))
            {
                return;
            }

            SaveFilelist(filelistPath, filelist);

            AssetDatabase.Refresh();
        }
    }

    private static bool CompileLuaBytesFiles(
        string sourceDir,
        string[] sourceFiles,
        string destDir,
        Dictionary<string, bool> sourceTable,
        ProgressIndicator progress,
        Dictionary<string, string> filelist)
    {
        if (!Directory.Exists(sourceDir))
        {
            return true;
        }

        // Compile source files into destination directory.
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        var uriUnity = new Uri(Application.dataPath + "/");
        var uriSrc = new Uri(sourceDir + "/");
        foreach (var file in sourceFiles)
        {
            var uriFile = new Uri(file);
            var relativePath = uriSrc.MakeRelativeUri(uriFile).OriginalString;
            if (progress.Show("Compile: {0}", relativePath))
            {
                return true;
            }

            var relativeToUnity = uriUnity.MakeRelativeUri(uriFile).OriginalString;

            sourceTable.Add(relativePath, true);

            var dest = Path.Combine(destDir, relativePath + ".bytes");
            var dir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var srcLastWriteTime = (long)(File.GetLastWriteTime(file) - startTime).TotalSeconds;
            if (!filelist.ContainsKey(relativeToUnity) || 
                srcLastWriteTime > long.Parse(filelist[relativeToUnity]))
            {
                filelist[relativeToUnity] = srcLastWriteTime.ToString();
                DietLua(file, dest);
            }
            progress.Next();
        }

        return false;
    }

    private static bool CompileLuaJitBytesFiles(
        string sourceDir,
        string[] sourceFiles,
        string destDir,
        Dictionary<string, bool> sourceTable,
        ProgressIndicator progress,
        Dictionary<string, string> filelist)
    {
        if (!Directory.Exists(sourceDir))
        {
            return true;
        }

        // Compile source files into destination directory.
        var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        var uriUnity = new Uri(Application.dataPath + "/");
        var uriSrc = new Uri(sourceDir + "/");
        foreach (var file in sourceFiles)
        {
            var uriFile = new Uri(file);
            var relativePath = uriSrc.MakeRelativeUri(uriFile).OriginalString;
            if (progress.Show("Compile: {0}", relativePath))
            {
                return true;
            }

            var relativeToUnity = uriUnity.MakeRelativeUri(uriFile).OriginalString;

            sourceTable.Add(relativePath, true);

            var dest = Path.Combine(destDir, relativePath + ".bytes");
            var dir = Path.GetDirectoryName(dest);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var srcLastWriteTime = (long)(File.GetLastWriteTime(file) - startTime).TotalSeconds;
            if (!filelist.ContainsKey(relativeToUnity) ||
                srcLastWriteTime > long.Parse(filelist[relativeToUnity]))
            {
                filelist[relativeToUnity] = srcLastWriteTime.ToString();
                CompileLuaJit(file, dest);
            }

            progress.Next();
        }

        return false;
    }

    private static bool RemoveDeletedSource(
        string destDir,
        Dictionary<string, bool> sourceTable,
        ProgressIndicator progress,
        Dictionary<string, string> filelist)
    {
        var destFiles = Directory.GetFiles(
            destDir, "*.lua.bytes", SearchOption.AllDirectories);
        foreach (var file in destFiles)
        {
            var uri1 = new Uri(file);
            var uri2 = new Uri(destDir + "/");
            var relativePath = uri2.MakeRelativeUri(uri1).OriginalString;
            if (progress.Show("Check delete: {0}", relativePath))
            {
                return true;
            }

            var key = relativePath.Remove(relativePath.Length - 6, 6);
            if (!sourceTable.ContainsKey(key))
            {
                File.Delete(file);
                var metaFile = file.Remove(file.Length - 6, 6) + ".meta";
                if (File.Exists(metaFile))
                {
                    File.Delete(metaFile);
                }
            }

            if (filelist.ContainsKey(file))
                filelist.Remove(file);
        }

        progress.Next();
        return false;
    }

    private static void DietLua(string sourceFile, string targetFile)
    {
        var executable = Path.GetFullPath("Tools/lua.exe");
        var argument = string.Format(
            "LuaSrcDiet.lua --basic {0} -o {1}", sourceFile, targetFile);
        var workingDirectory = Path.GetFullPath("Tools/LuaSrcDiet-0.11.2");
        RunProcess(executable, argument, workingDirectory);
    }

    private static void CompileLuaJit(string sourceFile, string targetFile)
    {
        var workingDirectory = Path.GetFullPath("Tools");
        var relativePath = RelativePath(workingDirectory, sourceFile);

        var executable = Path.GetFullPath("Tools/luajit.exe");
        var argument = string.Format("-b -g {0} {1}", relativePath, targetFile);
        RunProcess(executable, argument, workingDirectory);
    }

    private static void RunProcess(
        string executable,
        string argument,
        string workingDirectory,
        Func<string, bool> filter = null)
    {
        var startInfo = new ProcessStartInfo(executable, argument);
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.StandardOutputEncoding = Encoding.UTF8;
        startInfo.StandardErrorEncoding = Encoding.UTF8;
        startInfo.WorkingDirectory = workingDirectory;

        using (var proc = Process.Start(startInfo))
        {
            proc.OutputDataReceived += (sender, e) =>
            {
                var text = e.Data.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    if (filter != null)
                    {
                        if (filter(text))
                        {
                            UnityEngine.Debug.Log(text);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log(text);
                    }
                }
            };

            proc.ErrorDataReceived += (sender, e) =>
            {
                var text = e.Data.Trim();
                if (!string.IsNullOrEmpty(text))
                {
                    if (filter != null)
                    {
                        if (filter(text))
                        {
                            UnityEngine.Debug.LogError(text);
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(text);
                    }
                }
            };

            proc.BeginOutputReadLine();
            proc.WaitForExit();
            proc.Close();
        }
    }

    private static bool IsSubPathOf(this string path, string basePath)
    {
        var normalizedPath = Path.GetFullPath(
            path.Replace('/', '\\'));
        var normalizedBasePath = Path.GetFullPath(
            basePath.Replace('/', '\\'));

        return normalizedPath.StartsWith(
            normalizedBasePath, StringComparison.OrdinalIgnoreCase);
    }

    private static string RelativePath(
        string absolutePath, string relativeTo)
    {
        var absDirs = absolutePath.Split('\\', '/');
        var relDirs = relativeTo.Split('\\', '/');

        // Get the shortest of the two paths
        int length = Mathf.Min(absDirs.Length, relDirs.Length);

        // Find common root
        int lastCommonRoot = -1;
        for (int i = 0; i < length; ++i)
        {
            if (absDirs[i] == relDirs[i])
            {
                lastCommonRoot = i;
            }
            else
            {
                break;
            }
        }

        // If we didn't find a common prefix then throw
        if (lastCommonRoot == -1)
        {
            throw new ArgumentException("Paths do not have a common base");
        }

        //Build up the relative path
        var relativePath = new StringBuilder();

        // Add on the ..
        for (int i = lastCommonRoot + 1; i < absDirs.Length; ++i)
        {
            if (absDirs[i].Length > 0)
            {
                relativePath.Append("../");
            }
        }

        // Add on the folders
        for (int i = lastCommonRoot + 1; i < relDirs.Length - 1; ++i)
        {
            relativePath.Append(relDirs[i] + "/");
        }

        relativePath.Append(relDirs[relDirs.Length - 1]);
        return relativePath.ToString();
    }
}
