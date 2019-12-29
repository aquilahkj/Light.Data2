using System.Collections.Generic;

namespace Light.Data
{
	internal class DataTableMapperSetting
	{
		private readonly Dictionary<string, DataFieldMapperConfig> dataFieldMapConfigDict = new Dictionary<string, DataFieldMapperConfig>();

		private readonly Dictionary<string, RelationFieldMapConfig> relationFieldMapConfigDict = new Dictionary<string, RelationFieldMapConfig>();

		public DataTableMapperConfig DataTableMapConfig { get; }

		public DataTableMapperSetting(DataTableMapperConfig dataTableMapConfig)
		{
			DataTableMapConfig = dataTableMapConfig;
		}

		public void AddDataFieldMapConfig(string fieldName, DataFieldMapperConfig config)
		{
			dataFieldMapConfigDict.Add(fieldName, config);
		}

		public DataFieldMapperConfig GetDataFieldMapConfig(string fieldName){
			dataFieldMapConfigDict.TryGetValue(fieldName, out var config);
			return config;
		}

		public void AddRelationFieldMapConfig(string fieldName, RelationFieldMapConfig config)
		{
			relationFieldMapConfigDict.Add(fieldName, config);
		}

		public RelationFieldMapConfig GetRelationFieldMapConfig(string fieldName)
		{
			relationFieldMapConfigDict.TryGetValue(fieldName, out var config);
			return config;
		}

	}
}
