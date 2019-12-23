using System;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    internal class DataParameterMapping
    {
        public ParameterDirection Direction { get; }

        public Type ParameterType { get; }

        public bool ConvertString { get; }

        public GetValueHandler Get { get; }

        public SetValueHandler Set { get; }

        public PropertyInfo Property { get; }

        public string Name { get; }

        public DataParameterMapping(PropertyInfo property, string name, ParameterDirection direction)
        {
            if (property.CanRead) {
                Get = ReflectionHandlerFactory.PropertyGetHandler(property);
            }
            if (property.CanWrite) {
                Set = ReflectionHandlerFactory.PropertySetHandler(property);
            }
            ParameterType = property.PropertyType;
            var code = Type.GetTypeCode(ParameterType);
            ConvertString = code == TypeCode.Object || code == TypeCode.DBNull || code == TypeCode.Empty;
            if ((Direction == ParameterDirection.InputOutput || Direction == ParameterDirection.Output) && ConvertString) {
                throw new LightDataException(SR.OutputParameterNotSupportObjectType);
            }
            Property = property;
            Name = string.IsNullOrEmpty(name) ? property.Name : name;
            Direction = direction;
        }
    }
}
