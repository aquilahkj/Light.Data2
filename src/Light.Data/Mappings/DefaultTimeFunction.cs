using System;
using System.Collections.Generic;

namespace Light.Data
{
	/// <summary>
	/// Default time function.
	/// </summary>
	sealed class DefaultTimeFunction
	{
		static Dictionary<DefaultTime,DefaultTimeFunction> dict = new Dictionary<DefaultTime, DefaultTimeFunction> ();

		static DefaultTimeFunction ()
		{
			dict.Add (DefaultTime.Now, new DefaultTimeFunction (GetNow));
			dict.Add (DefaultTime.Today, new DefaultTimeFunction (GetToday));
		}

		public static DefaultTimeFunction GetFunction (DefaultTime defaultTime)
		{
			DefaultTimeFunction function;
			dict.TryGetValue (defaultTime, out function);
			return function;
		}

		Func<object> func;

		private DefaultTimeFunction (Func<object> func)
		{
			this.func = func;
			
		}

		internal object GetValue ()
		{
			return func ();
		}


		static object GetNow ()
		{
			return DateTime.Now;
		}

		static object GetToday ()
		{
			return DateTime.Now.Date;
		}
	}
}

