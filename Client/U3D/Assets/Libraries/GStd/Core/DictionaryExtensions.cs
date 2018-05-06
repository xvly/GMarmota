namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public static class DictionaryExtensions
    {
        public static void RemoveAll<K, V>(this Dictionary<K, V> dic, Func<K, V, bool> filter)
        {
            List<K> list = Class50<K>.smethod_0();
            Dictionary<K, V>.Enumerator enumerator = dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<K, V> current = enumerator.Current;
                if (filter(current.Key, current.Value))
                {
                    list.Add(current.Key);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                dic.Remove(list[i]);
            }
            list.Clear();
        }

        private static class Class50<T>
        {
            private static List<T> list_0;

            static Class50()
            {
                DictionaryExtensions.Class50<T>.list_0 = new List<T>();
            }

            public static List<T> smethod_0()
            {
                return DictionaryExtensions.Class50<T>.list_0;
            }
        }
    }
}

