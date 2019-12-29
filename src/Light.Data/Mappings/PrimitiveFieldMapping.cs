using System;

namespace Light.Data
{
    internal class PrimitiveFieldMapping : DataFieldMapping
    {
        private readonly object _minValue;

        private readonly object _defaultValue;
        
        public Type NullableType { get; }

        public override bool IsPrimaryKey { get; }

        public override bool IsIdentity { get; }

        public override bool IsAutoUpdate => false;

        public PrimitiveFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping,
            bool isNullable, string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                if (_typeCode == TypeCode.Int32 || _typeCode == TypeCode.Int64 || _typeCode == TypeCode.UInt32 ||
                    _typeCode == TypeCode.UInt64)
                {
                    IsIdentity = true;
                }
                else
                {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType,
                        fieldName, type));
                }
            }

            IsPrimaryKey = isPrimaryKey;

            var nullType = Type.GetType("System.Nullable`1", true);
            NullableType = nullType.MakeGenericType(type);

            _minValue = type.GetDefaultValue();

            if (Equals(_minValue, null))
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, ObjectType, fieldName,
                    type));
            }


            if (defaultValue != null)
            {
                var defaultValueType = defaultValue.GetType();
                _defaultValue = defaultValueType == type ? defaultValue : Convert.ChangeType(defaultValue, type);
            }
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                return null;
            }

            if (value is IConvertible ic)
            {
                if (ic.GetTypeCode() != _typeCode)
                {
                    return Convert.ChangeType(value, _typeCode, null);
                }

                return value;
            }

            return value;
        }

        public override object ToParameter(object value)
        {
            return value;
        }

        public override object ToUpdate(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            object result;
            var useDef = false;
            if (Equals(value, null))
            {
                if (IsNullable)
                {
                    result = null;
                }
                else
                {
                    useDef = true;
                    result = _minValue;
                }
            }
            else
            {
                result = value;
            }

            if (useDef && refreshField)
            {
                Handler.Set(entity, result);
            }

            return value;
        }

        public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            object result;
            var useDef = false;
            if (Equals(value, null))
            {
                if (_defaultValue != null)
                {
                    useDef = true;
                    result = _defaultValue;
                }
                else
                {
                    if (IsNullable)
                    {
                        result = null;
                    }
                    else
                    {
                        useDef = true;
                        result = _minValue;
                    }
                }
            }
            else
            {
                result = value;
            }

            if (useDef && refreshField)
            {
                Handler.Set(entity, result);
            }

            return result;
        }

        // private static bool TryGetMinValue(TypeCode typeCode, out object minValue)
        // {
        //     switch (typeCode)
        //     {
        //         case TypeCode.Boolean:
        //             minValue = false;
        //             break;
        //         case TypeCode.Byte:
        //             minValue = default(byte);
        //             break;
        //         case TypeCode.SByte:
        //             minValue = default(sbyte);
        //             break;
        //         case TypeCode.Char:
        //             minValue = default(char);
        //             break;
        //         case TypeCode.Int16:
        //             minValue = default(short);
        //             break;
        //         case TypeCode.Int32:
        //             minValue = default(int);
        //             break;
        //         case TypeCode.Int64:
        //             minValue = default(long);
        //             break;
        //         case TypeCode.UInt16:
        //             minValue = default(ushort);
        //             break;
        //         case TypeCode.UInt32:
        //             minValue = default(uint);
        //             break;
        //         case TypeCode.UInt64:
        //             minValue = default(ulong);
        //             break;
        //         // case TypeCode.Decimal:
        //         //     minValue = default(decimal);
        //         //     break;
        //         case TypeCode.Single:
        //             minValue = default(float);
        //             break;
        //         case TypeCode.Double:
        //             minValue = default(double);
        //             break;
        //         default:
        //             minValue = null;
        //             return false;
        //     }
        //
        //     return true;
        // }
    }
}