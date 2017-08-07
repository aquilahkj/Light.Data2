using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Light.Data
{
    /// <summary>
    /// Data field mapping.
    /// </summary>
    abstract class DataFieldMapping : FieldMapping
    {
        public static DataFieldMapping CreateDataFieldMapping(PropertyInfo property, DataFieldMapperConfig config, int positionOrder, DataMapping mapping)
        {
            Type type = property.PropertyType;
            TypeInfo typeInfo = type.GetTypeInfo();
            string indexName = property.Name;
            string fieldName = string.IsNullOrEmpty(config.Name) ? property.Name : config.Name;
            if (!Regex.IsMatch(fieldName, _fieldRegex, RegexOptions.IgnoreCase)) {
                throw new LightDataException(string.Format(SR.FieldNameIsInvalid, type, fieldName));
            }

            DataFieldMapping fieldMapping;

            if (typeInfo.IsGenericType) {
                Type frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    Type[] arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                }
            }
            if (type.IsArray) {
                if (type.FullName == "System.Byte[]") {
                    BytesFieldMapping bytesFieldMapping = new BytesFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType);
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
                EnumFieldMapping enumFieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType, config.DefaultValue);
                fieldMapping = enumFieldMapping;
            }
            else {
                TypeCode code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
                }
                else {
                    PrimitiveFieldMapping primitiveFieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping, config.IsNullable, config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            if (config.DataOrder > 0) {
                fieldMapping._dataOrder = config.DataOrder;
            }
            fieldMapping._positionOrder = positionOrder;
            fieldMapping._handler = new PropertyHandler(property);
            return fieldMapping;
        }

        public static DataFieldMapping CreateCustomFieldMapping(PropertyInfo property, DataMapping mapping)
        {
            Type type = property.PropertyType;
            TypeInfo typeInfo = type.GetTypeInfo();
            string indexName = property.Name;
            string fieldName = property.Name;

            DataFieldMapping fieldMapping;
            string dbType = null;
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

            if (type.IsArray) {
                if (type.FullName == "System.Byte[]") {
                    BytesFieldMapping bytesFieldMapping = new BytesFieldMapping(type, fieldName, indexName, mapping, isNullable, null);
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
                EnumFieldMapping enumFieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, isNullable);
                fieldMapping = enumFieldMapping;
            }
            else {
                TypeCode code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    return null;
                }
                else if (code == TypeCode.Object) {
                    return null;
                }
                else {
                    PrimitiveFieldMapping primitiveFieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping, isNullable, dbType, null, false, false);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            fieldMapping._handler = new PropertyHandler(property);
            return fieldMapping;
        }


        protected int? _dataOrder;

        protected int _positionOrder;

        protected PropertyHandler _handler;

        public int? DataOrder {
            get {
                return _dataOrder;
            }
        }

        public int PositionOrder {
            get {
                return _positionOrder;
            }
        }

        public PropertyHandler Handler {
            get {
                return _handler;
            }
        }

        protected DataFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {

        }

        public DataEntityMapping EntityMapping {
            get {
                return TypeMapping as DataEntityMapping;
            }
        }

        public abstract object ToColumn(object value);

        public abstract object ToParameter(object value);
    }
}
