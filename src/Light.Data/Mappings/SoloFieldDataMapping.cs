using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    class SoloFieldDataMapping : DataMapping, IJoinTableMapping
    {
        #region static

        static object _synobj = new object();

        static Dictionary<Type, SoloFieldDataMapping> _defaultMapping = new Dictionary<Type, SoloFieldDataMapping>();

        public static SoloFieldDataMapping GetMapping(Type type)
        {
            Dictionary<Type, SoloFieldDataMapping> mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out SoloFieldDataMapping mapping)) {
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
            SoloFieldDataMapping mapping = new SoloFieldDataMapping(type);
            return mapping;
        }

        #endregion
        
        DataDefine dataDefine;

        string name = "F";

        public string Name {
            get {
                return name;
            }
        }

        SoloFieldDataMapping(Type type)
            : base(type)
        {
            dataDefine = DataDefine.GetDefine(type);
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            QueryState queryState = state as QueryState;
            object value = null;
            if (queryState == null) {
                value = dataDefine.LoadData(context, datareader, name, state);
            } else if (queryState.CheckSelectField(name)) {
                value = dataDefine.LoadData(context, datareader, name, state);
            }
            return value;
        }

        public object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            string fieldname = string.Format("{0}_{1}", aliasName, name);
            object value = null;
            if (queryState == null) {
                value = dataDefine.LoadData(context, datareader, name, queryState);
            } else if (queryState.CheckSelectField(name)) {
                value = dataDefine.LoadData(context, datareader, name, queryState);
            }
            return value;
        }

        public override object InitialData()
        {
            object item = Activator.CreateInstance(ObjectType);
            return item;
        }
    }
}

