using System;
using System.Data;

namespace Light.Data
{
    class EnumDataDefine : DataDefine
    {
        readonly TypeCode _typeCode;

        readonly object _defaultValue = null;

        public EnumDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
            Array values = Enum.GetValues(type);
            _defaultValue = values.GetValue(0);
            _typeCode = Type.GetTypeCode(ObjectType);
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            object value = datareader[0];
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
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
            object value = datareader[name];
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
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
