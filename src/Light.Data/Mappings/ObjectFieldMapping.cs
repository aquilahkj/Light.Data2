using System;
using Newtonsoft.Json;

namespace Light.Data
{
    internal class ObjectFieldMapping : DataFieldMapping
    {
        private readonly string _defaultValue;
        
        public override bool IsPrimaryKey => false;
        public override bool IsIdentity => false;
        
        public override bool IsAutoUpdate => false;

        public ObjectFieldMapping(Type type, string fieldName, string indexName, DataMapping mapping, bool isNullable, string dbType, bool isIdentity, bool isPrimaryKey)
            : base(type, fieldName, indexName, mapping, isNullable, dbType)
        {
            if (isIdentity)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportIdentityFieldType, ObjectType, fieldName, type));
            }

            if (isPrimaryKey)
            {
                throw new LightDataException(string.Format(SR.DataMappingUnsupportPrimaryKeyFieldType, ObjectType, fieldName, type));
            }
            
            if (!isNullable) {
                var value = Activator.CreateInstance(type);
                _defaultValue = JsonConvert.SerializeObject(value);
            }
        }

     

        public override object ToParameter(object value)
        {
            if (Equals(value, null)) {
                return null;
            }

            var data = JsonConvert.SerializeObject(value);
            return data;
        }

        public override object ToProperty(object value)
        {
            if (Equals(value, DBNull.Value) || Equals(value, null)) {
                return null;
            }

            var data = value as string;
            value = JsonConvert.DeserializeObject(data, ObjectType);
            return value;
        }
        
        public override object ToUpdate(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }

                if (refreshField) {
                    var obj = Activator.CreateInstance(ObjectType);
                    Handler.Set(entity, obj);
                }
                return _defaultValue;
            }

            var data = JsonConvert.SerializeObject(value);
            return data;
        }
        
        public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            if (Equals(value, null)) {
                if (IsNullable) {
                    return null;
                }

                if (refreshField) {
                    var obj = Activator.CreateInstance(ObjectType);
                    Handler.Set(entity, obj);
                }
                return _defaultValue;
            }

            var data = JsonConvert.SerializeObject(value);
            return data;
        }

        
    }
}
