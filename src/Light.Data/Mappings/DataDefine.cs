using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    /// <summary>
    /// Data define.
    /// </summary>
    internal abstract class DataDefine : IDataDefine
    {
        private static readonly object locker = new object();

        private static readonly Dictionary<Type, DataDefine> _defaultDefine = new Dictionary<Type, DataDefine>();

        public static DataDefine GetDefine(Type type)
        {
            var defines = _defaultDefine;
            defines.TryGetValue(type, out var define);
            if (define == null) {
                lock (locker) {
                    defines.TryGetValue(type, out define);
                    if (define == null) {
                        define = CreateMapping(type);
                        defines[type] = define;
                    }
                }
            }
            return define;
        }

        private static DataDefine CreateMapping(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var isNullable = false;
            if (typeInfo.IsGenericType) {
                var frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    var arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                    isNullable = true;
                }
            }

            DataDefine define;
            if (type.IsArray) {
                if (type.FullName == "System.Byte[]") {
                    define = new BytesDataDefine(true);
                }
                else {
                    define = new ObjectDataDefine(type, true);
                }
            }
            // else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
            //     throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
            // }
            else if (type == typeof(Guid))
            {
                define = new GuidDataDefine(isNullable);
            }
            else if (type == typeof(string))
            {
                define = new StringDataDefine(true);
            }
            else if (type == typeof(DateTime))
            {
                define = new DateTimeDataDefine(isNullable);
            }
            else if (type == typeof(decimal))
            {
                define = new DecimalDataDefine(isNullable);
            }
            else if (typeInfo.IsEnum)
            {
                define = new EnumDataDefine(type, isNullable);
            }
            else if (type.IsPrimitive)
            {
                define = new PrimitiveDataDefine(type, isNullable);
            }
            else if (type == typeof(DBNull))
            {
                throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
            }
            else
            {
                define = new ObjectDataDefine(type, true);
            }
            return define;
        }

        public Type ObjectType { get; }

        protected DataDefine(Type type, bool isNullable)
        {
            ObjectType = type;
            IsNullable = isNullable;
        }

        public bool IsNullable { get; }

        public abstract object LoadData(DataContext context, IDataReader dataReader, object state);

        public abstract object LoadData(DataContext context, IDataReader dataReader, string fieldName, object state);
    }
}
