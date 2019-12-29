using System;
using System.Data;
using Newtonsoft.Json;

namespace Light.Data
{
    internal class ObjectDataDefine : DataDefine
    {

        public ObjectDataDefine(Type type,  bool isNullable)
            : base(type, isNullable)
        {
      
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var value = dataReader[0];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable) {
                    return Activator.CreateInstance(ObjectType);
                }

                return null;
            }

            return JsonConvert.SerializeObject(value);
        }

        public override object LoadData(DataContext context, IDataReader dataReader, string name, object state)
        {
            var value = dataReader[name];
            if (Equals(value, DBNull.Value) || Equals(value, null))
            {
                if (!IsNullable) {
                    return Activator.CreateInstance(ObjectType);
                }

                return null;
            }

            return JsonConvert.SerializeObject(value);;
        }
    }
}
