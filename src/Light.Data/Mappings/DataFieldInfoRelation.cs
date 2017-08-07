using System;

namespace Light.Data
{
	class DataFieldInfoRelation
	{
		readonly DataFieldInfo relateInfo;

		public DataFieldInfo RelateInfo {
			get {
				return relateInfo;
			}
		}

		readonly DataFieldInfo masterInfo;

		public DataFieldInfo MasterInfo {
			get {
				return masterInfo;
			}
		}

		public DataFieldInfoRelation (DataFieldInfo masterInfo, DataFieldInfo relateInfo)
		{
			this.masterInfo = masterInfo;
			this.relateInfo = relateInfo;
		}
	}
}

