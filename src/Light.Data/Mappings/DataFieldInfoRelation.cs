namespace Light.Data
{
	internal class DataFieldInfoRelation
	{
		public DataFieldInfo RelateInfo { get; }

		public DataFieldInfo MasterInfo { get; }

		public DataFieldInfoRelation (DataFieldInfo masterInfo, DataFieldInfo relateInfo)
		{
			this.MasterInfo = masterInfo;
			this.RelateInfo = relateInfo;
		}
	}
}

