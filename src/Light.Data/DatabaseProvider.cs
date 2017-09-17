using System;
using System.Data;
using System.Data.Common;

namespace Light.Data
{
    abstract class DatabaseProvider
    {
        protected CommandFactory _factory;

        private string configName;

        public string ConfigName {
            get {
                return configName;
            }
        }

        protected DatabaseProvider(string configName, ConfigParamSet configParams)
        {
            this.configName = configName;
            string batchInsertCount = configParams.GetParamValue("batchInsertCount");
            if (batchInsertCount != null) {
                if (int.TryParse(batchInsertCount, out int value) && value > 0)
                    _batchInsertCount = value;
            }

            string batchUpdateCount = configParams.GetParamValue("batchUpdateCount");
            if (batchUpdateCount != null) {
                if (int.TryParse(batchUpdateCount, out int value) && value > 0)
                    _batchUpdateCount = value;
            }

            string batchDeleteCount = configParams.GetParamValue("batchDeleteCount");
            if (batchDeleteCount != null) {
                if (int.TryParse(batchDeleteCount, out int value) && value > 0)
                    _batchDeleteCount = value;
            }

            string timeout = configParams.GetParamValue("timeout");
            if (timeout != null) {
                if (int.TryParse(batchInsertCount, out int value) && value > 0)
                    _commandTimeout = value;
            }
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        public abstract DbConnection CreateConnection();

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>The connection.</returns>
        /// <param name="connectionString">Connection string.</param>
        public abstract DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The command.</returns>
        /// <param name="sql">Sql.</param>
        public abstract DbCommand CreateCommand(string sql);

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>The command.</returns>
        public abstract DbCommand CreateCommand();

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <returns>The parameter.</returns>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public abstract IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction);

        /// <summary>
        /// Gets the factory.
        /// </summary>
        /// <value>The factory.</value>
        public CommandFactory Factory {
            get {
                return _factory;
            }
        }

        int _commandTimeout = 60000;

        public int CommandTimeout {
            get {
                return _commandTimeout;
            }
            set {
                if (value > 0) {
                    _commandTimeout = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        //bool _strictMode = true;

        //public bool StrictMode {
        //    get {
        //        return _strictMode;
        //    }
        //    set {
        //        _strictMode = value;
        //    }
        //}

        int _batchInsertCount;

        public int BatchInsertCount {
            get {
                return _batchInsertCount;
            }
            set {
                if (value > 0) {
                    _batchInsertCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        int _batchUpdateCount;

        public int BatchUpdateCount {
            get {
                return _batchUpdateCount;
            }
            set {
                if (value > 0) {
                    _batchUpdateCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        int _batchDeleteCount;

        public int BatchDeleteCount {
            get {
                return _batchDeleteCount;
            }
            set {
                if (value > 0) {
                    _batchDeleteCount = value;
                }
                else {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
            }
        }

        /// <summary>
        /// Formats the stored procedure parameter.
        /// </summary>
        /// <param name="dataParameter">Data parmeter.</param>
        public abstract void FormatStoredProcedureParameter(IDataParameter dataParameter);
    }

}
