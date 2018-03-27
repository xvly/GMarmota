namespace ns0
{
    using GStd;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    internal sealed class Class27
    {
        private HashSet<string> hashSet_0 = new HashSet<string>(StringComparer.Ordinal);
        private string string_0;

        internal Class27()
        {
            char[] separator = new char[] { '\n' };
            foreach (string str2 in StreamingAssets.ReadAllText("file_list.txt").Split(separator))
            {
                this.hashSet_0.Add(str2);
            }
        }

        internal string method_0()
        {
            if (string.IsNullOrEmpty(this.string_0))
            {
                this.string_0 = Path.Combine(Application.persistentDataPath, "AssetCache");
                if (!Directory.Exists(this.string_0))
                {
                    Directory.CreateDirectory(this.string_0);
                }
            }
            return this.string_0;
        }

        public string method_1()
        {
            string path = this.method_13("version.txt");
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return StreamingAssets.ReadAllText("AssetBundle/version.txt");
        }

        internal AssetBundle method_10(string string_1, Hash128 hash128_0, bool bool_0)
        {
            string str;
            if (bool_0)
            {
                str = this.method_17(string_1, hash128_0);
            }
            else
            {
                str = this.method_16(string_1, hash128_0);
            }
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return AssetBundle.LoadFromFile(str);
        }

        internal AssetBundleCreateRequest method_11(string string_1)
        {
            string str = this.method_15(string_1);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return AssetBundle.LoadFromFileAsync(str);
        }

        internal AssetBundleCreateRequest method_12(string string_1, Hash128 hash128_0, bool bool_0)
        {
            string str;
            if (bool_0)
            {
                str = this.method_17(string_1, hash128_0);
            }
            else
            {
                str = this.method_16(string_1, hash128_0);
            }
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return AssetBundle.LoadFromFileAsync(str);
        }

        private string method_13(string string_1)
        {
            return Path.Combine(this.method_0(), string_1);
        }

        private string method_14(string string_1, Hash128 hash128_0)
        {
            return (Path.Combine(this.method_0(), string_1) + "-" + hash128_0.ToString());
        }

        private string method_15(string string_1)
        {
            string path = this.method_13(string_1);
            if (File.Exists(path))
            {
                return path;
            }
            string str2 = Path.Combine("AssetBundle", string_1);
            if (this.method_18(str2))
            {
                return Path.Combine(Application.streamingAssetsPath, str2);
            }
            return string.Empty;
        }

        private string method_16(string string_1, Hash128 hash128_0)
        {
            string path = this.method_14(string_1, hash128_0);
            if (File.Exists(path))
            {
                return path;
            }
            string str2 = string_1 + "-" + hash128_0.ToString();
            string str3 = Path.Combine("AssetBundle", str2);
            if (this.method_18(str3))
            {
                return Path.Combine(Application.streamingAssetsPath, str3);
            }
            return string.Empty;
        }

        private string method_17(string string_1, Hash128 hash128_0)
        {
            string str = this.method_16(string_1, hash128_0);
            if (!string.IsNullOrEmpty(str))
            {
                return str;
            }
            string str2 = string.Empty;
            string path = this.method_13(string_1);
            string fileName = Path.GetFileName(string_1);
            string directoryName = Path.GetDirectoryName(path);
            if (Directory.Exists(directoryName))
            {
                string[] strArray = Directory.GetFiles(directoryName, string.Format("{0}-*", fileName), SearchOption.TopDirectoryOnly);
                if (strArray.Length > 0)
                {
                    return strArray[0];
                }
            }
            string str6 = Path.Combine("AssetBundle", string_1 + "-");
            string str7 = this.method_19(str6);
            if (string.IsNullOrEmpty(str7))
            {
                return str2;
            }
            return Path.Combine(Application.streamingAssetsPath, str7);
        }

        private bool method_18(string string_1)
        {
            string_1 = string_1.Replace(@"\", "/");
            return this.hashSet_0.Contains(string_1);
        }

        private string method_19(string string_1)
        {
            string_1 = string_1.Replace(@"\", "/");
            foreach (string str in this.hashSet_0)
            {
                if (str.StartsWith(string_1))
                {
                    return str;
                }
            }
            return string.Empty;
        }

        public void method_2(string string_1)
        {
            File.WriteAllText(Path.Combine(this.method_0(), "version.txt"), string_1);
        }

        internal bool method_3(string string_1, Hash128 hash128_0)
        {
            return !string.IsNullOrEmpty(this.method_16(string_1, hash128_0));
        }

        internal void method_4()
        {
            if (Directory.Exists(this.method_0()))
            {
                Directory.Delete(this.method_0(), true);
            }
        }

        internal Class8 method_5(string string_1)
        {
            return new Class8(string_1, this.method_13(string_1));
        }

        internal Class8 method_6(string string_1, Hash128 hash128_0)
        {
            return new Class8(string_1, this.method_14(string_1, hash128_0));
        }

        internal void method_7(string string_1)
        {
            string path = this.method_13(string_1);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        internal void method_8(string string_1, Hash128 hash128_0)
        {
            string path = this.method_14(string_1, hash128_0);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        internal AssetBundle method_9(string string_1)
        {
            string str = this.method_15(string_1);
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return AssetBundle.LoadFromFile(str);
        }
    }
}

