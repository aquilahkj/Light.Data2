using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    internal static class ParameterConvert
    {
        private static readonly Dictionary<Type, DataParameterMapping[]> TypeDict =
            new Dictionary<Type, DataParameterMapping[]>();

        public static DataParameter[] ConvertParameter(object data)
        {
            if (data == null)
            {
                return null;
            }

            var type = data.GetType();
            if (!TypeDict.TryGetValue(type, out var mappings))
            {
                lock (TypeDict)
                {
                    if (!TypeDict.TryGetValue(type, out mappings))
                    {
                        var typeInfo = type.GetTypeInfo();
                        var properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        var list = new List<DataParameterMapping>();
                        foreach (var property in properties)
                        {
                            var handler = new PropertyHandler(property);
                            string name = null;
                            var direction = ParameterDirection.Input;
                            var attributes =
                                AttributeCore.GetPropertyAttributes<DataParameterAttribute>(property, true);
                            if (attributes.Length > 0)
                            {
                                var attribute = attributes[0];
                                name = attribute.Name;
                                direction = attribute.Direction;
                            }

                            var mapping = new DataParameterMapping(property, name, direction);
                            list.Add(mapping);
                        }

                        mappings = list.ToArray();
                        TypeDict.Add(type, mappings);
                    }
                }
            }

            if (mappings.Length == 0)
            {
                return null;
            }

            var dataParameters = new DataParameter[mappings.Length];
            for (var i = 0; i < mappings.Length; i++)
            {
                var mapping = mappings[i];
                var value = mapping.Get(data);
                if (!Equals(value, null) && mapping.ConvertString)
                {
                    value = value.ToString();
                }

                var dataParameter = mapping.Direction != ParameterDirection.Input
                    ? new CallbackDataParameter(mapping.Name, value, mapping.Direction, data, mapping)
                    : new DataParameter(mapping.Name, value, mapping.Direction);
                dataParameters[i] = dataParameter;
            }

            return dataParameters;
        }
    }
}