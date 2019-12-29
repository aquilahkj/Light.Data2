using System.Collections.Generic;

namespace Light.Data
{
    internal class QueryState
    {
        private RelationMap relationMap;

        private readonly Dictionary<string, object> joinDatas = new Dictionary<string, object>();

        private readonly Dictionary<string, object> extendDatas = new Dictionary<string, object>();

        private HashSet<string> fieldHash;

        private HashSet<string> nodataSetNullHash;

        public void InitialJoinData()
        {
            joinDatas.Clear();
            if (extendDatas.Count > 0) {
                foreach (var kvs in extendDatas) {
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
                fieldHash = new HashSet<string>(selector.GetSelectFieldNames());
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
            if (relationMap.TryGetCycleFieldPath(fieldPath, out var m)) {
                return joinDatas.TryGetValue(m, out value);
            }

            return joinDatas.TryGetValue(fieldPath, out value);
        }

        public string GetAliasName(string fieldPath)
        {
            if (relationMap.CheckValid(fieldPath, out var alias)) {
                return alias;
            }

            throw new LightDataException(string.Format(SR.CanNotFindAliasNameViaSpecifiedPath, fieldPath));
        }

        public bool CheckSelectField(string fieldName)
        {
            if (fieldHash != null) {
                return fieldHash.Contains(fieldName);
            }

            return true;
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
            }

            return false;
        }
    }
}

