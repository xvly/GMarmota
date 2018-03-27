using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using GStd;
using System;

public class Main : MonoBehaviour {

    public static Main Instance { get; private set; }
    private void _DownloadStartLog(string url)
    {
        Debug.Log("##DownloadStart" + url);
    }
    private void _DownloadFinishLog(string url)
    {
        Debug.Log("##DownloadFinish" + url);
    }

    private void Awake()
    {
        Debug.Log("main start");

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

        // 初始化日志工具.
        LogSystem.AddAppender(new LogUnity());

        // 监听低内存更新
        Application.lowMemory -= this.OnLowMemory;
        Application.lowMemory += this.OnLowMemory;

        AssetManager.DownloadStartEvent += _DownloadStartLog;
        //url => Debug.Log("##DownloadStart: " + url);
        AssetManager.DownloadFinishEvent += _DownloadFinishLog;
        
    }

    public void OnLowMemory()
    {
        // Clear all cache.
        GameObjectPool.Instance.ClearAllUnused();
        PrefabPool.Instance.ClearAllUnused();
        ScriptablePool.Instance.ClearAllUnused();
        SpritePool.Instance.ClearAllUnused();

        // Clear lua memory.
        if (this.luaState != null)
            this.luaState.Collect();

        // Clear the unity memory.
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }


    public void Restart()
    {

    }

    private void OnDestroy()
    {
        
    }

    private void OnApplicationQuit()
    {
        
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

    IEnumerator StartGame()
    {
        // setup asset manager
        var waitMainfest = AssetManager.LoadLocalManifest("AssetBundle");
        yield return waitMainfest;
        if (waitMainfest.Error != null)
        {
            Debug.LogError(
                "Load local manifest failed: " + waitMainfest.Error);
            yield break;
        }

        AssetManager.IgnoreHashCheck = true;

        // 构造Lua脚本加载器.
        this.luaLoader = new LuaBundleLoader();
        this.luaLoader.SetupLoadTable();

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

        // 执行启动文件.
        try
        {
            this.luaState.LuaPushBoolean(Debug.isDebugBuild);
            this.luaState.LuaSetField(LuaIndexes.LUA_GLOBALSINDEX, "is_debug_build");
            this.luaState.LuaPop(1);

            this.luaState.DoFile("main.lua");

            this.luaUpdate = this.luaState.GetFunction("GameUpdate");
            this.luaStop = this.luaState.GetFunction("GameStop");
        }
        catch (LuaException exp)
        {
            Debug.LogError(exp.Message);
        }


        this.isRunning = true;

        // setup lua

        yield return null;
    }

    // Use this for initialization
    void Start () {
        Scheduler.RunCoroutine(this.StartGame());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private LuaState luaState;
    private LuaBundleLoader luaLoader;

    private LuaFunction luaUpdate;
    private LuaFunction luaStop;

    private bool isRunning = false;

}
