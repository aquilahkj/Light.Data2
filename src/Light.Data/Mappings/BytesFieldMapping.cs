using System;

namespace Light.Data
{
    internal class BytesFieldMapping : DataFieldMapping
    {
        public BytesFieldMapping(string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, bool isIdentity, bool isPrimaryKey)
            : base(typeof(byte[]), fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType, fieldName, ObjectType));
            }

            if (isPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportPrimaryKeyFieldType, ObjectType, fieldName, ObjectType));
            }
        }

        public override bool IsPrimaryKey => false;
        public override bool IsIdentity => false;

        public override bool IsAutoUpdate => false;

        public override object ToUpdate(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }

                object result = new byte[0];
                if (refreshField) {
                    Handler.Set(entity, result);
                }
                return result;
            }

            return value;
        }
        
        public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }

                object result = new byte[0];
                if (refreshField) {
                    Handler.Set(entity, result);
                }
                return result;
            }

            return value;
        }

        public override object ToParameter(object value)
        {
            if (Equals(value, null) || Equals(value, DBNull.Value)) {
                return null;
            }

            return value;
        }

        
        
        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                return null;
            }

            return value;
        }
    }
}
