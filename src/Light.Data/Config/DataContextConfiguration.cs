using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace Light.Data
{
    /// <summary>
    /// Data context configuration.
    /// </summary>
    public class DataContextConfiguration
    {
        static DataContextConfiguration instance = null;

        static bool useEntryAssemblyDirectory = true;

        static object locker = new object();

        static string gobalConfigFilePath;

        /// <summary>
        /// Sets the config file path.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public static void SetConfigFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (instance != null) {
                throw new LightDataException(SR.ConfigurationHasBeenInitialized);
            }
            lock (locker) {
                gobalConfigFilePath = filePath;
                instance = null;
            }
        }

        /// <summary>
        /// Gets the global configuration.
        /// </summary>
        /// <value>The global.</value>
        public static DataContextConfiguration Global {
            get {
                var myinstance = instance;
                if (myinstance == null) {
                    lock (locker) {
                        if (instance == null) {
                            LightDataOptions options = null;
                            if (string.IsNullOrWhiteSpace(gobalConfigFilePath)) {
                                gobalConfigFilePath = "lightdata.json";
                            }
                            FileInfo fileInfo;
                            if (useEntryAssemblyDirectory) {
                                fileInfo = FileHelper.GetFileInfo(gobalConfigFilePath, out bool absolute);
                                if (!fileInfo.Exists && !absolute) {
                                    fileInfo = new FileInfo(gobalConfigFilePath);
                                }
                            }
                            else {
                                fileInfo = new FileInfo(gobalConfigFilePath);
                            }
                            if (fileInfo.Exists) {
                                using (StreamReader reader = fileInfo.OpenText()) {
                                    string content = reader.ReadToEnd();
                                    JObject dom = JObject.Parse(content);
                                    var section = dom.GetValue("lightData");
                                    if (section != null) {
                                        options = section.ToObject<LightDataOptions>();
                                    }
                                }
                            }
                            instance = new DataContextConfiguration(options);
                        }
                        myinstance = instance;
                    }
                }
                return myinstance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContextConfiguration"/> class.
        /// </summary>
        /// <param name="configFilePath">Config file path.</param>
        internal DataContextConfiguration(string configFilePath)
        {
            FileInfo fileInfo;
            if (useEntryAssemblyDirectory) {
                fileInfo = FileHelper.GetFileInfo(configFilePath, out bool absolute);
                if (!fileInfo.Exists && !absolute) {
                    fileInfo = new FileInfo(configFilePath);
                }
            }
            else {
                fileInfo = new FileInfo(configFilePath);
            }
            if (fileInfo.Exists) {
                using (StreamReader reader = fileInfo.OpenText()) {
                    string content = reader.ReadToEnd();
                    JObject dom = JObject.Parse(content);
                    var section = dom.GetValue("lightData");
                    if (section != null) {
                        LightDataOptions options = section.ToObject<LightDataOptions>();
                        Internal_DataContextConfiguration(options);
                    }
                }
            }
            else {
                throw new FileNotFoundException("config file not found", configFilePath);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataContextConfiguration"/> class.
        /// </summary>
        /// <param name="optionList">Option list.</param>
        internal DataContextConfiguration(LightDataOptions optionList)
        {
            Internal_DataContextConfiguration(optionList);
        }

        private void Internal_DataContextConfiguration(LightDataOptions optionList)
        {
            if (optionList != null && optionList.Connections != null && optionList.Connections.Length > 0) {
                foreach (var connection in optionList.Connections) {
                    var setting = new ConnectionSetting() {
                        Name = connection.Name,
                        ConnectionString = connection.ConnectionString,
                        ProviderName = connection.ProviderName
                    };
                    var configParam = new ConfigParamSet();
                    if (connection.ConfigParams != null && connection.ConfigParams.Count > 0) {
                        foreach (var param in connection.ConfigParams) {
                            configParam.SetParamValue(param.Key, param.Value);
                        }
                    }
                    setting.ConfigParam = configParam;
                    var options = DataContextOptions.CreateOptions(setting);
                    if (options != null) {
                        if (defaultOptions == null) {
                            defaultOptions = options;
                        }
                        optionsDict.Add(setting.Name, options);
                    }
                }
            }
        }

        DataContextOptions defaultOptions;

        Dictionary<string, DataContextOptions> optionsDict = new Dictionary<string, DataContextOptions>();

        /// <summary>
        /// Gets the default options.
        /// </summary>
        /// <value>The default options.</value>
        public DataContextOptions DefaultOptions {
            get {
                if (defaultOptions == null) {
                    throw new LightDataException(SR.DefaultConfigNotExists);
                }
                return defaultOptions;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Light.Data.DataContextConfiguration"/> use entry
        /// assembly directory.
        /// </summary>
        /// <value><c>true</c> if use entry assembly directory; otherwise, <c>false</c>.</value>
        public static bool UseEntryAssemblyDirectory {
            get {
                return useEntryAssemblyDirectory;
            }
            set {
                useEntryAssemblyDirectory = value;
            }
        }

        /// <summary>
        /// Gets the options by name.
        /// </summary>
        /// <returns>The options.</returns>
        /// <param name="name">Name.</param>
        public DataContextOptions GetOptions(string name)
        {
            if (optionsDict.TryGetValue(name, out DataContextOptions options)) {
                return options;
            }
            else {
                throw new LightDataException(string.Format(SR.SpecifiedConfigNotExists, name));
            }
        }
    }
}