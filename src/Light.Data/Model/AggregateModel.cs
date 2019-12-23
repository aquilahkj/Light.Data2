using System.Collections.Generic;

namespace Light.Data
{
    internal class AggregateModel
    {
        public AggregateModel(DataEntityMapping entityMapping, CustomMapping aggregateMapping)
        {
            EntityMapping = entityMapping;
            OutputMapping = aggregateMapping;
        }

        private bool _hasGroupBy;

        public bool OnlyAggregate { get; set; }

        public CustomMapping OutputMapping { get; }

        public DataEntityMapping EntityMapping { get; }

        private readonly Dictionary<string, AggregateDataFieldInfo> _aggregateDict = new Dictionary<string, AggregateDataFieldInfo>();

        public void AddGroupByField(string name, DataFieldInfo fieldInfo)
        {
            _hasGroupBy = true;
            var agg = new AggregateDataFieldInfo(fieldInfo, name, false);
            _aggregateDict.Add(name, agg);
        }

        public void AddAggregateField(string name, DataFieldInfo fieldInfo)
        {
            var agg = new AggregateDataFieldInfo(fieldInfo, name, true);
            _aggregateDict.Add(name, agg);
        }

        public DataFieldInfo GetAggregateData(string name)
        {
            if (_aggregateDict.TryGetValue(name, out var info)) {
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
            var selecor = new AggregateSelector();
            foreach (var item in _aggregateDict.Values) {
                selecor.SetSelectField(item);
            }
            return selecor;
        }

        public AggregateGroupBy GetGroupBy()
        {
            if (!_hasGroupBy || OnlyAggregate) {
                return null;
            }
            var groupBy = new AggregateGroupBy();
            foreach (var item in _aggregateDict.Values) {
                if (!item.Aggregate) {
                    groupBy.SetGroupField(item);
                }
            }
            return groupBy;
        }

        public AggregateDataFieldInfo[] GetAggregateDataFieldInfos()
        {
            var array = new AggregateDataFieldInfo[_aggregateDict.Count];
            _aggregateDict.Values.CopyTo(array, 0);
            return array;
        }
    }
}

