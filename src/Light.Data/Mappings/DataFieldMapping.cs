using System;
using System.Reflection;
//using System.Text.RegularExpressions;

namespace Light.Data
{
    /// <summary>
    /// Data field mapping.
    /// </summary>
    internal abstract class DataFieldMapping : FieldMapping
    {
        public static DataFieldMapping CreateDataFieldMapping(PropertyInfo property, DataFieldMapperConfig config, int positionOrder, DataMapping mapping)
        {
            var type = property.PropertyType;
            var typeInfo = type.GetTypeInfo();
            var indexName = property.Name;
            var fieldName = string.IsNullOrEmpty(config.Name) ? property.Name : config.Name;
            //if (!Regex.IsMatch(fieldName, _fieldRegex, RegexOptions.IgnoreCase)) {
            //    throw new LightDataException(string.Format(SR.FieldNameIsInvalid, type, fieldName));
            //}

            DataFieldMapping fieldMapping;
            if (typeInfo.IsGenericType) {
                var frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    var arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                }
            }
            if (type.IsArray) {
                if (type.FullName == "System.Byte[]") {
                    var bytesFieldMapping = new BytesFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType);
                    fieldMapping = bytesFieldMapping;
                }
                else {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
                }
            }
            else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
            }
            else if (typeInfo.IsEnum) {
                var enumFieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType, config.DefaultValue);
                fieldMapping = enumFieldMapping;
            }
            else {
                var code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
                }
                else if (code == TypeCode.Object) {
                    var objectFieldMapping = new ObjectFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType);
                    fieldMapping = objectFieldMapping;
                }
                else {
                    var primitiveFieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            if (config.DataOrder > 0) {
                fieldMapping._dataOrder = config.DataOrder;
            }
            fieldMapping._positionOrder = positionOrder;
            fieldMapping._functionControl = config.FunctionControl == FunctionControl.Default ? FunctionControl.Full : config.FunctionControl;
            fieldMapping._handler = new PropertyHandler(property);
            return fieldMapping;
        }

        public static DataFieldMapping CreateCustomFieldMapping(PropertyInfo property, DataMapping mapping)
        {
            var type = property.PropertyType;
            var typeInfo = type.GetTypeInfo();
            var indexName = property.Name;
            var fieldName = property.Name;

            DataFieldMapping fieldMapping;
            string dbType = null;
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

            if (type.IsArray) {
                if (type.FullName == "System.Byte[]") {
                    var bytesFieldMapping = new BytesFieldMapping(type, fieldName, indexName, mapping, isNullable, null);
                    fieldMapping = bytesFieldMapping;
                }
                else {
                    return null;
                }
            }
            else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
                return null;
            }
            else if (typeInfo.IsEnum) {
                var enumFieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, isNullable);
                fieldMapping = enumFieldMapping;
            }
            else {
                var code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    return null;
                }
                else if (code == TypeCode.Object) {
                    var objectFieldMapping = new ObjectFieldMapping(type, fieldName, indexName, mapping, isNullable, dbType);
                    fieldMapping = objectFieldMapping;
                }
                else {
                    var primitiveFieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping, isNullable, dbType, null, false, false);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            fieldMapping._handler = new PropertyHandler(property);
            return fieldMapping;
        }


        protected int? _dataOrder;

        protected int _positionOrder;

        protected PropertyHandler _handler;

        protected FunctionControl _functionControl;

        public int? DataOrder => _dataOrder;

        public int PositionOrder => _positionOrder;

        public FunctionControl FunctionControl => _functionControl;


        public PropertyHandler Handler => _handler;

        protected DataFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {

        }

        public DataEntityMapping EntityMapping => TypeMapping as DataEntityMapping;

        public abstract object GetInsertData(object entity, bool refreshField);

        public abstract object ToParameter(object value);

        public virtual bool IsTimeStamp => false;

        public virtual object GetTimeStamp(object entity, bool refreshField)
        {
            return null;
        }

        private DataFieldInfo fieldInfo;

        public DataFieldInfo DefaultFieldInfo {
            get {
                if (fieldInfo == null) {
                    lock (this) {
                        if (fieldInfo == null) {
                            fieldInfo = new DataFieldInfo(this);
                        }
                    }
                }
                return fieldInfo;
            }
        }
    }
}
