using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using GStd;
using GStd.Asset;
using System;

public class Main : MonoBehaviour {

    public static Main Instance { get; private set; }

    
    private LuaState luaState;
    private LuaBundleLoader luaLoader;
    private LuaLooper luaLooper;

    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        // 监听低内存更新
        Application.lowMemory -= this.OnLowMemory;
        Application.lowMemory += this.OnLowMemory;
    }

    public void OnLowMemory()
    {
        // Clear all cache.
        AssetManager.Clear();

        // Clear lua memory.
        if (this.luaState != null)
            this.luaState.Collect();

        // Clear the unity memory.
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int LuaOpen_Socket_Core(IntPtr l)
    {
        return LuaDLL.luaopen_socket_core(l);
    }

    [MonoPInvokeCallback(typeof(LuaCSFunction))]
    private static int LuaOpen_Mime_Core(IntPtr l)
    {
        return LuaDLL.luaopen_mime_core(l);
    }

    private void OpenLuaSocket()
    {
        LuaConst.openLuaSocket = true;
        this.luaState.BeginPreLoad();
        this.luaState.RegFunction("socket.core", LuaOpen_Socket_Core);
        this.luaState.RegFunction("mime.core", LuaOpen_Mime_Core);
        this.luaState.EndPreLoad();
    }

    private void OpenCJson()
    {
        this.luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        this.luaState.OpenLibs(LuaDLL.luaopen_cjson);
        this.luaState.LuaSetField(-2, "cjson");

        this.luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
        this.luaState.LuaSetField(-2, "cjson.safe");
    }

    void InitLua()
    {
        // 构造Lua脚本加载器.
        this.luaLoader = new LuaBundleLoader();
        // this.luaLoader.SetupLoadTable();

        // 初始化Lua虚拟机.
        this.luaState = new LuaState();
        this.luaState.Start();
        this.luaState.OpenLibs(LuaDLL.luaopen_struct);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
        this.OpenLuaSocket();
        this.OpenCJson();

        LuaBinder.Bind(this.luaState);
        DelegateFactory.Init();
        LuaCoroutine.Register(this.luaState, this);
        LuaLog.OpenLibs(this.luaState);

        luaState.LuaSetTop(0);
        this.luaLooper = this.gameObject.AddComponent<LuaLooper>();
        this.luaLooper.luaState = this.luaState;

        // 执行启动文件.
        try
        {
            this.luaState.LuaPushBoolean(Debug.isDebugBuild);
            this.luaState.LuaSetField(LuaIndexes.LUA_GLOBALSINDEX, "_isDebugBuild");
            this.luaState.LuaPop(1);

            this.luaState.DoFile("Init/Main.lua");
        }
        catch (LuaException exp)
        {
            Debug.LogError(exp.Message);
        }
    }

    IEnumerator StartGame()
    {
        AssetManager.Setup();

        this.InitLua();

        this.isRunning = true;

        yield return null;
    }

    void Start () {
        StartCoroutine(this.StartGame());
    }
}
