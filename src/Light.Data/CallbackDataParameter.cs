using System;
using System.Data;

namespace Light.Data
{
    internal class CallbackDataParameter : DataParameter
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
                var value = Value;
                if (!Equals(value, null)) {
                    var type = value.GetType();
                    if (type != mapping.ParameterType)
                    {
                        value = mapping.ParameterType == typeof(string) ? value.ToString() : Convert.ChangeType(value, mapping.ParameterType);
                    }
                }
                mapping.Set(callbackData, value);
                return true;
            }

            return false;
        }
    }
}
