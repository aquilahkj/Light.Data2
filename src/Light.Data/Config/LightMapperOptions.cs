using System.Collections.Generic;

namespace Light.Data
{
    internal class LightMapperOptions
    {
        public DataTypeSection[] DataTypes { get; set; }
    }

    internal class DataTypeSection
    {
        public string Type { get; set; }

        public string TableName { get; set; }

        public bool? IsEntityTable { get; set; }

        public DataFieldSection[] DataFields { get; set; }

        public RelationFieldSection[] RelationFields { get; set; }

        public Dictionary<string, string> ConfigParams { get; set; }
    }

    internal class DataFieldSection
    {
        public string FieldName { get; set; }

        public string Name { get; set; }

        public bool IsNullable { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsIdentity { get; set; }

        public string DbType { get; set; }

        public string DefaultValue { get; set; }

        public int DataOrder { get; set; }

        public string FunctionControl { get; set; }
    }

    internal class RelationFieldSection
    {
        public string FieldName { get; set; }

        public RelationPairConfig[] RelationPairs { get; set; }
    }

    internal class RelationPairConfig
    {
        public string MasterKey { get; set; }

        public string RelateKey { get; set; }
    }
}
