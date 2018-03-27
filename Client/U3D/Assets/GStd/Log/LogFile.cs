namespace GStd
{
    using System;
    using System.IO;
    using UnityEngine;

    public sealed class LogFile : ILogAppender
    {
        private StreamWriter streamWriter_0;

        public LogFile()
        {
            string persistentDataPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    persistentDataPath = Application.dataPath + "/..";
                    break;

                case RuntimePlatform.IPhonePlayer:
                    persistentDataPath = Application.persistentDataPath;
                    break;

                case RuntimePlatform.Android:
                    persistentDataPath = Application.persistentDataPath;
                    break;

                case RuntimePlatform.OSXEditor:
                    persistentDataPath = Application.dataPath + "/..";
                    break;

                case RuntimePlatform.WindowsPlayer:
                    persistentDataPath = Application.dataPath;
                    break;
            }
            string fullPath = Path.GetFullPath(persistentDataPath + "/Log");
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string str3 = fullPath + "/DiagnosisLog.txt";
            this.method_0(str3);
        }

        public LogFile(string path)
        {
            this.method_0(path);
        }

        public void AppendLog(LogItem item)
        {
            object[] args = new object[] { item.Severity, item.ModuleName, item.RecordTime, item.Message };
            string str = string.Format("[{0}][{1}][{2}]: {3}", args);
            StreamWriter writer = this.streamWriter_0;
            lock (writer)
            {
                this.streamWriter_0.WriteLine(str);
            }
        }

        private void method_0(string string_0)
        {
            this.streamWriter_0 = new StreamWriter(string_0);
            this.streamWriter_0.AutoFlush = true;
        }
    }
}

