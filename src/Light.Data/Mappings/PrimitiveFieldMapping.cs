using System;

namespace Light.Data
{
    internal class PrimitiveFieldMapping : DataFieldMapping
    {
        private const SByte MinSByte = 0;

        private const Int16 MinInt16 = 0;

        private const Int32 MinInt32 = 0;

        private const Int64 MinInt64 = 0;

        private const Decimal MinDecimal = 0;

        private const Single MinSingle = 0;

        private const Double MinDouble = 0;

        private readonly object _minValue;

        private readonly object _defaultValue;

        private readonly DefaultTimeFunction _defaultTimeFunction;

        public PrimitiveFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (type != typeof(string)) {
                var itemstype = Type.GetType("System.Nullable`1");
                NullableType = itemstype.MakeGenericType(type);
            }
            else {
                NullableType = type;
            }
            if (defaultValue != null) {
                var defaultValueType = defaultValue.GetType();
                if (_typeCode == TypeCode.DateTime) {
                    if (defaultValueType == typeof(DefaultTime)) {
                        var defaultTime = (DefaultTime)defaultValue;
                        _defaultTimeFunction = DefaultTimeFunction.GetFunction(defaultTime);
                        if (defaultTime == DefaultTime.TimeStamp || defaultTime == DefaultTime.UtcTimeStamp) {
                            isTimeStamp = true;
                        }
                        _defaultValue = _defaultTimeFunction;
                    }
                    else if (defaultValueType == typeof(DateTime)) {
                        _defaultValue = defaultValue;
                    }
                    else if (defaultValueType == typeof(string)) {
                        var str = defaultValue as string;
                        if (DateTime.TryParse(str, out var dt)) {
                            _defaultValue = dt;
                        }
                    }
                }
                else if (defaultValueType == type) {
                    _defaultValue = defaultValue;
                }
                else {
                    _defaultValue = Convert.ChangeType(defaultValue, type);
                }

            }
            if (isIdentity) {
                if (_typeCode == TypeCode.Int32 || _typeCode == TypeCode.Int64 || _typeCode == TypeCode.UInt32 || _typeCode == TypeCode.UInt64) {
                    IsIdentity = true;
                }
                else {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType, fieldName, type));
                }
            }

            IsPrimaryKey = isPrimaryKey;
            switch (_typeCode) {
                case TypeCode.String:
                    _minValue = string.Empty;
                    break;
                case TypeCode.Boolean:
                    _minValue = false;
                    break;
                case TypeCode.Byte:
                    _minValue = byte.MinValue;
                    break;
                case TypeCode.SByte:
                    _minValue = MinSByte;
                    break;
                case TypeCode.DateTime:
                    _minValue = DateTime.MinValue;
                    break;
                case TypeCode.Char:
                    _minValue = Char.MinValue;
                    break;
                case TypeCode.Int16:
                    _minValue = MinInt16;
                    break;
                case TypeCode.Int32:
                    _minValue = MinInt32;
                    break;
                case TypeCode.Int64:
                    _minValue = MinInt64;
                    break;
                case TypeCode.UInt16:
                    _minValue = UInt16.MinValue;
                    break;
                case TypeCode.UInt32:
                    _minValue = UInt32.MinValue;
                    break;
                case TypeCode.UInt64:
                    _minValue = UInt64.MinValue;
                    break;
                case TypeCode.Decimal:
                    _minValue = MinDecimal;
                    break;
                case TypeCode.Single:
                    _minValue = MinSingle;
                    break;
                case TypeCode.Double:
                    _minValue = MinDouble;
                    break;
            }
        }

        public Type NullableType { get; }

        public bool IsPrimaryKey { get; }

        public bool IsIdentity { get; }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                return null;
            }
            else {
                if (value is IConvertible ic) {
                    if (ic.GetTypeCode() != _typeCode) {
                        return Convert.ChangeType(value, _typeCode, null);
                    }
                    else {
                        return value;
                    }
                }
                else {
                    return value;
                }
            }
        }

        public override object ToParameter(object value)
        {
            return value;
        }

        public override object GetInsertData(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            object result;
            var useDef = false;
            if (Equals(value, null)) {
                if (_defaultValue != null) {
                    useDef = true;
                    if (_typeCode == TypeCode.DateTime && _defaultTimeFunction != null) {
                        result = _defaultTimeFunction.GetValue();
                    }
                    else {
                        result = _defaultValue;
                    }
                }
                else {
                    if (IsNullable) {
                        result = null;
                    }
                    else {
                        useDef = true;
                        result = _minValue;
                    }
                }
            }
            else if (_typeCode == TypeCode.DateTime && Equals(value, DateTime.MinValue)) {
                if (_defaultTimeFunction != null) {
                    useDef = true;
                    result = _defaultTimeFunction.GetValue();
                }
                else if (_defaultValue != null) {
                    useDef = true;
                    result = _defaultValue;
                }
                else {
                    result = value;
                }
            }
            else if (isTimeStamp) {
                useDef = true;
                result = _defaultTimeFunction.GetValue();
            }
            else {
                result = value;
            }
            if (useDef && refreshField) {
                Handler.Set(entity, result);
            }
            return result;
        }

        private bool isTimeStamp = false;

        public override bool IsTimeStamp => isTimeStamp;

        public override object GetTimeStamp(object entity, bool refreshField)
        {
            if (isTimeStamp && _defaultTimeFunction != null) {
                var value = _defaultTimeFunction.GetValue();
                if (refreshField) {
                    Handler.Set(entity, value);
                }
                return value;
            }
            else {
                return base.GetTimeStamp(entity, refreshField);
            }
        }
    }

}
