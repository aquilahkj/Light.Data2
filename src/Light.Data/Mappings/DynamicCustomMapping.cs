using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    internal class DynamicCustomMapping : CustomMapping
    {
        protected Dictionary<string, DynamicFieldMapping> _fieldMappingDictionary = new Dictionary<string, DynamicFieldMapping>();

        protected ReadOnlyCollection<DynamicFieldMapping> _fieldList;

        #region static

        private static object _synobj = new object();

        private static Dictionary<Type, DynamicCustomMapping> _defaultMapping = new Dictionary<Type, DynamicCustomMapping>();

        public static DynamicCustomMapping GetMapping(Type type)
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

        private static DynamicCustomMapping CreateMapping(Type type)
        {
            var mapping = new DynamicCustomMapping(type);
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
            var propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var tmepList = new List<DynamicFieldMapping>();
            foreach (var pi in propertys) {
                var mapping = DynamicFieldMapping.CreateDynmaicFieldMapping(pi, this);
                _fieldMappingDictionary.Add(mapping.IndexName, mapping);
                tmepList.Add(mapping);
            }
            if (tmepList.Count == 0) {
                throw new LightDataException(string.Format(SR.NoMappingField, ObjectType.FullName));
            }
            _fieldList = new ReadOnlyCollection<DynamicFieldMapping>(tmepList);
        }

        public int FieldCount => _fieldList.Count;

        public ReadOnlyCollection<DynamicFieldMapping> DataEntityFields => _fieldList;

        public override object InitialData()
        {
            var args = new object[_fieldList.Count];
            var item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var args = new object[_fieldList.Count];
            var index = 0;
            foreach (var field in _fieldList) {
                var obj = datareader[field.Name];
                var value = field.ToProperty(obj);
                args[index] = value;
                index++;
            }
            var item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            var args = new object[_fieldList.Count];
            var index = 0;
            var nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            var hasData = false;
            foreach (var field in _fieldList) {
                var name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState == null) {
                    var obj = datareader[name];
                    var value = field.ToProperty(obj);
                    args[index] = value;
                } else if (queryState.CheckSelectField(name)) {
                    var obj = datareader[name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null)) {
                        hasData = true;
                    }
                    args[index] = value;
                }
                index++;
            }
            if (!hasData && nodataSetNull) {
                return null;
            }
            var item = Activator.CreateInstance(ObjectType, args);
            return item;
        }
    }
}

