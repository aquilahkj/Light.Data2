using Light.Data;
using Light.Data.Mssql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MssqlOptionsExtensions
    {
        public static DataContextOptionsBuilder<TContext> UseMssql<TContext>(this DataContextOptionsBuilder<TContext> builder, string connection) where TContext : DataContext
        {
            builder.SetDataConfig(connection, (configName, configParams) => {
                MssqlProvider database = new MssqlProvider(configName, configParams);
                return database;
            });
            return builder;
        }
    }
}
