using System;

namespace Light.Data
{
    internal class GuidFieldMapping : DataFieldMapping
    {
        private readonly object _minValue;

        private readonly object _defaultValue;
        
        private readonly object _min;

        private readonly object _default;

        public override bool IsPrimaryKey { get; }

        public override bool IsIdentity => false;
        
        public override bool IsAutoUpdate => false;

        public Type NullableType { get; }

        private readonly GuidStoreMode _mode;

        public GuidFieldMapping(string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType,
            object defaultValue, bool isIdentity, bool isPrimaryKey)
            : base(typeof(Guid), fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType,
                    fieldName, ObjectType));
            }

            IsPrimaryKey = isPrimaryKey;

            var nullType = Type.GetType("System.Nullable`1", true);
            NullableType = nullType.MakeGenericType(ObjectType);

            if (dbType != null && (dbType.StartsWith("varchar") || dbType.StartsWith("char") ||
                                   dbType.Equals("string", StringComparison.OrdinalIgnoreCase)))
            {
                _mode = GuidStoreMode.String;
            }
            else
            {
                _mode = GuidStoreMode.Raw;
            }

            _min = Guid.Empty;
            if (_mode == GuidStoreMode.Raw)
            {
                _minValue = Guid.Empty.ToByteArray();
            }
            else
            {
                _minValue = Guid.Empty.ToString();
            }


            if (defaultValue != null)
            {
                if (defaultValue is string valueString && Guid.TryParse(valueString, out var guidValue))
                {
                    _default = guidValue;
                    if (_mode == GuidStoreMode.Raw)
                    {
                        _defaultValue = guidValue.ToByteArray();
                    }
                    else
                    {
                        _defaultValue = guidValue.ToString();
                    }
                }
                else
                {
                    throw new LightDataException(string.Format(SR.DataMappingUnsupportFieldType, ObjectType, fieldName,
                        ObjectType));
                }
            }
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                return null;
            }

            if (value is string valueString)
            {
                value = Guid.Parse(valueString);
            }
            else if (value is byte[] valueBuffer)
            {
                value = new Guid(valueBuffer);
            }

            return value;
        }

        public override object ToParameter(object value)
        {
            if (Equals(value, null))
            {
                return null;
            }

            var guid = (Guid) value;
            if (_mode == GuidStoreMode.Raw)
            {
                value = guid.ToByteArray();
            }
            else
            {
                value = guid.ToString();
            }

            return value;
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

            var guid = (Guid) value;
            if (_mode == GuidStoreMode.Raw)
            {
                value = guid.ToByteArray();
            }
            else
            {
                value = guid.ToString();
            }

            return value;
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

            var guid = (Guid) value;
            if (_mode == GuidStoreMode.Raw)
            {
                value = guid.ToByteArray();
            }
            else
            {
                value = guid.ToString();
            }

            return value;
        }

        private enum GuidStoreMode
        {
            Raw,
            String
        }
    }
}