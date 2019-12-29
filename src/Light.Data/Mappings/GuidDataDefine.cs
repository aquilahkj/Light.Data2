using System;
using System.Data;

namespace Light.Data
{
    internal class GuidDataDefine : DataDefine
    {
        private readonly object _defaultValue;

        public GuidDataDefine(bool isNullable)
            : base(typeof(Guid), isNullable)
        {
            _defaultValue = Guid.Empty;
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
    }
}
