﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class GStd_Asset_AssetManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginStaticLibs("AssetManager");
		L.RegFunction("SetupSimulateLoader", SetupSimulateLoader);
		L.RegFunction("SetupABLoader", SetupABLoader);
		L.RegFunction("LoadObject", LoadObject);
		L.RegFunction("IsAssetBundleCache", IsAssetBundleCache);
		L.RegFunction("UnloadAssetBundle", UnloadAssetBundle);
		L.EndStaticLibs();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetupSimulateLoader(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			GStd.Asset.AssetManager.SetupSimulateLoader();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetupABLoader(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			GStd.Asset.AssetManager.SetupABLoader();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadObject(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			string arg0 = ToLua.CheckString(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			System.Type arg2 = ToLua.CheckMonoType(L, 3);
			UnityEngine.Object o = GStd.Asset.AssetManager.LoadObject(arg0, arg1, arg2);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAssetBundleCache(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			bool o = GStd.Asset.AssetManager.IsAssetBundleCache(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadAssetBundle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			string arg0 = ToLua.CheckString(L, 1);
			GStd.Asset.AssetManager.UnloadAssetBundle(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
