using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

/* 日志处理 */
public class LogHooker : MonoBehaviour
{
    public bool IsNormal = false;
    public bool IsWarning = true;
    public bool IsError = true;

    public bool isShowScreenLog = false;
    public bool isScreenRichText = false;

    #region print screen
    public int ScreenTextLineCount = 10;
    public GUIStyle ScreenTextStyle;
    public Vector2 ScreenTextPosition = new Vector2(0, 20);
    private string mPrintText;
    private List<string> mLogToPrint = new List<string>();
    void PreparePrint(string txt)
    {
        if (ScreenTextLineCount <= 0)
            return;

        mLogToPrint.Add(txt);

        mPrintText = "";
        for (int i = 0, max = mLogToPrint.Count; i < max; i++)
            mPrintText += mLogToPrint[i] + "\n";
    }

    void UpdateText()
    {
        if (ScreenTextLineCount <= 0)
            return;

        if (mLogToPrint.Count >= ScreenTextLineCount - 1)
            mLogToPrint.RemoveRange(0, mLogToPrint.Count - ScreenTextLineCount + 1);
    }
    void GUILog()
    {
        if (ScreenTextLineCount <= 0)
            return;

        if (isShowScreenLog)
        {
            GUILayout.BeginArea(new Rect(ScreenTextPosition.x, ScreenTextPosition.y, Screen.width - ScreenTextPosition.x, Screen.height - ScreenTextPosition.y));
            GUILayout.Label(mPrintText, ScreenTextStyle);
            GUILayout.EndArea();
        }
    }
    #endregion

    #region write file
    public bool IsSaveLocal = true;    // 是否保存到本地
    public bool IsSaveStack = true;    // 是否保存堆栈信息

    StreamWriter mSWException;

    void CreateLogWriter()
    {
        if (!IsSaveLocal)
            return;

        string logDirectory = Application.persistentDataPath + "/log";

        // 检测目录
        if (!Directory.Exists(logDirectory))
            Directory.CreateDirectory(logDirectory);

        // 生成文件
        DateTime dtNow = DateTime.Now;
        string filePath = logDirectory + "/" + dtNow.DayOfWeek + ".txt";
        FileInfo fi = new FileInfo(filePath);
        if (fi.Exists && (dtNow - fi.CreationTime < TimeSpan.FromDays(1))) // 文件已经存在，并且是同一天内，才做添加操作
            mSWException = fi.AppendText();
        else
            mSWException = fi.CreateText();

        Debug.Log("(log writer)path = " + filePath);
    }
    void DestroyLogWriter()
    {
        if (mSWException == null)
            return;

        mSWException.Close();
        mSWException.Dispose();
    }
    void WriteLog(string txt)
    {
        if (!IsSaveLocal)
            return;
        if (mSWException == null)
            return;

        mSWException.Write(txt);
        mSWException.Flush();
    }
    #endregion

    #region post http
    public bool IsPost = false;
    public string PostUrl = "http://test.com/gameLog.php";
    public string PostFunc = "crashLog";

    void Post(string txt)
    {
        if (!IsPost)
            return;

        txt = txt.Replace("\n", "<br>");

        // make json
        string sJson = "";

        StartCoroutine(PostEnumerator(sJson)); // TODO 不要每次都开协程，会很多内存碎片
    }

    IEnumerator PostEnumerator(string logtxt)
    {
        string url = PostUrl;

        WWWForm form = new WWWForm();
        form.AddField("func", PostFunc);
        form.AddField("errorlog", logtxt);

        WWW w = new WWW(url, form);
        yield return w;

        if (w.error != null)
        {
            Debug.LogError("logger post error, exception = " + w.error + ",url = " + url);
            yield break;
        }
    }
    #endregion

    void OnLog(string condition, string stackTrace, LogType t)
    {
        // 过滤
        if (t == LogType.Log && !IsNormal)
            return;
        else if (t == LogType.Warning && !IsWarning)
            return;
        if (t == LogType.Error && !IsError)
            return;

        // 显示到屏幕        
        string txtPrint = "";
        if (isScreenRichText)
        {
            if (t == LogType.Error || t == LogType.Exception)
                txtPrint = "<color=#ff0000>";
            else if (t == LogType.Warning)
                txtPrint = "<color=#ffff00>";

            txtPrint += "[" + t.ToString() + "]" + condition;

            if (t == LogType.Warning || t == LogType.Error || t == LogType.Exception)
                txtPrint += "</color>";
        }
        else
            txtPrint = "[" + t.ToString() + "]" + condition;
        PreparePrint(txtPrint);

        // 写文件
        string txt = "";
        if (IsSaveStack)
            txt = DateTime.Now.ToShortTimeString() + "[" + t + "]\ncon:" + condition + "\n\nst:" + stackTrace + "\n\n";
        else
            txt = DateTime.Now.ToShortTimeString() + "[" + t + "]\ncon:" + condition + "\n\n";
        WriteLog(txt);

        // 发送http
        Post(txt);

        condition = "[]" + condition;
    }

    void OnEnable()
    {
        CreateLogWriter();
        Application.logMessageReceived += OnLog;
    }

    void OnDisable()
    {
        DestroyLogWriter();
        Application.logMessageReceived -= OnLog;
    }

    void Update()
    {
        UpdateText();
    }

    void OnGUI()
    {
        GUILog();
    }
}