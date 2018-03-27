namespace GStd
{
    using System;

    public interface ILogAppender
    {
        void AppendLog(LogItem item);
    }
}

