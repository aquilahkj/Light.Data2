using System.Collections.Generic;
namespace Light.Data
{
	class DataTableMapperSetting
	{
		readonly DataTableMapperConfig dataTableMapConfig;

		readonly Dictionary<string, DataFieldMapperConfig> dataFieldMapConfigDict = new Dictionary<string, DataFieldMapperConfig>();

		readonly Dictionary<string, RelationFieldMapConfig> relationFieldMapConfigDict = new Dictionary<string, RelationFieldMapConfig>();

		public DataTableMapperConfig DataTableMapConfig {
			get {
				return dataTableMapConfig;
			}
		}

		public DataTableMapperSetting(DataTableMapperConfig dataTableMapConfig)
		{
			this.dataTableMapConfig = dataTableMapConfig;
		}

		public void AddDataFieldMapConfig(string fieldName, DataFieldMapperConfig config)
		{
			this.dataFieldMapConfigDict.Add(fieldName, config);
		}

		public DataFieldMapperConfig GetDataFieldMapConfig(string fieldName){
			this.dataFieldMapConfigDict.TryGetValue(fieldName, out DataFieldMapperConfig config);
			return config;
		}

		public void AddRelationFieldMapConfig(string fieldName, RelationFieldMapConfig config)
		{
			this.relationFieldMapConfigDict.Add(fieldName, config);
		}

		public RelationFieldMapConfig GetRelationFieldMapConfig(string fieldName)
		{
			this.relationFieldMapConfigDict.TryGetValue(fieldName, out RelationFieldMapConfig config);
			return config;
		}

	}
}
