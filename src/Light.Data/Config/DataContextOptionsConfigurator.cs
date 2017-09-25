using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    public class DataContextOptionsConfigurator<TContext> where TContext : DataContext
    {
        string _configName;

        ICommandOutput _commandOutput;

        public string ConfigName {
            get {
                return _configName;
            }
            set {
                if (string.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentNullException(nameof(ConfigName));
                }
                _configName = value;
            }
        }

        public void SetCommandOutput(ICommandOutput output)
        {
            _commandOutput = output;
        }

        public DataContextOptions<TContext> Create(DataContextConfiguration configuration)
        {
            var options = new DataContextOptions<TContext>();
            BuildOptions(options, configuration);
            return options;
        }

        void BuildOptions(DataContextOptions options, DataContextConfiguration configuration)
        {
            DataContextOptions baseOptions = null;
            if (_configName == null) {
                baseOptions = configuration.DefaultOptions;
            } else {
                baseOptions = configuration.GetOptions(_configName);
            }
            options.Database = baseOptions.Database;
            options.Connection = baseOptions.Connection;
            options.CommandOutput = _commandOutput;
        }
    }
}
