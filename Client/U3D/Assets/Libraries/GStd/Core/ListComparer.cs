namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public sealed class ListComparer<T> : IEqualityComparer<List<T>>
    {
        private static volatile ListComparer<T> listComparer_0;

        public bool Equals(List<T> x, List<T> y)
        {
            return x.SequenceEqual<T>(y);
        }

        public int GetHashCode(List<T> obj)
        {
            int num = 0;
            foreach (T local in obj)
            {
                num ^= local.GetHashCode();
            }
            return num;
        }

        public static ListComparer<T> Default
        {
            get
            {
                if (ListComparer<T>.listComparer_0 == null)
                {
                    ListComparer<T>.listComparer_0 = new ListComparer<T>();
                }
                return ListComparer<T>.listComparer_0;
            }
        }
    }
}

