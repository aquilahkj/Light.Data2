using System;
using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Default time function.
    /// </summary>
    internal sealed class DefaultTimeFunction
    {
        private static Dictionary<DefaultTime, DefaultTimeFunction> dict = new Dictionary<DefaultTime, DefaultTimeFunction>();

        static DefaultTimeFunction()
        {
            dict.Add(DefaultTime.Now, new DefaultTimeFunction(GetNow));
            dict.Add(DefaultTime.Today, new DefaultTimeFunction(GetToday));
            dict.Add(DefaultTime.TimeStamp, new DefaultTimeFunction(GetNow));
            dict.Add(DefaultTime.UtcNow, new DefaultTimeFunction(GetUtcNow));
            dict.Add(DefaultTime.UtcToday, new DefaultTimeFunction(GetUtcToday));
            dict.Add(DefaultTime.UtcTimeStamp, new DefaultTimeFunction(GetUtcNow));
        }

        public static DefaultTimeFunction GetFunction(DefaultTime defaultTime)
        {
            dict.TryGetValue(defaultTime, out var function);
            return function;
        }

        private Func<object> func;

        private DefaultTimeFunction(Func<object> func)
        {
            this.func = func;

        }

        private static bool removeMillisecond = false;

        internal static bool RemoveMillisecond {
            get => removeMillisecond;
            set => removeMillisecond = true;
        }


        internal object GetValue()
        {
            return func();
        }


        private static object GetNow()
        {
            var time = DateTime.Now;
            if (removeMillisecond) {
                return new DateTime(time.Ticks / 10000000L * 10000000L);
            }
            else {
                return time;
            }
        }

        private static object GetToday()
        {
            return DateTime.Now.Date;
        }


        private static object GetUtcNow()
        {
            var time = DateTime.UtcNow;
            if (removeMillisecond) {
                return new DateTime(time.Ticks / 10000000L * 10000000L);
            }
            else {
                return time;
            }
        }

        private static object GetUtcToday()
        {
            return DateTime.UtcNow.Date;
        }
        
    }
}

