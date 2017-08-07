using System;

namespace Light.Data
{
    class DataContextSetting
    {
        internal static DataContextSetting CreateSetting(IConnectionSetting setting, bool throwOnError)
        {
            if (setting == null) {
                throw new ArgumentNullException(nameof(setting));
            }
            Type type;
            string connection = setting.ConnectionString;

            type = Type.GetType(setting.ProviderName, throwOnError);

            if (type == null) {
                return null;
            }

            if (!throwOnError) {
                Type dataBaseType = typeof(DatabaseProvider);
                if (!type.IsInherit(dataBaseType)) {
                    return null;
                }
            }

            DatabaseProvider dataBase = Activator.CreateInstance(type, setting.Name, setting.ConfigParam) as DatabaseProvider;
            if (dataBase == null) {
                if (!throwOnError) {
                    return null;
                }
                else {
                    throw new LightDataException(string.Format(SR.TypeIsNotDatabaseType, type.FullName));
                }
            }
            //dataBase.SetExtendParams(setting.ConfigParam);
            DataContextSetting context = new DataContextSetting(connection, dataBase);
            return context;
        }

        readonly DatabaseProvider dataBase;

        public DatabaseProvider DataBase {
            get {
                return dataBase;
            }
        }

        public string Name {
            get {
                return dataBase.ConfigName;
            }
        }

        readonly string connection;

        public string Connection {
            get {
                return connection;
            }
        }

        public DataContextSetting(string connection, DatabaseProvider dataBase)
        {
            this.connection = connection;
            this.dataBase = dataBase;
        }
    }
}

