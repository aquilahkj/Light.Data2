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
        private static readonly Type LCollectionFrameType;

        static DataEntityMapping()
        {
            LCollectionFrameType = typeof(LCollection<object>).GetGenericTypeDefinition();
        }

        public static DataEntityMapping Default { get; } = new DataEntityMapping(typeof(DataEntityMapping));

        #region static

        private static readonly object locker = new object();

        private static readonly Dictionary<Type, DataEntityMapping> _defaultMapping =
            new Dictionary<Type, DataEntityMapping>();

        /// <summary>
        /// Gets the table mapping.
        /// </summary>
        /// <returns>The table mapping.</returns>
        /// <param name="type">Type.</param>
        public static DataTableEntityMapping GetTableMapping(Type type)
        {
            if (GetEntityMapping(type) is DataTableEntityMapping dataMapping)
            {
                return dataMapping;
            }

            throw new LightDataException(string.Format(SR.IsNotDataTableMapping, type));
        }

        /// <summary>
        /// Gets the entity mapping.
        /// </summary>
        /// <returns>The entity mapping.</returns>
        /// <param name="type">Type.</param>
        public static DataEntityMapping GetEntityMapping(Type type)
        {
            var mappings = _defaultMapping;
            if (!mappings.TryGetValue(type, out var mapping))
            {
                lock (locker)
                {
                    if (!mappings.TryGetValue(type, out mapping))
                    {
                        mapping = CreateMapping(type);
                        mappings[type] = mapping;
                    }
                }
            }

            return mapping;
        }

        public static void ResetEntityMapping()
        {
            lock (locker)
            {
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
            if (config != null)
            {
                tableName = config.TableName;
                isEntityTable = config.IsEntityTable;
            }
            else
            {
                throw new LightDataException(string.Format(SR.NoDataEntityConfig, type.Name));
            }

            if (string.IsNullOrEmpty(tableName))
            {
                tableName = type.Name;
            }

            if (typeInfo.IsSubclassOf(typeof(DataTableEntity)))
            {
                dataMapping = new DataTableEntityMapping(type, tableName, true, true);
            }
            else if (typeInfo.IsSubclassOf(typeof(DataEntity)))
            {
                dataMapping = new DataEntityMapping(type, tableName, true);
            }
            else
            {
                dataMapping = !isEntityTable
                    ? new DataEntityMapping(type, tableName, false)
                    : new DataTableEntityMapping(type, tableName, false, false);
            }

            dataMapping.ExtentParams = config.ConfigParams;
            return dataMapping;
        }

        #endregion

        protected readonly Dictionary<string, DataFieldMapping> _fieldMappingDictionary =
            new Dictionary<string, DataFieldMapping>();

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
            TableName = string.IsNullOrEmpty(tableName) ? type.Name : tableName;
            IsDataEntity = isDataEntity;
            InitialDataFieldMapping();
            InitialRelationField();
        }

        internal SingleRelationFieldMapping[] GetSingleRelationFieldMappings()
        {
            var len = _singleRelationFields.Count;
            var array = new SingleRelationFieldMapping[len];
            var index = 0;
            foreach (var item in _singleRelationFields)
            {
                array[index] = item;
                index++;
            }

            return array;
        }

        internal ReadOnlyCollection<SingleRelationFieldMapping> SingleJoinTableRelationFieldMappings =>
            _singleRelationFields;

        internal ReadOnlyCollection<CollectionRelationFieldMapping> CollectionRelationFieldMappings =>
            _collectionRelationFields;

        private void InitialRelationField()
        {
            var properties = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var collectionTmpList = new List<CollectionRelationFieldMapping>();
            var singleTmpList = new List<SingleRelationFieldMapping>();

            foreach (var pi in properties)
            {
                var config = MapperConfigManager.LoadRelationDataFieldConfig(ObjectType, pi);
                if (config != null && config.RelationKeyCount > 0)
                {
                    var type = pi.PropertyType;
                    var typeInfo = type.GetTypeInfo();
                    if (typeInfo.IsGenericType)
                    {
                        var frameType = type.GetGenericTypeDefinition();
                        if (frameType == LCollectionFrameType ||
                            frameType.FullName == "System.Collections.Generic.ICollection`1")
                        {
                            var arguments = typeInfo.GetGenericArguments();
                            type = arguments[0];
                            var handler = new PropertyHandler(pi);
                            var keyPairs = config.GetRelationKeys();
                            var mapping = new CollectionRelationFieldMapping(pi.Name, this, type, keyPairs, handler);
                            collectionTmpList.Add(mapping);
                        }
                    }
                    else
                    {
                        var handler = new PropertyHandler(pi);
                        var keyPairs = config.GetRelationKeys();
                        var mapping = new SingleRelationFieldMapping(pi.Name, this, type, keyPairs, handler);
                        singleTmpList.Add(mapping);
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
            return ObjectType == mapping.ObjectType;
        }

        protected void InitialDataFieldMapping()
        {
            var properties = ObjectTypeInfo.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var index = 0;
            var list = new List<FieldInfo>();
            foreach (var pi in properties)
            {
                var config = MapperConfigManager.LoadDataFieldConfig(ObjectType, pi);
                if (config != null)
                {
                    index++;
                    var info = new FieldInfo(pi, config, index);
                    list.Add(info);
                }
            }

            if (list.Count == 0)
            {
                throw new LightDataException(string.Format(SR.NoMappingField, ObjectType));
            }

            list.Sort((x, y) =>
            {
                if (x.DataOrder.HasValue && y.DataOrder.HasValue)
                {
                    if (x.DataOrder > y.DataOrder)
                    {
                        return 1;
                    }

                    if (x.DataOrder < y.DataOrder)
                    {
                        return -1;
                    }

                    return x.FieldOrder > y.FieldOrder ? 1 : -1;
                }

                if (x.DataOrder.HasValue && !y.DataOrder.HasValue)
                {
                    return -1;
                }

                if (!x.DataOrder.HasValue && y.DataOrder.HasValue)
                {
                    return 1;
                }

                return x.FieldOrder > y.FieldOrder ? 1 : -1;
            });
            var tmpList = new List<DataFieldMapping>();
            for (var i = 0; i < list.Count; i++)
            {
                var info = list[i];
                var mapping = DataFieldMapping.CreateDataFieldMapping(info.Property, info.Config, i + 1, this);
                _fieldMappingDictionary.Add(mapping.IndexName, mapping);
                if (mapping.Name != mapping.IndexName)
                {
                    _fieldMappingDictionary.Add(mapping.Name, mapping);
                }

                tmpList.Add(mapping);
            }

            _fieldList = new ReadOnlyCollection<DataFieldMapping>(tmpList);
        }

        public ReadOnlyCollection<DataFieldMapping> DataEntityFields => _fieldList;

        private readonly object joinLock = new object();

        private RelationMap relationMap;

        public RelationMap GetRelationMap()
        {
            if (relationMap == null)
            {
                lock (joinLock)
                {
                    if (relationMap == null)
                    {
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

        public virtual object LoadJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState,
            string fieldPath)
        {
            var item = Activator.CreateInstance(ObjectType);
            queryState.SetJoinData(fieldPath, item);
            var aliasName = queryState.GetAliasName(fieldPath);
            foreach (var field in _fieldList)
            {
                var name = $"{aliasName}_{field.Name}";
                if (queryState.CheckSelectField(name))
                {
                    var obj = dataReader[name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null))
                    {
                        field.Handler.Set(item, value);
                    }
                }
            }

            if (_collectionRelationFields.Count > 0)
            {
                foreach (var mapping in _collectionRelationFields)
                {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }

            foreach (var mapping in _singleRelationFields)
            {
                var path = $"{fieldPath}.{mapping.FieldName}";
                var value = mapping.ToProperty(context, dataReader, queryState, path);
                if (!Equals(value, null))
                {
                    mapping.Handler.Set(item, value);
                }
            }

            if (IsDataEntity && item is DataEntity entity)
            {
                entity.SetContext(context);
            }

            return item;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var queryState = state as QueryState;
            if (_singleRelationFields.Count > 0 && queryState != null)
            {
                queryState.InitialJoinData();
                return LoadJoinTableData(context, dataReader, queryState, string.Empty);
            }

            var item = Activator.CreateInstance(ObjectType);
            foreach (var field in _fieldList)
            {
                if (queryState == null)
                {
                    var obj = dataReader[field.Name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null))
                    {
                        field.Handler.Set(item, value);
                    }
                }
                else if (queryState.CheckSelectField(field.Name))
                {
                    var obj = dataReader[field.Name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null))
                    {
                        field.Handler.Set(item, value);
                    }
                }
            }

            if (_collectionRelationFields.Count > 0)
            {
                foreach (var mapping in _collectionRelationFields)
                {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, true));
                }
            }

            if (IsDataEntity && item is DataEntity entity)
            {
                entity.SetContext(context);
            }

            return item;
        }

        public virtual object LoadAliasJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState,
            string aliasName)
        {
            var item = Activator.CreateInstance(ObjectType);
            var nodataSetNull = queryState?.CheckNoDataSetNull(aliasName) ?? false;
            var hasData = false;
            foreach (var field in _fieldList)
            {
                if (field == null)
                    continue;
                var name = $"{aliasName}_{field.Name}";
                if (queryState == null)
                {
                    var obj = dataReader[name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null))
                    {
                        field.Handler.Set(item, value);
                    }
                }
                else if (queryState.CheckSelectField(name))
                {
                    var obj = dataReader[name];
                    var value = field.ToProperty(obj);
                    if (!Equals(value, null))
                    {
                        hasData = true;
                        field.Handler.Set(item, value);
                    }
                }
            }

            if (_collectionRelationFields.Count > 0)
            {
                foreach (var mapping in _collectionRelationFields)
                {
                    mapping.Handler.Set(item, mapping.ToProperty(context, item, false));
                }
            }

            if (!hasData && nodataSetNull)
            {
                return null;
            }

            if (IsDataEntity && item is DataEntity entity)
            {
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
                Property = property;
                Config = config;
                FieldOrder = fieldOrder;
                if (config.DataOrder > 0)
                {
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