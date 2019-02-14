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
        DbCommand _command;

        DataContext _context;

        SafeLevel _level = SafeLevel.None;

        List<CallbackDataParameter> callbackList;

        /// <summary>
        /// Gets or sets the command time out.
        /// </summary>
        /// <value>The command time out.</value>
        public int CommandTimeOut {
            get {
                return _command.CommandTimeout;
            }
            set {
                _command.CommandTimeout = value;
            }
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
            _command = context.CreateCommand(sql);
            _command.CommandType = commandType;
            if (parameters != null) {
                foreach (DataParameter param in parameters) {
                    string parameterName = param.ParameterName;
                    IDataParameter dataParameter = context.CreateParameter(parameterName, param.Value, param.DbType, (ParameterDirection)param.Direction, commandType);
                    param.SetDataParameter(dataParameter);
                    _command.Parameters.Add(dataParameter);
                    if (param is CallbackDataParameter callbackParam) {
                        if (callbackList == null) {
                            callbackList = new List<CallbackDataParameter>();
                        }
                        callbackList.Add(callbackParam);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <returns>The non query.</returns>
        public int ExecuteNonQuery()
        {
            int ret = _context.ExecuteNonQuery(_command, _level);
            Callback();
            return ret;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <returns>The scalar.</returns>
        public object ExecuteScalar()
        {
            object ret = _context.ExecuteScalar(_command, _level);
            Callback();
            return ret;
        }

        /// <summary>
        /// Query and return first data
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>First data</returns>
        public T QueryFirst<T>()
        {
            T target = _context.QueryDataDefineSingle<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, 0, null, null);
            Callback();
            return target;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="region">Query region</param>
        /// <returns>Data list</returns>
        private List<T> QueryList<T>(Region region)
        {
            List<T> list = _context.QueryDataDefineList<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null);
            Callback();
            return list;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <returns>Data list</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public List<T> QueryList<T>()
        {
            return QueryList<T>(null);
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <returns>Data list</returns>
        /// <param name="start">Start.</param>
        /// <param name="size">Size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public List<T> QueryList<T>(int start, int size)
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            Region region = new Region(start, size);
            return QueryList<T>(region);
        }

        /// <summary>
        /// Query the specified start and size.
        /// </summary>
        /// <param name="start">Start index. start from 0</param>
        /// <param name="size">Size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public IEnumerable Query<T>(int start, int size)
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            Region region = new Region(start, size);
            return Query<T>(region);
        }

        /// <summary>
        /// Query and return data enumerable
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="region">Data region</param>
        /// <returns>Data enumerable</returns>
        private IEnumerable<T> Query<T>(Region region)
        {
            IEnumerable<T> enumable = _context.QueryDataDefineReader<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null);
            Callback();
            return enumable;
        }

        /// <summary>
        /// Query and return data enumerable
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
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
            DataSet ds = _context.QueryDataSet(_level, _command);
            Callback();
            return ds;
        }

        #region async
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <returns>The non query.</returns>
        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int ret = await _context.ExecuteNonQueryAsync(_command, _level, cancellationToken);
            Callback();
            return ret;
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <returns>The scalar.</returns>
        public async Task<object> ExecuteScalarAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            object ret = await _context.ExecuteScalarAsync(_command, _level, cancellationToken);
            Callback();
            return ret;
        }

        /// <summary>
        /// Query and return first data
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>First data</returns>
        public async Task<T> QueryFirstAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            T target = await _context.QueryDataDefineSingleAsync<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, 0, null, null, cancellationToken);
            Callback();
            return target;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="region">Query region</param>
        /// <returns>Data list</returns>
        private async Task<List<T>> QueryListAsync<T>(Region region, CancellationToken cancellationToken = default(CancellationToken))
        {
            List<T> list = await _context.QueryDataDefineListAsync<T>(DataEntityMapping.GetEntityMapping(typeof(T)), _level, _command, region, null, null, cancellationToken);
            Callback();
            return list;
        }

        /// <summary>
        /// Query and return data list
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>Data list</returns>
        public async Task<List<T>> QueryListAsync<T>(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await QueryListAsync<T>(null, cancellationToken);
        }

        /// <summary>
        /// Query the specified start and size.
        /// </summary>
        /// <param name="start">Start index. start from 0</param>
        /// <param name="size">Size.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<List<T>> QueryListAsync<T>(int start, int size, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (start < 0) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            Region region = new Region(start, size);
            return await QueryListAsync<T>(region, cancellationToken);
        }

        #endregion

        void Callback()
        {
            if (callbackList != null) {
                foreach (CallbackDataParameter callbackParam in callbackList) {
                    callbackParam.Callback();
                }
            }
        }
    }
}
