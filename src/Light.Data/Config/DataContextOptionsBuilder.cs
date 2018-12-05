using System;
using System.Collections.Generic;

namespace Light.Data
{
    public class DataContextOptionsBuilder<TContext> where TContext : DataContext
    {
        public DataContextOptions<TContext> Build()
        {
            var options = new DataContextOptions<TContext>();
            BuildOptions(options);
            return options;
        }

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

        Dictionary<string, string> _dict = new Dictionary<string, string>();

        string _connection = null;

        protected ICommandOutput _commandOutput;

        Func<string, ConfigParamSet, DatabaseProvider> _func = null;

        internal void SetConfigParam(string name, object value)
        {
            if (value != null) {
                _dict[name] = value.ToString();
            }
        }

        public void SetCommandOutput(ICommandOutput output)
        {
            _commandOutput = output;
        }

        public void SetBatchInsertCount(int count)
        {
            SetConfigParam("batchInsertCount", count);

        }

        public void SetBatchUpdateCount(int count)
        {
            SetConfigParam("batchUpdateCount", count);
        }

        public void SetBatchDeleteCount(int count)
        {
            SetConfigParam("batchDeleteCount", count);
        }

        public void SetTimeout(int timeout)
        {
            SetConfigParam("timeout", timeout);
        }

        public void SetVersion(string version)
        {
            SetConfigParam("version", version);
        }

        public void SetStrictMode(bool strictMode)
        {
            SetConfigParam("strictMode", strictMode);
        }

        internal void SetDataConfig(string connection, Func<string, ConfigParamSet, DatabaseProvider> func)
        {
            _connection = connection;
            _func = func;
        }
    }
}