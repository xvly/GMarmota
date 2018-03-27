namespace GStd
{
    using System;
    using System.IO;
    using UnityEngine;

    public static class StreamingAssets
    {
        public static string ReadAllText(string filePath)
        {
            string path = Path.Combine(Application.streamingAssetsPath, filePath);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }
    }
}

