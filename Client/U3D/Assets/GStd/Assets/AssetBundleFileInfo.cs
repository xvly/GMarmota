namespace GStd
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Networking;

    public sealed class AssetBundleFileInfo
    {
        private bool bool_0;
        private Dictionary<string, int> dictionary_0 = new Dictionary<string, int>(StringComparer.Ordinal);
        private WaitLoadFileInfo waitLoadFileInfo_0;

        public int GetSize(string name)
        {
            int num;
            if (this.dictionary_0.TryGetValue(name, out num))
            {
                return num;
            }
            return 0;
        }

        internal bool method_0()
        {
            return this.bool_0;
        }

        internal bool method_1()
        {
            return (this.waitLoadFileInfo_0 != null);
        }

        internal WaitLoadFileInfo method_2(string string_0)
        {
            if (this.waitLoadFileInfo_0 == null)
            {
                if (this.method_0())
                {
                    return new WaitLoadFileInfo(this);
                }
                UnityWebRequest request = UnityWebRequest.Get(string_0);
                AsyncOperation operation = request.Send();
                this.waitLoadFileInfo_0 = new WaitLoadFileInfo(this, request, operation);
            }
            return this.waitLoadFileInfo_0;
        }

        internal void method_3()
        {
            Assert.IsNotNull<WaitLoadFileInfo>(this.waitLoadFileInfo_0);
            this.waitLoadFileInfo_0 = null;
        }

        internal bool method_4(string string_0)
        {
            this.dictionary_0.Clear();
            char[] separator = new char[] { '\n', ' ' };
            string[] strArray = string_0.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            if ((strArray.Length % 2) != 0)
            {
                return false;
            }
            for (int i = 0; i < strArray.Length; i += 2)
            {
                int num2;
                string key = strArray[i];
                if (!int.TryParse(strArray[i + 1], out num2))
                {
                    return false;
                }
                this.dictionary_0.Add(key, num2);
            }
            this.bool_0 = true;
            return true;
        }
    }
}

