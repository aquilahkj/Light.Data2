using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    internal class SoloFieldDataMapping : DataMapping, IJoinTableMapping
    {
        #region static

        private static readonly object locker = new object();

        private static readonly Dictionary<Type, SoloFieldDataMapping> _defaultMapping = new Dictionary<Type, SoloFieldDataMapping>();

        public static SoloFieldDataMapping GetMapping(Type type)
        {
            var mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out var mapping)) {
                lock (locker) {
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

        private readonly DataDefine dataDefine;

        public string Name { get; } = "F";

        private SoloFieldDataMapping(Type type)
            : base(type)
        {
            dataDefine = DataDefine.GetDefine(type);
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            // var queryState = state as QueryState;
            // object value = null;
            // if (queryState == null) {
            //     value = dataDefine.LoadData(context, dataReader, Name, state);
            // } else if (queryState.CheckSelectField(Name)) {
            //     value = dataDefine.LoadData(context, dataReader, Name, state);
            // }
            // return value;
            return dataDefine.LoadData(context, dataReader, state);
        }

        public object LoadAliasJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState, string aliasName)
        {
            // object value = null;
            // if (queryState == null) {
            //     value = dataDefine.LoadData(context, dataReader, Name, null);
            // } else if (queryState.CheckSelectField(Name)) {
            //     value = dataDefine.LoadData(context, dataReader, Name, queryState);
            // }
            // return value;
            return dataDefine.LoadData(context, dataReader, queryState);
        }

        public override object InitialData()
        {
            var item = Activator.CreateInstance(ObjectType);
            return item;
        }
    }
}

