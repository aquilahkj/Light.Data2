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

        public ReadOnlyCollection<DataFieldMapping> TimeStampFieldList { get; private set; }

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
            var createTmpList = new List<DataFieldMapping>();
            var updateTmpList = new List<DataFieldMapping>();

            foreach (FieldMapping field in _fieldList) {
                if (field is PrimitiveFieldMapping pfmapping) {
                    if (pfmapping.IsIdentity) {
                        if (IdentityField == null) {
                            IdentityField = pfmapping;
                        }
                        else {
                            throw new LightDataException(string.Format(SR.MultipleIdentityField, ObjectType));
                        }
                    }
                    else {
                        noIdentityTmpList.Add(pfmapping);
                    }
                    if (pfmapping.IsPrimaryKey) {
                        primaryKeyTmpList.Add(pfmapping);
                    }
                    else {
                        noPrimaryKeyTmpList.Add(pfmapping);
                    }
                }
                else {
                    var mapping = field as DataFieldMapping;
                    noIdentityTmpList.Add(mapping);
                    noPrimaryKeyTmpList.Add(mapping);
                }
            }

            NoIdentityFields = new ReadOnlyCollection<DataFieldMapping>(noIdentityTmpList);
            PrimaryKeyFields = new ReadOnlyCollection<DataFieldMapping>(primaryKeyTmpList);
            NoPrimaryKeyFields = new ReadOnlyCollection<DataFieldMapping>(noPrimaryKeyTmpList);
            CreateFieldList = new ReadOnlyCollection<DataFieldMapping>(noIdentityTmpList.FindAll(x => (x.FunctionControl & FunctionControl.Create) == FunctionControl.Create));
            UpdateFieldList = new ReadOnlyCollection<DataFieldMapping>(noPrimaryKeyTmpList.FindAll(x => (x.FunctionControl & FunctionControl.Update) == FunctionControl.Update));
            TimeStampFieldList = new ReadOnlyCollection<DataFieldMapping>(noPrimaryKeyTmpList.FindAll(x => x.IsTimeStamp));

        }

        public object[] GetPrimaryKeys(object data)
        {
            var rawkeys = new object[PrimaryKeyCount];
            for (var i = 0; i < PrimaryKeyCount; i++) {
                var field = PrimaryKeyFields[i];
                rawkeys[i] = field.Handler.Get(data);
            }
            return rawkeys;
        }

        public override object LoadAliasJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string aliasName)
        {
            var data = base.LoadAliasJoinTableData(context, datareader, queryState, aliasName);
            if (IsDataTableEntity) {
                UpdateDateTableEntity(data);
            }
            return data;
        }

        public override object LoadData(DataContext context, IDataReader datareader, object state)
        {
            var data = base.LoadData(context, datareader, state);
            if (IsDataTableEntity) {
                UpdateDateTableEntity(data);
            }
            return data;
        }

        public override object LoadJoinTableData(DataContext context, IDataReader datareader, QueryState queryState, string fieldPath)
        {
            var data = base.LoadJoinTableData(context, datareader, queryState, fieldPath);
            if (IsDataTableEntity) {
                UpdateDateTableEntity(data);
            }
            return data;
        }

        private void UpdateDateTableEntity(object data)
        {
            var tableEntity = data as DataTableEntity;
            tableEntity.LoadData();
            //if (tableEntity.IsAllowUpdatePrimaryKey()) {
            //    tableEntity.SetRawPrimaryKeys(GetRawKeys(tableEntity));
            //}
        }
    }
}
