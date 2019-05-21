using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// DataContext.
    /// </summary>
    public class DataContext : IDisposable
    {
        /// <summary>
        /// Get the name of the mapping table.
        /// </summary>
        /// <returns>The table name.</returns>
        /// <typeparam name="T">Specified object type.</typeparam>
        public static string GetTableName<T>()
        {
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            return mapping.TableName;
        }

        private string _connectionString;

        private DatabaseProvider _database;

        private DataContextOptions _options;

        private Dictionary<DataEntityMapping, string> _aliasTableDict;

        /// <summary>
        /// Sql Parameter Prefix
        /// </summary>
        public string ParameterPrefix {
            get {
                return _database.ParameterPrefix;
            }
        }

        /// <summary>
        /// Set the mapping table alias name for the specified object type
        /// </summary>
        /// <typeparam name="T">Specified object type.</typeparam>
        /// <param name="name">Alias name.</param>
        public void SetAliasTableName<T>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (_aliasTableDict == null) {
                _aliasTableDict = new Dictionary<DataEntityMapping, string>();
            }
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            _aliasTableDict[mapping] = name;
        }

        /// <summary>
        /// Clear the mapping table alias name for the specified object type
        /// </summary>
        /// <typeparam name="T">Specified object type.</typeparam>
        /// <returns>Clear success or not</returns>
        public bool ResetAliasTableName<T>()
        {
            if (_aliasTableDict == null) {
                return false;
            }
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            return _aliasTableDict.Remove(mapping);
        }

        /// <summary>
        /// Clear all the mapping table alias name
        /// </summary>
        public void ClearAliasTableName()
        {
            if (_aliasTableDict != null) {
                _aliasTableDict.Clear();
            }
        }

        internal bool TryGetAliasTableName(DataEntityMapping mapping, out string name)
        {
            if (_aliasTableDict == null) {
                name = null;
                return false;
            }
            else {
                return _aliasTableDict.TryGetValue(mapping, out name);
            }
        }
        /// <summary>
        /// DataContext Options
        /// </summary>
        protected internal DataContextOptions Options {
            get {
                return _options;
            }
        }

        internal ICommandOutput Output {
            get {
                return _output;
            }
        }

        /// <summary>
        /// The command output interface.
        /// </summary>
        ICommandOutput _output;

        /// <summary>
        /// Sets the command output.
        /// </summary>
        /// <param name="output">Output.</param>
        public void SetCommandOutput(ICommandOutput output)
        {
            if (output != null) {
                this._output = output;
            }
            else {
                this._output = _options.CommandOutput;
            }
        }


        int _batchInsertCount = -1;

        /// <summary>
        /// Set batch insert count
        /// </summary>
        /// <param name="value"></param>
        public void SetBatchInsertCount(int value)
        {
            if (value < 0) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _batchInsertCount = value;
        }

        /// <summary>
        ///Reset batch insert count
        /// </summary>
        public void ResetBatchInsertCount()
        {
            _batchInsertCount = -1;
        }

        int _batchUpdateCount = -1;

        /// <summary>
        /// Set batch update count
        /// </summary>
        /// <param name="value"></param>
        public void SetBatchUpdateCount(int value)
        {
            if (value < 0) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _batchUpdateCount = value;
        }

        /// <summary>
        ///Reset batch update count
        /// </summary>
        public void ResetBatchUpdateCount()
        {
            _batchUpdateCount = -1;
        }

        int _batchDeleteCount = -1;

        /// <summary>
        /// Set batch delete count
        /// </summary>
        /// <param name="value"></param>
        public void SetBatchDeleteCount(int value)
        {
            if (value < 0) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _batchDeleteCount = value;
        }

        /// <summary>
        ///Reset batch delete count
        /// </summary>
        public void ResetBatchDeleteCount()
        {
            _batchDeleteCount = -1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContext"/> class.
        /// </summary>
        public DataContext()
        {
            DataContextOptions options = DataContextConfiguration.Global.DefaultOptions;
            Internal_DataContext(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContext"/> class.
        /// </summary>
        /// <param name="configName">Config name.</param>
        public DataContext(string configName)
        {
            DataContextOptions options = DataContextConfiguration.Global.GetOptions(configName);
            Internal_DataContext(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContext"/> class.
        /// </summary>
        /// <param name="setting">Connection setting.</param>
        public DataContext(IConnectionSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));
            DataContextOptions options = DataContextOptions.CreateOptions(setting);
            Internal_DataContext(options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContext"/> class.
        /// </summary>
        /// <param name="options">Context options.</param>
        public DataContext(DataContextOptions options)
        {
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
            Internal_DataContext(options);
        }

        internal void Internal_DataContext(DataContextOptions options)
        {
            this._options = options;
            if (options.Database == null) {
                throw new ArgumentNullException(nameof(options.Database));
            }
            this._database = options.Database;
            if (string.IsNullOrEmpty(options.Connection)) {
                throw new ArgumentNullException(nameof(options.Connection));
            }
            this._connectionString = options.Connection;
            this._output = options.CommandOutput;
        }

        internal DatabaseProvider Database {
            get {
                return this._database;
            }
        }

        private void UpdateDateTableEntity(DataTableEntityMapping mapping, object data)
        {
            DataTableEntity tableEntity = data as DataTableEntity;
            tableEntity.LoadData();
            if (tableEntity.IsAllowUpdatePrimaryKey()) {
                tableEntity.SetRawPrimaryKeys(mapping.GetPrimaryKeys(tableEntity));
            }
            tableEntity.ClearUpdateFields();
        }

        private void ClearDataTableEntity(object data)
        {
            DataTableEntity tableEntity = data as DataTableEntity;
            if (tableEntity.IsAllowUpdatePrimaryKey()) {
                tableEntity.ClearRawPrimaryKeys();
            }
            tableEntity.ClearUpdateFields();
        }

        /// <summary>
        /// Create a new result.
        /// </summary>
        /// <returns>The new result.</returns>
        /// <typeparam name="T">Data type.</typeparam>
        public T CreateNew<T>() where T : class, new()
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            T obj = new T();//mapping.InitialData();
            if (mapping.IsDataEntity) {
                DataEntity entity = obj as DataEntity;
                entity.SetContext(this);
            }
            return obj;
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int InsertOrUpdate<T>(T data)
        {
            return InsertOrUpdate(data, SafeLevel.Default, false);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <returns>result.</returns>
        public int InsertOrUpdate<T>(T data, bool refresh)
        {
            return InsertOrUpdate(data, SafeLevel.Default, refresh);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data</param>
        /// <param name="level">Safe level</param>
        /// <returns>result.</returns>
        public int InsertOrUpdate<T>(T data, SafeLevel level)
        {
            return InsertOrUpdate(data, level, false);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data</param>
        /// <param name="level">Safe level</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <returns>result.</returns>
        public int InsertOrUpdate<T>(T data, SafeLevel level, bool refresh)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return InsertOrUpdate(mapping, data, level, refresh);
        }

        internal int InsertOrUpdate(DataTableEntityMapping mapping, object data, SafeLevel level, bool refresh)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            int rInt = 0;
            object[] primaryKeys = mapping.GetPrimaryKeys(data);
            object[] rawKeys = null;
            bool updateKey = false;
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                rawKeys = tableEntity.GetRawPrimaryKeys();
                if (rawKeys != null) {
                    for (int i = 0; i < rawKeys.Length; i++) {
                        if (!rawKeys[i].Equals(primaryKeys[i])) {
                            updateKey = true;
                        }
                    }
                }
            }
            if (level == SafeLevel.Default) {
                if (mapping.HasIdentity && mapping.PrimaryKeyCount == 1 && mapping.IdentityField == mapping.PrimaryKeyFields[0]) {
                    level = SafeLevel.High;
                }
                else {
                    level = SafeLevel.Serializable;
                }
            }
            QueryCommand queryCommand = _database.ExistsByKey(this, mapping, updateKey ? rawKeys : primaryKeys);
            TransactionConnection transaction = CreateInnerTransaction(level);
            try {
                DataDefine define = DataDefine.GetDefine(typeof(int?));
                int? obj = QueryDataDefineSingle<int?>(define, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, transaction);
                if (!obj.HasValue) {
                    if (updateKey) {
                        throw new LightDataException(SR.PrimaryKeyDataNotExists);
                    }
                    else {
                        QueryCommand insertCommand = _database.Insert(this, mapping, data, refresh, true);
                        if (insertCommand.IdentitySql) {
                            object id = ExecuteScalar(insertCommand.Command, SafeLevel.Default, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                            rInt = 1;
                        }
                        else {
                            rInt = ExecuteNonQuery(insertCommand.Command, SafeLevel.Default, transaction);
                            if (mapping.HasIdentity && rInt > 0) {
                                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                                object id = ExecuteScalar(identityCommand.Command, SafeLevel.Default, transaction);
                                _database.UpdateDataIdentity(mapping, data, id);
                            }
                        }
                    }
                }
                else {
                    QueryCommand updateCommand = _database.Update(this, mapping, data, refresh);
                    if (updateCommand != null) {
                        rInt = ExecuteNonQuery(updateCommand.Command, SafeLevel.Default, transaction);
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await InsertOrUpdateAsync(data, SafeLevel.Default, false, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="level">Safe level</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, SafeLevel level, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await InsertOrUpdateAsync(data, level, false, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await InsertOrUpdateAsync(data, SafeLevel.Default, refresh, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="level">Safe level</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, SafeLevel level, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await InsertOrUpdateAsync(mapping, data, level, refresh, cancellationToken);
        }

        internal async Task<int> InsertOrUpdateAsync(DataTableEntityMapping mapping, object data, SafeLevel level, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            int rInt = 0;
            object[] primaryKeys = mapping.GetPrimaryKeys(data);
            object[] rawKeys = null;
            bool updateKey = false;
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                rawKeys = tableEntity.GetRawPrimaryKeys();
                if (rawKeys != null) {
                    for (int i = 0; i < rawKeys.Length; i++) {
                        if (!rawKeys[i].Equals(primaryKeys[i])) {
                            updateKey = true;
                        }
                    }
                }
            }
            if (level == SafeLevel.Default) {
                if (mapping.HasIdentity && mapping.PrimaryKeyCount == 1 && mapping.IdentityField == mapping.PrimaryKeyFields[0]) {
                    level = SafeLevel.High;
                }
                else {
                    level = SafeLevel.Serializable;
                }
            }
            QueryCommand queryCommand = _database.ExistsByKey(this, mapping, updateKey ? rawKeys : primaryKeys);
            TransactionConnection transaction = CreateInnerTransaction(level);
            try {
                DataDefine define = DataDefine.GetDefine(typeof(int?));
                int? obj = await QueryDataDefineSingleAsync<int?>(define, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken, transaction);
                if (!obj.HasValue) {
                    if (updateKey) {
                        throw new LightDataException(SR.PrimaryKeyDataNotExists);
                    }
                    else {
                        QueryCommand insertCommand = _database.Insert(this, mapping, data, refresh, true);
                        if (insertCommand.IdentitySql) {
                            object id = await ExecuteScalarAsync(insertCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                            rInt = 1;
                        }
                        else {
                            rInt = await ExecuteNonQueryAsync(insertCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            if (mapping.HasIdentity && rInt > 0) {
                                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                                object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                                _database.UpdateDataIdentity(mapping, data, id);
                            }
                        }
                    }
                }
                else {
                    QueryCommand updateCommand = _database.Update(this, mapping, data, refresh);
                    if (updateCommand != null) {
                        rInt = await ExecuteNonQueryAsync(updateCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int Insert<T>(T data)
        {
            return Insert(data, false);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Whether to refresh data field</param>
        public int Insert<T>(T data, bool refresh)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return Insert(mapping, data, refresh);
        }

        internal int Insert(DataTableEntityMapping mapping, object data, bool refresh)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh, true);
            int rInt;
            if (queryCommand.IdentitySql) {
                object id = ExecuteScalar(queryCommand.Command, SafeLevel.Default);
                _database.UpdateDataIdentity(mapping, data, id);
                rInt = 1;
            }
            else {
                if (!mapping.HasIdentity) {
                    rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default);
                }
                else {
                    TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                    try {
                        rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                        if (rInt > 0) {
                            QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                            object id = ExecuteScalar(identityCommand.Command, SafeLevel.Default, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                        }
                    }
                    finally {
                        CommitInnerTransaction(transaction);
                    }
                }
            }

            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertAsync<T>(T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await InsertAsync(data, false, cancellationToken);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertAsync<T>(T data, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await InsertAsync(mapping, data, refresh, cancellationToken);
        }

        internal async Task<int> InsertAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh, true);
            int rInt;
            if (queryCommand.IdentitySql) {
                object id = await ExecuteScalarAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
                _database.UpdateDataIdentity(mapping, data, id);
                rInt = 1;
            }
            else {
                if (!mapping.HasIdentity) {
                    rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
                }
                else {
                    TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                    try {
                        rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                        if (rInt > 0) {
                            QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                            object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                        }
                    }
                    finally {
                        CommitInnerTransaction(transaction);
                    }
                }
            }

            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int Update<T>(T data)
        {
            return Update(data, false);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Whether to refresh data field</param>
        public int Update<T>(T data, bool refresh)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return Update(mapping, data, refresh);
        }

        internal int Update(DataTableEntityMapping mapping, object data, bool refresh)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Update(this, mapping, data, refresh);
            if (queryCommand == null) {
                return 0;
            }
            int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default);
            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> UpdateAsync<T>(T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await UpdateAsync(data, false, cancellationToken);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Whether to refresh data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> UpdateAsync<T>(T data, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await UpdateAsync(mapping, data, refresh, cancellationToken);
        }

        internal async Task<int> UpdateAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Update(this, mapping, data, refresh);
            if (queryCommand == null) {
                return 0;
            }
            int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int Delete<T>(T data)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return Delete(mapping, data);
        }

        internal int Delete(DataTableEntityMapping mapping, object data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Delete(this, mapping, data);
            int rInt;
            rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default);
            if (mapping.IsDataTableEntity) {
                ClearDataTableEntity(data);
            }
            return rInt;
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> DeleteAsync<T>(T data, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await DeleteAsync(mapping, data, cancellationToken);
        }

        internal async Task<int> DeleteAsync(DataTableEntityMapping mapping, object data, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(data.GetType());
            }
            QueryCommand queryCommand = _database.Delete(this, mapping, data);
            int rInt;
            rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
            if (mapping.IsDataTableEntity) {
                ClearDataTableEntity(data);
            }
            return rInt;
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public int InsertDatas<T>(params T[] datas)
        {
            return BatchInsert(datas, 0, int.MaxValue, false, true);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas)
        {
            return BatchInsert(datas, 0, int.MaxValue, false, true);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields..</param>
        /// <param name="updateIdentity">Whether to update the identity field.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, bool refresh, bool updateIdentity)
        {
            return BatchInsert(datas, 0, int.MaxValue, refresh, updateIdentity);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="index">Index.</param>
        /// <param name="size">Size.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, int index, int size)
        {
            return BatchInsert(datas, index, size, false, true);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="index">Index.</param>
        /// <param name="size">Size.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields..</param>
        /// <param name="updateIdentity">Whether to update the identity field.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, int index, int size, bool refresh, bool updateIdentity)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return BatchInsert(mapping, datas, index, size, refresh, updateIdentity);
        }

        internal int BatchInsert(DataTableEntityMapping mapping, IEnumerable datas, int index, int size, bool refresh, bool updateIdentity)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (size < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchInsertCount >= 0 ? _batchInsertCount : _database.BatchInsertCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.FieldCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }

            bool useIdentity = mapping.HasIdentity && updateIdentity;
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch || (useIdentity && !_database.Factory.SupportBatchInsertIdentity)) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == size) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh, updateIdentity);
                        if (queryCommand.IdentitySql) {
                            object id = ExecuteScalar(queryCommand.Command, SafeLevel.Default, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                        }
                        else {
                            int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                            if (rInt > 0 && useIdentity) {
                                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                                object id = ExecuteScalar(identityCommand.Command, SafeLevel.Default, transaction);
                                _database.UpdateDataIdentity(mapping, data, id);
                            }
                        }

                        result++;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == size) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            int rInt = BatchInsertData(mapping, list, refresh, useIdentity, transaction);
                            result += rInt;
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        int rInt = BatchInsertData(mapping, list, refresh, useIdentity, transaction);
                        result += rInt;
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        private int BatchInsertData(DataTableEntityMapping mapping, IList list, bool refresh, bool useIdentity, TransactionConnection transaction)
        {
            if (useIdentity && mapping.HasIdentity) {
                QueryCommand queryCommand = _database.BatchInsertWithIdentity(this, mapping, list, refresh);
                IdentityDefine define = new IdentityDefine();
                List<object> identity = QueryDataDefineList<object>(define, SafeLevel.Default, queryCommand.Command, null, queryCommand.State, null, transaction);
                if (identity.Count != list.Count) {
                    throw new LightDataException(SR.BatchInsertIdentityError);
                }
                for (int i = 0; i < identity.Count; i++) {
                    object id = identity[i];
                    object idata = list[i];
                    _database.UpdateDataIdentity(mapping, idata, id);
                    if (mapping.IsDataTableEntity) {
                        UpdateDateTableEntity(mapping, idata);
                    }
                }
                return identity.Count;
            }
            else {
                QueryCommand queryCommand = _database.BatchInsert(this, mapping, list, refresh);
                int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                if (mapping.IsDataTableEntity) {
                    foreach (var idata in list) {
                        UpdateDateTableEntity(mapping, idata);
                    }
                }
                return rInt;
            }
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchInsertAsync(datas, 0, int.MaxValue, false, true, cancellationToken);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields..</param>
        /// <param name="updateIdentity">Whether to update the identity field.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, bool refresh, bool updateIdentity, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchInsertAsync(datas, 0, int.MaxValue, refresh, updateIdentity, cancellationToken);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="index">Index.</param>
        /// <param name="size">Size.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int size, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchInsertAsync(datas, index, size, false, true, cancellationToken);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Data collection.</param>
        /// <param name="index">Index.</param>
        /// <param name="size">Size.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields..</param>
        /// <param name="updateIdentity">Whether to update the identity field.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Insert object type.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int size, bool refresh, bool updateIdentity, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await BatchInsertAsync(mapping, datas, index, size, refresh, updateIdentity, cancellationToken);
        }

        //internal async Task<int> BatchInsertAsync(DataTableEntityMapping mapping, IEnumerable datas, int index, int size, bool refresh, bool updateIdentity, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    if (datas == null) {
        //        throw new ArgumentNullException(nameof(datas));
        //    }
        //    if (index < 0) {
        //        throw new ArgumentOutOfRangeException(nameof(index));
        //    }
        //    if (size < 0) {
        //        throw new ArgumentOutOfRangeException(nameof(size));
        //    }
        //    if (size == 0) {
        //        return 0;
        //    }
        //    IEnumerator enumerator = datas.GetEnumerator();

        //    int result = 0;
        //    int curIndex = 0;
        //    int curCount = 0;
        //    int batchCount = _batchInsertCount >= 0 ? _batchInsertCount : _database.BatchInsertCount;
        //    while (true) {
        //        if (enumerator.MoveNext()) {
        //            if (curIndex < index) {
        //                curIndex++;
        //                continue;
        //            }
        //            else {
        //                curCount++;
        //                if (enumerator.Current == null) {
        //                    curIndex++;
        //                    continue;
        //                }
        //                else {
        //                    break;
        //                }
        //            }
        //        }
        //        else {
        //            return 0;
        //        }
        //    }
        //    if (mapping == null) {
        //        mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
        //    }
        //    if (batchCount > 0) {
        //        int maxBatchCount = _database.Factory.MaxParameterCount / mapping.FieldCount;
        //        if (maxBatchCount < batchCount) {
        //            batchCount = maxBatchCount;
        //        }
        //    }
        //    //QueryCommand identityCommand;
        //    //if (mapping.HasIdentity && updateIdentity) {
        //    //    identityCommand = _database.InsertIdentiy(this, mapping);
        //    //}
        //    //else {
        //    //    identityCommand = null;
        //    //}
        //    bool useIdentity = mapping.HasIdentity && updateIdentity;
        //    bool isBatch = !useIdentity && batchCount > 1;
        //    TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
        //    bool ft = false;
        //    try {
        //        if (!isBatch) {
        //            while (true) {
        //                object data;
        //                if (ft) {
        //                    if (curCount == size) {
        //                        break;
        //                    }
        //                    if (enumerator.MoveNext()) {
        //                        curCount++;
        //                        data = enumerator.Current;
        //                        if (data == null) {
        //                            continue;
        //                        }
        //                    }
        //                    else {
        //                        break;
        //                    }
        //                }
        //                else {
        //                    data = enumerator.Current;
        //                    ft = true;
        //                }
        //                QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh, updateIdentity);
        //                if (queryCommand.IdentitySql) {
        //                    object id = await ExecuteScalarAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                    _database.UpdateDataIdentity(mapping, data, id);
        //                }
        //                else {
        //                    int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                }
        //                //QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh);
        //                //int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                //if (rInt > 0 && identityCommand != null) {
        //                //    object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                //    _database.UpdateDataIdentity(mapping, data, id);
        //                //}
        //                //result += rInt;
        //                result++;
        //                if (mapping.IsDataTableEntity) {
        //                    UpdateDateTableEntity(mapping, data);
        //                }
        //            }
        //        }
        //        else {
        //            IList list = new ArrayList(batchCount);
        //            while (true) {
        //                object data;
        //                if (ft) {
        //                    if (curCount == size) {
        //                        break;
        //                    }
        //                    if (enumerator.MoveNext()) {
        //                        curCount++;
        //                        data = enumerator.Current;
        //                        if (data == null) {
        //                            continue;
        //                        }
        //                    }
        //                    else {
        //                        break;
        //                    }
        //                }
        //                else {
        //                    data = enumerator.Current;
        //                    ft = true;
        //                }
        //                list.Add(data);
        //                if (list.Count < batchCount) {
        //                    continue;
        //                }
        //                else {
        //                    QueryCommand queryCommand = _database.BatchInsert(this, mapping, list, refresh);
        //                    int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                    result += rInt;
        //                    if (mapping.IsDataTableEntity) {
        //                        foreach (var idata in list) {
        //                            UpdateDateTableEntity(mapping, idata);
        //                        }
        //                    }
        //                    list.Clear();
        //                }
        //            }
        //            if (list.Count > 0) {
        //                QueryCommand queryCommand = _database.BatchInsert(this, mapping, list, refresh);
        //                int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
        //                result += rInt;
        //                if (mapping.IsDataTableEntity) {
        //                    foreach (var idata in list) {
        //                        UpdateDateTableEntity(mapping, idata);
        //                    }
        //                }
        //                list.Clear();
        //            }
        //        }
        //    }
        //    finally {
        //        CommitInnerTransaction(transaction);
        //    }
        //    return result;
        //}

        internal async Task<int> BatchInsertAsync(DataTableEntityMapping mapping, IEnumerable datas, int index, int size, bool refresh, bool updateIdentity, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (size < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchInsertCount >= 0 ? _batchInsertCount : _database.BatchInsertCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.FieldCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }

            bool useIdentity = mapping.HasIdentity && updateIdentity;
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch || (useIdentity && !_database.Factory.SupportBatchInsertIdentity)) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == size) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh, updateIdentity);
                        if (queryCommand.IdentitySql) {
                            object id = await ExecuteScalarAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
                        }
                        else {
                            int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            if (rInt > 0 && useIdentity) {
                                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                                object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                                _database.UpdateDataIdentity(mapping, data, id);
                            }
                        }

                        result++;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == size) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            int rInt = await BatchInsertDataAsync(mapping, list, refresh, useIdentity, transaction, cancellationToken);
                            result += rInt;
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        int rInt = await BatchInsertDataAsync(mapping, list, refresh, useIdentity, transaction, cancellationToken);
                        result += rInt;
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        private async Task<int> BatchInsertDataAsync(DataTableEntityMapping mapping, IList list, bool refresh, bool useIdentity, TransactionConnection transaction, CancellationToken cancellationToken)
        {
            if (useIdentity && mapping.HasIdentity) {
                QueryCommand queryCommand = _database.BatchInsertWithIdentity(this, mapping, list, refresh);
                IdentityDefine define = new IdentityDefine();
                List<object> identity = await QueryDataDefineListAsync<object>(define, SafeLevel.Default, queryCommand.Command, null, queryCommand.State, null, cancellationToken, transaction);
                if (identity.Count != list.Count) {
                    throw new LightDataException(SR.BatchInsertIdentityError);
                }
                for (int i = 0; i < identity.Count; i++) {
                    object id = identity[i];
                    object idata = list[i];
                    _database.UpdateDataIdentity(mapping, idata, id);
                    if (mapping.IsDataTableEntity) {
                        UpdateDateTableEntity(mapping, idata);
                    }
                }
                return identity.Count;
            }
            else {
                QueryCommand queryCommand = _database.BatchInsert(this, mapping, list, refresh);
                int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                if (mapping.IsDataTableEntity) {
                    foreach (var idata in list) {
                        UpdateDateTableEntity(mapping, idata);
                    }
                }
                return rInt;
            }
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int UpdateDatas<T>(params T[] datas)
        {
            return BatchUpdate(datas, 0, int.MaxValue, false);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas)
        {
            return BatchUpdate(datas, 0, int.MaxValue, false);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, bool refresh)
        {
            return BatchUpdate(datas, 0, int.MaxValue, refresh);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, int index, int count)
        {
            return BatchUpdate(datas, index, count, false);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, int index, int count, bool refresh)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return BatchUpdate(mapping, datas, index, count, refresh);
        }

        internal int BatchUpdate(DataTableEntityMapping mapping, IEnumerable datas, int index, int count, bool refresh)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (count == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchUpdateCount >= 0 ? _batchUpdateCount : _database.BatchUpdateCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.FieldCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Update(this, mapping, data, refresh);
                        int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            QueryCommand queryCommand = _database.BatchUpdate(this, mapping, list, refresh);
                            if (queryCommand != null) {
                                int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                                result += rInt;
                                if (mapping.IsDataTableEntity) {
                                    foreach (var idata in list) {
                                        UpdateDateTableEntity(mapping, idata);
                                    }
                                }
                            }
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        QueryCommand queryCommand = _database.BatchUpdate(this, mapping, list, refresh);
                        if (queryCommand != null) {
                            int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                            result += rInt;
                            if (mapping.IsDataTableEntity) {
                                foreach (var idata in list) {
                                    UpdateDateTableEntity(mapping, idata);
                                }
                            }
                        }
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchUpdateAsync(datas, 0, int.MaxValue, false, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchUpdateAsync(datas, 0, int.MaxValue, refresh, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchUpdateAsync(datas, index, count, false, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">Whether to refresh null data with default value fields.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, int index, int count, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await BatchUpdateAsync(mapping, datas, index, count, refresh, cancellationToken);
        }

        internal async Task<int> BatchUpdateAsync(DataTableEntityMapping mapping, IEnumerable datas, int index, int count, bool refresh, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (count == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchUpdateCount >= 0 ? _batchUpdateCount : _database.BatchUpdateCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.FieldCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Update(this, mapping, data, refresh);
                        int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            QueryCommand queryCommand = _database.BatchUpdate(this, mapping, list, refresh);
                            if (queryCommand != null) {
                                int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                                result += rInt;
                                if (mapping.IsDataTableEntity) {
                                    foreach (var idata in list) {
                                        UpdateDateTableEntity(mapping, idata);
                                    }
                                }
                            }
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        QueryCommand queryCommand = _database.BatchUpdate(this, mapping, list, refresh);
                        if (queryCommand != null) {
                            int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            result += rInt;
                            if (mapping.IsDataTableEntity) {
                                foreach (var idata in list) {
                                    UpdateDateTableEntity(mapping, idata);
                                }
                            }
                        }
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        /// <summary>
        /// Batch delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int DeleteDatas<T>(params T[] datas)
        {
            return BatchDelete(datas, 0, int.MaxValue);
        }

        /// <summary>
        /// Batch delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchDelete<T>(IEnumerable<T> datas)
        {
            return BatchDelete(datas, 0, int.MaxValue);
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int BatchDelete<T>(IEnumerable<T> datas, int index, int count)
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return BatchDelete(mapping, datas, index, count);
        }

        internal int BatchDelete(DataTableEntityMapping mapping, IEnumerable datas, int index, int count)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (count == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchDeleteCount >= 0 ? _batchDeleteCount : _database.BatchDeleteCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.PrimaryKeyCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Delete(this, mapping, data);
                        int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            QueryCommand queryCommand = _database.BatchDelete(this, mapping, list);
                            int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                            result += rInt;
                            if (mapping.IsDataTableEntity) {
                                foreach (var idata in list) {
                                    UpdateDateTableEntity(mapping, idata);
                                }
                            }
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        QueryCommand queryCommand = _database.BatchDelete(this, mapping, list);
                        int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            foreach (var idata in list) {
                                UpdateDateTableEntity(mapping, idata);
                            }
                        }
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        /// <summary>
        /// Batch delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await BatchDeleteAsync(datas, 0, int.MaxValue, cancellationToken);
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            Type type = typeof(T);
            DataTableEntityMapping mapping = type.IsInterface || type.IsAbstract ? null : DataEntityMapping.GetTableMapping(type);
            return await BatchDeleteAsync(mapping, datas, index, count, cancellationToken);
        }

        internal async Task<int> BatchDeleteAsync(DataTableEntityMapping mapping, IEnumerable datas, int index, int count, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }
            if (index < 0) {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (count < 0) {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (count == 0) {
                return 0;
            }
            IEnumerator enumerator = datas.GetEnumerator();

            int result = 0;
            int curIndex = 0;
            int curCount = 0;
            int batchCount = _batchDeleteCount >= 0 ? _batchDeleteCount : _database.BatchDeleteCount;
            while (true) {
                if (enumerator.MoveNext()) {
                    if (curIndex < index) {
                        curIndex++;
                        continue;
                    }
                    else {
                        curCount++;
                        if (enumerator.Current == null) {
                            curIndex++;
                            continue;
                        }
                        else {
                            break;
                        }
                    }
                }
                else {
                    return 0;
                }
            }
            if (mapping == null) {
                mapping = DataEntityMapping.GetTableMapping(enumerator.Current.GetType());
            }
            if (batchCount > 0) {
                int maxBatchCount = _database.Factory.MaxParameterCount / mapping.PrimaryKeyCount;
                if (maxBatchCount < batchCount) {
                    batchCount = maxBatchCount;
                }
            }
            bool isBatch = batchCount > 1;
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            bool ft = false;
            try {
                if (!isBatch) {
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        QueryCommand queryCommand = _database.Delete(this, mapping, data);
                        int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            UpdateDateTableEntity(mapping, data);
                        }
                    }
                }
                else {
                    IList list = new ArrayList(batchCount);
                    while (true) {
                        object data;
                        if (ft) {
                            if (curCount == count) {
                                break;
                            }
                            if (enumerator.MoveNext()) {
                                curCount++;
                                data = enumerator.Current;
                                if (data == null) {
                                    continue;
                                }
                            }
                            else {
                                break;
                            }
                        }
                        else {
                            data = enumerator.Current;
                            ft = true;
                        }
                        list.Add(data);
                        if (list.Count < batchCount) {
                            continue;
                        }
                        else {
                            QueryCommand queryCommand = _database.BatchDelete(this, mapping, list);
                            int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            result += rInt;
                            if (mapping.IsDataTableEntity) {
                                foreach (var idata in list) {
                                    UpdateDateTableEntity(mapping, idata);
                                }
                            }
                            list.Clear();
                        }
                    }
                    if (list.Count > 0) {
                        QueryCommand queryCommand = _database.BatchDelete(this, mapping, list);
                        int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                        result += rInt;
                        if (mapping.IsDataTableEntity) {
                            foreach (var idata in list) {
                                UpdateDateTableEntity(mapping, idata);
                            }
                        }
                        list.Clear();
                    }
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            return result;
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public T SelectByKey<T>(params object[] primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectByKey(this, mapping, primaryKeys);
            return QueryDataDefineSingle<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object[] primaryKeys, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectByKey(this, mapping, primaryKeys);
            return await QueryDataDefineSingleAsync<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey }, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey1, object primaryKey2, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey1, primaryKey2 }, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey1, primaryKey2, primaryKey3 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public bool Exists<T>(params object[] primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.ExistsByKey(this, mapping, primaryKeys);
            DataDefine define = DataDefine.GetDefine(typeof(int?));
            int? obj = QueryDataDefineSingle<int?>(define, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null);
            return obj.HasValue;
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExistsAsync<T>(new object[] { primaryKey }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey1, object primaryKey2, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExistsAsync<T>(new object[] { primaryKey1, primaryKey2 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExistsAsync<T>(new object[] { primaryKey1, primaryKey2, primaryKey3 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<bool> ExistsAsync<T>(object[] primaryKeys, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.ExistsByKey(this, mapping, primaryKeys);
            DataDefine define = DataDefine.GetDefine(typeof(int?));
            int? obj = await QueryDataDefineSingleAsync<int?>(define, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
            return obj.HasValue;
        }

        /// <summary>
        /// Delete the object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary key.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public int DeleteByKey<T>(params object[] primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.DeleteByKey(this, mapping, primaryKeys);
            int rInt = ExecuteNonQuery(queryCommand.Command, SafeLevel.Default);
            return rInt;
        }

        /// <summary>
        /// Delete the object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> DeleteByKeyAsync<T>(object primaryKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DeleteByKeyAsync<T>(new object[] { primaryKey }, cancellationToken);
        }

        /// <summary>
        /// Delete the object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> DeleteByKeyAsync<T>(object primaryKey1, object primaryKey2, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DeleteByKeyAsync<T>(new object[] { primaryKey1, primaryKey2 }, cancellationToken);
        }

        /// <summary>
        /// Delete the object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> DeleteByKeyAsync<T>(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await DeleteByKeyAsync<T>(new object[] { primaryKey1, primaryKey2, primaryKey3 }, cancellationToken);
        }

        /// <summary>
        /// Delete the object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> DeleteByKeyAsync<T>(object[] primaryKeys, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.DeleteByKey(this, mapping, primaryKeys);
            int rInt = await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
            return rInt;
        }

        /// <summary>
        /// Select the single object by id.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="id">id</param>
        /// <returns>result.</returns>
        public T SelectById<T>(object id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectById(this, mapping, id);
            return QueryDataDefineSingle<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null);
        }

        /// <summary>
        /// Select the single object by id.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="id">id</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>result.</returns>
        public async Task<T> SelectByIdAsync<T>(object id, CancellationToken cancellationToken = default(CancellationToken))
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectById(this, mapping, id);
            return await QueryDataDefineSingleAsync<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
        }

        /// <summary>
        /// Create query expression.
        /// </summary>
        /// <returns>The queryable.</returns>
        /// <typeparam name="T">Data type.</typeparam>
        public IQuery<T> Query<T>()
        {
            return new LightQuery<T>(this);
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">Data type.</typeparam>
        public int TruncateTable<T>()
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.TruncateTable(this, mapping);
            return ExecuteNonQuery(queryCommand.Command, SafeLevel.Default);
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">Data type.</typeparam>
        public async Task<int> TruncateTableAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.TruncateTable(this, mapping);
            return await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
        }


        #region 核心数据库方法

        private void OutputCommand(string action, DbCommand command, SafeLevel level, bool isTransaction, int start, int size, DateTime startTime, DateTime endTime, bool success, object result, string exceptionMessage)
        {
            if (this._output != null) {
                try {
                    int count = command.Parameters.Count;
                    IDataParameter[] list = new IDataParameter[count];
                    command.Parameters.CopyTo(list, 0);
                    CommandOutputInfo info = new CommandOutputInfo() {
                        Action = action,
                        Command = command.CommandText,
                        CommandType = command.CommandType,
                        Datas = list,
                        IsTransaction = isTransaction,
                        Level = level,
                        Start = start,
                        Size = size,
                        StartTime = startTime,
                        EndTime = endTime,
                        Success = success,
                        Result = result,
                        ExceptionMessage = exceptionMessage
                    };
                    this._output.Output(info);
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        private void OutputCommand<T>(string action, DbCommand command, SafeLevel level, bool isTransaction, int start, int size, DateTime startTime, DateTime endTime, bool success, int resultCount, string exceptionMessage)
        {
            if (this._output != null) {
                try {
                    int count = command.Parameters.Count;
                    IDataParameter[] list = new IDataParameter[count];
                    command.Parameters.CopyTo(list, 0);
                    CommandOutputInfo info = new CommandOutputInfo() {
                        Action = action,
                        Command = command.CommandText,
                        CommandType = command.CommandType,
                        Datas = list,
                        IsTransaction = isTransaction,
                        Level = level,
                        Start = start,
                        Size = size,
                        StartTime = startTime,
                        EndTime = endTime,
                        Success = success,
                        Result = typeof(T).Name + " " + resultCount.ToString(),
                        ExceptionMessage = exceptionMessage
                    };
                    this._output.Output(info);
                }
                catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
        }

        private void ProcessExceptionTransaction(TransactionConnection transaction, bool trans)
        {
            if (trans) {
                if (transaction.IsOpen) {
                    transaction.Rollback();
                    transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                }
                else {
                    transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                }
            }
            else {
                transaction.Rollback();
                transaction.Dispose();
            }
        }

        internal int ExecuteNonQuery(DbCommand dbcommand, SafeLevel level, TransactionConnection transaction = null)
        {
            CheckStatus();
            int rInt;
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    transaction.Open();
                }
                transaction.SetupCommand(dbcommand);
                rInt = dbcommand.ExecuteNonQuery();
                result = rInt;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(ExecuteNonQuery), dbcommand, level, trans, 0, 0, startTime, endTime, success, result, exceptionMessage);
            }

            return rInt;
        }

        internal async Task<int> ExecuteNonQueryAsync(DbCommand dbcommand, SafeLevel level, CancellationToken cancellationToken, TransactionConnection transaction = null)
        {
            CheckStatus();
            int rInt;
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                rInt = await dbcommand.ExecuteNonQueryAsync(cancellationToken);
                result = rInt;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(ExecuteNonQueryAsync), dbcommand, level, trans, 0, 0, startTime, endTime, success, result, exceptionMessage);
            }

            return rInt;
        }

        internal object ExecuteScalar(DbCommand dbcommand, SafeLevel level, TransactionConnection transaction = null)
        {
            CheckStatus();
            object resultObj;
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    transaction.Open();
                }
                transaction.SetupCommand(dbcommand);
                resultObj = dbcommand.ExecuteScalar();
                result = resultObj;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(ExecuteScalar), dbcommand, level, trans, 0, 0, startTime, endTime, success, result, exceptionMessage);
            }

            return resultObj;
        }

        internal async Task<object> ExecuteScalarAsync(DbCommand dbcommand, SafeLevel level, CancellationToken cancellationToken, TransactionConnection transaction = null)
        {
            CheckStatus();
            object resultObj;
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                resultObj = await dbcommand.ExecuteScalarAsync(cancellationToken);
                result = resultObj;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(ExecuteScalarAsync), dbcommand, level, trans, 0, 0, startTime, endTime, success, result, exceptionMessage);
            }

            return resultObj;
        }

        internal IEnumerable<T> QueryDataDefineReader<T>(IDataDefine source, SafeLevel level, DbCommand dbcommand, Region region, object state, Delegate dele, TransactionConnection transaction = null)
        {
            CheckStatus();
            int start;
            int size;
            if (region != null) {
                start = region.Start;
                size = region.Size;
            }
            else {
                start = 0;
                size = int.MaxValue;
            }
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            IDataReader reader = null;
            bool error = false;
            try {
                try {
                    if (!transaction.IsOpen) {
                        transaction.Open();
                    }
                    transaction.SetupCommand(dbcommand);
                    reader = dbcommand.ExecuteReader();
                }
                catch (LightDataException ex) {
                    DateTime endTime = DateTime.Now;
                    exceptionMessage = ex.Message;
                    ProcessExceptionTransaction(transaction, trans);
                    throw ex;
                }
                catch (Exception ex) {
                    DateTime endTime = DateTime.Now;
                    exceptionMessage = ex.Message;
                    OutputCommand<T>(nameof(QueryDataDefineReader), dbcommand, level, trans, start, size, startTime, endTime, success, 0, exceptionMessage);
                    throw new LightDataDbException(ex.Message, ex);
                }

                success = true;
                while (true) {
                    T data;
                    try {
                        if (!reader.Read()) {
                            break;
                        }
                        if (count >= size) {
                            dbcommand.Cancel();
                            break;
                        }
                        if (index >= start) {
                            count++;
                            object item = source.LoadData(this, reader, state);
                            if (item == null) {
                                data = default(T);
                            }
                            else {
                                if (dele != null) {
                                    if (item is object[] objects) {
                                        item = dele.DynamicInvoke(objects);
                                    }
                                    else {
                                        item = dele.DynamicInvoke(item);
                                    }
                                }
                                data = (T)item;
                            }
                            index++;
                        }
                        else {
                            index++;
                            continue;
                        }
                    }
                    catch (LightDataException ex) {
                        error = true;
                        exceptionMessage = ex.Message;
                        ProcessExceptionTransaction(transaction, trans);
                        throw ex;
                    }
                    catch (Exception ex) {
                        error = true;
                        exceptionMessage = ex.Message;
                        ProcessExceptionTransaction(transaction, trans);
                        throw new LightDataDbException(ex.Message, ex);
                    }
                    yield return data;
                }

                reader.Close();
                reader = null;
                if (commit) {
                    transaction.Commit();
                }
            }
            finally {
                if (error) {
                    dbcommand.Cancel();
                }
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand<T>(nameof(QueryDataDefineList), dbcommand, level, trans, start, size, startTime, endTime, success, count, exceptionMessage);
            }
        }

        internal T QueryDataDefineSingle<T>(IDataDefine source, SafeLevel level, DbCommand dbcommand, int start, object state, Delegate dele, TransactionConnection transaction = null)
        {
            CheckStatus();
            if (start < 0)
                start = 0;

            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            IDataReader reader = null;
            T obj = default(T);
            try {
                if (!transaction.IsOpen) {
                    transaction.Open();
                }
                transaction.SetupCommand(dbcommand);
                reader = dbcommand.ExecuteReader();
                success = true;

                bool flat = false;

                while (reader.Read()) {
                    if (flat) {
                        dbcommand.Cancel();
                        break;
                    }
                    if (index >= start) {
                        count++;
                        object item = source.LoadData(this, reader, state);
                        if (item != null) {
                            if (dele != null) {
                                if (item is object[] objects) {
                                    item = dele.DynamicInvoke(objects);
                                }
                                else {
                                    item = dele.DynamicInvoke(item);
                                }
                            }
                        }
                        obj = (T)item;
                        flat = true;
                    }
                    index++;
                }

                reader.Close();
                reader = null;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand<T>(nameof(QueryDataDefineSingle), dbcommand, level, trans, start, 1, startTime, endTime, success, count, exceptionMessage);
            }

            return obj;
        }


        internal List<T> QueryDataDefineList<T>(IDataDefine source, SafeLevel level, DbCommand dbcommand, Region region, object state, Delegate dele, TransactionConnection transaction = null)
        {
            CheckStatus();
            int start;
            int size;
            if (region != null) {
                start = region.Start;
                size = region.Size;
            }
            else {
                start = 0;
                size = int.MaxValue;
            }
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            IDataReader reader = null;
            List<T> list = new List<T>();
            try {
                if (!transaction.IsOpen) {
                    transaction.Open();
                }
                transaction.SetupCommand(dbcommand);
                reader = dbcommand.ExecuteReader();
                success = true;

                while (reader.Read()) {
                    if (count >= size) {
                        dbcommand.Cancel();
                        break;
                    }
                    if (index >= start) {
                        count++;
                        object item = source.LoadData(this, reader, state);
                        if (item == null) {
                            list.Add(default(T));
                        }
                        else {
                            if (dele != null) {
                                if (item is object[] objects) {
                                    item = dele.DynamicInvoke(objects);
                                }
                                else {
                                    item = dele.DynamicInvoke(item);
                                }
                            }
                            list.Add((T)item);
                        }
                    }
                    index++;
                }

                reader.Close();
                reader = null;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(QueryDataDefineList), dbcommand, level, trans, start, size, startTime, endTime, success, count, exceptionMessage);
            }

            return list;
        }

        internal async Task<T> QueryDataDefineSingleAsync<T>(IDataDefine source, SafeLevel level, DbCommand dbcommand, int start, object state, Delegate dele, CancellationToken cancellationToken, TransactionConnection transaction = null)
        {
            CheckStatus();
            if (start < 0)
                start = 0;

            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            IDataReader reader = null;
            T obj = default(T);
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                reader = await dbcommand.ExecuteReaderAsync(cancellationToken);
                success = true;

                bool flat = false;

                while (reader.Read()) {
                    if (flat) {
                        dbcommand.Cancel();
                        break;
                    }
                    if (index >= start) {
                        count++;
                        object item = source.LoadData(this, reader, state);
                        if (item != null) {
                            if (dele != null) {
                                if (item is object[] objects) {
                                    item = dele.DynamicInvoke(objects);
                                }
                                else {
                                    item = dele.DynamicInvoke(item);
                                }
                            }
                        }
                        obj = (T)item;
                        flat = true;
                    }
                    index++;
                }

                reader.Close();
                reader = null;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand<T>(nameof(QueryDataDefineSingleAsync), dbcommand, level, trans, start, 1, startTime, endTime, success, count, exceptionMessage);
            }

            return obj;
        }

        internal async Task<List<T>> QueryDataDefineListAsync<T>(IDataDefine source, SafeLevel level, DbCommand dbcommand, Region region, object state, Delegate dele, CancellationToken cancellationToken, TransactionConnection transaction = null)
        {
            CheckStatus();
            int start;
            int size;
            if (region != null) {
                start = region.Start;
                size = region.Size;
            }
            else {
                start = 0;
                size = int.MaxValue;
            }
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            IDataReader reader = null;
            List<T> list = new List<T>();
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                reader = await dbcommand.ExecuteReaderAsync(cancellationToken);
                success = true;

                while (reader.Read()) {
                    if (count >= size) {
                        dbcommand.Cancel();
                        break;
                    }
                    if (index >= start) {
                        count++;
                        object item = source.LoadData(this, reader, state);
                        if (item == null) {
                            list.Add(default(T));
                        }
                        else {
                            if (dele != null) {
                                if (item is object[] objects) {
                                    item = dele.DynamicInvoke(objects);
                                }
                                else {
                                    item = dele.DynamicInvoke(item);
                                }
                            }
                            list.Add((T)item);
                        }
                    }
                    index++;
                }

                reader.Close();
                reader = null;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (reader != null) {
                    reader.Close();
                    reader = null;
                }
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand<T>(nameof(QueryDataDefineListAsync), dbcommand, level, trans, start, size, startTime, endTime, success, count, exceptionMessage);
            }

            return list;
        }

        internal DataSet QueryDataSet(SafeLevel level, DbCommand dbcommand, TransactionConnection transaction = null)
        {
            CheckStatus();
            transaction = BuildTransaction(level, transaction, out bool commit, out bool trans);

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            int index = 0;
            int count = 0;
            DataSet dataSet = new DataSet();
            try {
                if (!transaction.IsOpen) {
                    transaction.Open();
                }
                transaction.SetupCommand(dbcommand);
                DataAdapter adapter = this._database.CreateDataAdapter(dbcommand);
                adapter.Fill(dataSet);
                success = true;
                if (dataSet.Tables.Count > 0) {
                    count = dataSet.Tables[0].Rows.Count;
                }
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (LightDataException ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw ex;
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                ProcessExceptionTransaction(transaction, trans);
                throw new LightDataDbException(ex.Message, ex);
            }
            finally {
                if (commit && !transaction.IsDisposed) {
                    transaction.Dispose();
                }
                DateTime endTime = DateTime.Now;
                OutputCommand(nameof(QueryDataSet), dbcommand, level, trans, index, 0, startTime, endTime, success, count, exceptionMessage);
            }

            return dataSet;
        }

        #endregion

        #region 基础方法

        TransactionConnection CreateTransactionConnection(SafeLevel level)
        {
            DbConnection connection = _database.CreateConnection(_connectionString);
            return new TransactionConnection(connection, level);
        }

        DbConnection CreateDbConnection()
        {
            DbConnection connection = _database.CreateConnection(_connectionString);
            return connection;
        }

        #endregion

        #region SQL执行器

        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="param">Parameter.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString, DataParameter[] param, SafeLevel level)
        {
            SqlExecutor executor = new SqlExecutor(sqlString, param, CommandType.Text, level, this);
            return executor;
        }

        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="param">Parameter.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString, DataParameter[] param)
        {
            return CreateSqlStringExecutor(sqlString, param, SafeLevel.Default);
        }

        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Value.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString, object value, SafeLevel level)
        {
            TextFormatter textFormatter = new TextFormatter(sqlString, TextTemplateOptions.Compiled | TextTemplateOptions.NotAllowExtend);
            string sql = textFormatter.FormatSql(value, this.ParameterPrefix, out DataParameter[] parameters);
            return CreateSqlStringExecutor(sql, parameters, level);
        }

        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Value.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString, object value)
        {
            return CreateSqlStringExecutor(sqlString, value, SafeLevel.Default);
        }


        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString, SafeLevel level)
        {
            return CreateSqlStringExecutor(sqlString, null, level);
        }

        /// <summary>
        /// Creates the sql string executor.
        /// </summary>
        /// <returns>The sql string executor.</returns>
        /// <param name="sqlString">Sql string.</param>
        public SqlExecutor CreateSqlStringExecutor(string sqlString)
        {
            return CreateSqlStringExecutor(sqlString, null, SafeLevel.Default);
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        /// <param name="param">Parameter.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure, DataParameter[] param, SafeLevel level)
        {
            SqlExecutor executor = new SqlExecutor(storeProcedure, param, CommandType.StoredProcedure, level, this);
            return executor;
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        /// <param name="param">Parameter.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure, DataParameter[] param)
        {
            return CreateStoreProcedureExecutor(storeProcedure, param, SafeLevel.Default);
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        /// <param name="value">Value.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure, object value, SafeLevel level)
        {
            DataParameter[] parameters = ParameterConvert.ConvertParameter(value);
            return CreateStoreProcedureExecutor(storeProcedure, parameters, level);
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        /// <param name="value">Value.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure, object value)
        {
            return CreateStoreProcedureExecutor(storeProcedure, value, SafeLevel.Default);
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        /// <param name="level">Level.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure, SafeLevel level)
        {
            return CreateStoreProcedureExecutor(storeProcedure, null, level);
        }

        /// <summary>
        /// Creates the store procedure executor.
        /// </summary>
        /// <returns>The store procedure executor.</returns>
        /// <param name="storeProcedure">Store procedure.</param>
        public SqlExecutor CreateStoreProcedureExecutor(string storeProcedure)
        {
            return CreateStoreProcedureExecutor(storeProcedure, null, SafeLevel.Default);
        }

        #endregion

        #region single relate

        internal List<T> QueryCollectionRelateData<T>(QueryExpression query, object owner, string[] fieldPaths)
        {
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            RelationMap relationMap = mapping.GetRelationMap();
            ISelector selector = relationMap.CreateExceptSelector(fieldPaths);
            CommandData commandData;
            CreateSqlState state = new CreateSqlState(this);
            if (mapping.HasJoinRelateModel) {
                List<IJoinModel> models = relationMap.CreateJoinModels(query, null);
                commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, null, null, false, null, state);
            }
            else {
                commandData = _database.Factory.CreateSelectCommand(mapping, selector, query, null, false, null, state);
            }
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetRelationMap(relationMap);
                queryState.SetSelector(selector);
                if (fieldPaths != null && fieldPaths.Length > 0) {
                    foreach (string fieldPath in fieldPaths) {
                        queryState.SetExtendData(fieldPath, owner);
                    }
                }
                return QueryDataDefineList<T>(mapping, SafeLevel.Default, command, null, queryState, null);
            }
        }

        #endregion

        internal DbCommand CreateCommand(string sql)
        {
            return _database.CreateCommand(sql);
        }

        internal IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, CommandType commandType)
        {
            return _database.CreateParameter(name, value, dbType, direction, null, commandType);
        }

        //internal void FormatStoredProcedureParameter(IDataParameter dataParameter)
        //{
        //    _database.FormatStoredProcedureParameter(dataParameter);
        //}

        #region IDisposable Support
        private bool _isDisposed; // To detect redundant calls

        /// <summary>
        /// Check the context is disposed
        /// </summary>
        public bool IsDisposed {
            get {
                return _isDisposed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed) {
                if (disposing) {
                    // dispose managed state (managed objects).
                    if (_transaction != null) {
                        if (_transaction.IsOpen) {
                            _transaction.Rollback();
                        }
                        _transaction.Dispose();
                        _transaction = null;
                        _transguid = null;
                    }
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                _isDisposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~DataContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Check the context is in the transaction mode.
        /// </summary>
        public bool IsTransactionMode {
            get {
                return _transaction != null;
            }
        }

        TransactionConnection _transaction;

        Guid? _transguid = null;

        bool _autoRelease;

        /// <summary>
        /// Begin the transaction and set default level and not auto close.
        /// </summary>
        public TransactionScope BeginTrans()
        {
            return BeginTrans(SafeLevel.Default, false);
        }

        /// <summary>
        /// Begin the transaction and set not auto close.
        /// </summary>
        /// <param name="level">Level.</param>
        public TransactionScope BeginTrans(SafeLevel level)
        {
            return BeginTrans(level, false);
        }

        /// <summary>
        /// Begin the transaction and set default level.
        /// </summary>
        /// <param name="autoRelease">Auto release.</param>
        public TransactionScope BeginTrans(bool autoRelease)
        {
            return BeginTrans(SafeLevel.Default, autoRelease);
        }

        /// <summary>
        /// Begin the transaction.
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="autoRelease">Auto release.</param>
        public TransactionScope BeginTrans(SafeLevel level, bool autoRelease)
        {
            CheckStatus();
            if (_transaction != null) {
                throw new LightDataException(SR.TransactionHasBegun);
            }
            else {
                if (level == SafeLevel.None) {
                    _transaction = CreateTransactionConnection(SafeLevel.Default);
                }
                else {
                    _transaction = CreateTransactionConnection(level);
                }
                _transguid = Guid.NewGuid();
                _autoRelease = autoRelease;
                TransactionScope scope = new TransactionScope(this, _transguid.Value);
                return scope;
            }
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        public bool CommitTrans()
        {
            CheckStatus();
            if (_transaction != null) {
                if (_transaction.IsDisposed) {
                    throw new LightDataException(SR.TransactionHasClosed);
                }
                if (_transaction.IsOpen) {
                    _transaction.Commit();
                    _transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                    return true;
                }
                else {
                    _transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                    return false;
                }
            }
            else {
                throw new LightDataException(SR.TransactionNotBegin);
            }
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public bool RollbackTrans()
        {
            CheckStatus();
            if (_transaction != null) {
                if (_transaction.IsDisposed) {
                    throw new LightDataException(SR.TransactionHasClosed);
                }
                if (_transaction.IsOpen) {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                    return true;
                }
                else {
                    _transaction.Dispose();
                    if (_autoRelease) {
                        _transaction = null;
                        _transguid = null;
                    }
                    return false;
                }
            }
            else {
                throw new LightDataException(SR.TransactionNotBegin);
            }
        }

        /// <summary>
        /// Release the transaction
        /// </summary>
        public void ReleaseTrans()
        {
            if (_transaction != null) {
                if (_transaction.ExecuteFlag) {
                    _transaction.Rollback();
                }
                if (!_transaction.IsDisposed) {
                    _transaction.Dispose();
                }
                _transaction = null;
                _transguid = null;
            }
        }

        internal void ScopeCloseTrans(Guid transguid)
        {
            if (_transguid == null || _transguid.Value != transguid)
                return;

            if (_transaction != null) {
                if (_transaction.ExecuteFlag) {
                    _transaction.Rollback();
                }
                if (!_transaction.IsDisposed) {
                    _transaction.Dispose();
                }
                _transaction = null;
                _transguid = null;
            }
        }

        internal bool ScopeCheckTrans(Guid transguid)
        {
            if (_transguid == null || _transguid.Value != transguid)
                return false;
            return true;
        }

        TransactionConnection CreateInnerTransaction(SafeLevel level)
        {
            if (_transaction != null) {
                return null;
            }
            else {
                if (level == SafeLevel.None) {
                    return CreateTransactionConnection(SafeLevel.Default);
                }
                else {
                    return CreateTransactionConnection(level);
                }
            }
        }

        void CommitInnerTransaction(TransactionConnection transaction)
        {
            if (transaction != null) {
                if (transaction.IsOpen) {
                    transaction.Commit();
                    transaction.Dispose();
                }
                else {
                    transaction.Dispose();
                }
            }
        }

        TransactionConnection BuildTransaction(SafeLevel level, TransactionConnection transaction, out bool commit, out bool trans)
        {
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                    trans = false;
                }
            }
            else {
                commit = false;
                trans = false;
            }
            return transaction;
        }

        void CheckStatus()
        {
            if (this._isDisposed) {
                throw new ObjectDisposedException(nameof(DataContext));
            }
        }


        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <returns></returns>
        public List<T> QuerySqlList<T>(string sqlString)
        {
            return QuerySqlList<T>(sqlString, null);
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public List<T> QuerySqlList<T>(string sqlString, object value)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return sqlExecutor.QueryList<T>();
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<T> QuerySqlList<T>(string sqlString, int start, int size)
        {
            return QuerySqlList<T>(sqlString, null, start, size);
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Parameters object</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<T> QuerySqlList<T>(string sqlString, object value, int start, int size)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return sqlExecutor.QueryList<T>(start, size);
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <returns></returns>
        public async Task<List<T>> QuerySqlListAsync<T>(string sqlString)
        {
            return await QuerySqlListAsync<T>(sqlString, null);
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public async Task<List<T>> QuerySqlListAsync<T>(string sqlString, object value)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return await sqlExecutor.QueryListAsync<T>();
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public async Task<List<T>> QuerySqlListAsync<T>(string sqlString, int start, int size)
        {
            return await QuerySqlListAsync<T>(sqlString, null, start, size);
        }

        /// <summary>
        /// Query data list with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Parameters object</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public async Task<List<T>> QuerySqlListAsync<T>(string sqlString, object value, int start, int size)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return await sqlExecutor.QueryListAsync<T>(start, size);
        }

        /// <summary>
        /// Query data first item with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <returns></returns>
        public T QuerySqlFirst<T>(string sqlString)
        {
            return QuerySqlFirst<T>(sqlString, null);
        }

        /// <summary>
        /// Query data first item with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public T QuerySqlFirst<T>(string sqlString, object value)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return sqlExecutor.QueryFirst<T>();
        }

        /// <summary>
        /// Query data first item with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<T> QuerySqlFirstAsync<T>(string sqlString, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QuerySqlFirstAsync<T>(sqlString, null, cancellationToken);
        }

        /// <summary>
        /// Query data first item with direct sql string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<T> QuerySqlFirstAsync<T>(string sqlString, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return await sqlExecutor.QueryFirstAsync<T>();
        }

        /// <summary>
        /// Execute NonQuery with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <returns></returns>
        public int ExecuteNonQuerySqlString(string sqlString)
        {
            return ExecuteNonQuerySqlString(sqlString, null);
        }

        /// <summary>
        /// Execute NonQuery with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public int ExecuteNonQuerySqlString(string sqlString, object value)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return sqlExecutor.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute NonQuery with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQuerySqlStringAsync(string sqlString, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteNonQuerySqlStringAsync(sqlString, null, cancellationToken);
        }

        /// <summary>
        /// Execute NonQuery with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQuerySqlStringAsync(string sqlString, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return await sqlExecutor.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Execute Scalar with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <returns></returns>
        public object ExecuteScalarSqlString(string sqlString)
        {
            return ExecuteScalarSqlString(sqlString, null);
        }

        /// <summary>
        /// Execute Scalar with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public object ExecuteScalarSqlString(string sqlString, object value)
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return sqlExecutor.ExecuteScalar();
        }

        /// <summary>
        /// Execute Scalar with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarSqlStringAsync(string sqlString, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteScalarSqlStringAsync(sqlString, null, cancellationToken);
        }

        /// <summary>
        /// Execute Scalar with direct sql string
        /// </summary>
        /// <param name="sqlString">Sql string.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarSqlStringAsync(string sqlString, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateSqlStringExecutor(sqlString, value);
            return await sqlExecutor.ExecuteScalarAsync();
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <returns></returns>
        public List<T> QueryStoreProcedureList<T>(string storeProcedure)
        {
            return QueryStoreProcedureList<T>(storeProcedure, null);
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public List<T> QueryStoreProcedureList<T>(string storeProcedure, object value)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return sqlExecutor.QueryList<T>();
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<T> QueryStoreProcedureList<T>(string storeProcedure, int start, int size)
        {
            return QueryStoreProcedureList<T>(storeProcedure, null, start, size);
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Parameters object</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public List<T> QueryStoreProcedureList<T>(string storeProcedure, object value, int start, int size)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return sqlExecutor.QueryList<T>(start, size);
        }


        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <returns></returns>
        public async Task<List<T>> QueryStoreProcedureListAsync<T>(string storeProcedure)
        {
            return await QueryStoreProcedureListAsync<T>(storeProcedure, null);
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public async Task<List<T>> QueryStoreProcedureListAsync<T>(string storeProcedure, object value)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return await sqlExecutor.QueryListAsync<T>();
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public async Task<List<T>> QueryStoreProcedureListAsync<T>(string storeProcedure, int start, int size)
        {
            return await QueryStoreProcedureListAsync<T>(storeProcedure, null, start, size);
        }

        /// <summary>
        /// Query data list with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Parameters object</param>
        /// <param name="start">Page start</param>
        /// <param name="size">Page size</param>
        /// <returns></returns>
        public async Task<List<T>> QueryStoreProcedureListAsync<T>(string storeProcedure, object value, int start, int size)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return await sqlExecutor.QueryListAsync<T>(start, size);
        }


        /// <summary>
        /// Query data first item with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <returns></returns>
        public T QueryStoreProcedureFirst<T>(string storeProcedure)
        {
            return QueryStoreProcedureFirst<T>(storeProcedure, null);
        }

        /// <summary>
        /// Query data first item with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public T QueryStoreProcedureFirst<T>(string storeProcedure, object value)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return sqlExecutor.QueryFirst<T>();
        }

        /// <summary>
        /// Query data first item with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<T> QueryStoreProcedureFirstAsync<T>(string storeProcedure, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryStoreProcedureFirstAsync<T>(storeProcedure, null, cancellationToken);
        }

        /// <summary>
        /// Query data first item with store procedure
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<T> QueryStoreProcedureFirstAsync<T>(string storeProcedure, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return await sqlExecutor.QueryFirstAsync<T>();
        }

        /// <summary>
        /// Execute NonQuery with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <returns></returns>
        public int ExecuteNonQueryStoreProcedure(string storeProcedure)
        {
            return ExecuteNonQueryStoreProcedure(storeProcedure, null);
        }

        /// <summary>
        /// Execute NonQuery with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public int ExecuteNonQueryStoreProcedure(string storeProcedure, object value)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return sqlExecutor.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute NonQuery with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryStoreProcedureAsync(string storeProcedure, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteNonQueryStoreProcedureAsync(storeProcedure, null, cancellationToken);
        }

        /// <summary>
        /// Execute NonQuery with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryStoreProcedureAsync(string storeProcedure, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return await sqlExecutor.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Execute Scalar with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <returns></returns>
        public object ExecuteScalarStoreProcedure(string storeProcedure)
        {
            return ExecuteScalarStoreProcedure(storeProcedure, null);
        }

        /// <summary>
        /// Execute Scalar with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <returns></returns>
        public object ExecuteScalarStoreProcedure(string storeProcedure, object value)
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return sqlExecutor.ExecuteScalar();
        }

        /// <summary>
        /// Execute Scalar with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarStoreProcedureAsync(string storeProcedure, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ExecuteScalarStoreProcedureAsync(storeProcedure, null, cancellationToken);
        }

        /// <summary>
        /// Execute Scalar with store procedure
        /// </summary>
        /// <param name="storeProcedure">Store Procedure name.</param>
        /// <param name="value">Paratemers object</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarStoreProcedureAsync(string storeProcedure, object value, CancellationToken cancellationToken = default(CancellationToken))
        {
            SqlExecutor sqlExecutor = CreateStoreProcedureExecutor(storeProcedure, value);
            return await sqlExecutor.ExecuteScalarAsync();
        }
    }
}