using System;

namespace Light.Data
{
    internal class DataContextSetting
    {
        internal static DataContextSetting CreateSetting(IConnectionSetting setting, bool throwOnError)
        {
            if (setting == null) {
                throw new ArgumentNullException(nameof(setting));
            }

            var connection = setting.ConnectionString;

            var type = Type.GetType(setting.ProviderName, throwOnError);

            if (type == null) {
                return null;
            }

            if (!throwOnError) {
                var dataBaseType = typeof(DatabaseProvider);
                if (!type.IsInherit(dataBaseType)) {
                    return null;
                }
            }

            var dataBase = Activator.CreateInstance(type, setting.Name, setting.ConfigParam) as DatabaseProvider;
            if (dataBase == null)
            {
                if (!throwOnError) {
                    return null;
                }

                throw new LightDataException(string.Format(SR.TypeIsNotDatabaseType, type.FullName));
            }
            //dataBase.SetExtendParams(setting.ConfigParam);
            var context = new DataContextSetting(connection, dataBase);
            return context;
        }

        public DatabaseProvider DataBase { get; }

        public string Name => DataBase.ConfigName;

        public string Connection { get; }

        public DataContextSetting(string connection, DatabaseProvider dataBase)
        {
            Connection = connection;
            DataBase = dataBase;
        }
    }
}

