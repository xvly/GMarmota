namespace ns0
{
    using GStd;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal sealed class Class28
    {
        [CompilerGenerated]
        private bool bool_0;
        private Class27 class27_0;
        private Class6 class6_0;
        private LinkedList<Class5> linkedList_0 = new LinkedList<Class5>();
        [CompilerGenerated]
        private string string_0;
        [CompilerGenerated]
        private string string_1;

        internal Class28(Class27 class27_1, Class6 class6_1)
        {
            this.class27_0 = class27_1;
            this.class6_0 = class6_1;
        }

        [CompilerGenerated]
        internal string method_0()
        {
            return this.string_0;
        }

        [CompilerGenerated]
        internal void method_1(string string_2)
        {
            this.string_0 = string_2;
        }

        internal Class26 method_10(string string_2, Hash128 hash128_0)
        {
            AssetBundleCreateRequest request = this.class27_0.method_12(string_2, hash128_0, this.method_4());
            if (request == null)
            {
                return null;
            }
            Class5 class2 = new Class5(this.class27_0, request, string_2, hash128_0);
            this.linkedList_0.AddLast(class2);
            return new Class26(class2);
        }

        internal Class26 method_11(string string_2, Hash128 hash128_0)
        {
            WaitForSecondsRealtime realtime;
            if (this.method_4())
            {
                return this.method_10(string_2, hash128_0);
            }
            if (this.class27_0.method_3(string_2, hash128_0))
            {
                return this.method_10(string_2, hash128_0);
            }
            using (LinkedList<Class5>.Enumerator enumerator = this.linkedList_0.GetEnumerator())
            {
                Class5 current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if ((current.method_4() == string_2) && (current.method_5() == hash128_0))
                    {
                        goto Label_0067;
                    }
                }
                goto Label_0084;
            Label_0067:
                current.method_12(false);
                return new Class26(current);
            }
        Label_0084:
            realtime = AssetManager.Simulator.method_3();
            if (realtime != null)
            {
                return new Class26(realtime);
            }
            string str = this.method_16(string_2, hash128_0.ToString());
            Class5 class3 = new Class5(false, this.class6_0, str, this.class27_0, string_2, hash128_0);
            this.linkedList_0.AddLast(class3);
            this.class6_0.method_6(class3);
            return new Class26(class3);
        }

        internal Class26 method_12(string string_2)
        {
            string str;
            using (LinkedList<Class5>.Enumerator enumerator = this.linkedList_0.GetEnumerator())
            {
                Class5 current;
                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                    if (string.Equals(current.method_4(), string_2))
                    {
                        goto Label_0030;
                    }
                }
                goto Label_004D;
            Label_0030:
                current.method_12(false);
                return new Class26(current);
            }
        Label_004D:
            str = this.method_16(string_2, this.method_2());
            Class5 class3 = new Class5(false, this.class6_0, str, this.class27_0, string_2);
            this.linkedList_0.AddLast(class3);
            this.class6_0.method_6(class3);
            return new Class26(class3);
        }

        internal bool method_13(string string_2, Hash128 hash128_0)
        {
            return this.class27_0.method_3(string_2, hash128_0);
        }

        internal void method_14()
        {
            foreach (Class5 class2 in this.linkedList_0)
            {
                if (class2.method_13())
                {
                    class2.method_12(true);
                }
            }
        }

        internal void method_15()
        {
            LinkedListNode<Class5> next;
            for (LinkedListNode<Class5> node = this.linkedList_0.First; node != null; node = next)
            {
                next = node.Next;
                if (!node.Value.method_15())
                {
                    this.linkedList_0.Remove(node);
                }
            }
        }

        private string method_16(string string_2, string string_3)
        {
            if (string.IsNullOrEmpty(this.method_0()))
            {
                return string.Empty;
            }
            return string.Format("{0}/{1}?v={2}", this.method_0(), string_2, string_3);
        }

        [CompilerGenerated]
        internal string method_2()
        {
            return this.string_1;
        }

        [CompilerGenerated]
        internal void method_3(string string_2)
        {
            this.string_1 = string_2;
        }

        [CompilerGenerated]
        internal bool method_4()
        {
            return this.bool_0;
        }

        [CompilerGenerated]
        internal void method_5(bool bool_1)
        {
            this.bool_0 = bool_1;
        }

        internal WaitUpdateAssetBundle method_6(string string_2, Hash128 hash128_0)
        {
            if (!hash128_0.isValid)
            {
                return new WaitUpdateAssetBundle("Bundle hash is invalid.");
            }
            if (this.class27_0.method_3(string_2, hash128_0))
            {
                return new WaitUpdateAssetBundle();
            }
            foreach (Class5 class2 in this.linkedList_0)
            {
                if ((class2.method_4() == string_2) && (class2.method_5() == hash128_0))
                {
                    return new WaitUpdateAssetBundle(class2);
                }
            }
            WaitForSecondsRealtime realtime = AssetManager.Simulator.method_3();
            if (realtime != null)
            {
                return new WaitUpdateAssetBundle(realtime);
            }
            string str = this.method_16(string_2, hash128_0.ToString());
            Class5 class3 = new Class5(true, this.class6_0, str, this.class27_0, string_2, hash128_0);
            this.linkedList_0.AddLast(class3);
            this.class6_0.method_6(class3);
            return new WaitUpdateAssetBundle(class3);
        }

        internal AssetBundle method_7(string string_2)
        {
            return this.class27_0.method_9(string_2);
        }

        internal AssetBundle method_8(string string_2, Hash128 hash128_0)
        {
            return this.class27_0.method_10(string_2, hash128_0, this.method_4());
        }

        internal Class26 method_9(string string_2)
        {
            AssetBundleCreateRequest request = this.class27_0.method_11(string_2);
            if (request == null)
            {
                return null;
            }
            Class5 class2 = new Class5(this.class27_0, request, string_2);
            this.linkedList_0.AddLast(class2);
            return new Class26(class2);
        }
    }
}

