using System;
using System.Collections.Generic;
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
            string strictMode = configParams.GetParamValue("strictMode");
            if (strictMode != null) {
                if (bool.TryParse(strictMode, out bool value))
                    _factory.SetStrictMode(value);
            }
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

        public override DataAdapter CreateDataAdapter(DbCommand command)
        {
            return new MySqlDataAdapter((MySqlCommand)command);
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, Type dataType, CommandType commandType)
        {
            string parameterName = name;
            if (commandType == CommandType.StoredProcedure) {
                if (parameterName.StartsWith(ParameterPrefix, StringComparison.Ordinal)) {
                    parameterName = parameterName.TrimStart(ParameterPrefix[0]);
                }
            }
            else {
                if (!parameterName.StartsWith(ParameterPrefix, StringComparison.Ordinal)) {
                    parameterName = ParameterPrefix + parameterName;
                }
            }
            MySqlParameter sp = new MySqlParameter() {
                ParameterName = parameterName,
                Direction = direction
            };
            if (value == null) {
                sp.Value = DBNull.Value;
                if (string.IsNullOrEmpty(dbType) && dataType != null) {
                    if (ConvertDbType(dataType, out MySqlDbType sqlType)) {
                        sp.MySqlDbType = sqlType;
                    }
                }
            }
            else {
                sp.Value = value;
            }
            if (!string.IsNullOrEmpty(dbType)) {
                if (!dbTypeDict.TryGetValue(dbType, out DbTypeInfo info)) {
                    lock (dbTypeDict) {
                        if (!dbTypeDict.TryGetValue(dbType, out info)) {
                            info = new DbTypeInfo();
                            try {
                                if (ParseSqlDbType(dbType, out MySqlDbType sqltype)) {
                                    info.MySqlDbType = sqltype;
                                }
                                else if (Utility.ParseDbType(dbType, out DbType dType)) {
                                    info.DbType = dType;
                                }
                                if (Utility.ParseSize(dbType, out int size, out byte? scale)) {
                                    info.Size = size;
                                    info.Scale = scale;
                                }
                            }
                            catch (Exception ex) {
                                info.InnerException = ex;
                            }
                            finally {
                                dbTypeDict.Add(dbType, info);
                            }
                        }
                    }
                }
                if (info != null) {
                    if (info.InnerException != null) {
                        throw info.InnerException;
                    }
                    if (info.MySqlDbType != null) {
                        sp.MySqlDbType = info.MySqlDbType.Value;
                    }
                    else if (info.DbType != null) {
                        sp.DbType = info.DbType.Value;
                    }
                    if (info.Size != null) {
                        if (sp.MySqlDbType == MySqlDbType.Decimal) {
                            sp.Precision = (byte)info.Size.Value;
                        }
                        else {
                            sp.Size = info.Size.Value;
                        }
                    }
                    if (info.Scale != null && sp.MySqlDbType == MySqlDbType.Decimal) {
                        sp.Scale = info.Scale.Value;
                    }
                }
            }
            return sp;
        }
        
        #endregion

        bool ConvertDbType(Type type, out MySqlDbType sqlType)
        {
            bool ret = true;
            if (type == typeof(Byte[])) {
                sqlType = MySqlDbType.VarBinary;
            }
            else if (type == typeof(String)) {
                sqlType = MySqlDbType.VarChar;
            }
            else if (type == typeof(Boolean)) {
                sqlType = MySqlDbType.Bit;
            }
            else if (type == typeof(Byte)) {
                sqlType = MySqlDbType.UByte;
            }
            else if (type == typeof(SByte)) {
                sqlType = MySqlDbType.Byte;
            }
            else if (type == typeof(Int16)) {
                sqlType = MySqlDbType.Int16;
            }
            else if (type == typeof(Int32)) {
                sqlType = MySqlDbType.Int32;
            }
            else if (type == typeof(Int64)) {
                sqlType = MySqlDbType.Int64;
            }
            else if (type == typeof(UInt16)) {
                sqlType = MySqlDbType.UInt16;
            }
            else if (type == typeof(UInt32)) {
                sqlType = MySqlDbType.UInt32;
            }
            else if (type == typeof(UInt64)) {
                sqlType = MySqlDbType.UInt64;
            }
            else if (type == typeof(Single)) {
                sqlType = MySqlDbType.Float;
            }
            else if (type == typeof(Double)) {
                sqlType = MySqlDbType.Double;
            }
            else if (type == typeof(Decimal)) {
                sqlType = MySqlDbType.Decimal;
            }
            else if (type == typeof(DateTime)) {
                sqlType = MySqlDbType.DateTime;
            }
            else {
                sqlType = MySqlDbType.VarChar;
                ret = false;
            }
            return ret;
        }

        bool ParseSqlDbType(string dbType, out MySqlDbType type)
        {
            type = MySqlDbType.VarChar;
            int index = dbType.IndexOf('(');
            string typeString = string.Empty;
            if (index < 0) {
                typeString = dbType;
            }
            else if (index == 0) {
                return false;
            }
            else {
                typeString = dbType.Substring(0, index);
            }
            return Enum.TryParse(typeString, true, out type);
        }

        Dictionary<string, DbTypeInfo> dbTypeDict = new Dictionary<string, DbTypeInfo>();

        class DbTypeInfo
        {
            public MySqlDbType? MySqlDbType;
            public DbType? DbType;
            public int? Size;
            public byte? Scale;
            public Exception InnerException;
        }
    }
}
