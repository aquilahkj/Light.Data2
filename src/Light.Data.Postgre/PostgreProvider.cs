using System;
using System.Data;
using Npgsql;
using System.Data.Common;

namespace Light.Data.Postgre
{
    class PostgreProvider : DatabaseProvider
    {
        public PostgreProvider(string configName, ConfigParamSet configParams)
            : base(configName, configParams)
        {
             _factory = new PostgreCommandFactory();
            string strictMode = configParams.GetParamValue("strictMode");
            if (strictMode != null) {
                if (bool.TryParse(strictMode, out bool value))
                    _factory.SetStrictMode(value);
            }
        }
        
        #region implemented abstract members of Database

        public override DbConnection CreateConnection()
        {
            return new NpgsqlConnection();
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public override DbCommand CreateCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand {
                CommandText = sql,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DbCommand CreateCommand()
        {
            NpgsqlCommand command = new NpgsqlCommand {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction)
        {
            string parameterName = name;
            if (!parameterName.StartsWith(":", StringComparison.Ordinal)) {
                parameterName = ":" + parameterName;
            }
            NpgsqlParameter sp = new NpgsqlParameter() {
                ParameterName = parameterName,
                Direction = direction
            };
            if (value == null) {
                sp.Value = DBNull.Value;
            }
            else if (value is UInt16) {
                sp.Value = Convert.ToInt32(value);
            }
            else if (value is UInt32) {
                sp.Value = Convert.ToInt64(value);
            }
            else if (value is UInt64) {
                sp.Value = Convert.ToDecimal(value);
            }
            else {
                sp.Value = value;
            }
            if (!string.IsNullOrEmpty(dbType)) {
                //if (ParseSqlDbType(dbType, out NpgsqlDbType sqltype)) {
                //    sp.NpgsqlDbType = sqltype;
                //}
                //else 
                if (Utility.ParseDbType(dbType, out DbType dType)) {
                    sp.DbType = dType;
                }
                if (Utility.ParseSize(dbType, out int size)) {
                    sp.Size = size;
                }
            }
            return sp;
        }

        public override void FormatStoredProcedureParameter(IDataParameter dataParmeter)
        {
            if (dataParmeter.ParameterName.StartsWith(":", StringComparison.Ordinal)) {
                dataParmeter.ParameterName = dataParmeter.ParameterName.TrimStart(':');
            }
        }

        #endregion

        //bool ParseSqlDbType(string dbType, out NpgsqlDbType type)
        //{
        //    type = NpgsqlDbType.Varchar;
        //    int index = dbType.IndexOf('(');
        //    string typeString = string.Empty;
        //    if (index < 0) {
        //        typeString = dbType;
        //    }
        //    else if (index == 0) {
        //        return false;
        //    }
        //    else {
        //        typeString = dbType.Substring(0, index);
        //    }
        //    return Enum.TryParse(typeString, true, out type);
        //}
    }
}

