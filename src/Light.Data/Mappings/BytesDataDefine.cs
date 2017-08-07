using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data
{
    class BytesDataDefine : DataDefine
    {
        public BytesDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {

        }

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
                if (IsNullable) {
                    return null;
                } else {
                    return new byte[0];
                }
            } else {
                return value;
            }
        }

        public override object LoadData(DataContext context, IDataReader datareader, string name, object state)
        {
            object value = datareader[name];
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                if (IsNullable) {
                    return null;
                } else {
                    return new byte[0];
                }
            } else {
                return value;
            }
        }
    }
}
