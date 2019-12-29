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
        
        public override bool IsPrimaryKey => false;
        public override bool IsIdentity => false;
        public override bool IsAutoUpdate => false;

        public EnumFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable,
            string dbType, object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType,
                    fieldName, type));
            }

            if (isPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportPrimaryKeyFieldType, ObjectType,
                    fieldName, type));
            }

            var nullType = Type.GetType("System.Nullable`1", true);
            NullableType = nullType.MakeGenericType(type);
            var values = Enum.GetValues(ObjectType);
            var value = values.GetValue(0);
            _min = value;
            _minValue = Convert.ChangeType(value, _typeCode, null);

            if (defaultValue != null)
            {
                var defaultValueType = defaultValue.GetType();
                if (defaultValueType == type)
                {
                    _default = defaultValue;
                    _defaultValue = Convert.ChangeType(defaultValue, _typeCode, null);
                }
                else
                {
                    throw new LightDataException(string.Format(SR.EnumDefaultValueType, mapping.ObjectType, fieldName,
                        defaultValue));
                }
            }
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                return null;
            }

            value = Enum.ToObject(ObjectType, value);
            return value;
        }


        public override object ToParameter(object value)
        {
            if (Equals(value, null))
            {
                return null;
            }

            return Convert.ChangeType(value, _typeCode, null);
        }

        public override object ToUpdate(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null))
            {
                if (IsNullable)
                {
                    return null;
                }

                if (refreshField)
                {
                    Handler.Set(entity, _min);
                }

                return _minValue;
            }

            return Convert.ChangeType(value, _typeCode, null);
        }

        public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null))
            {
                if (_defaultValue != null)
                {
                    if (refreshField)
                    {
                        Handler.Set(entity, _default);
                    }

                    return _defaultValue;
                }

                if (IsNullable)
                {
                    return null;
                }

                if (refreshField)
                {
                    Handler.Set(entity, _min);
                }

                return _minValue;
            }

            return Convert.ChangeType(value, _typeCode, null);
            
            
        }
    }
}