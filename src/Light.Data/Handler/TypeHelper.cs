using System;
using System.Reflection;

namespace Light.Data
{
	static class TypeHelper
	{
		public static bool IsInherit(this Type target, Type parent) {
			if (target == null || parent == null) {
				return false;
			}
			TypeInfo targetInfo = target.GetTypeInfo();
			TypeInfo parentInfo = parent.GetTypeInfo();
			if (target == parent || targetInfo.BaseType == null) {
				return false;
			}
			if (parentInfo.IsInterface) {
				foreach (Type t in targetInfo.ImplementedInterfaces) {
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
