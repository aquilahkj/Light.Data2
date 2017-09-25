using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Light.Data
{
    public class DataContextConfiguration
    {
        static DataContextConfiguration instance = null;

        static object locker = new object();

        static string configFilePath;

        public static void SetConfigFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (instance != null) {
                throw new LightDataException(SR.ConfigurationHasBeenInitialized);
            }
            lock (locker) {
                configFilePath = filePath;
                instance = null;
            }
        }

        public static DataContextConfiguration Global {
            get {
                var myinstance = instance;
                if (myinstance == null) {
                    lock (locker) {
                        if (instance == null) {
                            LightDataOptions options = null;
                            if (string.IsNullOrWhiteSpace(configFilePath)) {
                                configFilePath = "lightdata.json";
                            }
                            FileInfo fileInfo = new FileInfo(configFilePath);
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

        internal DataContextConfiguration(LightDataOptions optionList)
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

        readonly DataContextOptions defaultOptions;

        readonly Dictionary<string, DataContextOptions> optionsDict = new Dictionary<string, DataContextOptions>();

        public DataContextOptions DefaultOptions {
            get {
                if (defaultOptions == null) {
                    throw new LightDataException(SR.DefaultConfigNotExists);
                }
                return defaultOptions;
            }
        }

        public DataContextOptions GetOptions(string name)
        {
            if (optionsDict.TryGetValue(name, out DataContextOptions options)) {
                return options;
            } else {
                throw new LightDataException(string.Format(SR.SpecifiedConfigNotExists, name));
            }
        }
    }
}