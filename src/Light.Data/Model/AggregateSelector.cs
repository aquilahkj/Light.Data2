using System;
using System.Collections.Generic;

namespace Light.Data
{
    class AggregateSelector : ISelector
    {
        List<AggregateDataFieldInfo> selectList = new List<AggregateDataFieldInfo>();

		public virtual void SetSelectField(AggregateDataFieldInfo field)
		{
			if (Object.Equals(field, null))
				throw new ArgumentNullException(nameof(field));
			selectList.Add(field);
		}

       
        public string CreateSelectString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
			string[] list = new string[this.selectList.Count];
			int index = 0;
			foreach (AggregateDataFieldInfo fieldInfo in this.selectList) {
                state.SetAliasData(fieldInfo.FieldInfo, factory.CreateDataFieldSql(fieldInfo.AggregateName));
				list[index] = fieldInfo.CreateAliasDataFieldSql(factory, false, state);
				index++;
			}
            string data = string.Join(",", list);
			return data;
        }

        public string[] GetSelectFieldNames()
        {
			List<string> list = new List<string>();
			foreach (AggregateDataFieldInfo fieldInfo in this.selectList) {
				string name = fieldInfo.FieldName;
				if (name != null) {
					list.Add(name);
				}
			}
			return list.ToArray();
        }
    }
}