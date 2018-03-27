namespace GStd
{
    using ns0;
    using System;
    using System.Collections.Generic;

    public static class LogSystem
    {
        private static Class51 class51_0 = new Class51();
        private static Dictionary<string, Logger> dictionary_0 = new Dictionary<string, Logger>(StringComparer.Ordinal);

        public static void AddAppender(ILogAppender appender)
        {
            class51_0.method_1(appender);
        }

        public static void ClearAppenders()
        {
            class51_0.method_3();
        }

        public static Logger GetLogger(string moduleName)
        {
            Dictionary<string, Logger> dictionary = dictionary_0;
            lock (dictionary)
            {
                Logger logger;
                if (!dictionary_0.TryGetValue(moduleName, out logger))
                {
                    logger = new Logger(class51_0, moduleName);
                    dictionary_0.Add(moduleName, logger);
                }
                return logger;
            }
        }

        public static bool RemoveAppender(ILogAppender appender)
        {
            return class51_0.method_2(appender);
        }
    }
}

