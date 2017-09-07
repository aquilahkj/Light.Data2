using Light.Data;
using Light.Data.Mysql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MysqlOptionsExtensions
    {
        public static DataContextOptionsBuilder UseMysql(this DataContextOptionsBuilder builder, string connection)
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                MysqlProvider database = new MysqlProvider(configName, configParams);
                return database;
            });
            return builder;
        }

        public static DataContextOptionsBuilder<TContext> UseMysql<TContext>(this DataContextOptionsBuilder<TContext> builder, string connection) where TContext : DataContext
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                MysqlProvider database = new MysqlProvider(configName, configParams);
                return database;
            });
            return builder;
        }
    }
}
