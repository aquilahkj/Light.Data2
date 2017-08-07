using System;
using System.Collections.Generic;

namespace Light.Data
{
	class MassUpdator
	{
		Dictionary<DataFieldInfo, DataFieldInfo> dict = new Dictionary<DataFieldInfo, DataFieldInfo> ();

		readonly DataTableEntityMapping mapping;

		internal DataTableEntityMapping Mapping {
			get {
				return mapping;
			}
		}

		public MassUpdator (DataTableEntityMapping mapping)
		{
			if (mapping == null)
				throw new ArgumentNullException (nameof (mapping));
			this.mapping = mapping;
		}

		public void SetUpdateData (DataFieldInfo key, DataFieldInfo value)
		{
			dict [key] = value;
		}

		public string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string [] setList = new string[dict.Count];
			int index = 0;
			foreach (KeyValuePair<DataFieldInfo, DataFieldInfo> kvs in dict) {
				string left = kvs.Key.CreateSqlString (factory, isFullName, state);
				string right = kvs.Value.CreateSqlString (factory, isFullName, state);
				setList [index] = string.Format ("{0}={1}", left, right);
				index++;
			}
			string sql = string.Join (",", setList);
			return sql;
		}
	}
}

