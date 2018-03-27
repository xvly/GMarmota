namespace GStd
{
    using System;
    using UnityEngine;

    public sealed class LogUnity : ILogAppender
    {
        public void AppendLog(LogItem item)
        {
            string message = string.Format("[{0}][{1}]: {2}", item.ModuleName, item.RecordTime, item.Message);
            switch (item.Severity)
            {
                case LogSeverity.Debug:
                case LogSeverity.Info:
                    Debug.Log(message);
                    return;

                case LogSeverity.Warning:
                    Debug.LogWarning(message);
                    return;
            }
            Debug.LogError(message);
        }
    }
}

