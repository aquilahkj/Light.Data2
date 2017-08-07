using System.Collections.Generic;

namespace Light.Data
{
    class QueryState
    {
        RelationMap relationMap;

        readonly Dictionary<string, object> joinDatas = new Dictionary<string, object>();

        readonly Dictionary<string, object> extendDatas = new Dictionary<string, object>();

        HashSet<string> fieldHash;

        HashSet<string> nodataSetNullHash;

        public void InitialJoinData()
        {
            this.joinDatas.Clear();
            if (this.extendDatas.Count > 0) {
                foreach (KeyValuePair<string, object> kvs in this.extendDatas) {
                    joinDatas.Add(kvs.Key, kvs.Value);
                }
            }
        }

        public void SetRelationMap(RelationMap relationMap)
        {
            this.relationMap = relationMap;
        }


        public void SetSelector(ISelector selector)
        {
            if (selector != null) {
                this.fieldHash = new HashSet<string>(selector.GetSelectFieldNames());
            }
        }

        public void SetExtendData(string fieldPath, object value)
        {
            extendDatas[fieldPath] = value;
        }

        public void SetJoinData(string fieldPath, object value)
        {
            joinDatas[fieldPath] = value;
        }

        public bool GetJoinData(string fieldPath, out object value)
        {
            if (relationMap.TryGetCycleFieldPath(fieldPath, out string m)) {
                return joinDatas.TryGetValue(m, out value);
            } else {
                return joinDatas.TryGetValue(fieldPath, out value);
            }
        }

        public string GetAliasName(string fieldPath)
        {
            if (this.relationMap.CheckValid(fieldPath, out string alias)) {
                return alias;
            } else {
                throw new LightDataException(string.Format(SR.CanNotFindAliasNameViaSpecifiedPath, fieldPath));
            }
        }

        public bool CheckSelectField(string fieldName)
        {
            if (fieldHash != null) {
                return fieldHash.Contains(fieldName);
            } else {
                return true;
            }
        }

        public void SetNoDataSetNull(string aliasName)
        {
            if (nodataSetNullHash == null) {
                nodataSetNullHash = new HashSet<string>();
            }
            nodataSetNullHash.Add(aliasName);
        }

        public bool CheckNoDataSetNull(string aliasName)
        {
            if (nodataSetNullHash != null) {
                return nodataSetNullHash.Contains(aliasName);
            } else {
                return false;
            }
        }
    }
}

