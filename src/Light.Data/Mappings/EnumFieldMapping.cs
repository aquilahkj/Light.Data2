using System;

namespace Light.Data
{
    internal class EnumFieldMapping : DataFieldMapping
    {
        private readonly object _minValue;

        private readonly object _defaultValue;

        private readonly object _min;

        private readonly object _default;

        public Type NullableType { get; }

        public EnumFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, object defaultValue)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            var itemstype = Type.GetType("System.Nullable`1");
            NullableType = itemstype.MakeGenericType(type);
            var values = Enum.GetValues(ObjectType);
            var value = values.GetValue(0);
            _min = value;
            _minValue = Convert.ChangeType(value, _typeCode, null);

            if (defaultValue != null) {
                var defaultValueType = defaultValue.GetType();
                if (defaultValueType == type) {
                    _default = defaultValue;
                    _defaultValue = Convert.ChangeType(defaultValue, _typeCode, null);
                } else {
                    throw new LightDataException(string.Format(SR.EnumDefaultValueType, mapping.ObjectType, fieldName, defaultValue));
                }
            }
        }

        public EnumFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable)
            : base(type, fieldName, indexName, mapping, isNullable, null)
        {
            var itemstype = Type.GetType("System.Nullable`1");
            NullableType = itemstype.MakeGenericType(type);
            var values = Enum.GetValues(ObjectType);
            var value = values.GetValue(0);
            _minValue = Convert.ChangeType(value, _typeCode, null);
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                return null;
            } else {
                value = Enum.ToObject(_objectType, value);
                return value;
            }
        }

        public override object ToParameter(object value)
        {
            if (Equals(value, null)) {
                return null;
            } else {
                return Convert.ChangeType(value, _typeCode, null);
            }
        }

        #region implemented abstract members of DataFieldMapping

        //public override object ToColumn(object value)
        //{
        //    if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
        //        if (_defaultValue != null) {
        //            return _defaultValue;
        //        } else {
        //            if (IsNullable) {
        //                return null;
        //            } else {
        //                return _minValue;
        //            }
        //        }
        //    } else {
        //        return Convert.ChangeType(value, _typeCode, null);
        //    }
        //}



        public override object GetInsertData(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (_defaultValue != null) {
                    if(refreshField) {
                        Handler.Set(entity, _default);
                    }
                    return _defaultValue;
                }
                else {
                    if (IsNullable) {
                        return null;
                    }
                    else {
                        if (refreshField) {
                            Handler.Set(entity, _min);
                        }
                        return _minValue;
                    }
                }
            }
            else {
                return Convert.ChangeType(value, _typeCode, null);
            }
        }
            #endregion
        }
}
