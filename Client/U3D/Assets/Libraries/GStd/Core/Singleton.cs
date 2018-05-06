namespace GStd
{
    using System;

    public class Singleton<T> where T: class, new()
    {
        private static T gparam_0;

        public static T Instance
        {
            get
            {
                if (Singleton<T>.gparam_0 == null)
                {
                    Singleton<T>.gparam_0 = Activator.CreateInstance<T>();
                }
                return Singleton<T>.gparam_0;
            }
        }
    }
}

