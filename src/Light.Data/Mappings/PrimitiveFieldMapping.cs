using System;

namespace Light.Data
{
    class PrimitiveFieldMapping : DataFieldMapping
    {
        const SByte MinSByte = 0;

        const Int16 MinInt16 = 0;

        const Int32 MinInt32 = 0;

        const Int64 MinInt64 = 0;

        const Decimal MinDecimal = 0;

        const Single MinSingle = 0;

        const Double MinDouble = 0;

        readonly object _minValue;

        readonly object _defaultValue;

        readonly DefaultTimeFunction _defaultTimeFunction;

        public PrimitiveFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (type != typeof(string)) {
                Type itemstype = Type.GetType("System.Nullable`1");
                _nullableType = itemstype.MakeGenericType(type);
            }
            else {
                _nullableType = type;
            }
            if (defaultValue != null) {
                Type defaultValueType = defaultValue.GetType();
                if (_typeCode == TypeCode.DateTime) {
                    if (defaultValueType == typeof(DefaultTime)) {
                        DefaultTime defaultTime = (DefaultTime)defaultValue;
                        this._defaultTimeFunction = DefaultTimeFunction.GetFunction(defaultTime);
                        if (defaultTime == DefaultTime.TimeStamp || defaultTime == DefaultTime.UtcTimeStamp) {
                            isTimeStamp = true;
                        }
                        this._defaultValue = this._defaultTimeFunction;
                    }
                    else if (defaultValueType == typeof(DateTime)) {
                        this._defaultValue = defaultValue;
                    }
                    else if (defaultValueType == typeof(string)) {
                        string str = defaultValue as string;
                        if (DateTime.TryParse(str, out DateTime dt)) {
                            this._defaultValue = dt;
                        }
                    }
                }
                else if (defaultValueType == type) {
                    this._defaultValue = defaultValue;
                }
                else {
                    this._defaultValue = Convert.ChangeType(defaultValue, type);
                }

            }
            if (isIdentity) {
                if (_typeCode == TypeCode.Int32 || _typeCode == TypeCode.Int64 || _typeCode == TypeCode.UInt32 || _typeCode == TypeCode.UInt64) {
                    _isIdentity = true;
                }
                else {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType, fieldName, type));
                }
            }

            _isPrimaryKey = isPrimaryKey;
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

        Type _nullableType;

        public Type NullableType {
            get {
                return _nullableType;
            }
        }

        bool _isPrimaryKey;

        public bool IsPrimaryKey {
            get {
                return _isPrimaryKey;
            }
        }

        bool _isIdentity;

        public bool IsIdentity {
            get {
                return _isIdentity;
            }
        }

        public override object ToProperty(object value)
        {
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
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
            object value = Handler.Get(entity);
            object result;
            bool useDef = false;
            if (Object.Equals(value, null)) {
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
            else if (_typeCode == TypeCode.DateTime && Object.Equals(value, DateTime.MinValue)) {
                if (_defaultTimeFunction != null) {
                    useDef = true;
                    result = this._defaultTimeFunction.GetValue();
                }
                else if (_defaultValue != null) {
                    useDef = true;
                    result = _defaultValue;
                }
                else {
                    result = value;
                }
            }
            else {
                result = value;
            }
            if (useDef && refreshField) {
                Handler.Set(entity, result);
            }
            return result;
        }

        bool isTimeStamp = false;

        public override bool IsTimeStamp {
            get {
                return isTimeStamp;
            }
        }

        public override object GetTimeStamp()
        {
            if (isTimeStamp && _defaultTimeFunction != null) {
                return _defaultTimeFunction.GetValue();
            }
            else {
                return base.GetTimeStamp();
            }
        }
    }

}
