using Light.Data;
using Light.Data.Mssql;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Mssql options extensions
    /// </summary>
    public static class MssqlOptionsExtensions
    {
        /// <summary>
        /// Uses the mssql database.
        /// </summary>
        /// <returns>The builder.</returns>
        /// <param name="builder">Builder.</param>
        /// <param name="connection">Connection.</param>
        /// <typeparam name="TContext">Data context.</typeparam>
        public static DataContextOptionsBuilder<TContext> UseMssql<TContext>(this DataContextOptionsBuilder<TContext> builder, string connection) where TContext : DataContext
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                var database = new MssqlProvider(configName, configParams);
                return database;
            });
            return builder;
        }
    }
}
