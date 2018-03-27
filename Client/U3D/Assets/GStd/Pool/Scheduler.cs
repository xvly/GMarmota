namespace GStd
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [DisallowMultipleComponent]
    public sealed class Scheduler : MonoBehaviour
    {
        private static Action action_0;

        private static List<Action> list_0 = new List<Action>();
        private static List<Action> list_1 = new List<Action>();
        private static List<Action> list_2 = new List<Action>();
        private static List<Struct14> list_3 = new List<Struct14>();
        private static GStd.Logger logger_0 = LogSystem.GetLogger("Scheduler");
        private static Scheduler scheduler_0;

        public static  event Action FrameEvent;

        private void Awake()
        {
            UnityEngine.Object.DontDestroyOnLoad(this);
            list_0.Clear();
            list_1.Clear();
            list_2.Clear();
            list_3.Clear();
            action_0 = null;
        }

        public static void Delay(Action task)
        {
            list_2.Add(task);
        }

        public static void Delay(Action task, float time)
        {
            Struct14 item = new Struct14 {
                action_0 = task,
                float_0 = Time.realtimeSinceStartup + time
            };
            list_3.Add(item);
        }

        private void method_0()
        {
            int num = 0;
            while (true)
            {
                if (num >= list_0.Count)
                {
                    break;
                }
                Action action = list_0[num];
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    logger_0.LogError(exception.ToString());
                }
                num++;
            }
            list_0.Clear();
        }

        private void OnDestroy()
        {
            list_0.Clear();
            list_1.Clear();
            list_2.Clear();
            list_3.Clear();
            action_0 = null;
        }

        public static void PostTask(Action task)
        {
            List<Action> list = list_1;
            lock (list)
            {
                list_1.Add(task);
            }
        }

        public static Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return smethod_0().StartCoroutine(coroutine);
        }

        private static Scheduler smethod_0()
        {
            smethod_1();
            return scheduler_0;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void smethod_1()
        {
            if ((scheduler_0 == null) && Application.isPlaying)
            {
                System.Type[] components = new System.Type[] { typeof(Scheduler) };
                GameObject target = new GameObject("Scheduler", components);
                UnityEngine.Object.DontDestroyOnLoad(target);
                scheduler_0 = target.GetComponent<Scheduler>();
            }
        }

        private void Update()
        {
            if (action_0 != null)
            {
                action_0();
            }
            List<Action> list = list_1;
            lock (list)
            {
                if (list_1.Count > 0)
                {
                    for (int i = 0; i < list_1.Count; i++)
                    {
                        list_0.Add(list_1[i]);
                    }
                    list_1.Clear();
                }
            }
            if (list_2.Count > 0)
            {
                for (int j = 0; j < list_2.Count; j++)
                {
                    list_0.Add(list_2[j]);
                }
                list_2.Clear();
            }
            if (list_3.Count > 0)
            {
                Class95 class2 = new Class95 {
                    float_0 = Time.realtimeSinceStartup
                };
                list_3.RemoveAll(new Predicate<Struct14>(class2.method_0));
            }
            this.method_0();
        }

        [CompilerGenerated]
        private sealed class Class95
        {
            internal float float_0;

            internal bool method_0(Scheduler.Struct14 struct14_0)
            {
                if (this.float_0 >= struct14_0.float_0)
                {
                    Scheduler.list_0.Add(struct14_0.action_0);
                    return true;
                }
                return false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Struct14
        {
            public float float_0;
            public Action action_0;
        }
    }
}

