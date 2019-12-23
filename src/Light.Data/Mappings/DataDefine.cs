using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Data define.
    /// </summary>
    internal abstract class DataDefine : IDataDefine
    {
        private static object _synobj = new object();

        private static Dictionary<Type, DataDefine> _defaultDefine = new Dictionary<Type, DataDefine>();

        public static DataDefine GetDefine(Type type)
        {
            var defines = _defaultDefine;
            defines.TryGetValue(type, out var define);
            if (define == null) {
                lock (_synobj) {
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
                    define = new BytesDataDefine(type, true);
                }
                else {
                    throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
                }
            }
            else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
                throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
            }
            else if (typeInfo.IsEnum) {
                define = new EnumDataDefine(type, isNullable);
            }
            else {
                var code = Type.GetTypeCode(type);
                switch (code) {
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
                    case TypeCode.String:
                        define = new PrimitiveDataDefine(type, true);
                        break;
                    default:
                        define = new PrimitiveDataDefine(type, isNullable);
                        break;
                }
            }
            return define;
        }

        protected Type _objectType;

        public Type ObjectType => _objectType;

        protected DataDefine(Type type, bool isNullable)
        {
            _objectType = type;
            IsNullable = isNullable;
        }

        public bool IsNullable { get; }

        public abstract object LoadData(DataContext context, IDataReader datareader, object state);

        public abstract object LoadData(DataContext context, IDataReader datareader, string fieldName, object state);
    }
}
