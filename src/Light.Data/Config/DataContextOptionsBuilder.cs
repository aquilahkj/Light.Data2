using System;
using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Data context options builder.
    /// </summary>
    public class DataContextOptionsBuilder<TContext> where TContext : DataContext
    {
        /// <summary>
        /// Build this instance.
        /// </summary>
        /// <returns>The build.</returns>
        public DataContextOptions<TContext> Build()
        {
            var options = new DataContextOptions<TContext>();
            BuildOptions(options);
            return options;
        }

        /// <summary>
        /// Builds the options.
        /// </summary>
        /// <param name="options">Options.</param>
        protected void BuildOptions(DataContextOptions options)
        {
            if (_func == null) {
                throw new LightDataException(SR.DataContextOptionsError);
            }
            var paramSet = new ConfigParamSet();
            foreach (var item in _dict) {
                if (item.Value != null) {
                    paramSet.SetParamValue(item.Key, item.Value);
                }
            }
            var database = _func(Guid.NewGuid().ToString("N"), paramSet);
            if (database == null) {
                throw new LightDataException(SR.DataContextOptionsError);
            }
            options.Connection = _connection;
            options.Database = database;
            options.CommandOutput = _commandOutput;

        }

        private Dictionary<string, string> _dict = new Dictionary<string, string>();

        private string _connection = null;

        /// <summary>
        /// CommandOutput
        /// </summary>
        protected ICommandOutput _commandOutput;

        private Func<string, ConfigParamSet, DatabaseProvider> _func = null;

        /// <summary>
        /// Sets the config parameter.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        internal void SetConfigParam(string name, object value)
        {
            if (value != null) {
                _dict[name] = value.ToString();
            }
        }

        /// <summary>
        /// Sets the command output.
        /// </summary>
        /// <param name="output">Output.</param>
        public void SetCommandOutput(ICommandOutput output)
        {
            _commandOutput = output;
        }

        /// <summary>
        /// Sets the batch insert count.
        /// </summary>
        /// <param name="count">Count.</param>
        public void SetBatchInsertCount(int count)
        {
            SetConfigParam("batchInsertCount", count);

        }

        /// <summary>
        /// Sets the batch update count.
        /// </summary>
        /// <param name="count">Count.</param>
        public void SetBatchUpdateCount(int count)
        {
            SetConfigParam("batchUpdateCount", count);
        }

        /// <summary>
        /// Sets the batch delete count.
        /// </summary>
        /// <param name="count">Count.</param>
        public void SetBatchDeleteCount(int count)
        {
            SetConfigParam("batchDeleteCount", count);
        }

        /// <summary>
        /// Sets the timeout.
        /// </summary>
        /// <param name="timeout">Timeout.</param>
        public void SetTimeout(int timeout)
        {
            SetConfigParam("timeout", timeout);
        }

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <param name="version">Version.</param>
        public void SetVersion(string version)
        {
            SetConfigParam("version", version);
        }

        /// <summary>
        /// Sets the strict mode.
        /// </summary>
        /// <param name="strictMode">If set to <c>true</c> strict mode.</param>
        public void SetStrictMode(bool strictMode)
        {
            SetConfigParam("strictMode", strictMode);
        }

        /// <summary>
        /// Sets the data config.
        /// </summary>
        /// <param name="connection">Connection.</param>
        /// <param name="func">Func.</param>
        internal void SetDataConfig(string connection, Func<string, ConfigParamSet, DatabaseProvider> func)
        {
            _connection = connection;
            _func = func;
        }
    }
}