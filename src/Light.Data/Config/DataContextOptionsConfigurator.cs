using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Data context options configurator.
    /// </summary>
    public class DataContextOptionsConfigurator<TContext> where TContext : DataContext
    {
        string _configName;

        ICommandOutput _commandOutput;

        /// <summary>
        /// Gets or sets the name of the config.
        /// </summary>
        /// <value>The name of the config.</value>
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

        /// <summary>
        /// Sets the command output.
        /// </summary>
        /// <param name="output">Output.</param>
        public void SetCommandOutput(ICommandOutput output)
        {
            _commandOutput = output;
        }

        /// <summary>
        /// Create the specified configuration.
        /// </summary>
        /// <returns>The create.</returns>
        /// <param name="configuration">Configuration.</param>
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
            }
            else {
                baseOptions = configuration.GetOptions(_configName);
            }
            options.Database = baseOptions.Database;
            options.Connection = baseOptions.Connection;
            options.CommandOutput = _commandOutput;
        }
    }
}
