using System;
using System.Data;

namespace Light.Data
{
    internal class EnumDataDefine : DataDefine
    {
        private readonly object _defaultValue;

        public EnumDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
            var values = Enum.GetValues(type);
            _defaultValue = values.GetValue(0);
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

            value = Enum.ToObject(ObjectType, value);
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

            value = Enum.ToObject(ObjectType, value);
            return value;
        }
    }
}
