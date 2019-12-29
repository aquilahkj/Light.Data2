using System;

namespace Light.Data
{
    internal class DateTimeFieldMapping : DataFieldMapping
    {
        private readonly object _minValue;

        private readonly object _defaultValue;

        private readonly DefaultTimeFunction _defaultTimeFunction;

        private readonly bool _isTimeStamp;

        public Type NullableType { get; }

        public override bool IsPrimaryKey { get; }
        public override bool IsIdentity => false;
        public override bool IsAutoUpdate => _isTimeStamp;
        
        public DateTimeFieldMapping(string fieldName, string indexName, DataMapping mapping, bool isNullable,
            string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(typeof(DateTime), fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType,
                    fieldName, ObjectType));
            }

            IsPrimaryKey = isPrimaryKey;

            var nullType = Type.GetType("System.Nullable`1", true);
            NullableType = nullType.MakeGenericType(ObjectType);
            _minValue = default(DateTime);

            if (defaultValue != null)
            {
                var defaultValueType = defaultValue.GetType();
                if (defaultValueType == typeof(DefaultTime))
                {
                    var defaultTime = (DefaultTime) defaultValue;
                    _defaultTimeFunction = DefaultTimeFunction.GetFunction(defaultTime);
                    if (defaultTime == DefaultTime.TimeStamp || defaultTime == DefaultTime.UtcTimeStamp)
                    {
                        _isTimeStamp = true;
                    }

                    _defaultValue = _defaultTimeFunction;
                }
                else if (defaultValueType == typeof(DateTime))
                {
                    _defaultValue = defaultValue;
                }
                else if (defaultValueType == typeof(string))
                {
                    var str = defaultValue as string;
                    if (DateTime.TryParse(str, out var dt))
                    {
                        _defaultValue = dt;
                    }
                    else
                    {
                        throw new LightDataException(string.Format(SR.DefaultValueError, ObjectType, fieldName,
                            ObjectType));
                    }
                }
            }
        }


        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                return null;
            }
            if (value.GetType() != ObjectType)
            {
                value = Convert.ToDateTime(value);
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
            else if (_isTimeStamp && _defaultTimeFunction != null)
            {
                useDef = true;
                result = _defaultTimeFunction.GetValue();
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

        public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            object result;
            var useDef = false;
            if (Equals(value, null))
            {
                if (_defaultTimeFunction != null)
                {
                    useDef = true;
                    result = _defaultTimeFunction.GetValue();
                }
                else if (_defaultValue != null)
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
            else if (Equals(value, _minValue))
            {
                if (_defaultTimeFunction != null)
                {
                    useDef = true;
                    result = _defaultTimeFunction.GetValue();
                }
                else if (_defaultValue != null)
                {
                    useDef = true;
                    result = _defaultValue;
                }
                else
                {
                    result = value;
                }
            }
            else if (_isTimeStamp && _defaultTimeFunction != null)
            {
                useDef = true;
                result = _defaultTimeFunction.GetValue();
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
    }
}