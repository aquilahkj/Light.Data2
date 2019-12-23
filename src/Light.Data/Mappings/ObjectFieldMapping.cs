using System;
using Newtonsoft.Json;

namespace Light.Data
{
    internal class ObjectFieldMapping : DataFieldMapping
    {
        private string emptyData;

        public ObjectFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (!isNullable) {
                var value = Activator.CreateInstance(type);
                emptyData = JsonConvert.SerializeObject(value);
            }
        }

        public override object GetInsertData(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }
                else {
                    if (refreshField) {
                        var obj = Activator.CreateInstance(ObjectType);
                        Handler.Set(entity, obj);
                    }
                    return emptyData;
                }
            }
            else {
                var data = JsonConvert.SerializeObject(value);
                return data;
            }
        }

        //public override object ToColumn(object value)
        //{
        //    if (Object.Equals(value, null) || Object.Equals(value, DBNull.Value)) {
        //        if (IsNullable) {
        //            return null;
        //        }
        //        else {
        //            return emptyData;
        //        }
        //    }
        //    else {
        //        string data = JsonConvert.SerializeObject(value);
        //        return data;
        //    }
        //}

        public override object ToParameter(object value)
        {
            if (Equals(value, null)) {
                return null;
            }
            else {
                var data = JsonConvert.SerializeObject(value);
                return data;
            }
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                return null;
            }
            else {
                var data = value as string;
                value = JsonConvert.DeserializeObject(data, ObjectType);
                return value;
            }
        }
    }
}
