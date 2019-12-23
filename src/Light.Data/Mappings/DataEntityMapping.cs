using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;

namespace Light.Data
{
    /// <summary>
    /// Data entity mapping.
    /// </summary>
    internal class DataEntityMapping : DataMapping, IJoinTableMapping
    {
        private static Type LCollectionFrameType;

        static DataEntityMapping()
        {
            var Ltype = typeof(LCollection<object>);
            LCollectionFrameType = Ltype.GetGenericTypeDefinition();
        }

        public static DataEntityMapping Default { get; } = new DataEntityMapping(typeof(DataEntityMapping));

        #region static

        private static readonly object _synobj = new object();

        private static Dictionary<Type, DataEntityMapping> _defaultMapping = new Dictionary<Type, DataEntityMapping>();

        /// <summary>
        /// Gets the table mapping.
        /// </summary>
        /// <returns>The table mapping.</returns>
        /// <param name="type">Type.</param>
        public static DataTableEntityMapping GetTableMapping(Type type)
        {
            var dataMapping = GetEntityMapping(type) as DataTableEntityMapping;
            if (dataMapping == null) {
                throw new LightDataException(SR.IsNotDataTableMapping);
            } else {
                return dataMapping;
            }
        }

        /// <summary>
        /// Gets the entity mapping.
        /// </summary>
        /// <returns>The entity mapping.</returns>
        /// <param name="type">Type.</param>
        public static DataEntityMapping GetEntityMapping(Type type)
        {
            //Dictionary<Type, DataEntityMapping> mappings = _defaultMapping;
            _defaultMapping.TryGetValue(type, out var mapping);
            if (mapping == null) {
                lock (_synobj) {
                    _defaultMapping.TryGetValue(type, out mapping);
                    if (mapping == null) {
                        mapping = CreateMapping(type);
                        _defaultMapping[type] = mapping;
                    }
                }
            }
            return mapping;
        }

        public static void ResetEntityMapping()
        {
            lock (_synobj) {
                _defaultMapping.Clear();
            }
        }

        /// <summary>
        /// Checks the entity mapping.must execute GetMapping method first
        /// </summary>
        /// <returns>The entity mapping.</returns>
        /// <param name="type">Type.</param>
        public static bool CheckMapping(Type type)
        {
            return _defaultMapping.ContainsKey(type);
        }

        /// <summary>
        /// Creates the mapping.
        /// </summary>
        /// <returns>The mapping.</returns>
        /// <param name="type">Type.</param>
        private static DataEntityMapping CreateMapping(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            string tableName;
            bool isEntityTable;
            DataEntityMapping dataMapping;

            var config = MapperConfigManager.LoadDataTableConfig(type);
            if (config != null) {
                tableName = config.TableName;
                isEntityTable = config.IsEntityTable;
            } else {
                throw new LightDataException(string.Format(SR.NoDataEntityConfig, type.Name));
            }

            if (string.IsNullOrEmpty(tableName)) {
                tableName = type.Name;
            }

            if (typeInfo.IsSubclassOf(typeof(DataTableEntity))) {
                dataMapping = new DataTableEntityMapping(type, tableName, true, true);
            } else if (typeInfo.IsSubclassOf(typeof(DataEntity))) {
                dataMapping = new DataEntityMapping(type, tableName, true);
            } else {
                if (!isEntityTable) {
                    dataMapping = new DataEntityMapping(type, tableName, false);
                } else {
                    dataMapping = new DataTableEntityMapping(type, tableName, false, false);
                }
            }
            dataMapping.ExtentParams = config.ConfigParams;
            return dataMapping;
        }

        #endregion

        protected Dictionary<string, DataFieldMapping> _fieldMappingDictionary = new Dictionary<string, DataFieldMapping>();

        protected ReadOnlyCollection<DataFieldMapping> _fieldList;

        protected ReadOnlyCollection<CollectionRelationFieldMapping> _collectionRelationFields;

        protected ReadOnlyCollection<SingleRelationFieldMapping> _singleRelationFields;

        private DataEntityMapping(Type type)
            : base(type)
        {

        }

        internal DataEntityMapping(Type type, string tableName, bool isDataEntity)
            : base(type)
        {
            if (string.IsNullOrEmpty(tableName)) {
                TableName = type.Name;
            } else {
                TableName = tableName;
            }
            IsDataEntity = isDataEntity;
            InitialDataFieldMapping();
            InitialRelationField();
            //InitialExtendParams();
        }

        internal SingleRelationFieldMapping[] GetSingleRelationFieldMappings()
        {
            var len = _singleRelationFields.Count;
            var array = new SingleRelationFieldMapping[len];
            var index = 0;
            foreach (var item in _singleRelationFields) {
                array[index] = item;
                index++;
            }
            return array;
        }

        internal ReadOnlyCollection<SingleRelationFieldMapping> SingleJoinTableRelationFieldMappings => _singleRelationFields;

        internal ReadOnlyCollection<CollectionRelationFieldMapping> CollectionRelationFieldMappings => _collectionRelationFields;

        private void InitialRelationField()
        {
            var propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var collectionTmpList = new List<CollectionRelationFieldMapping>();
            var singleTmpList = new List<SingleRelationFieldMapping>();

            foreach (var pi in propertys) {
                var config = MapperConfigManager.LoadRelationDataFieldConfig(ObjectType, pi);
                if (config != null && config.RelationKeyCount > 0) {
                    var type = pi.PropertyType;
                    var typeInfo = type.GetTypeInfo();
                    if (typeInfo.IsGenericType) {
                        var frameType = type.GetGenericTypeDefinition();
                        if (frameType == LCollectionFrameType || frameType.FullName == "System.Collections.Generic.ICollection`1") {
                            var arguments = typeInfo.GetGenericArguments();
                            type = arguments[0];
                            var handler = new PropertyHandler(pi);
                            var keypairs = config.GetRelationKeys();
                            var rmapping = new CollectionRelationFieldMapping(pi.Name, this, type, keypairs, handler);
                            collectionTmpList.Add(rmapping);
                        }
                    } else {
                        var handler = new PropertyHandler(pi);
                        var keypairs = config.GetRelationKeys();
                        var rmapping = new SingleRelationFieldMapping(pi.Name, this, type, keypairs, handler);
                        singleTmpList.Add(rmapping);
                    }
                }
            }
            _collectionRelationFields = new ReadOnlyCollection<CollectionRelationFieldMapping>(collectionTmpList);
            _singleRelationFields = new ReadOnlyCollection<SingleRelationFieldMapping>(singleTmpList);
        }

        public bool IsDataEntity { get; }

        public string TableName { get; }

        public bool Equals(DataEntityMapping mapping)
        {
            if (mapping == null)
                return false;
            return ObjectType.Equals(mapping.ObjectType);
        }

        protected void InitialDataFieldMapping()
        {
            var propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var index = 0;
            var list = new List<FieldInfo>();
            foreach (var pi in propertys) {
                var config = MapperConfigManager.LoadDataFieldConfig(ObjectType, pi);
                if (config != null) {
                    index++;
                    var info = new FieldInfo(pi, config, index);
                    list.Add(info);
                }
            }
            if (list.Count == 0) {
                throw new LightDataException(string.Format(SR.NoMappingField, ObjectType));
            }
            list.Sort((x, y) => {
                if (x.DataOrder.HasValue && y.DataOrder.HasValue) {
                    if (x.DataOrder > y.DataOrder) {
                        return 1;
                    } else if (x.DataOrder < y.DataOrder) {
                        return -1;
                    } else {
                        return x.FieldOrder > y.FieldOrder ? 1 : -1;
                    }
                } else if (x.DataOrder.HasValue && !y.DataOrder.HasValue) {
                    return -1;
                } else if (!x.DataOrder.HasValue && y.DataOrder.HasValue) {
                    return 1;
                } else {
                    return x.FieldOrder > y.FieldOrder ? 1 : -1;
                }
            });
            var tmplist = new List<DataFieldMapping>();
            for (var i = 0; i < list.Count; i++) {
                var info = list[i];
                var mapping = DataFieldMapping.CreateDataFieldMapping(info.Property, info.Config, i + 1, this);
                _fieldMappingDictionary.Add(mapping.IndexName, mapping);
                if (mapping.Name != mapping.IndexName) {
                    _fieldMappingDictionary.Add(mapping.Name, mapping);
                }
                tmplist.Add(mapping);
            }
            _fieldList = new ReadOnlyCollection<DataFieldMapping>(tmplist);
        }

        public ReadOnlyCollection<DataFieldMapping> DataEntityFields => _fieldList;

        private readonly object joinLock = new object();

        private RelationMap relationMap;

        public RelationMap GetRelationMap()
        {
            if (relationMap == null) {
                lock (joinLock) {
                    if (relationMap == null) {
                        relationMap = new RelationMap(this);
                    }
                }
            }
            return relationMap;
        }


        public DataFieldMapping FindDataEntityField(string fieldName)
        {
            _fieldMappingDictionary.TryGetValue(fieldName, out var mapping);
            return mapping;
        }

        public int FieldCount => _fieldList.Count;

        public bool HasJoinRelateModel => _singleRelationFields.Count > 0;

        //public virtual void LoadJoinTableData(DataContext context, IDataReader datareader, object item, QueryState queryState, string fieldPath)
        //{
        //    string aliasName = queryState.GetAliasName(fieldPath);
        //    foreach (DataFieldMapping field in this._fieldList) {
        //        string name = string.Format("{0}_{1}", aliasName, field.Name);
        //        if (queryState.CheckSelectField(name)) {
        //            object obj = datareader[name];
        //            object value = field.ToProperty(obj);
        //            if (!Object.Equals(value, null)) {
        //                field.Handler.Set(item, value);
        //            }
        //        }
        //    }
        //    if (_collectionRelationFields.Count > 0) {
        //        foreach (CollectionRelationFieldMapping mapping in _collectionRelationFields) {
        //            mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
        //        }
        //    }

        //    foreach (SingleRelationFieldMapping mapping in _singleRelationFields) {
        //        string fpath = string.Format("{0}.{1}", fieldPath, mapping.FieldName);
        //        object value = mapping.ToProperty(context, datareader, queryState, fpath);
        //        if (!Object.Equals(value, null)) {
        //            mapping.Handler.Set(item, value);
        //        }
        //    }
        //    if (item is DataEntity entity) {
        //        entity.SetContext(context);
        //        entity.LoadDataComplete();
        //    }
        //}

        public virtual object LoadJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string fieldPath)
        {
            var item = Activator.CreateInstance(ObjectType);
            queryState.SetJoinData(fieldPath, item);
            var aliasName = queryState.GetAliasName(fieldPath);
            foreach (var field in _fieldList) {
                var name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState.CheckSelectField(name)) {
                    var obj = datareader[name];
                    var value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (var mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }

            foreach (var mapping in _singleRelationFields) {
                var fpath = string.Format("{0}.{1}", fieldPath, mapping.FieldName);
                var value = mapping.ToProperty(context, datareader, queryState, fpath);
                if (!Object.Equals(value, null)) {
                    mapping.Handler.Set(item, value);
                }
            }
            if(IsDataEntity) {
                var entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var queryState = state as QueryState;
            if (_singleRelationFields.Count > 0) {
                queryState.InitialJoinData();
                return LoadJoinTableData(context, datareader, queryState, string.Empty);
            }
            var item = Activator.CreateInstance(ObjectType);
            foreach (var field in _fieldList) {
                if (queryState == null) {
                    var obj = datareader[field.Name];
                    var value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                } else if (queryState.CheckSelectField(field.Name)) {
                    var obj = datareader[field.Name];
                    var value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (var mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }
            if (IsDataEntity) {
                var entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public virtual object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            var item = Activator.CreateInstance(ObjectType);
            var nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            var hasData = false;
            foreach (var field in _fieldList) {
                if (field == null)
                    continue;
                var name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState == null) {
                    var obj = datareader[name];
                    var value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                } else if (queryState.CheckSelectField(name)) {
                    var obj = datareader[name];
                    var value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        hasData = true;
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (var mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, false));
                }
            }
            if (!hasData && nodataSetNull) {
                return null;
            }
            if (IsDataEntity) {
                var entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public override object InitialData()
        {
            var item = Activator.CreateInstance(ObjectType);
            return item;
        }

        #region alise

        #endregion

        private class FieldInfo
        {
            public FieldInfo(PropertyInfo property, DataFieldMapperConfig config, int fieldOrder)
            {
                this.Property = property;
                this.Config = config;
                this.FieldOrder = fieldOrder;
                if (config.DataOrder > 0) {
                    DataOrder = config.DataOrder;
                }
            }

            public PropertyInfo Property { get; }

            public DataFieldMapperConfig Config { get; }

            public int? DataOrder { get; }

            public int FieldOrder { get; }
        }
    }
}
