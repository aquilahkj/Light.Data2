using System;
using System.Data;

namespace Light.Data
{
    internal class EnumDataDefine : DataDefine
    {
        private readonly TypeCode _typeCode;

        private readonly object _defaultValue = null;

        public EnumDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
            var values = Enum.GetValues(type);
            _defaultValue = values.GetValue(0);
            _typeCode = Type.GetTypeCode(ObjectType);
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var value = datareader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                if (!IsNullable) {
                    return _defaultValue;
                } else {
                    return null;
                }
            } else {
                value = Enum.ToObject(_objectType, value);
                return value;
            }
        }

        public override object LoadData(DataContext context, IDataReader datareader, string name, object state)
        {
            var value = datareader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                if (!IsNullable) {
                    return _defaultValue;
                } else {
                    return null;
                }
            } else {
                value = Enum.ToObject(_objectType, value);
                return value;
            }
        }
    }
}
