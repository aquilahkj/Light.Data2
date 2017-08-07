using System;

namespace Light.Data
{
    class BytesFieldMapping : DataFieldMapping
    {
        public BytesFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {

        }

        public override object ToColumn(object value)
        {
            if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
                if (IsNullable) {
                    return null;
                }
                else {
                    return new byte[0];
                }
            }
            else {
                return value;
            }
        }

        public override object ToParameter(object value)
        {
            if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
                return null;
            }
            else {
                return value;
            }
        }

        public override object ToProperty(object value)
        {
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                return null;
            }
            else {
                return value;
            }
        }
    }
}
