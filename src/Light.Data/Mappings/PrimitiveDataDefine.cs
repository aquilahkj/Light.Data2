using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    class PrimitiveDataDefine : DataDefine
    {
        public PrimitiveDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
            _typeCode = Type.GetTypeCode(type);
        }

        TypeCode _typeCode;

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="context">Context.</param>
        /// <param name="datareader">Datareader.</param>
        /// <param name="state">State.</param>
        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            object value = datareader[0];
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                if (!IsNullable) {
                    return Utility.GetDefaultValue(_typeCode);
                } else {
                    return null;
                }
            } else {
                return Convert.ChangeType(value, ObjectType);
            }
        }

        public override object LoadData(DataContext context, IDataReader datareader, string name, object state)
        {
            object value = datareader[name];
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                if (!IsNullable) {
                    return Utility.GetDefaultValue(_typeCode);
                } else {
                    return null;
                }
            } else {
                return Convert.ChangeType(value, ObjectType);
            }
        }
    }
}
