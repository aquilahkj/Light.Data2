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
    class DataEntityMapping : DataMapping, IJoinTableMapping
    {
        static Type LCollectionFrameType;

        static DataEntityMapping()
        {
            Type Ltype = typeof(LCollection<object>);
            LCollectionFrameType = Ltype.GetGenericTypeDefinition();
        }

        static DataEntityMapping defaultMapping = new DataEntityMapping(typeof(DataEntityMapping));

        public static DataEntityMapping Default {
            get {
                return defaultMapping;
            }
        }

        #region static

        static object _synobj = new object();

        static Dictionary<Type, DataEntityMapping> _defaultMapping = new Dictionary<Type, DataEntityMapping>();

        /// <summary>
        /// Gets the table mapping.
        /// </summary>
        /// <returns>The table mapping.</returns>
        /// <param name="type">Type.</param>
        public static DataTableEntityMapping GetTableMapping(Type type)
        {
            DataTableEntityMapping dataMapping = GetEntityMapping(type) as DataTableEntityMapping;
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
            _defaultMapping.TryGetValue(type, out DataEntityMapping mapping);
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
            TypeInfo typeInfo = type.GetTypeInfo();
            string tableName;
            bool isEntityTable;
            DataEntityMapping dataMapping;

            DataTableMapperConfig config = MapperConfigManager.LoadDataTableConfig(type);
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
                _tableName = type.Name;
            } else {
                _tableName = tableName;
            }
            _isDataEntity = isDataEntity;
            InitialDataFieldMapping();
            InitialRelationField();
            //InitialExtendParams();
        }

        internal SingleRelationFieldMapping[] GetSingleRelationFieldMappings()
        {
            int len = this._singleRelationFields.Count;
            SingleRelationFieldMapping[] array = new SingleRelationFieldMapping[len];
            int index = 0;
            foreach (SingleRelationFieldMapping item in this._singleRelationFields) {
                array[index] = item;
                index++;
            }
            return array;
        }

        internal ReadOnlyCollection<SingleRelationFieldMapping> SingleJoinTableRelationFieldMappings {
            get {
                return this._singleRelationFields;
            }
        }

        internal ReadOnlyCollection<CollectionRelationFieldMapping> CollectionRelationFieldMappings {
            get {
                return this._collectionRelationFields;
            }
        }

        private void InitialRelationField()
        {
            PropertyInfo[] propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            List<CollectionRelationFieldMapping> collectionTmpList = new List<CollectionRelationFieldMapping>();
            List<SingleRelationFieldMapping> singleTmpList = new List<SingleRelationFieldMapping>();

            foreach (PropertyInfo pi in propertys) {
                RelationFieldMapConfig config = MapperConfigManager.LoadRelationDataFieldConfig(ObjectType, pi);
                if (config != null && config.RelationKeyCount > 0) {
                    Type type = pi.PropertyType;
                    TypeInfo typeInfo = type.GetTypeInfo();
                    if (typeInfo.IsGenericType) {
                        Type frameType = type.GetGenericTypeDefinition();
                        if (frameType == LCollectionFrameType || frameType.FullName == "System.Collections.Generic.ICollection`1") {
                            Type[] arguments = typeInfo.GetGenericArguments();
                            type = arguments[0];
                            PropertyHandler handler = new PropertyHandler(pi);
                            RelationKey[] keypairs = config.GetRelationKeys();
                            CollectionRelationFieldMapping rmapping = new CollectionRelationFieldMapping(pi.Name, this, type, keypairs, handler);
                            collectionTmpList.Add(rmapping);
                        }
                    } else {
                        PropertyHandler handler = new PropertyHandler(pi);
                        RelationKey[] keypairs = config.GetRelationKeys();
                        SingleRelationFieldMapping rmapping = new SingleRelationFieldMapping(pi.Name, this, type, keypairs, handler);
                        singleTmpList.Add(rmapping);
                    }
                }
            }
            _collectionRelationFields = new ReadOnlyCollection<CollectionRelationFieldMapping>(collectionTmpList);
            _singleRelationFields = new ReadOnlyCollection<SingleRelationFieldMapping>(singleTmpList);
        }

        bool _isDataEntity;

        public bool IsDataEntity {
            get {
                return _isDataEntity;
            }
        }

        string _tableName;

        public string TableName {
            get {
                return _tableName;
            }
        }

        public bool Equals(DataEntityMapping mapping)
        {
            if (mapping == null)
                return false;
            return this.ObjectType.Equals(mapping.ObjectType);
        }

        protected void InitialDataFieldMapping()
        {
            PropertyInfo[] propertys = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int index = 0;
            List<FieldInfo> list = new List<FieldInfo>();
            foreach (PropertyInfo pi in propertys) {
                //字段属性
                DataFieldMapperConfig config = MapperConfigManager.LoadDataFieldConfig(ObjectType, pi);
                if (config != null) {
                    index++;
                    FieldInfo info = new FieldInfo(pi, config, index);
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
            List<DataFieldMapping> tmplist = new List<DataFieldMapping>();
            for (int i = 0; i < list.Count; i++) {
                FieldInfo info = list[i];
                DataFieldMapping mapping = DataFieldMapping.CreateDataFieldMapping(info.Property, info.Config, i + 1, this);
                _fieldMappingDictionary.Add(mapping.IndexName, mapping);
                if (mapping.Name != mapping.IndexName) {
                    _fieldMappingDictionary.Add(mapping.Name, mapping);
                }
                tmplist.Add(mapping);
            }
            _fieldList = new ReadOnlyCollection<DataFieldMapping>(tmplist);
        }

        public ReadOnlyCollection<DataFieldMapping> DataEntityFields {
            get {
                return _fieldList;
            }
        }

        object joinLock = new object();

        RelationMap relationMap;

        public RelationMap GetRelationMap()
        {
            if (this.relationMap == null) {
                lock (joinLock) {
                    if (this.relationMap == null) {
                        this.relationMap = new RelationMap(this);
                    }
                }
            }
            return this.relationMap;
        }


        public DataFieldMapping FindDataEntityField(string fieldName)
        {
            _fieldMappingDictionary.TryGetValue(fieldName, out DataFieldMapping mapping);
            return mapping;
        }

        public int FieldCount {
            get {
                return this._fieldList.Count;
            }
        }

        public bool HasJoinRelateModel {
            get {
                return _singleRelationFields.Count > 0;
            }
        }

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
            object item = Activator.CreateInstance(this.ObjectType);
            queryState.SetJoinData(fieldPath, item);
            string aliasName = queryState.GetAliasName(fieldPath);
            foreach (DataFieldMapping field in this._fieldList) {
                string name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState.CheckSelectField(name)) {
                    object obj = datareader[name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (CollectionRelationFieldMapping mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }

            foreach (SingleRelationFieldMapping mapping in _singleRelationFields) {
                string fpath = string.Format("{0}.{1}", fieldPath, mapping.FieldName);
                object value = mapping.ToProperty(context, datareader, queryState, fpath);
                if (!Object.Equals(value, null)) {
                    mapping.Handler.Set(item, value);
                }
            }
            if(_isDataEntity) {
                DataEntity entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            QueryState queryState = state as QueryState;
            if (this._singleRelationFields.Count > 0) {
                queryState.InitialJoinData();
                //queryState.SetJoinData(string.Empty, item);
                //LoadJoinTableData(context, datareader, item, queryState, string.Empty);
                //return item;
                return LoadJoinTableData(context, datareader, queryState, string.Empty);
            }
            object item = Activator.CreateInstance(ObjectType);
            foreach (DataFieldMapping field in this._fieldList) {
                if (queryState == null) {
                    object obj = datareader[field.Name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                } else if (queryState.CheckSelectField(field.Name)) {
                    object obj = datareader[field.Name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (CollectionRelationFieldMapping mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }
            if (_isDataEntity) {
                DataEntity entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public virtual object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            object item = Activator.CreateInstance(ObjectType);
            bool nodataSetNull = queryState != null ? queryState.CheckNoDataSetNull(aliasName) : false;
            bool hasData = false;
            foreach (DataFieldMapping field in this._fieldList) {
                if (field == null)
                    continue;
                string name = string.Format("{0}_{1}", aliasName, field.Name);
                if (queryState == null) {
                    object obj = datareader[name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        field.Handler.Set(item, value);
                    }
                } else if (queryState.CheckSelectField(name)) {
                    object obj = datareader[name];
                    object value = field.ToProperty(obj);
                    if (!Object.Equals(value, null)) {
                        hasData = true;
                        field.Handler.Set(item, value);
                    }
                }
            }
            if (_collectionRelationFields.Count > 0) {
                foreach (CollectionRelationFieldMapping mapping in _collectionRelationFields) {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, false));
                }
            }
            if (!hasData && nodataSetNull) {
                return null;
            }
            if (_isDataEntity) {
                DataEntity entity = item as DataEntity;
                entity.SetContext(context);
            }
            return item;
        }

        public override object InitialData()
        {
            object item = Activator.CreateInstance(ObjectType);
            return item;
        }

        #region alise

        #endregion

        class FieldInfo
        {
            public FieldInfo(PropertyInfo property, DataFieldMapperConfig config, int fieldOrder)
            {
                this.property = property;
                this.config = config;
                this.fieldOrder = fieldOrder;
                if (config.DataOrder > 0) {
                    this.dataOrder = config.DataOrder;
                }
            }

            readonly PropertyInfo property;

            public PropertyInfo Property {
                get {
                    return property;
                }
            }

            readonly DataFieldMapperConfig config;

            public DataFieldMapperConfig Config {
                get {
                    return config;
                }
            }

            readonly int? dataOrder;

            public int? DataOrder {
                get {
                    return dataOrder;
                }
            }

            readonly int fieldOrder;

            public int FieldOrder {
                get {
                    return fieldOrder;
                }
            }
        }
    }
}
