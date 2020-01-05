using System;

namespace Light.Data
{
    /// <summary>
    /// DataContext Options
    /// </summary>
    public class DataContextOptions
    {
        internal static DataContextOptions CreateOptions(IConnectionSetting setting)
        {
            if (setting == null) {
                throw new ArgumentNullException(nameof(setting));
            }

            var connection = setting.ConnectionString;

            var type = Type.GetType(setting.ProviderName, true);

            var dataBase = (DatabaseProvider)Activator.CreateInstance(type, setting.Name, setting.ConfigParam);
            if (dataBase == null) {
                throw new LightDataException(string.Format(SR.TypeIsNotDatabaseType, type.FullName));
            }
            var contextOptions = new DataContextOptions
            {
                Connection = connection,
                Database = dataBase
            };
            return contextOptions;
        }


        internal DataContextOptions()
        {

        }

        internal string Connection {
            get;
            set;
        }

        internal DatabaseProvider Database {
            get;
            set;
        }

        internal ICommandOutput CommandOutput {
            get;
            set;
        }
    }

    /// <summary>
    /// DataContext Options
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public sealed class DataContextOptions<TContext> : DataContextOptions where TContext : DataContext
    {

    }
}
