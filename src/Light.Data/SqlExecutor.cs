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
    /// SqlString executor.
    /// </summary>
    public class SqlExecutor
    {
        private DbCommand _command;

        private DataContext _context;

        private SafeLevel _level = SafeLevel.None;

        private DataParameter[] _parameters;

        /// <summary>
        /// Gets or sets the command time out.
        /// </summary>
        /// <value>The command time out.</value>
        public int CommandTimeOut {
            get => _command.CommandTimeout;
            set => _command.CommandTimeout = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExecutor"/> class.
        /// </summary>
        /// <param name="sql">Sql.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="level">Level.</param>
        /// <param name="context">Context.</param>
        internal SqlExecutor(string sql, DataParameter[] parameters, CommandType commandType, SafeLevel level, DataContext context)
        {
            _level = level;
            _context = context;
            var database = context.Database;
            _command = database.CreateCommand(sql);
            _command.CommandType = commandType;
            if (parameters != null) {
                _parameters = new DataParameter[parameters.Length];
                var i = 0;
                foreach (var param in parameters) {
                    var dataParameter = param.ConvertDbParameter(database, commandType);
                    _command.Parameters.Add(dataParameter);
                    _parameters[i] = param;
                    i++;
                }

            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <returns>The affected rows.</returns>
        public int ExecuteNonQuery()
        {
            var ret = _context.ExecuteNonQuery(_command, _level);
            Callback();
            return ret;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <returns>The execute result.</returns>
        public object ExecuteScalar()
        {
            var ret = _context.ExecuteScalar(_command, _level);
            Callback();
            return ret;
        }

        /// <summary>
        /// Query and return first data
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>First data</returns>
        public T QueryFirst<T>()
        {
            var target = _context.QueryDataDefineSingle<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, 0, null, null);
            Callback();
            return target;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="region">Query region</param>
        /// <returns>Data list</returns>
        private List<T> QueryList<T>(Region region)
        {
            var list = _context.QueryDataDefineList<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null);
            Callback();
            return list;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>Data list</returns>
        public List<T> QueryList<T>()
        {
            return QueryList<T>(null);
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="start">Start.</param>
        /// <param name="size">Size.</param>
        /// <returns>Data list</returns>
        public List<T> QueryList<T>(int start, int size)
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            var region = new Region(start, size);
            return QueryList<T>(region);
        }

        /// <summary>
        /// Query the specified start and size.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="start">Start index. start from 0</param>
        /// <param name="size">Size.</param>
        public IEnumerable Query<T>(int start, int size)
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            var region = new Region(start, size);
            return Query<T>(region);
        }

        /// <summary>
        /// Query and return data enumerable
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="region">Data region</param>
        /// <returns>Data enumerable</returns>
        private IEnumerable<T> Query<T>(Region region)
        {
            var enumable = _context.QueryDataDefineReader<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null);
            Callback();
            return enumable;
        }

        /// <summary>
        /// Query and return data enumerable
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <returns>Data enumerable</returns>
        public IEnumerable<T> Query<T>()
        {
            return Query<T>(null);
        }

        /// <summary>
        /// Query and return dataset
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet QueryDataSet()
        {
            var ds = _context.QueryDataSet(_level, _command);
            Callback();
            return ds;
        }

        #region async
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>The affected rows.</returns>
        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var ret = await _context.ExecuteNonQueryAsync(_command, _level, cancellationToken);
            Callback();
            return ret;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>The execute result.</returns>
        public async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var ret = await _context.ExecuteScalarAsync(_command, _level, cancellationToken);
            Callback();
            return ret;
        }

        /// <summary>
        /// Query and return first data
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>First data</returns>
        public async Task<T> QueryFirstAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            var target = await _context.QueryDataDefineSingleAsync<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, 0, null, null, cancellationToken);
            Callback();
            return target;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="region">Query region</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>Data list</returns>
        private async Task<List<T>> QueryListAsync<T>(Region region, CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await _context.QueryDataDefineListAsync<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null, cancellationToken);
            Callback();
            return list;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>Data list</returns>
        public async Task<List<T>> QueryListAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryListAsync<T>(null, cancellationToken);
        }

        /// <summary>
        /// Query the specified start and size.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="start">Start index. start from 0</param>
        /// <param name="size">Size.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>Data list</returns>
        public async Task<List<T>> QueryListAsync<T>(int start, int size, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            var region = new Region(start, size);
            return await QueryListAsync<T>(region, cancellationToken);
        }

        #endregion

        private void Callback()
        {
            if (_parameters != null) {
                foreach (var item in _parameters) {
                    item.Callback();
                }
            }
        }
    }
}
