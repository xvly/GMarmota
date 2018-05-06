namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public sealed class PriorityQueue<T> : IEnumerable<T>, IEnumerable
    {
        private T[] gparam_0;
        private IComparer<T> icomparer_0;
        [CompilerGenerated]
        private int int_0;

        public PriorityQueue() : this((IComparer<T>) null)
        {
        }

        public PriorityQueue(IComparer<T> comparer) : this(0x10, comparer)
        {
        }

        public PriorityQueue(int capacity) : this(capacity, null)
        {
        }

        public PriorityQueue(int capacity, IComparer<T> comparer)
        {
            this.icomparer_0 = (comparer != null) ? comparer : Comparer<T>.Default;
            this.gparam_0 = new T[capacity];
        }

        public void Clear()
        {
            this.Count = 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            // TODO:check
            return null;
            //return new Enumerator<T>(this.gparam_0, this.Count);
        }

        private void method_0(int int_1)
        {
            T x = this.gparam_0[int_1];
            for (int i = int_1 / 2; int_1 > 0; i /= 2)
            {
                if (this.icomparer_0.Compare(x, this.gparam_0[i]) <= 0)
                {
                    break;
                }
                this.gparam_0[int_1] = this.gparam_0[i];
                int_1 = i;
            }
            this.gparam_0[int_1] = x;
        }

        private void method_1(int int_1)
        {
            T x = this.gparam_0[int_1];
            for (int i = int_1 * 2; i < this.Count; i *= 2)
            {
                if (((i + 1) < this.Count) && (this.icomparer_0.Compare(this.gparam_0[i + 1], this.gparam_0[i]) > 0))
                {
                    i++;
                }
                if (this.icomparer_0.Compare(x, this.gparam_0[i]) >= 0)
                {
                    break;
                }
                this.gparam_0[int_1] = this.gparam_0[i];
                int_1 = i;
            }
            this.gparam_0[int_1] = x;
        }

        public T Pop()
        {
            int num;
            T local = this.Top();
            this.Count = num = this.Count - 1;
            this.gparam_0[0] = this.gparam_0[num];
            if (this.Count > 0)
            {
                this.method_1(0);
            }
            return local;
        }

        public void Push(T v)
        {
            int num;
            if (this.Count >= this.gparam_0.Length)
            {
                Array.Resize<T>(ref this.gparam_0, this.Count * 2);
            }
            this.gparam_0[this.Count] = v;
            this.Count = (num = this.Count) + 1;
            this.method_0(num);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public T Top()
        {
            if (this.Count <= 0)
            {
                throw new InvalidOperationException("The PriorityQueue is empty.");
            }
            return this.gparam_0[0];
        }

        public int Count
        {
            [CompilerGenerated]
            get
            {
                return this.int_0;
            }
            [CompilerGenerated]
            private set
            {
                this.int_0 = value;
            }
        }

        public T this[int index]
        {
            get
            {
                return this.gparam_0[index];
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator : IEnumerator, IDisposable, IEnumerator<T>
        {
            private readonly T[] gparam_0;
            private readonly int int_0;
            private int int_1;
            internal Enumerator(T[] gparam_1, int int_2)
            {
                this.gparam_0 = gparam_1;
                this.int_0 = int_2;
                this.int_1 = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            public T Current
            {
                get
                {
                    return this.gparam_0[this.int_1];
                }
            }
            public void Dispose()
            {
            }

            public void Reset()
            {
                this.int_1 = -1;
            }

            public bool MoveNext()
            {
                return ((this.int_1 <= this.int_0) && (++this.int_1 < this.int_0));
            }
        }
    }
}

