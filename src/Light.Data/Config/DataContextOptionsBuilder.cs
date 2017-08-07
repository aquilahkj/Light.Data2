using System;
using System.Collections.Generic;

namespace Light.Data
{
    public class DataContextOptionsBuilder
    {
        public DataContextOptions Build()
        {
            if (_configName != null) {
                var configOptions = DataContextConfiguration.GetOptions(_configName);
                var options = new DataContextOptions() {
                    Database = configOptions.Database,
                    Connection = configOptions.Connection,
                    CommandOutput = _commandOutput
                };
                return options;
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
                var options = new DataContextOptions() {
                    Connection = _connection,
                    Database = database,
                    CommandOutput = _commandOutput
                };
                return options;
            }
        }


        Dictionary<string, object> _dict = new Dictionary<string, object>();

        internal Dictionary<string, object> ParamDict {
            get {
                return _dict;
            }
        }

        string _configName;

        public string ConfigName {
            get {
                return _configName;
            }
            set {
                _configName = value;
            }
        }

        ICommandOutput _commandOutput;

        public ICommandOutput CommandOutput {
            get {
                return _commandOutput;
            }
            set {
                _commandOutput = value;
            }
        }

        string _connection = null;

        Func<string, ConfigParamSet, DatabaseProvider> _func = null;

        internal void SetDataConfig(string connection, Func<string, ConfigParamSet, DatabaseProvider> func)
        {
            _connection = connection;
            _func = func;
        }
    }

    public class DataContextOptionsBuilder<TContext> where TContext : DataContext
    {
        public DataContextOptions<TContext> Build()
        {
            if (_configName != null) {
                var configOptions = DataContextConfiguration.GetOptions(_configName);
                var options = new DataContextOptions<TContext>() {
                    Database = configOptions.Database,
                    Connection = configOptions.Connection,
                    CommandOutput = _commandOutput
                };
                return options;
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
                var options = new DataContextOptions<TContext>() {
                    Connection = _connection,
                    Database = database,
                    CommandOutput = _commandOutput
                };
                return options;
            }
        }


        Dictionary<string, object> _dict = new Dictionary<string, object>();

        internal Dictionary<string, object> ParamDict {
            get {
                return _dict;
            }
        }

        string _configName;

        public string ConfigName {
            get {
                return _configName;
            }
            set {
                _configName = value;
            }
        }

        ICommandOutput _commandOutput;

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

        string _connection = null;

        Func<string, ConfigParamSet, DatabaseProvider> _func = null;

        internal void SetDataConfig(string connection, Func<string, ConfigParamSet, DatabaseProvider> func)
        {
            _connection = connection;
            _func = func;
        }
    }
}