using System;
using System.Data;

namespace Light.Data
{
    internal class StringDataDefine : DataDefine
    {
        private readonly object _defaultValue;

        public StringDataDefine(bool isNullable)
            : base(typeof(string), isNullable)
        {
            _defaultValue = string.Empty;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var value = dataReader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable) {
                    return _defaultValue;
                }

                return null;
            }

            return value;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, string name, object state)
        {
            var value = dataReader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable) {
                    return _defaultValue;
                }

                return null;
            }

            return value;
        }
    }
}
