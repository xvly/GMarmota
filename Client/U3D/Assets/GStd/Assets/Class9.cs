namespace ns0
{
    using GStd;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Assertions;

    internal sealed class Class9
    {
        private AssetBundleManifest assetBundleManifest_0;
        private Class28 class28_0;
        private Dictionary<string, Class7> dictionary_0 = new Dictionary<string, Class7>(StringComparer.Ordinal);
        [CompilerGenerated]
        private static Func<string, Class7, bool> func_0;
        [CompilerGenerated]
        private static Func<KeyValuePair<string, Class7>, int> func_1;
        private LinkedList<Class7> linkedList_0 = new LinkedList<Class7>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("AssetBundleManager");
        private string[] string_0 = new string[0];

        internal Class9(Class28 class28_1)
        {
            this.class28_0 = class28_1;
        }

        internal string[] method_0()
        {
            return this.string_0;
        }

        internal void method_1(string[] string_1)
        {
            this.string_0 = string_1;
        }

        internal void method_10(string string_1)
        {
            Class7 class2;
            if (this.dictionary_0.TryGetValue(string_1, out class2))
            {
                class2.method_11();
                this.dictionary_0.Remove(string_1);
            }
        }

        internal bool method_11()
        {
            foreach (Class7 class2 in this.linkedList_0)
            {
                if (!class2.method_8())
                {
                    return true;
                }
            }
            return false;
        }

        internal void method_12()
        {
            foreach (KeyValuePair<string, Class7> pair in this.dictionary_0)
            {
                Class7 class2 = pair.Value;
                if (class2.method_3() != null)
                {
                    class2.method_3().Unload(false);
                }
            }
            this.linkedList_0.Clear();
            this.dictionary_0.Clear();
        }

        internal string[] method_13(string string_1)
        {
            Assert.IsNotNull<AssetBundleManifest>(this.assetBundleManifest_0);
            HashSet<string> set = new HashSet<string>(StringComparer.Ordinal);
            this.method_16(string_1, set);
            return set.ToArray<string>();
        }

        internal void method_14()
        {
            LinkedListNode<Class7> next;
            for (LinkedListNode<Class7> node = this.linkedList_0.First; node != null; node = next)
            {
                next = node.Next;
                if (node.Value.method_8())
                {
                    this.linkedList_0.Remove(node);
                }
            }
            if (func_0 == null)
            {
                func_0 = new Func<string, Class7, bool>(Class9.smethod_0);
            }
            this.dictionary_0.RemoveAll<string, Class7>(func_0);
        }

        internal void method_15()
        {
            if (func_1 == null)
            {
                func_1 = new Func<KeyValuePair<string, Class7>, int>(Class9.smethod_1);
            }
            IEnumerator<KeyValuePair<string, Class7>> enumerator = Enumerable.OrderBy<KeyValuePair<string, Class7>, int>(this.dictionary_0, func_1).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    KeyValuePair<string, Class7> current = enumerator.Current;
                    string key = current.Key;
                    Class7 class2 = current.Value;
                    if (!string.IsNullOrEmpty(class2.method_1()))
                    {
                        Color color = GUI.color;
                        GUI.color = Color.red;
                        EditorGUILayout.LabelField(string.Format("{0}: {1}", key, class2.method_1()), new GUILayoutOption[0]);
                        GUI.color = color;
                    }
                    else
                    {
                        string str3;
                        if (class2.method_0() > 0)
                        {
                            str3 = string.Format("{0} ({1})", key, class2.method_0());
                        }
                        else
                        {
                            str3 = string.Format("{0} ({1}): {2}", key, class2.method_0(), class2.method_7());
                        }
                        if (class2.method_3() == null)
                        {
                            str3 = str3 + "Loading...";
                        }
                        EditorGUILayout.BeginHorizontal(new GUILayoutOption[0]);
                        EditorGUILayout.LabelField(str3, new GUILayoutOption[0]);
                        if (GUILayout.Button("Unload", new GUILayoutOption[0]))
                        {
                            this.method_9(key);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
        }

        private void method_16(string string_1, HashSet<string> hashSet_0)
        {
            if (!hashSet_0.Contains(string_1))
            {
                Hash128 assetBundleHash = this.assetBundleManifest_0.GetAssetBundleHash(string_1);
                if (!this.class28_0.method_13(string_1, assetBundleHash))
                {
                    hashSet_0.Add(string_1);
                }
            }
            string[] allDependencies = this.assetBundleManifest_0.GetAllDependencies(string_1);
            for (int i = 0; i < allDependencies.Length; i++)
            {
                string item = this.method_19(allDependencies[i]);
                if (!hashSet_0.Contains(item))
                {
                    Hash128 hash2 = this.assetBundleManifest_0.GetAssetBundleHash(item);
                    if (!this.class28_0.method_13(item, hash2))
                    {
                        hashSet_0.Add(item);
                    }
                }
            }
        }

        private Class7 method_17(string string_1)
        {
            Class7 class2;
            if (this.dictionary_0.TryGetValue(string_1, out class2))
            {
                class2.method_9();
                return class2;
            }
            Assert.IsNotNull<AssetBundleManifest>(this.assetBundleManifest_0);
            Hash128 assetBundleHash = this.assetBundleManifest_0.GetAssetBundleHash(string_1);
            if (!assetBundleHash.isValid)
            {
                object[] args = new object[] { string_1 };
                logger_0.LogError("The bundle {0} is not record in the manifest.", args);
                return null;
            }
            AssetBundle bundle = this.class28_0.method_8(string_1, assetBundleHash);
            if (bundle == null)
            {
                return null;
            }
            class2 = new Class7(bundle);
            this.dictionary_0.Add(string_1, class2);
            string[] allDependencies = this.assetBundleManifest_0.GetAllDependencies(string_1);
            Class7[] classArray = new Class7[allDependencies.Length];
            for (int i = 0; i < allDependencies.Length; i++)
            {
                string str = this.method_19(allDependencies[i]);
                classArray[i] = this.method_17(str);
            }
            class2.method_6(classArray);
            class2.method_9();
            return class2;
        }

        private Class7 method_18(string string_1)
        {
            Class7 class2;
            if (this.dictionary_0.TryGetValue(string_1, out class2))
            {
                class2.method_9();
                return class2;
            }
            if (this.assetBundleManifest_0 == null)
            {
                object[] args = new object[] { string_1 };
                logger_0.LogError("The manifest is null, can not load {0}", args);
                return null;
            }
            Hash128 assetBundleHash = this.assetBundleManifest_0.GetAssetBundleHash(string_1);
            if (!assetBundleHash.isValid)
            {
                object[] objArray2 = new object[] { string_1 };
                logger_0.LogError("The bundle {0} is not record in the manifest.", objArray2);
                return null;
            }
            Class26 class3 = this.class28_0.method_11(string_1, assetBundleHash);
            if ((class3 == null) || !string.IsNullOrEmpty(class3.method_0()))
            {
                object[] objArray4 = new object[] { string_1, assetBundleHash };
                logger_0.LogError("Load bundle {0} with hash {1} is failed.", objArray4);
                return null;
            }
            class2 = new Class7(class3);
            this.dictionary_0.Add(string_1, class2);
            bool flag = false;
            string[] allDependencies = this.assetBundleManifest_0.GetAllDependencies(string_1);
            Class7[] classArray = new Class7[allDependencies.Length];
            for (int i = 0; i < allDependencies.Length; i++)
            {
                string str = this.method_19(allDependencies[i]);
                classArray[i] = this.method_18(str);
                if (classArray[i] == null)
                {
                    object[] objArray3 = new object[] { str };
                    logger_0.LogError("Missing dependency bundle {0}.", objArray3);
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                this.dictionary_0.Remove(string_1);
                return null;
            }
            class2.method_6(classArray);
            class2.method_9();
            this.linkedList_0.AddLast(class2);
            return class2;
        }

        private string method_19(string string_1)
        {
            string[] allAssetBundlesWithVariant = this.assetBundleManifest_0.GetAllAssetBundlesWithVariant();
            char[] separator = new char[] { '.' };
            string[] strArray2 = string_1.Split(separator);
            int num = 0x7fffffff;
            int index = -1;
            for (int i = 0; i < allAssetBundlesWithVariant.Length; i++)
            {
                char[] chArray2 = new char[] { '.' };
                string[] strArray3 = allAssetBundlesWithVariant[i].Split(chArray2);
                if (strArray3[0] == strArray2[0])
                {
                    int num4 = Array.IndexOf<string>(this.string_0, strArray3[1]);
                    if (num4 == -1)
                    {
                        num4 = 0x7ffffffe;
                    }
                    if (num4 < num)
                    {
                        num = num4;
                        index = i;
                    }
                }
            }
            if (num == 0x7ffffffe)
            {
                logger_0.LogWarning("Ambitious asset bundle variant chosen because there was no matching active variant: " + allAssetBundlesWithVariant[index]);
            }
            if (index != -1)
            {
                return allAssetBundlesWithVariant[index];
            }
            return string_1;
        }

        internal AssetBundleManifest method_2()
        {
            return this.assetBundleManifest_0;
        }

        internal void method_3(AssetBundleManifest assetBundleManifest_1)
        {
            this.assetBundleManifest_0 = assetBundleManifest_1;
        }

        internal Class7 method_4(string string_1)
        {
            Class7 class2;
            if (!this.dictionary_0.TryGetValue(string_1, out class2))
            {
                return null;
            }
            if (class2.method_1() == null)
            {
                if (class2.method_3() == null)
                {
                    return null;
                }
                Class7[] classArray = class2.method_5();
                if (classArray == null)
                {
                    return class2;
                }
                foreach (Class7 class3 in classArray)
                {
                    if (class3.method_1() != null)
                    {
                        class2.method_2(class3.method_1());
                        return class2;
                    }
                    if (class3.method_3() == null)
                    {
                        return null;
                    }
                }
            }
            return class2;
        }

        internal bool method_5(string string_1)
        {
            Class26 class2 = this.class28_0.method_9(string_1);
            if (class2 == null)
            {
                return false;
            }
            Class7 class3 = new Class7(class2);
            class3.method_9();
            this.dictionary_0.Add(string_1, class3);
            this.linkedList_0.AddLast(class3);
            return true;
        }

        internal bool method_6(string string_1)
        {
            Class26 class2 = this.class28_0.method_12(string_1);
            if (class2 == null)
            {
                return false;
            }
            Class7 class3 = new Class7(class2);
            class3.method_9();
            this.dictionary_0.Add(string_1, class3);
            this.linkedList_0.AddLast(class3);
            return true;
        }

        internal AssetBundle method_7(string string_1)
        {
            Class7 class2 = this.method_17(string_1);
            if (class2 != null)
            {
                return class2.method_3();
            }
            return null;
        }

        internal bool method_8(string string_1)
        {
            return (this.method_18(string_1) != null);
        }

        internal bool method_9(string string_1)
        {
            Class7 class2;
            if (!this.dictionary_0.TryGetValue(string_1, out class2))
            {
                return false;
            }
            if (class2.method_3() != null)
            {
                class2.method_10();
            }
            return true;
        }

        [CompilerGenerated]
        private static bool smethod_0(string string_1, Class7 class7_0)
        {
            return class7_0.method_12();
        }

        [CompilerGenerated]
        private static int smethod_1(KeyValuePair<string, Class7> keyValuePair_0)
        {
            return keyValuePair_0.Value.method_0();
        }
    }
}

