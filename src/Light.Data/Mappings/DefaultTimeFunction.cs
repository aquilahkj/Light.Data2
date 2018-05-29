using System;
using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Default time function.
    /// </summary>
    sealed class DefaultTimeFunction
    {
        static Dictionary<DefaultTime, DefaultTimeFunction> dict = new Dictionary<DefaultTime, DefaultTimeFunction>();

        static DefaultTimeFunction()
        {
            dict.Add(DefaultTime.Now, new DefaultTimeFunction(GetNow));
            dict.Add(DefaultTime.Today, new DefaultTimeFunction(GetToday));
            dict.Add(DefaultTime.UtcNow, new DefaultTimeFunction(GetUtcNow));
            dict.Add(DefaultTime.UtcToday, new DefaultTimeFunction(GetUtcToday));
        }

        public static DefaultTimeFunction GetFunction(DefaultTime defaultTime)
        {
            DefaultTimeFunction function;
            dict.TryGetValue(defaultTime, out function);
            return function;
        }

        Func<object> func;

        private DefaultTimeFunction(Func<object> func)
        {
            this.func = func;

        }

        static bool removeMillisecond = false;

        internal static bool RemoveMillisecond {
            get {
                return removeMillisecond;
            }
            set {
                removeMillisecond = true;
            }
        }


        internal object GetValue()
        {
            return func();
        }


        static object GetNow()
        {
            var time = DateTime.Now;
            if (removeMillisecond) {
                return new DateTime(time.Ticks / 10000000L * 10000000L);
            }
            else {
                return time;
            }
        }

        static object GetToday()
        {
            return DateTime.Now.Date;
        }

        static object GetUtcNow()
        {
            var time = DateTime.UtcNow;
            if (removeMillisecond) {
                return new DateTime(time.Ticks / 10000000L * 10000000L);
            }
            else {
                return time;
            }
        }

        static object GetUtcToday()
        {
            return DateTime.UtcNow.Date;
        }
    }
}

