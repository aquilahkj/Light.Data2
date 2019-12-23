using System;
using System.Collections.Generic;

namespace Light.Data
{
    internal class AggregateGroupBy
    {
        private List<AggregateDataFieldInfo> groupList = new List<AggregateDataFieldInfo>();

        //public bool HasData {
        //    get {
        //        return groupList.Count > 0;
        //    }
        //}

        public int FieldCount => groupList.Count;

        public AggregateDataFieldInfo this[int index] => groupList[index];

        public void SetGroupField(AggregateDataFieldInfo field)
        {
            if (Equals(field, null))
                throw new ArgumentNullException(nameof(field));
            groupList.Add(field);
        }

        public string CreateGroupByString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var list = new string[groupList.Count];
            var index = 0;
            foreach (var fieldInfo in groupList) {
                list[index] = fieldInfo.CreateGroupBySqlString(factory, isFullName, state);
                index++;
            }
            var data = string.Join(",", list);
            return data;
        }
    }
}
