using System;
using System.Collections.Generic;

namespace Light.Data
{
    class AggregateModel
    {
        public AggregateModel(DataEntityMapping entityMapping, CustomMapping aggregateMapping)
        {
            _entityMapping = entityMapping;
            _outputMapping = aggregateMapping;
        }

        DataEntityMapping _entityMapping;

        CustomMapping _outputMapping;

        bool _onlyAggregate;

        bool _hasGroupBy;

        public bool OnlyAggregate {
            get {
                return _onlyAggregate;
            }

            set {
                _onlyAggregate = value;
            }
        }

        public CustomMapping OutputMapping {
            get {
                return _outputMapping;
            }
        }

        public DataEntityMapping EntityMapping {
            get {
                return _entityMapping;
            }
        }

        readonly Dictionary<string, AggregateDataFieldInfo> _aggregateDict = new Dictionary<string, AggregateDataFieldInfo>();

        public void AddGroupByField(string name, DataFieldInfo fieldInfo)
        {
            _hasGroupBy = true;
            AggregateDataFieldInfo agg = new AggregateDataFieldInfo(fieldInfo, name, false);
            _aggregateDict.Add(name, agg);
        }

        public void AddAggregateField(string name, DataFieldInfo fieldInfo)
        {
            AggregateDataFieldInfo agg = new AggregateDataFieldInfo(fieldInfo, name, true);
            _aggregateDict.Add(name, agg);
        }

        public DataFieldInfo GetAggregateData(string name)
        {
            if (_aggregateDict.TryGetValue(name, out AggregateDataFieldInfo info)) {
                return info;
            }
            else {
                return null;
            }
        }

        public bool CheckName(string name)
        {
            return _aggregateDict.ContainsKey(name);
        }

        public AggregateSelector GetSelector()
        {
            AggregateSelector selecor = new AggregateSelector();
            foreach (AggregateDataFieldInfo item in _aggregateDict.Values) {
                selecor.SetSelectField(item);
            }
            return selecor;
        }

        public AggregateGroupBy GetGroupBy()
        {
            if (!_hasGroupBy || _onlyAggregate) {
                return null;
            }
            AggregateGroupBy groupBy = new AggregateGroupBy();
            foreach (AggregateDataFieldInfo item in _aggregateDict.Values) {
                if (!item.Aggregate) {
                    groupBy.SetGroupField(item);
                }
            }
            return groupBy;
        }

        public AggregateDataFieldInfo[] GetAggregateDataFieldInfos()
        {
            AggregateDataFieldInfo[] array = new AggregateDataFieldInfo[_aggregateDict.Count];
            _aggregateDict.Values.CopyTo(array, 0);
            return array;
        }
    }
}

