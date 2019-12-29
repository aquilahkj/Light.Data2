using System;
using System.Reflection;

namespace Light.Data
{
    internal abstract class DynamicFieldMapping : FieldMapping
    {
        public static DynamicFieldMapping CreateDynamicFieldMapping(PropertyInfo property, DynamicDataMapping mapping)
        {
            DynamicFieldMapping fieldMapping;
            var type = property.PropertyType;
            var typeInfo = type.GetTypeInfo();
            var fieldName = property.Name;
            if (typeInfo.IsGenericType) {
                var frameType = type.GetGenericTypeDefinition();
                if (frameType.FullName == "System.Nullable`1") {
                    var arguments = typeInfo.GetGenericArguments();
                    type = arguments[0];
                    typeInfo = type.GetTypeInfo();
                }
            }

            if (type.IsArray) {
                if (type.FullName == "System.Byte[]")
                {
                    fieldMapping = new BytesDynamicFieldMapping(fieldName, mapping);
                }
                else
                {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType,
                        property.Name, type));
                }
            }
            else if (type == typeof(Guid))
            {
                fieldMapping = new GuidDynamicFieldMapping(fieldName, mapping);
            }
            else if (type == typeof(string))
            {
                fieldMapping = new StringDynamicFieldMapping(fieldName, mapping);
            }
            else if (type == typeof(DateTime))
            {
                fieldMapping = new DateTimeDynamicFieldMapping(fieldName, mapping);
            }
            else if (type == typeof(decimal))
            {
                fieldMapping = new DecimalDynamicFieldMapping(fieldName, mapping);
            }
            else if (typeInfo.IsEnum)
            {
                fieldMapping = new EnumDynamicFieldMapping(type, fieldName, mapping);
            }
            else if (type.IsPrimitive)
            {
                fieldMapping = new PrimitiveDynamicFieldMapping(type, fieldName, mapping);
            }
            else if (type == typeof(DBNull))
            {
                throw new LightDataException(string.Format(SR.DataDefineUnsupportFieldType, type));
            }
            else
            {
                fieldMapping = new ObjectDynamicFieldMapping(type, fieldName, mapping);
            }
            
            return fieldMapping;
        }

        protected DynamicFieldMapping(Type type, string fieldName, DynamicDataMapping mapping, bool isNullable)
            : base(type, fieldName, fieldName, mapping, isNullable, null)
        {

        }
    }
}

