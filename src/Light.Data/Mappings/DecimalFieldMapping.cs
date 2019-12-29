using System;

namespace Light.Data
{
    internal class DecimalFieldMapping : DataFieldMapping
    {
        private readonly object _minValue;

        private readonly object _defaultValue;

        public override bool IsPrimaryKey { get; }

        public override bool IsIdentity => false;

        public override bool IsAutoUpdate => false;
        
        public Type NullableType { get; }


        public DecimalFieldMapping(string fieldName, string indexName, DataMapping mapping,
            bool isNullable, string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(typeof(decimal), fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType,
                    fieldName, ObjectType));
            }

            IsPrimaryKey = isPrimaryKey;

            var nullType = Type.GetType("System.Nullable`1", true);
            NullableType = nullType.MakeGenericType(ObjectType);

            _minValue = ObjectType.GetDefaultValue();

            if (defaultValue != null)
            {
                var defaultValueType = defaultValue.GetType();
                _defaultValue = defaultValueType == ObjectType ? defaultValue : Convert.ChangeType(defaultValue, ObjectType);
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
                value = Convert.ToDecimal(value);
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
    }
}