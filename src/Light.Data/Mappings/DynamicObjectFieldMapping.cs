using System;
using Newtonsoft.Json;

namespace Light.Data
{
    internal class DynamicObjectFieldMapping : DynamicFieldMapping
    {
        public DynamicObjectFieldMapping(Type type, string fieldName, DynamicCustomMapping mapping)
            : base(type, fieldName, mapping, true)
        {
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
