using System;
using System.Reflection;

namespace Light.Data
{
	internal class AttributeCore
	{
		private AttributeCore()
		{

		}

		public static T[] GetMethodAttributes<T>(MethodInfo mi, bool inherit) where T : Attribute
		{
			return (T[])mi.GetCustomAttributes(typeof(T), inherit);
		}

		public static T[] GetParameterAttributes<T>(ParameterInfo pi, bool inherit) where T : Attribute
		{
			return (T[])pi.GetCustomAttributes(typeof(T), inherit);
		}

		public static T[] GetPropertyAttributes<T>(PropertyInfo pi, bool inherit) where T : Attribute
		{
			return (T[])pi.GetCustomAttributes(typeof(T), inherit);
		}

		public static T[] GetTypeAttributes<T>(Type type, bool inherit) where T : Attribute
		{
			return (T[])type.GetTypeInfo().GetCustomAttributes(typeof(T), inherit);
		}

		public static T[] GetAssemblyAttributes<T>(Assembly assembly) where T : Attribute
		{
			return (T[])assembly.GetCustomAttributes(typeof(T));
		}
	}
}
