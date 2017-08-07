using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Light.Data
{
    public static class DataContextConfiguration
    {
        static object locker = new object();

        static DataContextOptions defaultOptions;

        static string configFilePath;

        static bool initialed;

        static Dictionary<string, DataContextOptions> optionsDict = new Dictionary<string, DataContextOptions>();

        public static void SetConfigFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (initialed) {
                throw new LightDataException(SR.ConfigurationHasBeenInitialized);
            }
            configFilePath = filePath;
        }

        static void Initial()
        {
            if (string.IsNullOrWhiteSpace(configFilePath)) {
                configFilePath = "lightdata.json";
            }
            LoadData();
            initialed = true;
        }

        static void LoadData()
        {
            FileInfo fileInfo = new FileInfo(configFilePath);
            if (fileInfo.Exists) {
                using (StreamReader reader = fileInfo.OpenText()) {
                    string content = reader.ReadToEnd();
                    JObject dom = JObject.Parse(content);
                    var section = dom.GetValue("lightData");
                    if (section == null) {
                        return;
                    }
                    var optionList = section.ToObject<LightDataOptions>();
                    if (optionList != null && optionList.Connections != null && optionList.Connections.Length > 0) {
                        foreach (var connection in optionList.Connections) {
                            var setting = new ConnectionSetting() {
                                Name = connection.Name,
                                ConnectionString = connection.ConnectionString,
                                ProviderName = connection.ProviderName
                            };
                            var configParam = new ConfigParamSet();
                            if (connection.ConfigParams != null && connection.ConfigParams.Length > 0) {
                                foreach (var param in connection.ConfigParams) {
                                    configParam.SetParamValue(param.Name, param.Value);
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
            }
        }

        private static void CheckData()
        {
            if (!initialed) {
                lock (locker) {
                    if (!initialed) {
                        Initial();
                    }
                }
            }
        }

        internal static DataContextOptions DefaultOptions {
            get {
                CheckData();
                if (defaultOptions == null) {
                    throw new LightDataException(SR.DefaultConfigNotExists);
                }
                return defaultOptions;
            }
        }

        internal static DataContextOptions GetOptions(string name)
        {
            CheckData();
            if (optionsDict.TryGetValue(name, out DataContextOptions options)) {
                return options;
            }
            else {
                throw new LightDataException(string.Format(SR.SpecifiedConfigNotExists, name));
            }
        }
    }
}