using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    internal class DynamicDataMapping : CustomDataMapping
    {
        // protected readonly Dictionary<string, DynamicFieldMapping> _fieldMappingDictionary = new Dictionary<string, DynamicFieldMapping>();

        protected ReadOnlyCollection<DynamicFieldMapping> _fieldList;

        #region static

        private static readonly object locker = new object();

        private static readonly Dictionary<Type, DynamicDataMapping> _defaultMapping = new Dictionary<Type, DynamicDataMapping>();

        public static DynamicDataMapping GetMapping(Type type)
        {
            var mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out var mapping)) {
                lock (locker) {
                    if (!mappings.TryGetValue(type, out mapping)) {
                        mapping = CreateMapping(type);
                        mappings[type] = mapping;
                    }
                }
            }
            return mapping;
        }

        private static DynamicDataMapping CreateMapping(Type type)
        {
            var mapping = new DynamicDataMapping(type);
            return mapping;
        }

        #endregion


        public DynamicDataMapping(Type type)
            : base(type)
        {
            InitialDynamicFieldMapping();
        }

        private void InitialDynamicFieldMapping()
        {
            var properties = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var tempList = new List<DynamicFieldMapping>();
            foreach (var pi in properties) {
                var mapping = DynamicFieldMapping.CreateDynamicFieldMapping(pi, this);
                tempList.Add(mapping);
            }
            if (tempList.Count == 0) {
                throw new LightDataException(string.Format(SR.NoMappingField, ObjectType.FullName));
            }
            _fieldList = new ReadOnlyCollection<DynamicFieldMapping>(tempList);
        }

        public int FieldCount => _fieldList.Count;

        public ReadOnlyCollection<DynamicFieldMapping> DataEntityFields => _fieldList;

        public override object InitialData()
        {
            var args = new object[_fieldList.Count];
            var item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var args = new object[_fieldList.Count];
            var index = 0;
            foreach (var field in _fieldList) {
                var obj = dataReader[field.Name];
                var value = field.ToProperty(obj);
                args[index] = value;
                index++;
            }
            var item = Activator.CreateInstance(ObjectType, args);
            return item;
        }

        public override object LoadAliasJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState, string aliasName)
        {
            var args = new object[_fieldList.Count];
            var index = 0;
            var nodataSetNull = queryState?.CheckNoDataSetNull(aliasName) ?? false;
            var hasData = false;
            foreach (var field in _fieldList) {
                var name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState == null) {
                    var obj = dataReader[name];
                    var value = field.ToProperty(obj);
                    args[index] = value;
                } else if (queryState.CheckSelectField(name)) {
                    var obj = dataReader[name];
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

