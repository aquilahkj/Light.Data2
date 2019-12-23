using System;
using System.Data;

namespace Light.Data
{
    internal class PrimitiveDataDefine : DataDefine
    {
        public PrimitiveDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
            _typeCode = Type.GetTypeCode(type);
        }

        private TypeCode _typeCode;

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="context">Context.</param>
        /// <param name="datareader">Datareader.</param>
        /// <param name="state">State.</param>
        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var value = datareader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
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
            var value = datareader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
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
