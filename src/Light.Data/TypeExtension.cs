using System;
using System.Collections.Generic;

namespace Light.Data
{ 
    internal static class TypeExtension
    {
        private static readonly Dictionary<Type, object> BasicTypes = new Dictionary<Type,object>();

        static TypeExtension()
        {
            BasicTypes.Add(typeof(char), default(char));
            BasicTypes.Add(typeof(bool), default(bool));
            BasicTypes.Add(typeof(sbyte), default(sbyte));
            BasicTypes.Add(typeof(byte), default(byte));
            BasicTypes.Add(typeof(short), default(short));
            BasicTypes.Add(typeof(ushort), default(ushort));
            BasicTypes.Add(typeof(int), default(int));
            BasicTypes.Add(typeof(uint), default(uint));
            BasicTypes.Add(typeof(long), default(long));
            BasicTypes.Add(typeof(ulong), default(ulong));
            BasicTypes.Add(typeof(float), default(float));
            BasicTypes.Add(typeof(double), default(double));
            BasicTypes.Add(typeof(decimal), default(decimal));
            BasicTypes.Add(typeof(DateTime), default(DateTime));
            BasicTypes.Add(typeof(Guid), default(Guid));
        }

        public static object AdjustValue(this object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                if (type.IsEnum)
                {
                    var code = Type.GetTypeCode(type);
                    value = Convert.ChangeType(value, code);
                }
            }

            return value;
        }
        
        public static object GetDefaultValue(this Type type)
        {
            if (BasicTypes.TryGetValue(type, out var value))
            {
                return value;
            }

            return null;
        }
    }
}