using System;
using System.Reflection;

namespace Light.Data
{
    internal abstract class DynamicFieldMapping : FieldMapping
    {
        public static DynamicFieldMapping CreateDynmaicFieldMapping(PropertyInfo property, DynamicCustomMapping mapping)
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
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
            }
            else if (type.IsGenericParameter || typeInfo.IsGenericTypeDefinition) {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
            }
            else if (typeInfo.IsEnum) {
                var enumFieldMapping = new DynamicEnumFieldMapping(type, fieldName, mapping);
                fieldMapping = enumFieldMapping;
            }
            else {
                var code = Type.GetTypeCode(type);
                if (code == TypeCode.Empty) {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, property.DeclaringType, property.Name, type));
                }
                else if (code == TypeCode.Object) {
                    var objectFieldMapping = new DynamicObjectFieldMapping(type, fieldName, mapping);
                    fieldMapping = objectFieldMapping;
                }
                else {
                    var primitiveFieldMapping = new DynamicPrimitiveFieldMapping(type, fieldName, mapping);
                    fieldMapping = primitiveFieldMapping;
                }
            }
            return fieldMapping;
        }

        protected DynamicFieldMapping(Type type, string fieldName, DynamicCustomMapping mapping, bool isNullable)
            : base(type, fieldName, fieldName, mapping, isNullable, null)
        {

        }
    }
}

