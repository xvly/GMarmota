namespace ns0
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class Class6
    {
        private Action<string> action_0;
        private Action<string> action_1;
        private const int int_0 = 4;
        private int int_1 = 4;
        private LinkedList<Class5> linkedList_0 = new LinkedList<Class5>();
        private Queue<Class5> queue_0 = new Queue<Class5>();
        private Stack<byte[]> stack_0 = new Stack<byte[]>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void method_0(Action<string> action_2)
        {
            this.action_0 = (Action<string>) Delegate.Combine(this.action_0, action_2);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void method_1(Action<string> action_2)
        {
            this.action_0 = (Action<string>) Delegate.Remove(this.action_0, action_2);
        }

        private void method_10(byte[] byte_0)
        {
            if (this.stack_0.Count < 4)
            {
                this.stack_0.Push(byte_0);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void method_2(Action<string> action_2)
        {
            this.action_1 = (Action<string>) Delegate.Combine(this.action_1, action_2);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void method_3(Action<string> action_2)
        {
            this.action_1 = (Action<string>) Delegate.Remove(this.action_1, action_2);
        }

        internal int method_4()
        {
            return this.int_1;
        }

        internal void method_5(int int_2)
        {
            this.int_1 = int_2;
        }

        internal void method_6(Class5 class5_0)
        {
            if (this.linkedList_0.Count < this.int_1)
            {
                this.method_8(class5_0);
            }
            else
            {
                this.queue_0.Enqueue(class5_0);
            }
        }

        internal void method_7(Class5 class5_0)
        {
            this.linkedList_0.Remove(class5_0);
            if (this.action_1 != null)
            {
                this.action_1(class5_0.method_6().url);
            }
            class5_0.method_6().Dispose();
            while (this.linkedList_0.Count < this.int_1)
            {
                if (this.queue_0.Count <= 0)
                {
                    break;
                }
                Class5 class2 = this.queue_0.Dequeue();
                this.method_8(class2);
            }
        }

        private void method_8(Class5 class5_0)
        {
            class5_0.method_14();
            this.linkedList_0.AddLast(class5_0);
            if (this.action_0 != null)
            {
                this.action_0(class5_0.method_6().url);
            }
        }

        private byte[] method_9()
        {
            if (this.stack_0.Count > 0)
            {
                return this.stack_0.Pop();
            }
            return new byte[0x1000];
        }
    }
}

