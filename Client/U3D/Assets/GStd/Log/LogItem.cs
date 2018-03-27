namespace GStd
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct LogItem
    {
        public string ModuleName;
        public LogSeverity Severity;
        public DateTime RecordTime;
        public string Message;
        public UnityEngine.Object Context;
    }
}

