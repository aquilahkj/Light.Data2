using System;
using System.Data;

namespace Light.Data
{
    internal class BytesDataDefine : DataDefine
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
            var value = datareader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
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
            var value = datareader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
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
