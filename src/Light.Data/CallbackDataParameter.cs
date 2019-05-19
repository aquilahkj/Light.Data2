using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Light.Data
{
    class CallbackDataParameter : DataParameter
    {
        private readonly object callbackData;
        private readonly DataParameterMapping mapping;

        public CallbackDataParameter(string paramName, object paramValue, ParameterDirection direction, object callbackData, DataParameterMapping mapping)
            : base(paramName, paramValue, direction)
        {
            this.callbackData = callbackData;
            this.mapping = mapping;
        }

        internal override bool Callback()
        {
            if (base.Callback()) {
                object value = Value;
                if (!Object.Equals(value, null)) {
                    Type type = value.GetType();
                    if (type != mapping.ParameterType) {
                        if (mapping.ParameterType == typeof(string)) {
                            value = value.ToString();
                        }
                        else {
                            value = Convert.ChangeType(value, mapping.ParameterType);
                        }
                    }
                }
                mapping.Set(callbackData, value);
                return true;
            }
            else {
                return false;
            }
        }
    }
}
