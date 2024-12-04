using System;
using System.Data;
using System.Text.Json;

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

            var json = JsonSerializer.Serialize(value);
            return JsonSerializer.Deserialize<object>(json);
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

            var json = JsonSerializer.Serialize(value);
            return JsonSerializer.Deserialize<object>(json);
        }
    }
}
