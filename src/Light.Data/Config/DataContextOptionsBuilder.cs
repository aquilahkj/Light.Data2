using System;
using System.Collections.Generic;

namespace Light.Data
{
    public class DataContextOptionsBuilder : DataContextOptionsBuilderBase<DataContextOptions>
    {

    }

    public class DataContextOptionsBuilder<TContext> : DataContextOptionsBuilderBase<DataContextOptions<TContext>> where TContext : DataContext
    {

    }

    public abstract class DataContextOptionsBuilderBase<TOptions> where TOptions : DataContextOptions, new()
    {
        public TOptions Build()
        {
            if (_configName != null) {
                var configOptions = DataContextConfiguration.GetOptions(_configName);
                var options = new TOptions() {
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
                var options = new TOptions() {
                    Connection = _connection,
                    Database = database,
                    CommandOutput = _commandOutput
                };
                return options;
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

        //protected T CreateDatabaseProvider<T>() where T : DataContextOptions, new()
        //{
        //    if (_func == null) {
        //        throw new LightDataException(SR.DataContextOptionsError);
        //    }
        //    var paramSet = new ConfigParamSet();
        //    foreach (var item in _dict) {
        //        if (item.Value != null) {
        //            paramSet.SetParamValue(item.Key, item.Value.ToString());
        //        }
        //    }
        //    var database = _func(Guid.NewGuid().ToString("N"), paramSet);
        //    if (database == null) {
        //        throw new LightDataException(SR.DataContextOptionsError);
        //    }
        //    T options = new T() {
        //        CommandOutput = _commandOutput,
        //        Connection = _connection,
        //        Database = database
        //    };
        //    return options;
        //}

        //protected T CreateDatabaseProvider<T>(DatabaseProvider database, string connection) where T : DataContextOptions, new()
        //{
        //    if (_func == null) {
        //        throw new LightDataException(SR.DataContextOptionsError);
        //    }
        //    var paramSet = new ConfigParamSet();
        //    foreach (var item in _dict) {
        //        if (item.Value != null) {
        //            paramSet.SetParamValue(item.Key, item.Value.ToString());
        //        }
        //    }
        //    var database = _func(Guid.NewGuid().ToString("N"), paramSet);
        //    if (database == null) {
        //        throw new LightDataException(SR.DataContextOptionsError);
        //    }
        //    T options = new T() {
        //        CommandOutput = _commandOutput,
        //        Connection = _connection,
        //        Database = database
        //    };
        //    return options;
        //}
    }
}