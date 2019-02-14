using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Light.Data
{
    static class ParameterConvert
    {
        static readonly Dictionary<Type, DataParameterMapping[]> TypeDict = new Dictionary<Type, DataParameterMapping[]>();

        public static DataParameter[] ConvertParameter(object data)
        {
            if (data == null) {
                return null;
            }
            Type type = data.GetType();
            if (!TypeDict.TryGetValue(type, out DataParameterMapping[] mappings)) {
                lock (TypeDict) {
                    if (!TypeDict.TryGetValue(type, out mappings)) {
                        TypeInfo typeInfo = type.GetTypeInfo();
                        PropertyInfo[] properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        List<DataParameterMapping> list = new List<DataParameterMapping>();
                        foreach (PropertyInfo propertie in properties) {
                            var handler = new PropertyHandler(propertie);
                            string name = null;
                            DataParameterMode direction = DataParameterMode.Input;
                            var attributes = AttributeCore.GetPropertyAttributes<DataParameterAttribute>(propertie, true);
                            if (attributes.Length > 0) {
                                var attribute = attributes[0];
                                name = attribute.Name;
                                direction = attribute.Direction;
                            }
                            DataParameterMapping mapping = new DataParameterMapping(propertie, name, direction);
                            list.Add(mapping);
                        }
                        mappings = list.ToArray();
                        TypeDict.Add(type, mappings);
                    }
                }
            }
            if (mappings.Length == 0) {
                return null;
            }
            DataParameter[] dataParameters = new DataParameter[mappings.Length];
            for (int i = 0; i < mappings.Length; i++) {
                DataParameterMapping mapping = mappings[i];
                object value = mapping.Get(data);
                if (!Object.Equals(value, null) && mapping.ConvertString) {
                    value = value.ToString();
                }
                DataParameter dataParameter;
                if (mapping.Direction == DataParameterMode.Input) {
                    dataParameter = new DataParameter(mapping.Name, value, mapping.Direction);
                }
                else {
                    dataParameter = new CallbackDataParameter(mapping.Name, value, mapping.Direction, data, mapping);
                }
                dataParameters[i] = dataParameter;
            }
            return dataParameters;
        }
    }
}
