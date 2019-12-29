using System;
using System.Data;

namespace Light.Data
{
    internal class DateTimeDataDefine : DataDefine
    {
        private readonly object _defaultValue;

        public DateTimeDataDefine(bool isNullable)
            : base(typeof(DateTime), isNullable)
        {
            _defaultValue = default(DateTime);
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

            if (value.GetType() != ObjectType)
            {
                return Convert.ToDateTime(value);
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
            if (value.GetType() != ObjectType)
            {
                return Convert.ToDateTime(value);
            }

            return value;
        }
    }
}
