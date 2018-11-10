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
        /// Gets the name of the mapping table.
        /// </summary>
        /// <returns>The table name.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static string GetTableName<T>()
        {
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            return mapping.TableName;
        }

        ///// <summary>
        ///// The connection string.
        ///// </summary>
        private string _connectionString;

        private DatabaseProvider _database;

        private DataContextOptions _options;

        private Dictionary<DataEntityMapping, string> _aliasTableDict;

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

        public bool ResetAliasTableName<T>()
        {
            if (_aliasTableDict == null) {
                return false;
            }
            DataEntityMapping mapping = DataEntityMapping.GetEntityMapping(typeof(T));
            return _aliasTableDict.Remove(mapping);
        }

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

        internal DataContextOptions Options {
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
        protected ICommandOutput _output;

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
        /// <param name="setting">Setting.</param>
        public DataContext(IConnectionSetting setting)
        {
            if (setting == null)
                throw new ArgumentNullException(nameof(setting));
            DataContextOptions options = DataContextOptions.CreateOptions(setting);
            Internal_DataContext(options);
        }

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
        /// Creates the new object.
        /// </summary>
        /// <returns>The new.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
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
        /// <param name="refresh">is refresh null data field</param>
        /// <returns></returns>
        public int InsertOrUpdate<T>(T data, bool refresh)
        {
            return InsertOrUpdate(data, SafeLevel.Default, refresh);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data</param>
        /// <param name="level">safe level</param>
        /// <returns></returns>
        public int InsertOrUpdate<T>(T data, SafeLevel level)
        {
            return InsertOrUpdate(data, level, false);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Data</param>
        /// <param name="level">safe level</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <returns></returns>
        public int InsertOrUpdate<T>(T data, SafeLevel level, bool refresh)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return InsertOrUpdate(mapping, data, level, refresh);
        }

        internal int InsertOrUpdate(DataTableEntityMapping mapping, object data, SafeLevel level, bool refresh)
        {
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
                        QueryCommand insertCommand = _database.Insert(this, mapping, data, refresh);
                        rInt = ExecuteNonQuery(insertCommand.Command, SafeLevel.Default, transaction);
                        if (mapping.HasIdentity && rInt > 0) {
                            QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                            object id = ExecuteScalar(identityCommand.Command, SafeLevel.Default, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
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
        public async Task<int> InsertOrUpdateAsync<T>(T data, CancellationToken cancellationToken)
        {
            return await InsertOrUpdateAsync(data, SafeLevel.Default, false, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="level">safe level</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, SafeLevel level, CancellationToken cancellationToken)
        {
            return await InsertOrUpdateAsync(data, level, false, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, bool refresh, CancellationToken cancellationToken)
        {
            return await InsertOrUpdateAsync(data, SafeLevel.Default, refresh, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="level">safe level</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, SafeLevel level, bool refresh, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return await InsertOrUpdateAsync(mapping, data, level, refresh, cancellationToken);
        }

        internal async Task<int> InsertOrUpdateAsync(DataTableEntityMapping mapping, object data, SafeLevel level, bool refresh, CancellationToken cancellationToken)
        {
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
                        QueryCommand insertCommand = _database.Insert(this, mapping, data, refresh);
                        rInt = await ExecuteNonQueryAsync(insertCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                        if (mapping.HasIdentity && rInt > 0) {
                            QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                            object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            _database.UpdateDataIdentity(mapping, data, id);
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
        /// <param name="refresh">Is refresh data field</param>
        public int Insert<T>(T data, bool refresh)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return Insert(mapping, data, refresh);
        }

        internal int Insert(DataTableEntityMapping mapping, object data, bool refresh)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh);
            int rInt;
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
        public async Task<int> InsertAsync<T>(T data, CancellationToken cancellationToken)
        {
            return await InsertAsync(data, false, cancellationToken);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertAsync<T>(T data, bool refresh, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return await InsertAsync(mapping, data, refresh, cancellationToken);
        }

        internal async Task<int> InsertAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            QueryCommand queryCommand = _database.Insert(this, mapping, data, refresh);
            int rInt;
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
            if (mapping.IsDataTableEntity) {
                UpdateDateTableEntity(mapping, data);
            }
            return rInt;//return rInt;
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
        /// <param name="refresh">Is refresh data field</param>
        public int Update<T>(T data, bool refresh)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return Update(mapping, data, refresh);
        }

        internal int Update(DataTableEntityMapping mapping, object data, bool refresh)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
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
        public async Task<int> UpdateAsync<T>(T data, CancellationToken cancellationToken)
        {
            return await UpdateAsync(data, false, cancellationToken);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">Is refresh data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> UpdateAsync<T>(T data, bool refresh, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return await UpdateAsync(mapping, data, refresh, cancellationToken);
        }

        internal async Task<int> UpdateAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return Delete(mapping, data);
        }

        internal int Delete(DataTableEntityMapping mapping, object data)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
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
        public async Task<int> DeleteAsync<T>(T data, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(data.GetType());
            return await DeleteAsync(mapping, data, cancellationToken);
        }

        internal async Task<int> DeleteAsync(DataTableEntityMapping mapping, object data, CancellationToken cancellationToken)
        {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
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
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas)
        {
            return BatchInsert(datas, false, true);
        }

        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="updateIdentity">is update data identity field</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, bool refresh, bool updateIdentity)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchInsert(mapping, list, refresh, updateIdentity);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, int index, int count)
        {
            return BatchInsert(datas, index, count, false, true);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, int index, int count, bool refresh, bool updateIdentity)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchInsert(mapping, list, refresh, updateIdentity);
        }

        internal int BatchInsert(DataTableEntityMapping mapping, IList datas, bool refresh, bool updateIdentity)
        {
            int result = 0;
            if (mapping.HasIdentity && updateIdentity) {
                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                QueryCommand[] queryCommands = new QueryCommand[datas.Count];
                for (int i = 0; i < datas.Count; i++) {
                    queryCommands[i] = _database.Insert(this, mapping, datas[i], refresh);
                }
                TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                try {
                    for (int j = 0; j < queryCommands.Length; j++) {
                        int rInt = ExecuteNonQuery(queryCommands[j].Command, SafeLevel.Default, transaction);
                        if (rInt > 0) {
                            object id = ExecuteScalar(identityCommand.Command, SafeLevel.Default, transaction);
                            _database.UpdateDataIdentity(mapping, datas[j], id);
                        }
                        result += rInt;
                    }
                }
                finally {
                    CommitInnerTransaction(transaction);
                }
            }
            else {
                QueryCommands queryCommands = _database.BatchInsert(this, mapping, datas, refresh);
                TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                try {
                    foreach (DbCommand command in queryCommands.Commands) {
                        int rInt = ExecuteNonQuery(command, SafeLevel.Default, transaction);
                        result += rInt;
                    }
                }
                finally {
                    CommitInnerTransaction(transaction);
                }
            }

            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            return await BatchInsertAsync(datas, false, true, cancellationToken);
        }

        /// <summary>
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="updateIdentity">is update data identity field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, bool refresh, bool updateIdentity, CancellationToken cancellationToken)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchInsertAsync(mapping, list, refresh, updateIdentity, cancellationToken);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await BatchInsertAsync(datas, index, count, false, true, cancellationToken);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="updateIdentity">is update data identity field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int count, bool refresh, bool updateIdentity, CancellationToken cancellationToken)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchInsertAsync(mapping, list, refresh, updateIdentity, cancellationToken);
        }

        internal async Task<int> BatchInsertAsync(DataTableEntityMapping mapping, IList datas, bool refresh, bool updateIdentity, CancellationToken cancellationToken)
        {
            int result = 0;
            if (mapping.HasIdentity && updateIdentity) {
                QueryCommand identityCommand = _database.InsertIdentiy(this, mapping);
                QueryCommand[] queryCommands = new QueryCommand[datas.Count];
                for (int i = 0; i < datas.Count; i++) {
                    queryCommands[i] = _database.Insert(this, mapping, datas[i], refresh);
                }
                TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                try {
                    for (int j = 0; j < queryCommands.Length; j++) {
                        int rInt = await ExecuteNonQueryAsync(queryCommands[j].Command, SafeLevel.Default, cancellationToken, transaction);
                        if (rInt > 0) {
                            object id = await ExecuteScalarAsync(identityCommand.Command, SafeLevel.Default, cancellationToken, transaction);
                            _database.UpdateDataIdentity(mapping, datas[j], id);
                        }
                        result += rInt;
                    }
                }
                finally {
                    CommitInnerTransaction(transaction);
                }
            }
            else {
                QueryCommands queryCommands = _database.BatchInsert(this, mapping, datas, refresh);
                TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
                try {
                    foreach (DbCommand command in queryCommands.Commands) {
                        int rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken, transaction);
                        result += rInt;
                    }
                }
                finally {
                    CommitInnerTransaction(transaction);
                }
            }

            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas)
        {
            return BatchUpdate(datas, false);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, bool refresh)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchUpdate(mapping, list, refresh);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, int index, int count)
        {
            return BatchUpdate(datas, index, count, false);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchUpdate<T>(IEnumerable<T> datas, int index, int count, bool refresh)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchUpdate(mapping, list, refresh);
        }

        internal int BatchUpdate(DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            int result = 0;
            QueryCommands queryCommands = _database.BatchUpdate(this, mapping, datas, refresh);
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            try {
                foreach (DbCommand command in queryCommands.Commands) {
                    int rInt = ExecuteNonQuery(command, SafeLevel.Default, transaction);
                    result += rInt;
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            return await BatchUpdateAsync(datas, false, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, bool refresh, CancellationToken cancellationToken)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchUpdateAsync(mapping, list, refresh, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await BatchUpdateAsync(datas, index, count, false, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, int index, int count, bool refresh, CancellationToken cancellationToken)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchUpdateAsync(mapping, list, refresh, cancellationToken);
        }

        internal async Task<int> BatchUpdateAsync(DataTableEntityMapping mapping, IList datas, bool refresh, CancellationToken cancellationToken)
        {
            int result = 0;
            QueryCommands queryCommands = _database.BatchUpdate(this, mapping, datas, refresh);
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            try {
                foreach (DbCommand command in queryCommands.Commands) {
                    int rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken, transaction);
                    result += rInt;
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Batchs delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchDelete<T>(IEnumerable<T> datas)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchDelete(mapping, list);
        }

        /// <summary>
        /// Batchs delete datas.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchDelete<T>(IEnumerable<T> datas, int index, int count)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return BatchDelete(mapping, list);
        }

        internal int BatchDelete(DataTableEntityMapping mapping, IList datas)
        {
            int result = 0;
            QueryCommands queryCommands = _database.BatchDelete(this, mapping, datas);
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            try {
                foreach (DbCommand command in queryCommands.Commands) {
                    int rInt = ExecuteNonQuery(command, SafeLevel.Default, transaction);
                    result += rInt;
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Batchs delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchDeleteAsync(mapping, list, cancellationToken);
        }

        /// <summary>
        /// Batchs delete datas.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
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

            List<T> list = new List<T>(count);
            int mindex = 0;
            int max = index + count;
            foreach (T item in datas) {
                if (mindex >= index && mindex < max) {
                    list.Add(item);
                }
                mindex++;
            }
            if (list.Count == 0) {
                return 0;
            }
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(list[0].GetType());
            return await BatchDeleteAsync(mapping, list, cancellationToken);
        }

        internal async Task<int> BatchDeleteAsync(DataTableEntityMapping mapping, IList datas, CancellationToken cancellationToken)
        {
            int result = 0;
            QueryCommands queryCommands = _database.BatchDelete(this, mapping, datas);
            TransactionConnection transaction = CreateInnerTransaction(SafeLevel.Default);
            try {
                foreach (DbCommand command in queryCommands.Commands) {
                    int rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken, transaction);
                    result += rInt;
                }
            }
            finally {
                CommitInnerTransaction(transaction);
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    UpdateDateTableEntity(mapping, data);
                }
            }
            return result;
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectByKey<T>(params object[] primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectByKey(this, mapping, primaryKeys);
            return QueryDataDefineSingle<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null);
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object[] primaryKeys, CancellationToken cancellationToken)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectByKey(this, mapping, primaryKeys);
            return await QueryDataDefineSingleAsync<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey, CancellationToken cancellationToken)
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey }, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey1, object primaryKey2, CancellationToken cancellationToken)
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey1, primaryKey2 }, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken)
        {
            return await SelectByKeyAsync<T>(new object[] { primaryKey1, primaryKey2, primaryKey3 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
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
        /// Check exist the object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey, CancellationToken cancellationToken)
        {
            return await ExistsAsync<T>(new object[] { primaryKey }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey1, object primaryKey2, CancellationToken cancellationToken)
        {
            return await ExistsAsync<T>(new object[] { primaryKey1, primaryKey2 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool> ExistsAsync<T>(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken)
        {
            return await ExistsAsync<T>(new object[] { primaryKey1, primaryKey2, primaryKey3 }, cancellationToken);
        }

        /// <summary>
        /// Check exist the object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<bool> ExistsAsync<T>(object[] primaryKeys, CancellationToken cancellationToken)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.ExistsByKey(this, mapping, primaryKeys);
            DataDefine define = DataDefine.GetDefine(typeof(int?));
            int? obj = await QueryDataDefineSingleAsync<int?>(define, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
            return obj.HasValue;
        }

        public T SelectById<T>(object id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectById(this, mapping, id);
            return QueryDataDefineSingle<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null);
        }

        public async Task<T> SelectByIdAsync<T>(object id, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.SelectById(this, mapping, id);
            return await QueryDataDefineSingleAsync<T>(mapping, SafeLevel.Default, queryCommand.Command, 0, queryCommand.State, null, cancellationToken);
        }

        /// <summary>
        /// Create query expression.
        /// </summary>
        /// <returns>The ueryable.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IQuery<T> Query<T>()
        {
            return new LightQuery<T>(this);
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
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
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> TruncateTableAsync<T>(CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _database.TruncateTable(this, mapping);
            return await ExecuteNonQueryAsync(queryCommand.Command, SafeLevel.Default, cancellationToken);
        }


        #region 核心数据库方法


        /// <summary>
        /// Outputs the command.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="command">Command.</param>
        /// <param name="level">Level.</param>
        /// <param name="start">Start.</param>
        /// <param name="size">Size.</param>
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

        internal int ExecuteNonQuery(DbCommand dbcommand, SafeLevel level, TransactionConnection transaction = null)
        {
            CheckStatus();
            int rInt;
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                rInt = await dbcommand.ExecuteNonQueryAsync(CancellationToken.None);
                result = rInt;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

            DateTime startTime = DateTime.Now;
            bool success = false;
            string exceptionMessage = null;
            object result = null;
            try {
                if (!transaction.IsOpen) {
                    await transaction.OpenAsync(cancellationToken);
                }
                transaction.SetupCommand(dbcommand);
                resultObj = await dbcommand.ExecuteScalarAsync();
                result = resultObj;
                success = true;
                if (commit) {
                    transaction.Commit();
                }
            }
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
                catch (Exception ex) {
                    DateTime endTime = DateTime.Now;
                    exceptionMessage = ex.Message;
                    OutputCommand<T>(nameof(QueryDataDefineReader), dbcommand, level, trans, start, size, startTime, endTime, success, 0, exceptionMessage);
                    throw ex;
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
                    catch (Exception ex) {
                        error = true;
                        exceptionMessage = ex.Message;
                        if (trans) {
                            RollbackTrans(false);
                        }
                        else {
                            transaction.Rollback();
                            transaction.Dispose();
                        }
                        throw ex;
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

            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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

            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
                reader = await dbcommand.ExecuteReaderAsync();
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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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
            bool commit;
            bool trans = false;
            if (transaction == null) {
                if (_transaction != null) {
                    transaction = _transaction;
                    commit = false;
                    trans = true;
                }
                else {
                    transaction = CreateTransactionConnection(level);
                    commit = true;
                }
            }
            else {
                commit = false;
            }

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
            catch (Exception ex) {
                exceptionMessage = ex.Message;
                if (trans) {
                    RollbackTrans(false);
                }
                else {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                throw ex;
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

        internal IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction)
        {
            return _database.CreateParameter(name, value, dbType, direction, null);
        }

        internal void FormatStoredProcedureParameter(IDataParameter dataParameter)
        {
            _database.FormatStoredProcedureParameter(dataParameter);
        }

        #region IDisposable Support
        private bool _isDisposed; // To detect redundant calls

        public bool IsDisposed {
            get {
                return _isDisposed;
            }
        }

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
                    }
                }

                // free unmanaged resources (unmanaged objects) and override a finalizer below.
                // set large fields to null.

                _isDisposed = true;
            }
        }

        // override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        ~DataContext()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }
        #endregion


        public bool IsTransactionMode {
            get {
                return _transaction != null;
            }
        }

        TransactionConnection _transaction;

        bool _isCreateTrans;

        public TransactionScope CreateTransactionScope()
        {
            if (_isCreateTrans) {
                throw new LightDataException(SR.TransactionScopeHasBeenCreated);
            }
            _isCreateTrans = true;
            TransactionScope scope = new TransactionScope(this);
            return scope;
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="level">Level.</param>
        internal bool BeginTrans(SafeLevel level)
        {
            CheckStatus();
            if (_transaction != null) {
                return false;
            }
            else {
                if (level == SafeLevel.None) {
                    _transaction = CreateTransactionConnection(SafeLevel.Default);
                }
                else {
                    _transaction = CreateTransactionConnection(level);
                }
                return true;
            }
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        internal bool CommitTrans()
        {
            CheckStatus();
            if (_transaction != null) {
                if (_transaction.IsOpen) {
                    _transaction.Commit();
                    _transaction.Dispose();
                    _transaction = null;
                    return true;
                }
                else {
                    _transaction.Dispose();
                    _transaction = null;
                    return false;
                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        internal bool RollbackTrans(bool check)
        {
            if (check)
                CheckStatus();
            if (_transaction != null) {
                if (_transaction.IsOpen) {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    _transaction = null;
                    return true;
                }
                else {
                    _transaction.Dispose();
                    _transaction = null;
                    return false;
                }
            }
            else {
                return false;
            }
        }

        internal void CloseTrans(bool check)
        {
            if (check)
                CheckStatus();
            _isCreateTrans = false;
            if (_transaction != null) {
                if (_transaction.ExecuteFlag) {
                    _transaction.Rollback();
                }
                _transaction.Dispose();
                _transaction = null;
            }
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

        void CheckStatus()
        {
            if (this._isDisposed) {
                throw new ObjectDisposedException(nameof(DataContext));
            }
        }

    }
}




//internal int QueryDelete(DataTableEntityMapping mapping, QueryExpression query, SafeLevel level)
//{
//    int rInt;
//    CreateSqlState state = new CreateSqlState(this);
//    CommandData commandData = _database.Factory.CreateMassDeleteCommand(mapping, query, state);
//    using (DbCommand command = commandData.CreateCommand(_database, state)) {
//        rInt = ExecuteNonQuery(command, level);
//    }
//    return rInt;
//}

//internal async Task<int> QueryDeleteAsync(DataTableEntityMapping mapping, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
//{
//    int rInt;
//    CreateSqlState state = new CreateSqlState(this);
//    CommandData commandData = _database.Factory.CreateMassDeleteCommand(mapping, query, state);
//    using (DbCommand command = commandData.CreateCommand(_database, state)) {
//        rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
//    }
//    return rInt;
//}

//internal int QueryUpdate(DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query, SafeLevel level)
//{
//    int rInt;
//    CreateSqlState state = new CreateSqlState(this);
//    CommandData commandData = _database.Factory.CreateMassUpdateCommand(mapping, updator, query, state);
//    using (DbCommand command = commandData.CreateCommand(_database, state)) {
//        rInt = ExecuteNonQuery(command, level);
//    }
//    return rInt;
//}

//internal async Task<int> QueryUpdateAsync(DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
//{
//    int rInt;
//    CreateSqlState state = new CreateSqlState(this);
//    CommandData commandData = _database.Factory.CreateMassUpdateCommand(mapping, updator, query, state);
//    using (DbCommand command = commandData.CreateCommand(_database, state)) {
//        rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
//    }
//    return rInt;
//}