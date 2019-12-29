using System;
using Newtonsoft.Json;

namespace Light.Data
{
    internal class ObjectDynamicFieldMapping : DynamicFieldMapping
    {
        public ObjectDynamicFieldMapping(Type type, string fieldName, DynamicDataMapping mapping)
            : base(type, fieldName, mapping, true)
        {
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
    }
}
