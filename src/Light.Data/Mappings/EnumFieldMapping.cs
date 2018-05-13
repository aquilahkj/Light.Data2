using System;

namespace Light.Data
{
    class EnumFieldMapping : DataFieldMapping
    {
        readonly object _minValue;

        readonly object _defaultValue;

        Type _nullableType;

        public Type NullableType {
            get {
                return _nullableType;
            }
        }

        public EnumFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, object defaultValue)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            Type itemstype = Type.GetType("System.Nullable`1");
            _nullableType = itemstype.MakeGenericType(type);
            Array values = Enum.GetValues(ObjectType);
            object value = values.GetValue(0);
            _minValue = Convert.ChangeType(value, _typeCode, null);

            if (defaultValue != null) {
                Type defaultValueType = defaultValue.GetType();
                if (defaultValueType == type) {
                    _defaultValue = Convert.ChangeType(defaultValue, _typeCode, null);
                } else {
                    throw new LightDataException(string.Format(SR.EnumDefaultValueType, mapping.ObjectType, fieldName, defaultValue));
                }
            }
        }

        public EnumFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable)
            : base(type, fieldName, indexName, mapping, isNullable, null)
        {
            Type itemstype = Type.GetType("System.Nullable`1");
            _nullableType = itemstype.MakeGenericType(type);
            Array values = Enum.GetValues(ObjectType);
            object value = values.GetValue(0);
            _minValue = Convert.ChangeType(value, _typeCode, null);
        }

        public override object ToProperty(object value)
        {
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                return null;
            } else {
                value = Enum.ToObject(_objectType, value);
                return value;
            }
        }

        public override object ToParameter(object value)
        {
            if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
                return null;
            } else {
                return Convert.ChangeType(value, _typeCode, null);
            }
        }

        #region implemented abstract members of DataFieldMapping

        public override object ToColumn(object value)
        {
            if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
                if (_defaultValue != null) {
                    return _defaultValue;
                } else {
                    if (IsNullable) {
                        return null;
                    } else {
                        return _minValue;
                    }
                }
            } else {
                return Convert.ChangeType(value, _typeCode, null);
            }
        }

        #endregion
    }
}
