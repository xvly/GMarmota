namespace GStd
{
    using ns0;
    using System;
    using System.Diagnostics;
    using UnityEngine;

    public sealed class Logger
    {
        private Class51 class51_0;
        private string string_0;

        internal Logger(Class51 class51_1, string string_1)
        {
            this.class51_0 = class51_1;
            this.string_0 = string_1;
        }

        public void Log(LogSeverity severity, string message)
        {
            LogItem item = new LogItem {
                ModuleName = this.string_0,
                Severity = severity,
                RecordTime = DateTime.Now,
                Message = message
            };
            this.class51_0.method_0(item);
        }

        public void Log(LogSeverity severity, string format, params object[] args)
        {
            this.Log(severity, string.Format(format, args));
        }

        public void Log(LogSeverity severity, UnityEngine.Object context, string message)
        {
            LogItem item = new LogItem {
                ModuleName = this.string_0,
                Severity = severity,
                RecordTime = DateTime.Now,
                Message = message,
                Context = context
            };
            this.class51_0.method_0(item);
        }

        public void Log(LogSeverity severity, UnityEngine.Object context, string format, params object[] args)
        {
            this.Log(severity, context, string.Format(format, args));
        }

        [Conditional("DEBUG")]
        public void LogDebug(string message)
        {
            this.Log(LogSeverity.Debug, message);
        }

        [Conditional("DEBUG")]
        public void LogDebug(string format, params object[] args)
        {
            this.Log(LogSeverity.Debug, format, args);
        }

        [Conditional("DEBUG")]
        public void LogDebug(UnityEngine.Object context, string message)
        {
            this.Log(LogSeverity.Debug, context, message);
        }

        [Conditional("DEBUG")]
        public void LogDebug(UnityEngine.Object context, string format, params object[] args)
        {
            this.Log(LogSeverity.Debug, context, format, args);
        }

        public void LogError(Exception e)
        {
            this.Log(LogSeverity.Error, e.ToString());
        }

        public void LogError(string message)
        {
            this.Log(LogSeverity.Error, message);
        }

        public void LogError(string format, params object[] args)
        {
            this.Log(LogSeverity.Error, format, args);
        }

        public void LogError(UnityEngine.Object context, Exception e)
        {
            this.Log(LogSeverity.Error, context, e.ToString());
        }

        public void LogError(UnityEngine.Object context, string message)
        {
            this.Log(LogSeverity.Error, context, message);
        }

        public void LogError(UnityEngine.Object context, string format, params object[] args)
        {
            this.Log(LogSeverity.Error, context, format, args);
        }

        public void LogInfo(string message)
        {
            this.Log(LogSeverity.Info, message);
        }

        public void LogInfo(string format, params object[] args)
        {
            this.Log(LogSeverity.Info, format, args);
        }

        public void LogInfo(UnityEngine.Object context, string message)
        {
            this.Log(LogSeverity.Info, context, message);
        }

        public void LogInfo(UnityEngine.Object context, string format, params object[] args)
        {
            this.Log(LogSeverity.Info, context, format, args);
        }

        public void LogWarning(string message)
        {
            this.Log(LogSeverity.Warning, message);
        }

        public void LogWarning(string format, params object[] args)
        {
            this.Log(LogSeverity.Warning, format, args);
        }

        public void LogWarning(UnityEngine.Object context, string message)
        {
            this.Log(LogSeverity.Warning, context, message);
        }

        public void LogWarning(UnityEngine.Object context, string format, params object[] args)
        {
            this.Log(LogSeverity.Warning, context, format, args);
        }
    }
}

