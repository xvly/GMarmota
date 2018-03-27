namespace ns0
{
    using GStd;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal sealed class Class51
    {
        private List<ILogAppender> list_0 = new List<ILogAppender>();

        internal void method_0(LogItem logItem_0)
        {
            foreach (ILogAppender appender in this.list_0)
            {
                appender.AppendLog(logItem_0);
            }
            if (this.list_0.Count == 0)
            {
                switch (logItem_0.Severity)
                {
                    case LogSeverity.Warning:
                        Debug.LogWarning(logItem_0.Message, logItem_0.Context);
                        return;

                    case LogSeverity.Error:
                        Debug.LogError(logItem_0.Message, logItem_0.Context);
                        return;
                }
                Debug.Log(logItem_0.Message, logItem_0.Context);
            }
        }

        internal void method_1(ILogAppender ilogAppender_0)
        {
            this.list_0.Add(ilogAppender_0);
        }

        internal bool method_2(ILogAppender ilogAppender_0)
        {
            return this.list_0.Remove(ilogAppender_0);
        }

        internal void method_3()
        {
            this.list_0.Clear();
        }
    }
}

