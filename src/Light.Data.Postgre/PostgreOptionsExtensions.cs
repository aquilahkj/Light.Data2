using Light.Data;
using Light.Data.Postgre;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MysqlOptionsExtensions
    {
        public static DataContextOptionsBuilder UsePostgre(this DataContextOptionsBuilder builder, string connection)
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                PostgreProvider database = new PostgreProvider(configName, configParams);
                return database;
            });
            return builder;
        }

        public static DataContextOptionsBuilder<TContext> UsePostgre<TContext>(this DataContextOptionsBuilder<TContext> builder, string connection) where TContext : DataContext
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                PostgreProvider database = new PostgreProvider(configName, configParams);
                return database;
            });
            return builder;
        }
    }
}
