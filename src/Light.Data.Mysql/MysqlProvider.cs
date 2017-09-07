using System;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Light.Data.Mysql
{
    class MysqlProvider : DatabaseProvider
    {
        public MysqlProvider(string configName, ConfigParamSet configParams)
            : base(configName, configParams)
        {
            _factory = new MysqlCommandFactory();
        }

        #region IDatabase 成员

        public override DbConnection CreateConnection()
        {
            return new MySqlConnection();
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override DbCommand CreateCommand(string sql)
        {
            MySqlCommand command = new MySqlCommand() {
                CommandText = sql,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DbCommand CreateCommand()
        {
            MySqlCommand command = new MySqlCommand() {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction)
        {
            string parameterName = name;
            if (!parameterName.StartsWith("?", StringComparison.Ordinal)) {
                parameterName = "?" + parameterName;
            }
            MySqlParameter sp = new MySqlParameter() {
                ParameterName = parameterName,
                Direction = direction
            };
            if (value == null) {
                sp.Value = DBNull.Value;
            }
            else {
                sp.Value = value;
            }
            if (!string.IsNullOrEmpty(dbType)) {
                //if (ParseSqlDbType(dbType, out MySqlDbType sqltype)) {
                //    sp.MySqlDbType = sqltype;
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

        public override void FormatStoredProcedureParameter(IDataParameter dataParameter)
        {
            if (dataParameter.ParameterName.StartsWith("?", StringComparison.Ordinal)) {
                dataParameter.ParameterName = dataParameter.ParameterName.TrimStart('?');
            }
        }

        #endregion

        //bool ParseSqlDbType(string dbType, out MySqlDbType type)
        //{
        //    type = MySqlDbType.VarChar;
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
