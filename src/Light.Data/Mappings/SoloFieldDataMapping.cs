using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    internal class SoloFieldDataMapping : DataMapping, IJoinTableMapping
    {
        #region static

        private static object _synobj = new object();

        private static Dictionary<Type, SoloFieldDataMapping> _defaultMapping = new Dictionary<Type, SoloFieldDataMapping>();

        public static SoloFieldDataMapping GetMapping(Type type)
        {
            var mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out var mapping)) {
                lock (_synobj) {
                    if (!mappings.ContainsKey(type)) {
                        mapping = CreateMapping(type);
                        mappings[type] = mapping;
                    }
                }
            }
            return mapping;
        }

        private static SoloFieldDataMapping CreateMapping(Type type)
        {
            var mapping = new SoloFieldDataMapping(type);
            return mapping;
        }

        #endregion

        private DataDefine dataDefine;

        public string Name { get; } = "F";

        private SoloFieldDataMapping(Type type)
            : base(type)
        {
            dataDefine = DataDefine.GetDefine(type);
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var queryState = state as QueryState;
            object value = null;
            if (queryState == null) {
                value = dataDefine.LoadData(context, datareader, Name, state);
            } else if (queryState.CheckSelectField(Name)) {
                value = dataDefine.LoadData(context, datareader, Name, state);
            }
            return value;
        }

        public object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            var fieldname = string.Format("{0}_{1}", aliasName, Name);
            object value = null;
            if (queryState == null) {
                value = dataDefine.LoadData(context, datareader, Name, queryState);
            } else if (queryState.CheckSelectField(Name)) {
                value = dataDefine.LoadData(context, datareader, Name, queryState);
            }
            return value;
        }

        public override object InitialData()
        {
            var item = Activator.CreateInstance(ObjectType);
            return item;
        }
    }
}

