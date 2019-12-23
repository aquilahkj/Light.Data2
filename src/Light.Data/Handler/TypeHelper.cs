using System;
using System.Reflection;

namespace Light.Data
{
	internal static class TypeHelper
	{
		public static bool IsInherit(this Type target, Type parent) {
			if (target == null || parent == null) {
				return false;
			}
			var targetInfo = target.GetTypeInfo();
			var parentInfo = parent.GetTypeInfo();
			if (target == parent || targetInfo.BaseType == null) {
				return false;
			}
			if (parentInfo.IsInterface) {
				foreach (var t in targetInfo.ImplementedInterfaces) {
					if (t == parent) {
						return true;
					}
				}
			}
			else {
				do {
					if (targetInfo.BaseType == parent) {
						return true;
					}
					target = targetInfo.BaseType;
				}
				while (target != null);
			}
			return false;
		}


    }
}
