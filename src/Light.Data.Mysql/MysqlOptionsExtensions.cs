using Light.Data;
using Light.Data.Mysql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Mysql options extensions.
    /// </summary>
    public static class MysqlOptionsExtensions
    {
        /// <summary>
        /// Uses the mysql database.
        /// </summary>
        /// <returns>The builder.</returns>
        /// <param name="builder">Builder.</param>
        /// <param name="connection">Connection.</param>
        /// <typeparam name="TContext">Data context.</typeparam>
        public static DataContextOptionsBuilder<TContext> UseMysql<TContext>(this DataContextOptionsBuilder<TContext> builder, string connection) where TContext : DataContext
        {
            builder.SetDataConfig(connection, (configName, configParams) =>
            {
                var database = new MysqlProvider(configName, configParams);
                return database;
            });
            return builder;
        }
    }
}