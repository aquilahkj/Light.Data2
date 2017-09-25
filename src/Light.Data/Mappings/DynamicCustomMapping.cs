using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    class DynamicCustomMapping : CustomMapping
    {
        protected Dictionary<string, DynamicFieldMapping> _fieldMappingDictionary = new Dictionary<string, DynamicFieldMapping>();

        protected ReadOnlyCollection<DynamicFieldMapping> _fieldList;

        #region static

        static object _synobj = new object();

        static Dictionary<Type, DynamicCustomMapping> _defaultMapping = new Dictionary<Type, DynamicCustomMapping>();

        public static DynamicCustomMapping GetMapping(Type type)
        {
            Dictionary<Type, DynamicCustomMapping> mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out DynamicCustomMapping mapping)) {
                lock (_synobj) {
                    if (!mappings.ContainsKey(type)) {
                        mapping = CreateMapping(type);
                        mappings[type] = mapping;
                    }
                }
            }
            return mapping;
        }

        private static DynamicCustomMapping CreateMapping(Type type)
        {
            DynamicCustomMapping mapping = new DynamicCustomMapping(type);
            return mapping;
        }

        #endregion


        public DynamicCustomMapping(Type type)
            : base(type)
        {
            InitialDynamicFieldMapping();
        }

        private void InitialDynamicFieldMapping()
        {
            PropertyInfo[] propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<DynamicFieldMapping> tmepList = new List<DynamicFieldMapping>();
            foreach (PropertyInfo pi in propertys) {
                DynamicFieldMapping mapping = DynamicFieldMapping.CreateDynmaicFieldMapping(pi, this);
                _fieldMappingDictionary.Add(mapping.IndexName, mapping);
                tmepList.Add(mapping);
            }
            if (tmepList.Count == 0) {
                throw new LightDataException(string.Format(SR.NoMappingField, ObjectType.FullName));
            }
            _fieldList = new ReadOnlyCollection<DynamicFieldMapping>(tmepList);
        }

        public int FieldCount {
            get {
                return this._fieldList.Count;
            }
        }

        public ReadOnlyCollection<DynamicFieldMapping> DataEntityFields {
            get {
                return _fieldList;
            }
        }

        public override object InitialData()
        {
            object[] args = new object[this._fieldList.Count];
            object item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            object[] args = new object[this._fieldList.Count];
            int index = 0;
            foreach (DynamicFieldMapping field in this._fieldList) {
                object obj = datareader[field.Name];
                object value = field.ToProperty(obj);
                args[index] = value;
                index++;
            }
            object item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            object[] args = new object[this._fieldList.Count];
            int index = 0;
            bool nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            bool hasData = false;
            foreach (DynamicFieldMapping field in this._fieldList) {
                string name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState == null) {
                    object obj = datareader[name];
                    object value = field.ToProperty(obj);
                    args[index] = value;
                } else if (queryState.CheckSelectField(name)) {
                    object obj = datareader[name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        hasData = true;
                    }
                    args[index] = value;
                }
                index++;
            }
            if (!hasData && nodataSetNull) {
                return null;
            }
            object item = Activator.CreateInstance(ObjectType, args);
            return item;
        }
    }
}

