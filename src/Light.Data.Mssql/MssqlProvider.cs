using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Light.Data.Mssql
{
    internal class MssqlProvider : DatabaseProvider
    {
        public MssqlProvider(string configName, ConfigParamSet configParams)
            : base(configName, configParams)
        {
            var version = configParams.GetParamValue("version");
            MssqlCommandFactory mssqlCommandFactory = null;
            if (!string.IsNullOrWhiteSpace(version)) {
                var arr = version.Split('.');
                string vc;
                if (arr.Length > 1) {
                    vc = string.Concat(arr[0].Trim(), ".", arr[1].Trim());
                }
                else {
                    vc = arr[0].Trim();
                }
                if (decimal.TryParse(vc, out var v)) {
                    if (v >= 11) {
                        mssqlCommandFactory = new MssqlCommandFactory_2012();
                    }
                    else if (v >= 10) {
                        mssqlCommandFactory = new MssqlCommandFactory_2008();
                    }
                    else {
                        mssqlCommandFactory = new MssqlCommandFactory();
                    }
                }
            }
            _factory = mssqlCommandFactory ?? new MssqlCommandFactory_2008();
            var strictMode = configParams.GetParamValue("strictMode");
            if (strictMode != null) {
                if (bool.TryParse(strictMode, out var value))
                    _factory.SetStrictMode(value);
            }
        }

        #region IDatabase 成员

        public override DbConnection CreateConnection()
        {
            return new SqlConnection();
        }

        public override DbConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public override DbCommand CreateCommand(string sql)
        {
            var command = new SqlCommand() {
                CommandText = sql,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DbCommand CreateCommand()
        {
            var command = new SqlCommand() {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DataAdapter CreateDataAdapter(DbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, Type dataType, CommandType commandType)
        {
            var parameterName = name;
            if (!parameterName.StartsWith(ParameterPrefix, StringComparison.Ordinal)) {
                parameterName = ParameterPrefix + parameterName;
            }
            var sp = new SqlParameter() {
                ParameterName = parameterName,
                Direction = direction
            };
            if (value == null) {
                sp.Value = DBNull.Value;
                if (string.IsNullOrEmpty(dbType) && dataType != null) {
                    if (ConvertDbType(dataType, out var sqlType)) {
                        sp.SqlDbType = sqlType;
                    }
                }
            }
            else if (value is SByte) {
                sp.Value = Convert.ToInt16(value);
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
                if (!dbTypeDict.TryGetValue(dbType, out var info)) {
                    lock (dbTypeDict) {
                        if (!dbTypeDict.TryGetValue(dbType, out info)) {
                            info = new DbTypeInfo();
                            try {
                                if (ParseSqlDbType(dbType, out var sqltype)) {
                                    info.SqlDbType = sqltype;
                                }
                                else if (Utility.ParseDbType(dbType, out var dType)) {
                                    info.DbType = dType;
                                }
                                if (Utility.ParseSize(dbType, out var size, out var scale)) {
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
                    if (info.SqlDbType != null) {
                        sp.SqlDbType = info.SqlDbType.Value;
                    }
                    else if (info.DbType != null) {
                        sp.DbType = info.DbType.Value;
                    }
                    if (info.Size != null) {
                        if (sp.SqlDbType == SqlDbType.Decimal) {
                            sp.Precision = (byte)info.Size.Value;
                        }
                        else {
                            sp.Size = info.Size.Value;
                        }
                    }
                    if (info.Scale != null) {
                        sp.Scale = info.Scale.Value;
                    }
                }
            }

            return sp;
        }
        
        #endregion

        private bool ConvertDbType(Type type, out SqlDbType sqlType)
        {
            var ret = true;
            if (type == typeof(Byte[])) {
                sqlType = SqlDbType.VarBinary;
            }
            else if (type == typeof(String)) {
                sqlType = SqlDbType.VarChar;
            }
            else if (type == typeof(Boolean)) {
                sqlType = SqlDbType.Bit;
            }
            else if (type == typeof(Byte)) {
                sqlType = SqlDbType.TinyInt;
            }
            else if (type == typeof(SByte)) {
                sqlType = SqlDbType.SmallInt;
            }
            else if (type == typeof(Int16)) {
                sqlType = SqlDbType.SmallInt;
            }
            else if (type == typeof(Int32)) {
                sqlType = SqlDbType.Int;
            }
            else if (type == typeof(Int64)) {
                sqlType = SqlDbType.BigInt;
            }
            else if (type == typeof(UInt16)) {
                sqlType = SqlDbType.Int;
            }
            else if (type == typeof(UInt32)) {
                sqlType = SqlDbType.BigInt;
            }
            else if (type == typeof(UInt64)) {
                sqlType = SqlDbType.Decimal;
            }
            else if (type == typeof(Single)) {
                sqlType = SqlDbType.Real;
            }
            else if (type == typeof(Double)) {
                sqlType = SqlDbType.Float;
            }
            else if (type == typeof(Decimal)) {
                sqlType = SqlDbType.Decimal;
            }
            else if (type == typeof(DateTime)) {
                sqlType = SqlDbType.DateTime;
            }
            else {
                sqlType = SqlDbType.VarChar;
                ret = false;
            }
            return ret;
        }

        private static bool ParseSqlDbType(string dbType, out SqlDbType type)
        {
            type = SqlDbType.VarChar;
            var index = dbType.IndexOf('(');
            string typeString;
            if (index < 0) {
                typeString = dbType;
            }
            else if (index == 0) {
                return false;
            }
            else {
                typeString = dbType.Substring(0, index);
            }
            try {
                type = (SqlDbType)Enum.Parse(typeof(SqlDbType), typeString, true);
                return true;
            }
            catch {
                return false;
            }
        }

        private readonly Dictionary<string, DbTypeInfo> dbTypeDict = new Dictionary<string, DbTypeInfo>();

        private class DbTypeInfo
        {
            public SqlDbType? SqlDbType;
            public DbType? DbType;
            public int? Size;
            public byte? Scale;
            public Exception InnerException;
        }
    }
}

