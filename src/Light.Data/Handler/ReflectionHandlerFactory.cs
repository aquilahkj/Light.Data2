using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Light.Data
{
	static class ReflectionHandlerFactory
	{
		private static readonly Dictionary<FieldInfo, GetValueHandler> mFieldGetHandlers = new Dictionary<FieldInfo, GetValueHandler>();
		private static readonly Dictionary<FieldInfo, SetValueHandler> mFieldSetHandlers = new Dictionary<FieldInfo, SetValueHandler>();
		private static readonly Dictionary<Type, ObjectInstanceHandler> mInstanceHandlers = new Dictionary<Type, ObjectInstanceHandler>();
		private static readonly Dictionary<MethodInfo, FastMethodHandler> mMethodHandlers = new Dictionary<MethodInfo, FastMethodHandler>();
		private static readonly Dictionary<PropertyInfo, GetValueHandler> mPropertyGetHandlers = new Dictionary<PropertyInfo, GetValueHandler>();
		private static readonly Dictionary<PropertyInfo, SetValueHandler> mPropertySetHandlers = new Dictionary<PropertyInfo, SetValueHandler>();

		private static GetValueHandler CreateFieldGetHandler(FieldInfo field) {
			DynamicMethod method = new DynamicMethod("", typeof(object), new Type[] { typeof(object) }, field.DeclaringType);
			ILGenerator iLGenerator = method.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldfld, field);
			EmitBoxIfNeeded(iLGenerator, field.FieldType);
			iLGenerator.Emit(OpCodes.Ret);
			return (GetValueHandler)method.CreateDelegate(typeof(GetValueHandler));
		}

		private static SetValueHandler CreateFieldSetHandler(FieldInfo field) {
			DynamicMethod method = new DynamicMethod("", null, new Type[] { typeof(object), typeof(object) }, field.DeclaringType);
			ILGenerator iLGenerator = method.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldarg_1);
			EmitCastToReference(iLGenerator, field.FieldType);
			iLGenerator.Emit(OpCodes.Stfld, field);
			iLGenerator.Emit(OpCodes.Ret);
			return (SetValueHandler)method.CreateDelegate(typeof(SetValueHandler));
		}

		private static ObjectInstanceHandler CreateInstanceHandler(Type type) {
			DynamicMethod method = new DynamicMethod(string.Empty, type, null, type.GetTypeInfo().Module);
			ILGenerator iLGenerator = method.GetILGenerator();
			iLGenerator.DeclareLocal(type, true);
			iLGenerator.Emit(OpCodes.Newobj, type.GetTypeInfo().GetConstructor(new Type[0]));
			iLGenerator.Emit(OpCodes.Stloc_0);
			iLGenerator.Emit(OpCodes.Ldloc_0);
			iLGenerator.Emit(OpCodes.Ret);
			return (ObjectInstanceHandler)method.CreateDelegate(typeof(ObjectInstanceHandler));
		}

		private static FastMethodHandler CreateMethodHandler(MethodInfo methodInfo) {
			int num;
			DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] {
				typeof(object),
				typeof(object[])
			}, methodInfo.DeclaringType.GetTypeInfo().Module);
			ILGenerator iLGenerator = method.GetILGenerator();
			ParameterInfo[] parameters = methodInfo.GetParameters();
			Type[] typeArray = new Type[parameters.Length];
			for (num = 0; num < typeArray.Length; num++) {
				if (parameters[num].ParameterType.IsByRef) {
					typeArray[num] = parameters[num].ParameterType.GetElementType();
				}
				else {
					typeArray[num] = parameters[num].ParameterType;
				}
			}
			LocalBuilder[] builderArray = new LocalBuilder[typeArray.Length];
			for (num = 0; num < typeArray.Length; num++) {
				builderArray[num] = iLGenerator.DeclareLocal(typeArray[num], true);
			}
			for (num = 0; num < typeArray.Length; num++) {
				iLGenerator.Emit(OpCodes.Ldarg_1);
				EmitFastInt(iLGenerator, num);
				iLGenerator.Emit(OpCodes.Ldelem_Ref);
				EmitCastToReference(iLGenerator, typeArray[num]);
				iLGenerator.Emit(OpCodes.Stloc, builderArray[num]);
			}
			if (!methodInfo.IsStatic) {
				iLGenerator.Emit(OpCodes.Ldarg_0);
			}
			for (num = 0; num < typeArray.Length; num++) {
				if (parameters[num].ParameterType.IsByRef) {
					iLGenerator.Emit(OpCodes.Ldloca_S, builderArray[num]);
				}
				else {
					iLGenerator.Emit(OpCodes.Ldloc, builderArray[num]);
				}
			}
			if (methodInfo.IsStatic) {
				iLGenerator.EmitCall(OpCodes.Call, methodInfo, null);
			}
			else {
				iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
			}
			if (methodInfo.ReturnType == typeof(void)) {
				iLGenerator.Emit(OpCodes.Ldnull);
			}
			else {
				EmitBoxIfNeeded(iLGenerator, methodInfo.ReturnType);
			}
			for (num = 0; num < typeArray.Length; num++) {
				if (parameters[num].ParameterType.IsByRef) {
					iLGenerator.Emit(OpCodes.Ldarg_1);
					EmitFastInt(iLGenerator, num);
					iLGenerator.Emit(OpCodes.Ldloc, builderArray[num]);
					if (builderArray[num].LocalType.GetTypeInfo().IsValueType) {
						iLGenerator.Emit(OpCodes.Box, builderArray[num].LocalType);
					}
					iLGenerator.Emit(OpCodes.Stelem_Ref);
				}
			}
			iLGenerator.Emit(OpCodes.Ret);
			return (FastMethodHandler)method.CreateDelegate(typeof(FastMethodHandler));
		}

		private static GetValueHandler CreatePropertyGetHandler(PropertyInfo property) {
			DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, property.DeclaringType.GetTypeInfo().Module);
			ILGenerator iLGenerator = method.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.EmitCall(OpCodes.Callvirt, property.GetMethod, null);
			EmitBoxIfNeeded(iLGenerator, property.PropertyType);
			iLGenerator.Emit(OpCodes.Ret);
			return (GetValueHandler)method.CreateDelegate(typeof(GetValueHandler));
		}

		private static SetValueHandler CreatePropertySetHandler(PropertyInfo property) {
			DynamicMethod method = new DynamicMethod(string.Empty, null, new Type[] { typeof(object), typeof(object) }, property.DeclaringType.GetTypeInfo().Module);
			ILGenerator iLGenerator = method.GetILGenerator();
			iLGenerator.Emit(OpCodes.Ldarg_0);
			iLGenerator.Emit(OpCodes.Ldarg_1);
			EmitCastToReference(iLGenerator, property.PropertyType);
			iLGenerator.EmitCall(OpCodes.Callvirt, property.SetMethod, null);
			iLGenerator.Emit(OpCodes.Ret);
			return (SetValueHandler)method.CreateDelegate(typeof(SetValueHandler));
		}

		private static void EmitBoxIfNeeded(ILGenerator il, Type type) {
			if (type.GetTypeInfo().IsValueType) {
				il.Emit(OpCodes.Box, type);
			}
		}

		private static void EmitCastToReference(ILGenerator il, Type type) {
			if (type.GetTypeInfo().IsValueType) {
				il.Emit(OpCodes.Unbox_Any, type);
			}
			else {
				il.Emit(OpCodes.Castclass, type);
			}
		}

		private static void EmitFastInt(ILGenerator il, int value) {
			switch (value) {
				case -1:
					il.Emit(OpCodes.Ldc_I4_M1);
					break;

				case 0:
					il.Emit(OpCodes.Ldc_I4_0);
					break;

				case 1:
					il.Emit(OpCodes.Ldc_I4_1);
					break;

				case 2:
					il.Emit(OpCodes.Ldc_I4_2);
					break;

				case 3:
					il.Emit(OpCodes.Ldc_I4_3);
					break;

				case 4:
					il.Emit(OpCodes.Ldc_I4_4);
					break;

				case 5:
					il.Emit(OpCodes.Ldc_I4_5);
					break;

				case 6:
					il.Emit(OpCodes.Ldc_I4_6);
					break;

				case 7:
					il.Emit(OpCodes.Ldc_I4_7);
					break;

				case 8:
					il.Emit(OpCodes.Ldc_I4_8);
					break;

				default:
					if ((value > -129) && (value < 0x80)) {
						il.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
					}
					else {
						il.Emit(OpCodes.Ldc_I4, value);
					}
					break;
			}
		}

		public static GetValueHandler FieldGetHandler(FieldInfo field) {
			GetValueHandler handler;
			if (mFieldGetHandlers.ContainsKey(field)) {
				return mFieldGetHandlers[field];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mFieldGetHandlers.ContainsKey(field)) {
					return mFieldGetHandlers[field];
				}
				handler = CreateFieldGetHandler(field);
				mFieldGetHandlers.Add(field, handler);
			}
			return handler;
		}

		public static SetValueHandler FieldSetHandler(FieldInfo field) {
			SetValueHandler handler;
			if (mFieldSetHandlers.ContainsKey(field)) {
				return mFieldSetHandlers[field];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mFieldSetHandlers.ContainsKey(field)) {
					return mFieldSetHandlers[field];
				}
				handler = CreateFieldSetHandler(field);
				mFieldSetHandlers.Add(field, handler);
			}
			return handler;
		}

		public static ObjectInstanceHandler InstanceHandler(Type type) {
			ObjectInstanceHandler handler;
			if (mInstanceHandlers.ContainsKey(type)) {
				return mInstanceHandlers[type];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mInstanceHandlers.ContainsKey(type)) {
					return mInstanceHandlers[type];
				}
				handler = CreateInstanceHandler(type);
				mInstanceHandlers.Add(type, handler);
			}
			return handler;
		}

		public static FastMethodHandler MethodHandler(MethodInfo method) {
			FastMethodHandler handler;
			if (mMethodHandlers.ContainsKey(method)) {
				return mMethodHandlers[method];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mMethodHandlers.ContainsKey(method)) {
					return mMethodHandlers[method];
				}
				handler = CreateMethodHandler(method);
				mMethodHandlers.Add(method, handler);
			}
			return handler;
		}

		public static GetValueHandler PropertyGetHandler(PropertyInfo property) {
			GetValueHandler handler;
			if (mPropertyGetHandlers.ContainsKey(property)) {
				return mPropertyGetHandlers[property];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mPropertyGetHandlers.ContainsKey(property)) {
					return mPropertyGetHandlers[property];
				}
				handler = CreatePropertyGetHandler(property);
				mPropertyGetHandlers.Add(property, handler);
			}
			return handler;
		}

		public static SetValueHandler PropertySetHandler(PropertyInfo property) {
			SetValueHandler handler;
			if (mPropertySetHandlers.ContainsKey(property)) {
				return mPropertySetHandlers[property];
			}
			lock (typeof(ReflectionHandlerFactory)) {
				if (mPropertySetHandlers.ContainsKey(property)) {
					return mPropertySetHandlers[property];
				}
				handler = CreatePropertySetHandler(property);
				mPropertySetHandlers.Add(property, handler);
			}
			return handler;
		}
	}
}
