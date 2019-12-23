using System;
using System.Reflection;

namespace Light.Data
{
	internal class AttributeCore
	{
		private AttributeCore()
		{

		}

		public static T[] GetMethodAttributes<T>(MethodInfo mi, bool inhert) where T : Attribute
		{
			return (T[])mi.GetCustomAttributes(typeof(T), inhert);
		}

		public static T[] GetParemeterAttributes<T>(ParameterInfo pi, bool inhert) where T : Attribute
		{
			return (T[])pi.GetCustomAttributes(typeof(T), inhert);
		}

		public static T[] GetPropertyAttributes<T>(PropertyInfo pi, bool inhert) where T : Attribute
		{
			return (T[])pi.GetCustomAttributes(typeof(T), inhert);
		}

		public static T[] GetTypeAttributes<T>(Type type, bool inhert) where T : Attribute
		{
			return (T[])type.GetTypeInfo().GetCustomAttributes(typeof(T), inhert);
		}

		public static T[] GetAssemblyAttributes<T>(Assembly assembly) where T : Attribute
		{
			return (T[])assembly.GetCustomAttributes(typeof(T));
		}
	}
}
