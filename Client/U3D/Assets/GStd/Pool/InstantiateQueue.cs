namespace GStd
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public sealed class InstantiateQueue
    {
        private int int_0;
        private int int_1 = 1;
        private PriorityQueue<Struct5> priorityQueue_0 = new PriorityQueue<Struct5>(Class63.smethod_0());

        public InstantiateQueue()
        {
            Scheduler.FrameEvent += new Action(this.method_1);
        }

        ~InstantiateQueue()
        {
            Scheduler.FrameEvent -= new Action(this.method_1);
        }

        internal void method_0(GameObject gameObject_0, int int_2, Action<GameObject> action_0)
        {
            if (this.int_0 < this.InstantiateCountPerFrame)
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(gameObject_0);
                this.int_0++;
                action_0(obj2);
            }
            else
            {
                Struct5 v = new Struct5 {
                    gameObject_0 = gameObject_0,
                    action_0 = action_0
                };
                this.priorityQueue_0.Push(v);
            }
        }

        private void method_1()
        {
            try
            {
                while (this.priorityQueue_0.Count > 0)
                {
                    if (this.int_0 >= this.InstantiateCountPerFrame)
                    {
                        return;
                    }
                    Struct5 struct2 = this.priorityQueue_0.Pop();
                    GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(struct2.gameObject_0);
                    this.int_0++;
                    struct2.action_0(obj2);
                }
            }
            finally
            {
                this.int_0 = 0;
            }
        }

        public int InstantiateCountPerFrame
        {
            get
            {
                return this.int_1;
            }
            set
            {
                this.int_1 = value;
            }
        }

        private class Class63 : IComparer<InstantiateQueue.Struct5>
        {
            private static InstantiateQueue.Class63 class63_0;

            public int Compare(InstantiateQueue.Struct5 x, InstantiateQueue.Struct5 y)
            {
                return x.int_0.CompareTo(y.int_0);
            }

            public static InstantiateQueue.Class63 smethod_0()
            {
                if (class63_0 == null)
                {
                    class63_0 = new InstantiateQueue.Class63();
                }
                return class63_0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct5
        {
            public GameObject gameObject_0;
            public Action<GameObject> action_0;
            public int int_0;
        }
    }
}

