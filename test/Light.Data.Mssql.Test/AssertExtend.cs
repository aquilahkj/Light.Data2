using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Xunit.Sdk;

namespace Light.Data.Mssql.Test
{
    public static class AssertExtend
    {
        delegate object GetValueHandler(object source);

        class GetPropertyHandler
        {
            private GetValueHandler mGetValue;
            private PropertyInfo mProperty;
            private string mName;

            public GetValueHandler Get {
                get {
                    return this.mGetValue;
                }
            }

            public PropertyInfo Property {
                get {
                    return this.mProperty;
                }
            }

            public string Name {
                get {
                    return this.mName;
                }
            }

            public GetPropertyHandler(PropertyInfo property)
            {
                if (property.CanRead) {
                    this.mGetValue = PropertyGetHandler(property);
                }
                this.mProperty = property;
                this.mName = property.Name;
            }

            private static readonly Dictionary<PropertyInfo, GetValueHandler> mPropertyGetHandlers = new Dictionary<PropertyInfo, GetValueHandler>();

            public static GetValueHandler PropertyGetHandler(PropertyInfo property)
            {
                GetValueHandler handler;
                if (mPropertyGetHandlers.ContainsKey(property)) {
                    return mPropertyGetHandlers[property];
                }
                lock (mPropertyGetHandlers) {
                    if (mPropertyGetHandlers.ContainsKey(property)) {
                        return mPropertyGetHandlers[property];
                    }
                    handler = CreatePropertyGetHandler(property);
                    mPropertyGetHandlers.Add(property, handler);
                }
                return handler;
            }

            private static GetValueHandler CreatePropertyGetHandler(PropertyInfo property)
            {
                DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, property.DeclaringType.GetTypeInfo().Module);
                ILGenerator iLGenerator = method.GetILGenerator();
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.EmitCall(OpCodes.Callvirt, property.GetGetMethod(), null);
                EmitBoxIfNeeded(iLGenerator, property.PropertyType);
                iLGenerator.Emit(OpCodes.Ret);
                return (GetValueHandler)method.CreateDelegate(typeof(GetValueHandler));
            }

            private static void EmitBoxIfNeeded(ILGenerator il, Type type)
            {
                if (type.GetTypeInfo().IsValueType) {
                    il.Emit(OpCodes.Box, type);
                }
            }
        }

        public static void Equal<T, K>(T expected, K actual)
        {
            AreObjectsEqual(expected, actual, typeof(T).Name, typeof(K).Name, false);
        }

        public static void StrictEqual<T>(T expected, T actual)
        {
            AreObjectsEqual(expected, actual, typeof(T).Name, typeof(T).Name, true);
        }

        private static void AreObjectsEqual(object expected, object actual, string expectedName, string actualName, bool checkType)
        {
            // 若为相同为空
            if (Object.Equals(expected, null) && Object.Equals(actual, null)) {
                return;
            }
            // 若为相同引用，则通过验证
            if (Object.ReferenceEquals(expected, actual)) {
                return;
            }

            if (!Object.Equals(expected, null) && Object.Equals(actual, null)) {
                throw new AssertActualExpectedException(expected, actual, "actual value is null", expectedName, actualName);
            }
            else if (Object.Equals(expected, null) && !Object.Equals(actual, null)) {
                throw new AssertActualExpectedException(expected, actual, "expected value is null", expectedName, actualName);
            }

            Type expectedType = expected.GetType();
            Type actualType = actual.GetType();
            if (checkType) {
                // 判断类型是否相同
                if (!Object.Equals(expectedType, actualType)) {
                    throw new AssertActualExpectedException(expected, actual, "actual type is not equal expected type", expectedName, actualName);
                }
            }
            TypeCode typeCode = Type.GetTypeCode(expectedType);
            TypeCode typeCode2 = Type.GetTypeCode(actualType);
            if (!Object.Equals(typeCode, typeCode2)) {
                throw new AssertActualExpectedException(typeCode, typeCode2, "actual typecode is not equal expected typecode", expectedName, actualName);
            }

            if (typeCode == TypeCode.Object) {
                if (expected is IEnumerable ie1) {
                    IEnumerable ie2 = actual as IEnumerable;
                    if (ie2 == null) {
                        throw new AssertActualExpectedException(expected, actual, "actual type is not IEnumerable type", expectedName, actualName);
                    }
                    ArrayList list1 = new ArrayList();
                    ArrayList list2 = new ArrayList();
                    foreach (object item in ie1) {
                        list1.Add(item);
                    }
                    foreach (object item in ie2) {
                        list2.Add(item);
                    }
                    if (list1.Count != list2.Count) {
                        throw new AssertActualExpectedException(list1.Count, list2.Count, "actual count is not equal expected count", expectedName, actualName);
                    }
                    for (int i = 0; i < list1.Count; i++) {
                        AreObjectsEqual(list1[i], list2[i], string.Format("{0}[{1}]", expectedName, i), string.Format("{0}[{1}]", actualName, i), checkType);
                    }
                }
                else {
                    PropertyInfo[] expectedProperties = expectedType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    foreach (PropertyInfo property in expectedProperties) {
                        string propertyName = property.Name;
                        string expectedPropertyName = string.Format("{0}.{1}", expectedName, propertyName);
                        string actualPropertyName = string.Format("{0}.{1}", actualName, propertyName);
                        var expectedHandle = GetPropertyHandler.PropertyGetHandler(property);
                        object obj1 = expectedHandle(expected);
                        PropertyInfo property2 = actualType.GetProperty(propertyName);
                        if (property2 == null) {
                            throw new AssertActualExpectedException(expected, actual, string.Format("actual property {0} is not exists", propertyName), expectedName, actualName);
                        }
                        if (!Object.Equals(property.PropertyType, property2.PropertyType)) {
                            throw new AssertActualExpectedException(property.PropertyType, property2.PropertyType, string.Format("actual property {0} type is not equal expected property {0} type", propertyName), expectedPropertyName, actualPropertyName);
                        }
                        var actualHandle = GetPropertyHandler.PropertyGetHandler(property2);
                        object obj2 = actualHandle(actual);
                        AreObjectsEqual(obj1, obj2, expectedPropertyName, actualPropertyName, checkType);
                    }
                }
            }
            else if (typeCode == TypeCode.Empty) {
                return;
            }
            else {
                if (typeCode == TypeCode.Double) {
                    double d1 = Math.Round((double)expected, 4);
                    double d2 = Math.Round((double)actual, 4);
                    if (d1.CompareTo(d2) != 0) {
                        throw new AssertActualExpectedException(expected, actual, "actual value is not equal expected value", expectedName, actualName);
                    }
                }
                else if (typeCode == TypeCode.Single) {
                    double d1 = Math.Round((float)expected, 4);
                    double d2 = Math.Round((float)actual, 4);
                    if (d1.CompareTo(d2) != 0) {
                        throw new AssertActualExpectedException(expected, actual, "actual value is not equal expected value", expectedName, actualName);
                    }
                }
                else {
                    if (!Object.Equals(expected, actual)) {
                        throw new AssertActualExpectedException(expected, actual, "actual value is not equal expected value", expectedName, actualName);
                    }
                }
            }
        }
    }
}
