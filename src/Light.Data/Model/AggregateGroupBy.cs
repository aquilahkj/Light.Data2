using System;
using System.Collections.Generic;

namespace Light.Data
{
    class AggregateGroupBy
    {
        List<AggregateDataFieldInfo> groupList = new List<AggregateDataFieldInfo>();

        //public bool HasData {
        //    get {
        //        return groupList.Count > 0;
        //    }
        //}

        public int FieldCount {
            get {
                return groupList.Count;
            }
        }

        public AggregateDataFieldInfo this[int index] {
            get {
                return groupList[index];
            }
        }

        public void SetGroupField(AggregateDataFieldInfo field)
        {
            if (Object.Equals(field, null))
                throw new ArgumentNullException(nameof(field));
            groupList.Add(field);
        }

        public string CreateGroupByString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string[] list = new string[this.groupList.Count];
            int index = 0;
            foreach (AggregateDataFieldInfo fieldInfo in this.groupList) {
                list[index] = fieldInfo.CreateGroupBySqlString(factory, isFullName, state);
                index++;
            }
            string data = string.Join(",", list);
            return data;
        }
    }
}
