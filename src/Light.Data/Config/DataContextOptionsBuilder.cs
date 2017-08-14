using System;
using System.Collections.Generic;

namespace Light.Data
{
    public class DataContextOptionsBuilder : DataContextOptionsBuilderBase
    {
        public DataContextOptions Build()
        {
            var options = new DataContextOptions();
            BuildOptions(options);
            return options;
        }
    }

    public class DataContextOptionsBuilder<TContext> : DataContextOptionsBuilderBase where TContext : DataContext
    {
        public DataContextOptions<TContext> Build()
        {
            var options = new DataContextOptions<TContext>();
            BuildOptions(options);
            return options;
        }
    }

    public abstract class DataContextOptionsBuilderBase
    {
        protected void BuildOptions(DataContextOptions options)
        {
            if (_configName != null) {
                var configOptions = DataContextConfiguration.GetOptions(_configName);
                options.Database = configOptions.Database;
                options.Connection = configOptions.Connection;
                options.CommandOutput = _commandOutput;
            }
            else {
                if (_func == null) {
                    throw new LightDataException(SR.DataContextOptionsError);
                }
                var paramSet = new ConfigParamSet();
                foreach (var item in _dict) {
                    if (item.Value != null) {
                        paramSet.SetParamValue(item.Key, item.Value.ToString());
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
        }

        Dictionary<string, object> _dict = new Dictionary<string, object>();

        string _configName;

        string _connection = null;

        protected ICommandOutput _commandOutput;

        Func<string, ConfigParamSet, DatabaseProvider> _func = null;

        internal Dictionary<string, object> ParamDict {
            get {
                return _dict;
            }
        }

        public string ConfigName {
            get {
                return _configName;
            }
            set {
                _configName = value;
            }
        }

        public void SetCommandOutput(ICommandOutput output)
        {
            _commandOutput = output;
        }

        public void SetBatchInsertCount(int count)
        {
            _dict["batchInsertCount"] = count;
        }

        public void SetBatchUpdateCount(int count)
        {
            _dict["batchUpdateCount"] = count;
        }

        public void SetBatchDeleteCount(int count)
        {
            _dict["batchDeleteCount"] = count;
        }

        public void SetTimeout(int timeout)
        {
            _dict["timeout"] = timeout;
        }

        internal void SetDataConfig(string connection, Func<string, ConfigParamSet, DatabaseProvider> func)
        {
            _connection = connection;
            _func = func;
        }
    }
}