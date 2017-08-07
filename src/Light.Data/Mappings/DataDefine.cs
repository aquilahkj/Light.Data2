using System;
using System.Reflection;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Data define.
    /// </summary>
    abstract class DataDefine : IDataDefine
    {
        static object _synobj = new object();

        static Dictionary<Type, DataDefine> _defaultDefine = new Dictionary<Type, DataDefine>();

        public static DataDefine GetDefine(Type type)
        {
            Dictionary<Type, DataDefine> defines = _defaultDefine;
            defines.TryGetValue(type, out DataDefine define);
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

        static DataDefine CreateMapping(Type type)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            bool isNullable = false;
            if (typeInfo.IsGenericType) {
                Type frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    Type[] arguments = typeInfo.GetGenericArguments();
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
                TypeCode code = Type.GetTypeCode(type);
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

        public Type ObjectType {
            get {
                return _objectType;
            }
        }

        protected DataDefine(Type type, bool isNullable)
        {
            _objectType = type;
            _isNullable = isNullable;
        }

        readonly bool _isNullable;

        public bool IsNullable {
            get {
                return _isNullable;
            }
        }

        public abstract object LoadData(DataContext context, IDataReader datareader, object state);

        public abstract object LoadData(DataContext context, IDataReader datareader, string fieldName, object state);
    }
}
