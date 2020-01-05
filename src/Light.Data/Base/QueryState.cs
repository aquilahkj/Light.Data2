using System.Collections.Generic;

namespace Light.Data
{
    internal class QueryState
    {
        private RelationMap _relationMap;

        private readonly Dictionary<string, object> _joinDatas = new Dictionary<string, object>();

        private readonly Dictionary<string, object> _extendDatas = new Dictionary<string, object>();

        private HashSet<string> _fieldHash;

        private HashSet<string> _nodataSetNullHash;

        public void InitialJoinData()
        {
            _joinDatas.Clear();
            if (_extendDatas.Count > 0) {
                foreach (var kvs in _extendDatas) {
                    _joinDatas.Add(kvs.Key, kvs.Value);
                }
            }
        }

        public void SetRelationMap(RelationMap relationMap)
        {
            _relationMap = relationMap;
        }


        public void SetSelector(ISelector selector)
        {
            if (selector != null) {
                _fieldHash = new HashSet<string>(selector.GetSelectFieldNames());
            }
        }

        public void SetExtendData(string fieldPath, object value)
        {
            _extendDatas[fieldPath] = value;
        }

        public void SetJoinData(string fieldPath, object value)
        {
            _joinDatas[fieldPath] = value;
        }

        public bool GetJoinData(string fieldPath, out object value)
        {
            if (_relationMap.TryGetCycleFieldPath(fieldPath, out var m)) {
                return _joinDatas.TryGetValue(m, out value);
            }

            return _joinDatas.TryGetValue(fieldPath, out value);
        }

        public string GetAliasName(string fieldPath)
        {
            if (_relationMap.CheckValid(fieldPath, out var alias)) {
                return alias;
            }

            throw new LightDataException(string.Format(SR.CanNotFindAliasNameViaSpecifiedPath, fieldPath));
        }

        public bool CheckSelectField(string fieldName)
        {
            if (_fieldHash != null) {
                return _fieldHash.Contains(fieldName);
            }

            return true;
        }

        public void SetNoDataSetNull(string aliasName)
        {
            if (_nodataSetNullHash == null) {
                _nodataSetNullHash = new HashSet<string>();
            }
            _nodataSetNullHash.Add(aliasName);
        }

        public bool CheckNoDataSetNull(string aliasName)
        {
            if (_nodataSetNullHash != null) {
                return _nodataSetNullHash.Contains(aliasName);
            }

            return false;
        }
    }
}

