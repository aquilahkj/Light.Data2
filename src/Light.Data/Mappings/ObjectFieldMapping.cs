using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Light.Data
{
    class ObjectFieldMapping : DataFieldMapping
    {
        string emptyData;

        public ObjectFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (!isNullable) {
                object value = Activator.CreateInstance(type);
                emptyData = JsonConvert.SerializeObject(value);
            }
        }

        public override object GetInsertData(object entity, bool refreshField)
        {
            object value = Handler.Get(entity);
            if (Object.Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }
                else {
                    if (refreshField) {
                        object obj = Activator.CreateInstance(ObjectType);
                        Handler.Set(entity, obj);
                    }
                    return emptyData;
                }
            }
            else {
                string data = JsonConvert.SerializeObject(value);
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
            if (Object.Equals(value, null)) {
                return null;
            }
            else {
                string data = JsonConvert.SerializeObject(value);
                return data;
            }
        }

        public override object ToProperty(object value)
        {
            if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
                return null;
            }
            else {
                string data = value as string;
                value = JsonConvert.DeserializeObject(data, ObjectType);
                return value;
            }
        }
    }
}
