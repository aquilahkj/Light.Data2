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

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int InsertOrUpdate<T>(T data)
        {
            return InsertOrUpdate(data, false);
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return InsertOrUpdate(mapping, data, refresh);
        }

        internal int InsertOrUpdate(DataTableEntityMapping mapping, object data, bool refresh)
        {
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int i = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = new DataFieldInfo(fieldMapping);
                object value = fieldMapping.Handler.Get(data);
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, value);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                i++;
            }
            bool exists = Exists(mapping, queryExpression);
            int result;
            if (exists) {
                result = Update(mapping, data);
            }
            else {
                result = Insert(mapping, data, refresh);
            }
            return result;
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, CancellationToken cancellationToken)
        {
            return await InsertOrUpdateAsync(data, false, cancellationToken);
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await InsertOrUpdateAsync(mapping, data, refresh, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data)
        {
            return await InsertOrUpdateAsync(data, CancellationToken.None);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">is refresh null data field</param>
        public async Task<int> InsertOrUpdateAsync<T>(T data, bool refresh)
        {
            return await InsertOrUpdateAsync(data, refresh, CancellationToken.None);
        }

        internal async Task<int> InsertOrUpdateAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken)
        {
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int i = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = new DataFieldInfo(fieldMapping);
                object value = fieldMapping.Handler.Get(data);
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, value);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                i++;
            }
            bool exists = await ExistsAsync(mapping, queryExpression, cancellationToken);
            int result;
            if (exists) {
                result = await UpdateAsync(mapping, data, cancellationToken);
            }
            else {
                result = await InsertAsync(mapping, data, refresh, cancellationToken);
            }
            return result;
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

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public async Task<int> InsertAsync<T>(T data)
        {
            return await InsertAsync(data, CancellationToken.None);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="refresh">is refresh null data field</param>
        public async Task<int> InsertAsync<T>(T data, bool refresh)
        {
            return await InsertAsync(data, refresh, CancellationToken.None);
        }

        internal int Insert(DataTableEntityMapping mapping, object data, bool refresh)
        {
            object obj = null;
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateInsertCommand(mapping, data, refresh, state);
            CommandData commandDataIdentity = null;
            if (mapping.IdentityField != null) {
                CreateSqlState state1 = new CreateSqlState(this);
                commandDataIdentity = _database.Factory.CreateIdentityCommand(mapping, state1);
            }
            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, SafeLevel.Default, transaction);
            }
            if (commandDataIdentity != null) {
                using (DbCommand identityCommand = commandDataIdentity.CreateCommand(_database)) {
                    obj = ExecuteScalar(identityCommand, SafeLevel.Default, transaction);
                }
            }
            
            CommitInnerTransaction(transaction);
            if (!Equals(obj, null)) {
                object id = Convert.ChangeType(obj, mapping.IdentityField.ObjectType);
                mapping.IdentityField.Handler.Set(data, id);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                tableEntity.ClearUpdateFields();
            }
            return rInt;
        }

        internal async Task<int> InsertAsync(DataTableEntityMapping mapping, object data, bool refresh, CancellationToken cancellationToken)
        {
            object obj = null;
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateInsertCommand(mapping, data, refresh, state);
            CommandData commandDataIdentity = null;
            if (mapping.IdentityField != null) {
                CreateSqlState state1 = new CreateSqlState(this);
                commandDataIdentity = _database.Factory.CreateIdentityCommand(mapping, state1);
            }
            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken, transaction);
            }
            if (commandDataIdentity != null) {
                using (DbCommand identityCommand = commandDataIdentity.CreateCommand(_database)) {
                    obj = await ExecuteScalarAsync(identityCommand, SafeLevel.Default, cancellationToken, transaction);
                }
            }

            CommitInnerTransaction(transaction);
            if (!Equals(obj, null)) {
                object id = Convert.ChangeType(obj, mapping.IdentityField.ObjectType);
                mapping.IdentityField.Handler.Set(data, id);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                tableEntity.ClearUpdateFields();
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return Update(mapping, data);
        }

        internal int Update(DataTableEntityMapping mapping, object data)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateUpdateCommand(mapping, data, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, SafeLevel.Default);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                tableEntity.ClearUpdateFields();
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await UpdateAsync(mapping, data, cancellationToken);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public async Task<int> UpdateAsync<T>(T data)
        {
            return await UpdateAsync(data, CancellationToken.None);
        }

        internal async Task<int> UpdateAsync(DataTableEntityMapping mapping, object data, CancellationToken cancellationToken)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateUpdateCommand(mapping, data, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                tableEntity.ClearUpdateFields();
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return Delete(mapping, data);
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> DeleteAsync<T>(T data, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await DeleteAsync(mapping, data, cancellationToken);
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public async Task<int> DeleteAsync<T>(T data)
        {
            return await DeleteAsync(data, CancellationToken.None);
        }

        internal int Delete(DataTableEntityMapping mapping, object data)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateDeleteCommand(mapping, data, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, SafeLevel.Default);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.ClearRawPrimaryKeys();
                tableEntity.ClearUpdateFields();
            }
            return rInt;
        }

        internal async Task<int> DeleteAsync(DataTableEntityMapping mapping, object data, CancellationToken cancellationToken)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateDeleteCommand(mapping, data, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken);
            }
            if (mapping.IsDataTableEntity) {
                DataTableEntity tableEntity = data as DataTableEntity;
                tableEntity.ClearRawPrimaryKeys();
                tableEntity.ClearUpdateFields();
            }
            return rInt;
        }

        /// <summary>
        /// Creates the new object.
        /// </summary>
        /// <returns>The new.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T CreateNew<T>()
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            object obj = mapping.InitialData();
            if (mapping.IsDataEntity) {
                DataEntity entity = obj as DataEntity;
                entity.SetContext(this);
            }
            return (T)obj;
        }



        internal int Delete(DataTableEntityMapping mapping, QueryExpression query, SafeLevel level)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateDeleteMassCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> DeleteAsync(DataTableEntityMapping mapping, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateDeleteMassCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
            }
            return rInt;
        }

        internal int Update(DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query, SafeLevel level)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateUpdateMassCommand(mapping, updator, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> UpdateAsync(DataTableEntityMapping mapping, MassUpdator updator, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateUpdateMassCommand(mapping, updator, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
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
            return BatchInsert(datas, false);
        }

        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public int BatchInsert<T>(IEnumerable<T> datas, bool refresh)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return BatchInsert(mapping, list, refresh);
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
            return BatchInsert(datas, index, count, false);
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
        public int BatchInsert<T>(IEnumerable<T> datas, int index, int count, bool refresh)
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return BatchInsert(mapping, list, refresh);
        }

        internal int BatchInsert(DataTableEntityMapping mapping, IList datas, bool refresh)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchInsertCount;
            else
                batchCount = 10;
            int result = 0;
            object obj = null;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();

            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchInsertCommand(mapping, datas, start, batchCount, refresh, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            CommandData commandDataIdentity = null;
            if (mapping.IdentityField != null) {
                CreateSqlState state = new CreateSqlState(this);
                commandDataIdentity = _database.Factory.CreateIdentityCommand(mapping, state);
            }
            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = ExecuteNonQuery(dbcommand, SafeLevel.Default, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }
            if (commandDataIdentity != null) {
                using (DbCommand identityCommand = commandDataIdentity.CreateCommand(_database)) {
                    obj = ExecuteScalar(identityCommand, SafeLevel.Default, transaction);
                }
            }
 
            CommitInnerTransaction(transaction);
            if (!Equals(obj, null)) {
                object id = Convert.ChangeType(obj, mapping.IdentityField.ObjectType);
                int len = datas.Count;
                object[] ids = CreateObjectList(id, len);

                for (int i = 0; i < len; i++) {
                    object data = datas[i];
                    object value = ids[i];
                    mapping.IdentityField.Handler.Set(data, value);
                }
            }
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                    tableEntity.ClearUpdateFields();
                }
            }
            return result;
        }


        /// <summary>
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, bool refresh)
        {
            return await BatchInsertAsync(datas, refresh, CancellationToken.None);
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
            return await BatchInsertAsync(datas, false, cancellationToken);
        }

        /// <summary>
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, bool refresh, CancellationToken cancellationToken)
        {
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchInsertAsync(mapping, list, refresh, cancellationToken);
        }


        /// <summary>
        /// Batch insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas)
        {
            return await BatchInsertAsync(datas, CancellationToken.None);
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
            return await BatchInsertAsync(datas, index, count, false, cancellationToken);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="refresh">is refresh null data field</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int count, bool refresh, CancellationToken cancellationToken)
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchInsertAsync(mapping, list, refresh, cancellationToken);
        }

        /// <summary>
        /// Mass insert data.
        /// </summary>
        /// <returns>The insert rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchInsertAsync<T>(IEnumerable<T> datas, int index, int count)
        {
            return await BatchInsertAsync(datas, index, count, CancellationToken.None);
        }

        internal async Task<int> BatchInsertAsync(DataTableEntityMapping mapping, IList datas, bool refresh, CancellationToken cancellationToken)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchInsertCount;
            else
                batchCount = 10;
            int result = 0;
            object obj = null;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();
            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchInsertCommand(mapping, datas, start, batchCount, refresh, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            CommandData commandDataIdentity = null;
            if (mapping.IdentityField != null) {
                CreateSqlState state = new CreateSqlState(this);
                commandDataIdentity = _database.Factory.CreateIdentityCommand(mapping, state);
            }
            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = await ExecuteNonQueryAsync(dbcommand, SafeLevel.Default, cancellationToken, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }
            if (commandDataIdentity != null) {
                using (DbCommand identityCommand = commandDataIdentity.CreateCommand(_database)) {
                    obj = await ExecuteScalarAsync(identityCommand, SafeLevel.Default, cancellationToken, transaction);
                }
            }

            CommitInnerTransaction(transaction);
            if (!Equals(obj, null)) {
                object id = Convert.ChangeType(obj, mapping.IdentityField.ObjectType);
                int len = datas.Count;
                object[] ids = CreateObjectList(id, len);

                for (int i = 0; i < len; i++) {
                    object data = datas[i];
                    object value = ids[i];
                    mapping.IdentityField.Handler.Set(data, value);
                }
            }
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                    tableEntity.ClearUpdateFields();
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
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return BatchUpdate(mapping, list);
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return BatchUpdate(mapping, list);
        }

        internal int BatchUpdate(DataTableEntityMapping mapping, IList datas)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchUpdateCount;
            else
                batchCount = 10;
            int result = 0;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();
            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchUpdateCommand(mapping, datas, start, batchCount, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = ExecuteNonQuery(dbcommand, SafeLevel.Default, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }

            CommitInnerTransaction(transaction);
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                    tableEntity.ClearUpdateFields();
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
            if (datas == null) {
                throw new ArgumentNullException(nameof(datas));
            }

            List<T> list = new List<T>(datas);
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchUpdateAsync(mapping, list, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas)
        {
            return await BatchUpdateAsync(datas, CancellationToken.None);
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchUpdateAsync(mapping, list, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>The update rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchUpdateAsync<T>(IEnumerable<T> datas, int index, int count)
        {
            return await BatchUpdateAsync(datas, index, count, CancellationToken.None);
        }

        internal async Task<int> BatchUpdateAsync(DataTableEntityMapping mapping, IList datas, CancellationToken cancellationToken)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchUpdateCount;
            else
                batchCount = 10;
            int result = 0;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();
            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchUpdateCommand(mapping, datas, start, batchCount, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = await ExecuteNonQueryAsync(dbcommand, SafeLevel.Default, cancellationToken, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }

            CommitInnerTransaction(transaction);
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.SetRawPrimaryKeys(mapping.GetRawKeys(data));
                    tableEntity.ClearUpdateFields();
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return BatchDelete(mapping, list);
        }

        internal int BatchDelete(DataTableEntityMapping mapping, IList datas)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchDeleteCount;
            else
                batchCount = 10;
            int result = 0;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();
            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchDeleteCommand(mapping, datas, start, batchCount, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = ExecuteNonQuery(dbcommand, SafeLevel.Default, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }

            CommitInnerTransaction(transaction);
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.ClearRawPrimaryKeys();
                    tableEntity.ClearUpdateFields();
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchDeleteAsync(mapping, list, cancellationToken);
        }

        /// <summary>
        /// Batchs delete data.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas)
        {
            return await BatchDeleteAsync(datas, CancellationToken.None);
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
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await BatchDeleteAsync(mapping, list, cancellationToken);
        }

        /// <summary>
        /// Batchs delete datas.
        /// </summary>
        /// <returns>The delete rows.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> BatchDeleteAsync<T>(IEnumerable<T> datas, int index, int count)
        {
            return await BatchDeleteAsync(datas, index, count, CancellationToken.None);
        }

        internal async Task<int> BatchDeleteAsync(DataTableEntityMapping mapping, IList datas, CancellationToken cancellationToken)
        {
            if (datas.Count == 0) {
                return 0;
            }
            int batchCount;
            if (_database.BatchInsertCount > 0)
                batchCount = _database.BatchDeleteCount;
            else
                batchCount = 10;
            int result = 0;
            int start = 0;
            List<Tuple<CommandData, CreateSqlState>> commandDatas = new List<Tuple<CommandData, CreateSqlState>>();
            while (true) {
                CreateSqlState state = new CreateSqlState(this);
                Tuple<CommandData, int> commandDataResult = _database.Factory.CreateBatchDeleteCommand(mapping, datas, start, batchCount, state);
                if (commandDataResult.Item2 == 0) {
                    break;
                }
                Tuple<CommandData, CreateSqlState> commandData = new Tuple<CommandData, CreateSqlState>(commandDataResult.Item1, state);
                commandDatas.Add(commandData);
                start += commandDataResult.Item2;
                if (start >= datas.Count) {
                    break;
                }
            }

            
            var transaction = CreateInnerTransaction(SafeLevel.Default);
            foreach (Tuple<CommandData, CreateSqlState> data in commandDatas) {
                using (DbCommand dbcommand = data.Item1.CreateCommand(_database, data.Item2)) {
                    int ret = await ExecuteNonQueryAsync(dbcommand, SafeLevel.Default, cancellationToken, transaction);
                    if (data.Item1.ReturnRowCount) {
                        result += ret;
                    }
                }
            }

            CommitInnerTransaction(transaction);
            if (result == 0) {
                result = start;
            }
            if (mapping.IsDataTableEntity) {
                foreach (object data in datas) {
                    DataTableEntity tableEntity = data as DataTableEntity;
                    tableEntity.ClearRawPrimaryKeys();
                    tableEntity.ClearUpdateFields();
                }
            }
            return result;
        }

        static object[] CreateObjectList(object lastId, int len)
        {
            TypeCode code = Type.GetTypeCode(lastId.GetType());
            object[] results = new object[len];
            switch (code) {
                case TypeCode.Int16: {
                        short id = (short)lastId;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
                case TypeCode.Int32: {
                        int id = (int)lastId;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
                case TypeCode.Int64: {
                        long id = (long)lastId;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
                case TypeCode.UInt16: {
                        ushort id = (ushort)lastId;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
                case TypeCode.UInt32: {
                        uint id = (uint)lastId;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
                case TypeCode.UInt64: {
                        ulong id = (ulong)lastId;
                        id++;
                        for (int i = len - 1; i >= 0; i--) {
                            results[i] = id--;
                        }
                    }
                    break;
            }

            return results;
        }

        internal int SelectInsert(DataTableEntityMapping insertMapping, DataEntityMapping selectMapping, QueryExpression query, OrderExpression order, SafeLevel level)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(insertMapping, selectMapping, query, order, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> SelectInsertAsync(DataTableEntityMapping insertMapping, DataEntityMapping selectMapping, QueryExpression query, OrderExpression order, SafeLevel level, CancellationToken cancellationToken)
        {
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(insertMapping, selectMapping, query, order, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
            }
            return rInt;
        }

        internal int SelectInsert(InsertSelector selector, DataEntityMapping mapping, QueryExpression query, OrderExpression order, bool distinct, SafeLevel level)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            CommandData commandData;
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            if (mapping.HasJoinRelateModel) {
                QueryExpression subQuery = null;
                QueryExpression mainQuery = null;
                OrderExpression subOrder = null;
                OrderExpression mainOrder = null;
                if (query != null) {
                    if (query.MutliQuery) {
                        mainQuery = query;
                    }
                    else {
                        subQuery = query;
                    }
                }
                if (order != null) {
                    if (order.MutliOrder) {
                        mainOrder = order;
                    }
                    else {
                        subOrder = order;
                    }
                }
                List<IJoinModel> models = relationMap.CreateJoinModels(subQuery, subOrder);
                commandData = _database.Factory.CreateSelectInsertCommand(selector, models, mainQuery, mainOrder, distinct, state);
            }
            else {
                commandData = _database.Factory.CreateSelectInsertCommand(selector, mapping, query, order, distinct, state);
            }
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> SelectInsertAsync(InsertSelector selector, DataEntityMapping mapping, QueryExpression query, OrderExpression order, bool distinct, SafeLevel level, CancellationToken cancellationToken)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            CommandData commandData;
            int rInt;
            CreateSqlState state = new CreateSqlState(this);
            if (mapping.HasJoinRelateModel) {
                QueryExpression subQuery = null;
                QueryExpression mainQuery = null;
                OrderExpression subOrder = null;
                OrderExpression mainOrder = null;
                if (query != null) {
                    if (query.MutliQuery) {
                        mainQuery = query;
                    }
                    else {
                        subQuery = query;
                    }
                }
                if (order != null) {
                    if (order.MutliOrder) {
                        mainOrder = order;
                    }
                    else {
                        subOrder = order;
                    }
                }
                List<IJoinModel> models = relationMap.CreateJoinModels(subQuery, subOrder);
                commandData = _database.Factory.CreateSelectInsertCommand(selector, models, mainQuery, mainOrder, distinct, state);
            }
            else {
                commandData = _database.Factory.CreateSelectInsertCommand(selector, mapping, query, order, distinct, state);
            }
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
            }
            return rInt;
        }

        internal int SelectInsertWithJoinTable(InsertSelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, SafeLevel level)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(selector, models, query, order, distinct, state);
            int rInt;
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> SelectInsertWithJoinTableAsync(InsertSelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, SafeLevel level, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(selector, models, query, order, distinct, state);
            int rInt;
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
            }
            return rInt;
        }

        internal int SelectInsertWithAggregate(InsertSelector selector, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, SafeLevel level)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector aselector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(selector, model.EntityMapping, aselector, groupBy, query, having, order, state);
            int rInt;
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = ExecuteNonQuery(command, level);
            }
            return rInt;
        }

        internal async Task<int> SelectInsertWithAggregateAsync(InsertSelector selector, AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, SafeLevel level, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector aselector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateSelectInsertCommand(selector, model.EntityMapping, aselector, groupBy, query, having, order, state);
            int rInt;
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                rInt = await ExecuteNonQueryAsync(command, level, cancellationToken);
            }
            return rInt;
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectByKey<T>(params object[] primaryKeys)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return SelectByKey<T>(mapping, primaryKeys);
        }

        internal T SelectByKey<T>(DataTableEntityMapping mapping, object[] primaryKeys)
        {
            if (primaryKeys == null || primaryKeys.Length == 0)
                throw new ArgumentNullException(nameof(primaryKeys));
            if (primaryKeys.Length != mapping.PrimaryKeyCount) {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int i = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = new DataFieldInfo(fieldMapping);
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, primaryKeys[i]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                i++;
            }
            T target = default(T);
            Region region = new Region(0, 1);
            target = QueryEntityDataSingle<T>(mapping, null, queryExpression, null, false, region, null);
            return target;
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
            if (primaryKeys.Length != mapping.PrimaryKeyCount) {
                throw new LightDataException(string.Format(SR.NotMatchPrimaryKeyField, mapping.ObjectType));
            }
            if (!mapping.HasPrimaryKey) {
                throw new LightDataException(string.Format(SR.NotContainPrimaryKeyFields, mapping.ObjectType));
            }
            QueryExpression queryExpression = null;
            int i = 0;
            foreach (DataFieldMapping fieldMapping in mapping.PrimaryKeyFields) {
                DataFieldInfo info = new DataFieldInfo(fieldMapping);
                QueryExpression keyExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, info, primaryKeys[i]);
                queryExpression = QueryExpression.And(queryExpression, keyExpression);
                i++;
            }
            T target = default(T);
            Region region = new Region(0, 1);
            target = await QueryEntityDataSingleAsync<T>(mapping, null, queryExpression, null, false, region, null, cancellationToken);
            return target;
        }

        /// <summary>
        /// Selects the single object from key.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByKeyAsync<T>(params object[] primaryKeys)
        {
            return await SelectByKeyAsync<T>(primaryKeys, CancellationToken.None);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectById<T>(int id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return SelectById<T>(mapping, id);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectById<T>(uint id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return SelectById<T>(mapping, id);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectById<T>(long id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return SelectById<T>(mapping, id);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T SelectById<T>(ulong id)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return SelectById<T>(mapping, id);
        }


        internal T SelectById<T>(DataTableEntityMapping mapping, object id)
        {
            if (mapping.IdentityField == null) {
                throw new LightDataException(string.Format(SR.NoIdentityField, mapping.ObjectType));
            }
            DataFieldInfo idfield = new DataFieldInfo(mapping.IdentityField);
            QueryExpression queryExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, idfield, id);
            T target = default(T);
            Region region = new Region(0, 1);
            target = QueryEntityDataSingle<T>(mapping, null, queryExpression, null, false, region, null);
            return target;
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(int id, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await SelectByIdAsync<T>(mapping, id, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(int id)
        {
            return await SelectByIdAsync<T>(id, CancellationToken.None);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(uint id, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await SelectByIdAsync<T>(mapping, id, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(uint id)
        {
            return await SelectByIdAsync<T>(id, CancellationToken.None);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(long id, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await SelectByIdAsync<T>(mapping, id, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(long id)
        {
            return await SelectByIdAsync<T>(id, CancellationToken.None);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(ulong id, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await SelectByIdAsync<T>(mapping, id, cancellationToken);
        }

        /// <summary>
        /// Selects the single object from identifier.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SelectByIdAsync<T>(ulong id)
        {
            return await SelectByIdAsync<T>(id, CancellationToken.None);
        }

        internal async Task<T> SelectByIdAsync<T>(DataTableEntityMapping mapping, object id, CancellationToken cancellationToken)
        {
            if (mapping.IdentityField == null) {
                throw new LightDataException(string.Format(SR.NoIdentityField, mapping.ObjectType));
            }
            DataFieldInfo idfield = new DataFieldInfo(mapping.IdentityField);
            QueryExpression queryExpression = new LightBinaryQueryExpression(mapping, QueryPredicate.Eq, idfield, id);
            T target = default(T);
            Region region = new Region(0, 1);
            target = await QueryEntityDataSingleAsync<T>(mapping, null, queryExpression, null, false, region, null, cancellationToken);
            return target;
        }

        /// <summary>
        /// LQs the ueryable.
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
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateTruncateTableCommand(mapping, state);
            using (DbCommand command = commandData.CreateCommand(_database)) {
                return ExecuteNonQuery(command, SafeLevel.Default);
            }
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> TruncateTableAsync<T>(CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateTruncateTableCommand(mapping, state);
            using (DbCommand command = commandData.CreateCommand(_database)) {
                return await ExecuteNonQueryAsync(command, SafeLevel.Default, cancellationToken);
            }
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<int> TruncateTableAsync<T>()
        {
            return await TruncateTableAsync<T>(CancellationToken.None);
        }

        internal T QueryEntityDataSingle<T>(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetRelationMap(relationMap);
                queryState.SetSelector(selector);
                return QueryDataDefineSingle<T>(mapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele);
            }
        }

        internal List<T> QueryEntityDataList<T>(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetRelationMap(relationMap);
                queryState.SetSelector(selector);
                return QueryDataDefineList<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele);
            }
        }

        internal IEnumerable<T> QueryEntityDataReader<T>(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(_database, state);
            QueryState queryState = new QueryState();
            queryState.SetRelationMap(relationMap);
            queryState.SetSelector(selector);
            return QueryDataDefineReader<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele);
        }

        internal async Task<T> QueryEntityDataSingleAsync<T>(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, CancellationToken cancellationToken)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetRelationMap(relationMap);
                queryState.SetSelector(selector);
                return await QueryDataDefineSingleAsync<T>(mapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele, cancellationToken);
            }
        }

        internal async Task<List<T>> QueryEntityDataListAsync<T>(DataEntityMapping mapping, ISelector selector, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, CancellationToken cancellationToken)
        {
            RelationMap relationMap = mapping.GetRelationMap();
            if (selector == null) {
                selector = relationMap.GetDefaultSelector();
            }
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectDataCommand(mapping, relationMap, selector, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetRelationMap(relationMap);
                queryState.SetSelector(selector);
                return await QueryDataDefineListAsync<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele, cancellationToken);
            }
        }


        internal T QueryJoinDataSingle<T>(DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, List<int> nodataSetNull)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                if (nodataSetNull != null && nodataSetNull.Count > 0) {
                    foreach (int i in nodataSetNull) {
                        if (i < models.Count - 1) {
                            IJoinModel model = models[i];
                            queryState.SetNoDataSetNull(model.AliasTableName);
                        }
                    }
                }
                queryState.SetSelector(selector);
                return QueryDataDefineSingle<T>(mapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele);
            }
        }

        internal List<T> QueryJoinDataList<T>(DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, List<int> nodataSetNull)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                if (nodataSetNull != null && nodataSetNull.Count > 0) {
                    foreach (int i in nodataSetNull) {
                        if (i < models.Count) {
                            IJoinModel model = models[i];
                            queryState.SetNoDataSetNull(model.AliasTableName);
                        }
                    }
                }
                queryState.SetSelector(selector);
                return QueryDataDefineList<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele);
            }
        }

        internal IEnumerable<T> QueryJoinDataReader<T>(DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, List<int> nodataSetNull)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(_database, state);
            QueryState queryState = new QueryState();
            if (nodataSetNull != null && nodataSetNull.Count > 0) {
                foreach (int i in nodataSetNull) {
                    if (i < models.Count - 1) {
                        IJoinModel model = models[i];
                        queryState.SetNoDataSetNull(model.AliasTableName);
                    }
                }
            }
            queryState.SetSelector(selector);
            return QueryDataDefineReader<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele);
        }

        internal async Task<T> QueryJoinDataSingleAsync<T>(DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, List<int> nodataSetNull, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                if (nodataSetNull != null && nodataSetNull.Count > 0) {
                    foreach (int i in nodataSetNull) {
                        if (i < models.Count - 1) {
                            IJoinModel model = models[i];
                            queryState.SetNoDataSetNull(model.AliasTableName);
                        }
                    }
                }
                queryState.SetSelector(selector);
                return await QueryDataDefineSingleAsync<T>(mapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele, cancellationToken);
            }
        }

        internal async Task<List<T>> QueryJoinDataListAsync<T>(DataMapping mapping, ISelector selector, List<IJoinModel> models, QueryExpression query, OrderExpression order, bool distinct, Region region, Delegate dele, List<int> nodataSetNull, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectJoinTableCommand(selector, models, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                if (nodataSetNull != null && nodataSetNull.Count > 0) {
                    foreach (int i in nodataSetNull) {
                        if (i < models.Count - 1) {
                            IJoinModel model = models[i];
                            queryState.SetNoDataSetNull(model.AliasTableName);
                        }
                    }
                }
                queryState.SetSelector(selector);
                return await QueryDataDefineListAsync<T>(mapping, command, commandData.InnerPage ? null : region, queryState, dele, cancellationToken);
            }
        }

        internal T QuerySingleFieldSingle<T>(DataFieldInfo fieldInfo, Type outputType, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(outputType);
                return QueryDataDefineSingle<T>(define, command, commandData.InnerPage ? 0 : region.Start, null, null);
            }
        }

        internal List<T> QuerySingleFieldList<T>(DataFieldInfo fieldInfo, Type outputType, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(outputType);
                return QueryDataDefineList<T>(define, command, commandData.InnerPage ? null : region, null, null);
            }
        }

        internal IEnumerable<T> QuerySingleFieldReader<T>(DataFieldInfo fieldInfo, Type outputType, QueryExpression query, OrderExpression order, bool distinct, Region region)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            DbCommand command = commandData.CreateCommand(_database, state);
            DataDefine define = DataDefine.GetDefine(outputType);
            return QueryDataDefineReader<T>(define, command, commandData.InnerPage ? null : region, null, null);
        }

        internal async Task<T> QuerySingleFieldSingleAsync<T>(DataFieldInfo fieldInfo, Type outputType, QueryExpression query, OrderExpression order, bool distinct, Region region, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(outputType);
                return await QueryDataDefineSingleAsync<T>(define, command, commandData.InnerPage ? 0 : region.Start, null, null, cancellationToken);
            }
        }

        internal async Task<List<T>> QuerySingleFieldListAsync<T>(DataFieldInfo fieldInfo, Type outputType, QueryExpression query, OrderExpression order, bool distinct, Region region, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateSelectSingleFieldCommand(fieldInfo, query, order, distinct, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(outputType);
                return await QueryDataDefineListAsync<T>(define, command, commandData.InnerPage ? null : region, null, null, cancellationToken);
            }
        }


        internal T QueryDynamicAggregateSingle<T>(AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region, Delegate dele)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetSelector(selector);
                return QueryDataDefineSingle<T>(model.OutputMapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele);
            }
        }

        internal List<T> QueryDynamicAggregateList<T>(AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region, Delegate dele)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            DbCommand command = commandData.CreateCommand(_database, state);
            QueryState queryState = new QueryState();
            queryState.SetSelector(selector);
            return QueryDataDefineList<T>(model.OutputMapping, command, commandData.InnerPage ? null : region, queryState, dele);
        }

        internal IEnumerable<T> QueryDynamicAggregateReader<T>(AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region, Delegate dele)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetSelector(selector);
                return QueryDataDefineReader<T>(model.OutputMapping, command, commandData.InnerPage ? null : region, queryState, dele);
            }
        }

        internal async Task<T> QueryDynamicAggregateSingleAsync<T>(AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region, Delegate dele, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetSelector(selector);
                return await QueryDataDefineSingleAsync<T>(model.OutputMapping, command, commandData.InnerPage ? 0 : region.Start, queryState, dele, cancellationToken);
            }
        }

        internal async Task<List<T>> QueryDynamicAggregateAsync<T>(AggregateModel model, QueryExpression query, QueryExpression having, OrderExpression order, Region region, Delegate dele, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            AggregateSelector selector = model.GetSelector();
            AggregateGroupBy groupBy = model.GetGroupBy();
            CommandData commandData = _database.Factory.CreateAggregateTableCommand(model.EntityMapping, selector, groupBy, query, having, order, region, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                QueryState queryState = new QueryState();
                queryState.SetSelector(selector);
                return await QueryDataDefineListAsync<T>(model.OutputMapping, command, commandData.InnerPage ? null : region, queryState, dele, cancellationToken);
            }
        }


        internal object AggregateCount(DataEntityMapping mapping, QueryExpression query, SafeLevel level)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateCountCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                return ExecuteScalar(command, level);
            }
        }

        internal async Task<object> AggregateCountAsync(DataEntityMapping mapping, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateCountCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                return await ExecuteScalarAsync(command, level, cancellationToken);
            }
        }


        internal object AggregateJoinTableCount(List<IJoinModel> models, QueryExpression query, SafeLevel level)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateJoinCountCommand(models, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                return ExecuteScalar(command, level);
            }
        }

        internal async Task<object> AggregateJoinTableCountAsync(List<IJoinModel> models, QueryExpression query, SafeLevel level, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateJoinCountCommand(models, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                return await ExecuteScalarAsync(command, level, cancellationToken);
            }
        }

        internal object Aggregate(DataFieldInfo field, TypeCode typeCode, AggregateType aggregateType, QueryExpression query, bool distinct, SafeLevel level)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateFunctionCommand(field, aggregateType, query, distinct, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                object obj = ExecuteScalar(command, level);
                if (Equals(obj, DBNull.Value)) {
                    return null;
                }
                else {
                    return Convert.ChangeType(obj, typeCode, null);
                }
            }
        }

        internal async Task<object> AggregateAsync(DataFieldInfo field, TypeCode typeCode, AggregateType aggregateType, QueryExpression query, bool distinct, SafeLevel level, CancellationToken cancellationToken)
        {
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateAggregateFunctionCommand(field, aggregateType, query, distinct, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                object obj = await ExecuteScalarAsync(command, level, cancellationToken);
                if (Equals(obj, DBNull.Value)) {
                    return null;
                }
                else {
                    return Convert.ChangeType(obj, typeCode, null);
                }
            }
        }

        internal bool Exists(DataEntityMapping mapping, QueryExpression query)
        {
            bool exists = false;
            Region region = new Region(0, 1);
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateExistsCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(typeof(int?));
                int? obj = QueryDataDefineSingle<int?>(define, command, region.Start, null, null);
                exists = obj.HasValue;
            }
            return exists;
        }

        internal async Task<bool> ExistsAsync(DataEntityMapping mapping, QueryExpression query, CancellationToken cancellationToken)
        {
            bool exists = false;
            Region region = new Region(0, 1);
            CreateSqlState state = new CreateSqlState(this);
            CommandData commandData = _database.Factory.CreateExistsCommand(mapping, query, state);
            using (DbCommand command = commandData.CreateCommand(_database, state)) {
                DataDefine define = DataDefine.GetDefine(typeof(int));
                int? obj = await QueryDataDefineSingleAsync<int?>(define, command, 0, null, null, cancellationToken);
                exists = obj.HasValue;
            }
            return exists;
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
                rInt = await dbcommand.ExecuteNonQueryAsync();
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
            //CheckStatus();
            //object resultObj;
            //if (_transaction == null) {
            //    using (TransactionConnection transaction = CreateTransactionConnection(level)) {
            //        transaction.Open();
            //        DateTime startTime = DateTime.Now;
            //        bool success = false;
            //        string exceptionMessage = null;
            //        object result = null;
            //        try {
            //            transaction.SetupCommand(dbcommand);
            //            resultObj = dbcommand.ExecuteScalar();
            //            result = resultObj;
            //            success = true;
            //        }
            //        catch (Exception ex) {
            //            exceptionMessage = ex.Message;
            //            transaction.Rollback();
            //            throw ex;
            //        }
            //        finally {
            //            DateTime endTime = DateTime.Now;
            //            OutputCommand(nameof(ExecuteScalar), dbcommand, level, false, 0, 0, startTime, endTime, success, result, exceptionMessage);
            //        }
            //        transaction.Commit();
            //    }
            //}
            //else {
            //    if (!_transaction.IsOpen) {
            //        _transaction.Open();
            //    }
            //    DateTime startTime = DateTime.Now;
            //    bool success = false;
            //    string exceptionMessage = null;
            //    object result = null;
            //    try {
            //        _transaction.SetupCommand(dbcommand);
            //        resultObj = dbcommand.ExecuteScalar();
            //        result = resultObj;
            //        success = true;
            //    }
            //    catch (Exception ex) {
            //        exceptionMessage = ex.Message;
            //        RollbackTrans(false);
            //        throw ex;
            //    }
            //    finally {
            //        DateTime endTime = DateTime.Now;
            //        OutputCommand(nameof(ExecuteScalar), dbcommand, level, true, 0, 0, startTime, endTime, success, result, exceptionMessage);
            //    }
            //}
            //return resultObj;
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

        internal IEnumerable<T> QueryDataDefineReader<T>(IDataDefine source, DbCommand dbcommand, Region region, object state, Delegate dele)
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
            using (DbConnection connection = CreateDbConnection()) {
                connection.Open();
                DateTime startTime = DateTime.Now;
                bool success = false;
                string exceptionMessage = null;
                IDataReader reader = null;
                try {
                    dbcommand.Connection = connection;
                    reader = dbcommand.ExecuteReader();
                    success = true;
                }
                catch (Exception ex) {
                    exceptionMessage = ex.Message;
                    dbcommand.Dispose();
                    throw ex;
                }
                finally {
                    DateTime endTime = DateTime.Now;
                    OutputCommand(nameof(QueryDataDefineReader), dbcommand, SafeLevel.None, false, start, size, startTime, endTime, success, "reader", exceptionMessage);
                }
                int index = 0;
                int count = 0;
                try {
                    while (reader.Read()) {
                        if (count >= size) {
                            dbcommand.Cancel();
                            break;
                        }
                        if (index >= start) {
                            count++;
                            object item = source.LoadData(this, reader, state);
                            if (item == null) {
                                yield return default(T);
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
                                yield return (T)item;
                            }
                        }
                        index++;
                    }
                }
                finally {
                    dbcommand.Dispose();
                }
            }
        }

        internal T QueryDataDefineSingle<T>(IDataDefine source, DbCommand dbcommand, int start, object state, Delegate dele)
        {
            CheckStatus();
            if (start < 0)
                start = 0;

            using (DbConnection connection = CreateDbConnection()) {
                connection.Open();
                DateTime startTime = DateTime.Now;
                bool success = false;
                string exceptionMessage = null;
                IDataReader reader = null;
                object result = null;
                try {
                    dbcommand.Connection = connection;
                    reader = dbcommand.ExecuteReader();
                    success = true;

                    int index = 0;
                    bool flat = false;
                    T obj = default(T);
                    while (reader.Read()) {
                        if (flat) {
                            dbcommand.Cancel();
                            break;
                        }
                        if (index >= start) {
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
                    result = obj;
                    return obj;
                }
                catch (Exception ex) {
                    exceptionMessage = ex.Message;
                    throw ex;
                }
                finally {
                    DateTime endTime = DateTime.Now;
                    OutputCommand(nameof(QueryDataDefineSingle), dbcommand, SafeLevel.None, false, start, 1, startTime, endTime, success, result, exceptionMessage);
                }
            }
        }

        internal List<T> QueryDataDefineList<T>(IDataDefine source, DbCommand dbcommand, Region region, object state, Delegate dele)
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
            using (DbConnection connection = CreateDbConnection()) {
                connection.Open();
                DateTime startTime = DateTime.Now;
                bool success = false;
                string exceptionMessage = null;
                IDataReader reader = null;
                int result = 0;
                try {
                    dbcommand.Connection = connection;
                    reader = dbcommand.ExecuteReader();
                    success = true;

                    int index = 0;
                    int count = 0;
                    List<T> list = new List<T>();
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
                        result = list.Count;
                        index++;
                    }
                    return list;
                }
                catch (Exception ex) {
                    exceptionMessage = ex.Message;
                    throw ex;
                }
                finally {
                    DateTime endTime = DateTime.Now;
                    OutputCommand(nameof(QueryDataDefineList), dbcommand, SafeLevel.None, false, start, size, startTime, endTime, success, result, exceptionMessage);
                }
            }
        }

        internal async Task<T> QueryDataDefineSingleAsync<T>(IDataDefine source, DbCommand dbcommand, int start, object state, Delegate dele, CancellationToken cancellationToken)
        {
            CheckStatus();
            if (start < 0)
                start = 0;

            using (DbConnection connection = CreateDbConnection()) {
                await connection.OpenAsync(cancellationToken);
                DateTime startTime = DateTime.Now;
                bool success = false;
                string exceptionMessage = null;
                IDataReader reader = null;
                object result = null;
                try {
                    dbcommand.Connection = connection;
                    reader = await dbcommand.ExecuteReaderAsync(cancellationToken);
                    success = true;

                    int index = 0;
                    bool flat = false;
                    T obj = default(T);
                    while (reader.Read()) {
                        if (flat) {
                            dbcommand.Cancel();
                            break;
                        }
                        if (index >= start) {
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
                    result = obj;
                    return obj;
                }
                catch (Exception ex) {
                    exceptionMessage = ex.Message;
                    throw ex;
                }
                finally {
                    DateTime endTime = DateTime.Now;
                    OutputCommand(nameof(QueryDataDefineSingleAsync), dbcommand, SafeLevel.None, false, start, 1, startTime, endTime, success, result, exceptionMessage);
                }
            }
        }

        internal async Task<List<T>> QueryDataDefineListAsync<T>(IDataDefine source, DbCommand dbcommand, Region region, object state, Delegate dele, CancellationToken cancellationToken)
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
            using (DbConnection connection = CreateDbConnection()) {
                await connection.OpenAsync(cancellationToken);
                DateTime startTime = DateTime.Now;
                bool success = false;
                string exceptionMessage = null;
                IDataReader reader = null;
                int result = 0;
                try {
                    dbcommand.Connection = connection;
                    reader = await dbcommand.ExecuteReaderAsync(cancellationToken);
                    success = true;

                    int index = 0;
                    int count = 0;
                    List<T> list = new List<T>();
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
                        result = list.Count;
                        index++;
                    }
                    return list;
                }
                catch (Exception ex) {
                    exceptionMessage = ex.Message;
                    throw ex;
                }
                finally {
                    DateTime endTime = DateTime.Now;
                    OutputCommand(nameof(QueryDataDefineListAsync), dbcommand, SafeLevel.None, false, start, size, startTime, endTime, success, result, exceptionMessage);
                }
            }
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
                return QueryDataDefineList<T>(mapping, command, null, queryState, null);
            }
        }

        #endregion

        internal DbCommand CreateCommand(string sql)
        {
            return _database.CreateCommand(sql);
        }

        internal IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction)
        {
            return _database.CreateParameter(name, value, dbType, direction);
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