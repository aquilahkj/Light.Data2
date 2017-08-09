using System;
using System.Collections.Generic;
using System.Text;

namespace Light.Data
{
    public class DataContextOptions
    {
        internal static DataContextOptions CreateOptions(IConnectionSetting setting)
        {
            if (setting == null) {
                throw new ArgumentNullException(nameof(setting));
            }
            Type type;
            string connection = setting.ConnectionString;

            type = Type.GetType(setting.ProviderName, true);

            if (type == null) {
                return null;
            }

            DatabaseProvider dataBase = (DatabaseProvider)Activator.CreateInstance(type, setting.Name, setting.ConfigParam) as DatabaseProvider;
            if (dataBase == null) {
                throw new LightDataException(string.Format(SR.TypeIsNotDatabaseType, type.FullName));
            }
            DataContextOptions contextOptions = new DataContextOptions() {
                Connection = connection,
                Database = dataBase
            };
            return contextOptions;
        }


        public DataContextOptions()
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


    public sealed class DataContextOptions<TContext> : DataContextOptions where TContext : DataContext
    {

    }
}
