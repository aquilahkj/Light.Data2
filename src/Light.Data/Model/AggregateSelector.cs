using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class AggregateSelector : ISelector
    {
	    private List<AggregateDataFieldInfo> selectList = new List<AggregateDataFieldInfo>();

		public virtual void SetSelectField(AggregateDataFieldInfo field)
		{
			if (Equals(field, null))
				throw new ArgumentNullException(nameof(field));
			selectList.Add(field);
		}

       
        public string CreateSelectString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
			var list = new string[selectList.Count];
			var index = 0;
			foreach (var fieldInfo in selectList) {
                state.SetAliasData(fieldInfo.FieldInfo, factory.CreateDataFieldSql(fieldInfo.AggregateName));
				list[index] = fieldInfo.CreateAliasDataFieldSql(factory, false, state);
				index++;
			}
            var data = string.Join(",", list);
			return data;
        }

        public string[] GetSelectFieldNames()
        {
			var list = new List<string>();
			foreach (var fieldInfo in selectList) {
				var name = fieldInfo.FieldName;
				if (name != null) {
					list.Add(name);
				}
			}
			return list.ToArray();
        }
    }
}