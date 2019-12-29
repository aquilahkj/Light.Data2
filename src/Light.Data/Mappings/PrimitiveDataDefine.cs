using System;
using System.Data;

namespace Light.Data
{
    internal class PrimitiveDataDefine : DataDefine
    {
        public PrimitiveDataDefine(Type type, bool isNullable)
            : base(type, isNullable)
        {
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="context">Context.</param>
        /// <param name="dataReader">DataReader.</param>
        /// <param name="state">State.</param>
        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var value = dataReader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable)
                {
                    return ObjectType.GetDefaultValue();
                }

                return null;
            }

            if (value.GetType() != ObjectType)
            {
                return Convert.ChangeType(value, ObjectType);
            }

            return value;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, string name, object state)
        {
            var value = dataReader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable) {
                    return ObjectType.GetDefaultValue();
                }

                return null;
            }

            if (value.GetType() != ObjectType)
            {
                return Convert.ChangeType(value, ObjectType);
            }

            return value;
        }
    }
}
