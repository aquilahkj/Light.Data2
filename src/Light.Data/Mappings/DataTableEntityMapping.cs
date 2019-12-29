using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Data table entity mapping.
    /// </summary>
    internal class DataTableEntityMapping : DataEntityMapping
    {
        public bool IsDataTableEntity { get; }

        internal DataTableEntityMapping(Type type, string tableName, bool isDataEntity, bool isDataTableEntity)
            : base(type, tableName, isDataEntity)
        {
            IsDataTableEntity = isDataTableEntity;
            InitialFieldList();
        }

        public DataFieldMapping IdentityField { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> NoIdentityFields { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> PrimaryKeyFields { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> NoPrimaryKeyFields { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> CreateFieldList { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> UpdateFieldList { get; private set; }

        public ReadOnlyCollection<DataFieldMapping> AutoUpdateFieldList { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has identity.
        /// </summary>
        /// <value><c>true</c> if this instance has identity; otherwise, <c>false</c>.</value>
        public bool HasIdentity => IdentityField != null;

        /// <summary>
        /// Gets a value indicating whether this instance has primary key.
        /// </summary>
        /// <value><c>true</c> if this instance has primary key; otherwise, <c>false</c>.</value>
        public bool HasPrimaryKey => PrimaryKeyFields.Count > 0;

        /// <summary>
        /// Gets the primary key count.
        /// </summary>
        /// <value>The primary key count.</value>
        public int PrimaryKeyCount => PrimaryKeyFields.Count;

        private void InitialFieldList()
        {
            var noIdentityTmpList = new List<DataFieldMapping>();
            var primaryKeyTmpList = new List<DataFieldMapping>();
            var noPrimaryKeyTmpList = new List<DataFieldMapping>();
            var createFieldTmpList = new List<DataFieldMapping>();
            var updateFieldTmpList = new List<DataFieldMapping>();
            var autoUpdateTmpList = new List<DataFieldMapping>();

            foreach (var field in _fieldList)
            {
                if (field.IsIdentity)
                {
                    if (IdentityField == null)
                    {
                        IdentityField = field;
                    }
                    else
                    {
                        throw new LightDataException(string.Format(SR.MultipleIdentityField, ObjectType));
                    }
                }
                else
                {
                    noIdentityTmpList.Add(field);
                    if ((field.FunctionControl & FunctionControl.Create) == FunctionControl.Create)
                    {
                        createFieldTmpList.Add(field);
                    }
                }

                if (field.IsPrimaryKey)
                {
                    primaryKeyTmpList.Add(field);
                }
                else
                {
                    noPrimaryKeyTmpList.Add(field);
                    if ((field.FunctionControl & FunctionControl.Update) == FunctionControl.Update)
                    {
                        updateFieldTmpList.Add(field);
                    }
                    if (field.IsAutoUpdate)
                    {
                        autoUpdateTmpList.Add(field);
                    }
                }
            }

            NoIdentityFields = new ReadOnlyCollection<DataFieldMapping>(noIdentityTmpList);
            PrimaryKeyFields = new ReadOnlyCollection<DataFieldMapping>(primaryKeyTmpList);
            NoPrimaryKeyFields = new ReadOnlyCollection<DataFieldMapping>(noPrimaryKeyTmpList);
            CreateFieldList = new ReadOnlyCollection<DataFieldMapping>(createFieldTmpList);
            UpdateFieldList = new ReadOnlyCollection<DataFieldMapping>(updateFieldTmpList);
            AutoUpdateFieldList = new ReadOnlyCollection<DataFieldMapping>(autoUpdateTmpList);
        }

        public object[] GetPrimaryKeys(object data)
        {
            var rawKeys = new object[PrimaryKeyCount];
            for (var i = 0; i < PrimaryKeyCount; i++)
            {
                var field = PrimaryKeyFields[i];
                rawKeys[i] = field.Handler.Get(data);
            }

            return rawKeys;
        }

        public override object LoadAliasJoinTableData(DataContext context, IDataReader dataReader,
            QueryState queryState, string aliasName)
        {
            var data = base.LoadAliasJoinTableData(context, dataReader, queryState, aliasName);
            if (IsDataTableEntity)
            {
                UpdateDateTableEntity(data);
            }

            return data;
        }

        public override object LoadData(DataContext context, IDataReader dataReader, object state)
        {
            var data = base.LoadData(context, dataReader, state);
            if (IsDataTableEntity)
            {
                UpdateDateTableEntity(data);
            }

            return data;
        }

        public override object LoadJoinTableData(DataContext context, IDataReader dataReader, QueryState queryState,
            string fieldPath)
        {
            var data = base.LoadJoinTableData(context, dataReader, queryState, fieldPath);
            if (IsDataTableEntity)
            {
                UpdateDateTableEntity(data);
            }

            return data;
        }

        private void UpdateDateTableEntity(object data)
        {
            if (data is DataTableEntity tableEntity) tableEntity.LoadData();
            //if (tableEntity.IsAllowUpdatePrimaryKey()) {
            //    tableEntity.SetRawPrimaryKeys(GetRawKeys(tableEntity));
            //}
        }
    }
}