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
        public static DataFieldMapping CreateDataFieldMapping(PropertyInfo property, DataFieldMapperConfig config,
            int positionOrder, DataMapping mapping)
        {
            var type = property.PropertyType;
            var typeInfo = type.GetTypeInfo();
            var indexName = property.Name;
            var fieldName = string.IsNullOrEmpty(config.Name) ? property.Name : config.Name;

            DataFieldMapping fieldMapping;
            if (typeInfo.IsGenericType)
            {
                var frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1")
                {
                    var arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                }
            }

            if (type.IsArray)
            {
                if (type.FullName == "System.Byte[]")
                {
                    fieldMapping = new BytesFieldMapping(fieldName, indexName, mapping,
                        config.IsNullable, config.DbType, config.IsIdentity, config.IsPrimaryKey);
                }
                else
                {
                    fieldMapping = new ObjectFieldMapping(type, fieldName, indexName, mapping,
                        config.IsNullable, config.DbType, config.IsIdentity, config.IsPrimaryKey);
                }
            }
            // else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition)
            // {
            //     throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType,
            //         property.Name, type));
            // }
            else if (type == typeof(Guid))
            {
                fieldMapping = new GuidFieldMapping(fieldName, indexName, mapping, config.IsNullable,
                    config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (type == typeof(string))
            {
                fieldMapping = new StringFieldMapping(fieldName, indexName, mapping, config.IsNullable,
                    config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (type == typeof(DateTime))
            {
                fieldMapping = new DateTimeFieldMapping(fieldName, indexName, mapping, config.IsNullable,
                    config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (type == typeof(decimal))
            {
                fieldMapping = new DecimalFieldMapping(fieldName, indexName, mapping, config.IsNullable,
                    config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (typeInfo.IsEnum)
            {
                fieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, config.IsNullable,
                    config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (type.IsPrimitive)
            {
                fieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping,
                    config.IsNullable, config.DbType, config.DefaultValue, config.IsIdentity, config.IsPrimaryKey);
            }
            else if (type == typeof(DBNull))
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType,
                    property.Name, type));
            }
            else
            {
                fieldMapping = new ObjectFieldMapping(type, fieldName, indexName, mapping,
                    config.IsNullable, config.DbType, config.IsIdentity, config.IsPrimaryKey);
            }

            if (config.DataOrder > 0)
            {
                fieldMapping.DataOrder = config.DataOrder;
            }

            fieldMapping.PositionOrder = positionOrder;
            fieldMapping.FunctionControl = config.FunctionControl == FunctionControl.Default
                ? FunctionControl.Full
                : config.FunctionControl;
            fieldMapping.Handler = new PropertyHandler(property);
            return fieldMapping;
        }

        public static DataFieldMapping CreateCustomFieldMapping(PropertyInfo property, DataMapping mapping)
        {
            var type = property.PropertyType;
            var typeInfo = type.GetTypeInfo();
            var indexName = property.Name;
            var fieldName = property.Name;

            DataFieldMapping fieldMapping;
            var isNullable = false;
            if (typeInfo.IsGenericType)
            {
                var frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1")
                {
                    var arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                    isNullable = true;
                }
            }

            if (type.IsArray)
            {
                if (type.FullName == "System.Byte[]")
                {
                    fieldMapping = new BytesFieldMapping(fieldName, indexName, mapping, isNullable, null, false,
                        false);
                }
                else
                {
                    fieldMapping =
                        new ObjectFieldMapping(type, fieldName, indexName, mapping, isNullable, null, false, false);
                }
            }
            
            else if (type == typeof(Guid))
            {
                fieldMapping =
                    new GuidFieldMapping(fieldName, indexName, mapping, isNullable, null, null, false, false);
            }
            else if (type == typeof(string))
            {
                fieldMapping =
                    new StringFieldMapping(fieldName, indexName, mapping, isNullable, null, null, false, false);
            }
            else if (type == typeof(DateTime))
            {
                fieldMapping =
                    new DateTimeFieldMapping(fieldName, indexName, mapping, isNullable, null, null, false, false);
            }
            else if (type == typeof(decimal))
            {
                fieldMapping =
                    new DecimalFieldMapping(fieldName, indexName, mapping, isNullable, null, null, false, false);
            }
            else if (typeInfo.IsEnum)
            {
                fieldMapping = new EnumFieldMapping(type, fieldName, indexName, mapping, isNullable, null, null, false,
                    false);
            }
            else if (type.IsPrimitive)
            {
                fieldMapping = new PrimitiveFieldMapping(type, fieldName, indexName, mapping,
                    isNullable, null, null, false, false);
            }
            else if (type == typeof(DBNull))
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType,
                    property.Name, type));
            }
            else
            {
                fieldMapping =
                    new ObjectFieldMapping(type, fieldName, indexName, mapping, isNullable, null, false, false);
            }

            fieldMapping.Handler = new PropertyHandler(property);
            return fieldMapping;
        }


        public abstract bool IsPrimaryKey { get; }

        public abstract bool IsIdentity { get; }

        public abstract bool IsAutoUpdate { get; }

        public int? DataOrder { get; private set; }

        public int PositionOrder { get; private set; }

        public FunctionControl FunctionControl { get; private set; }

        public PropertyHandler Handler { get; private set; }

        protected DataFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable,
            string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
        }

        public DataEntityMapping EntityMapping => TypeMapping as DataEntityMapping;

        public abstract object ToInsert(object entity, bool refreshField);

        public abstract object ToUpdate(object entity, bool refreshField);

        public abstract object ToParameter(object value);

        // public virtual object GetTimeStamp(object entity, bool refreshField)
        // {
        //     return null;
        // }

        private DataFieldInfo fieldInfo;

        public DataFieldInfo DefaultFieldInfo
        {
            get
            {
                if (fieldInfo == null)
                {
                    lock (this)
                    {
                        if (fieldInfo == null)
                        {
                            fieldInfo = new DataFieldInfo(this);
                        }
                    }
                }

                return fieldInfo;
            }
        }
    }
}