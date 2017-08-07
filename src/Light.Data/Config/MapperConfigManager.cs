using System;
using System.Reflection;

namespace Light.Data
{
    static class MapperConfigManager
    {
        public static DataTableMapperConfig LoadDataTableConfig(Type type)
        {
            if (DataMapperConfiguration.TryGetSetting(type, out DataTableMapperSetting setting)) {
                return setting.DataTableMapConfig;
            }

            var attributes = AttributeCore.GetTypeAttributes<DataTableAttribute>(type, true);
            if (attributes.Length > 0) {
                var attribute = attributes[0];
                var paramAttributes = AttributeCore.GetTypeAttributes<ConfigParamAttribute>(type, true);
                var configParam = new ConfigParamSet();
                if (paramAttributes != null && paramAttributes.Length > 0) {
                    foreach (var extendAttribute in paramAttributes) {
                        configParam.SetParamValue(extendAttribute.Name, extendAttribute.Value);
                    }
                }
                var config = new DataTableMapperConfig(type) {
                    TableName = attribute.TableName,
                    IsEntityTable = attribute.IsEntityTable,
                    ConfigParams = configParam
                };
                return config;
            }
            else {
                return null;
            }
        }

        public static DataFieldMapperConfig LoadDataFieldConfig(Type type, PropertyInfo pi)
        {
            if (DataMapperConfiguration.TryGetSetting(type, out DataTableMapperSetting setting)) {
                var fieldMapConfig = setting.GetDataFieldMapConfig(pi.Name);
                if (fieldMapConfig != null)
                    return fieldMapConfig;
            }

            var attributes = AttributeCore.GetPropertyAttributes<DataFieldAttribute>(pi, true);
            if (attributes.Length > 0) {
                var attribute = attributes[0];
                var config = new DataFieldMapperConfig(pi.Name) {
                    Name = attribute.Name,
                    IsPrimaryKey = attribute.IsPrimaryKey,
                    IsNullable = attribute.IsNullable,
                    IsIdentity = attribute.IsIdentity,
                    DefaultValue = attribute.DefaultValue,
                    DbType = attribute.DbType,
                    DataOrder = attribute.DataOrder
                };
                return config;
            }
            else {
                return null;
            }
        }

        public static RelationFieldMapConfig LoadRelationDataFieldConfig(Type type, PropertyInfo pi)
        {
            if (DataMapperConfiguration.TryGetSetting(type, out DataTableMapperSetting setting)) {
                var fieldMapConfig = setting.GetRelationFieldMapConfig(pi.Name);
                if (fieldMapConfig != null)
                    return fieldMapConfig;
            }
            var relationAttributes = AttributeCore.GetPropertyAttributes<RelationFieldAttribute>(pi, true);
            if (relationAttributes.Length > 0) {
                RelationFieldMapConfig rfConfig = new RelationFieldMapConfig(pi.Name);
                foreach (var ra in relationAttributes) {
                    rfConfig.AddRelationKeys(ra.MasterKey, ra.RelateKey);
                }
                return rfConfig;
            }
            else {
                return null;
            }
        }
    }
}
