namespace GStd
{
    using System;
    using UnityEngine;

    public sealed class RepeatedTimer : IDisposable
    {
        private Action action_0;
        private bool bool_0;
        private float float_0;
        private float float_1;
        private float float_2 = 1f;
        private static GStd.Logger logger_0 = LogSystem.GetLogger("RepeatedTimer");

        public void Dispose()
        {
            Scheduler.FrameEvent -= new Action(this.method_1);
        }

        private void method_0()
        {
            Scheduler.FrameEvent += new Action(this.method_1);
        }

        private void method_1()
        {
            if (this.bool_0)
            {
                this.float_0 -= Time.unscaledDeltaTime * this.float_2;
            }
            else
            {
                this.float_0 -= Time.deltaTime * this.float_2;
            }
            if (this.float_0 <= 0f)
            {
                try
                {
                    this.action_0();
                }
                catch (Exception exception)
                {
                    logger_0.LogError(exception);
                }
                finally
                {
                    this.float_0 = this.float_1;
                }
            }
        }

        public static RepeatedTimer Repeat(float interval, Action task)
        {
            RepeatedTimer timer = new RepeatedTimer {
                float_0 = interval,
                float_1 = interval,
                bool_0 = false,
                action_0 = task
            };
            timer.method_0();
            return timer;
        }

        public static RepeatedTimer Repeat(float delay, float interval, Action task)
        {
            RepeatedTimer timer = new RepeatedTimer {
                float_0 = delay,
                float_1 = interval,
                bool_0 = false,
                action_0 = task
            };
            timer.method_0();
            return timer;
        }

        public float LeftTime
        {
            get
            {
                return this.float_0;
            }
        }

        public float RepeatTime
        {
            get
            {
                return this.float_1;
            }
        }

        public float Speed
        {
            get
            {
                return this.float_2;
            }
            set
            {
                this.float_2 = value;
            }
        }
    }
}

