using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Light.Data
{
    class DynamicObjectFieldMapping : DynamicFieldMapping
    {
        public DynamicObjectFieldMapping(Type type, string fieldName, DynamicCustomMapping mapping)
            : base(type, fieldName, mapping, true)
        {
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
