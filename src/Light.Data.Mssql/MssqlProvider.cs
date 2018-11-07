using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Light.Data.Mssql
{
    class MssqlProvider : DatabaseProvider
    {
        public MssqlProvider(string configName, ConfigParamSet configParams)
            : base(configName, configParams)
        {
            string version = configParams.GetParamValue("version");
            MssqlCommandFactory mssqlCommandFactory = null;
            if (!string.IsNullOrWhiteSpace(version)) {
                string[] arr = version.Split('.');
                string vc;
                if (arr.Length > 1) {
                    vc = string.Concat(arr[0].Trim(), ".", arr[1].Trim());
                }
                else {
                    vc = arr[0].Trim();
                }
                if (decimal.TryParse(vc, out decimal v)) {
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
            string strictMode = configParams.GetParamValue("strictMode");
            if (strictMode != null) {
                if (bool.TryParse(strictMode, out bool value))
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
            SqlCommand command = new SqlCommand() {
                CommandText = sql,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DbCommand CreateCommand()
        {
            SqlCommand command = new SqlCommand() {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DataAdapter CreateDataAdapter(DbCommand command)
        {
            return new SqlDataAdapter((SqlCommand)command);
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType, ParameterDirection direction, Type dataType)
        {
            string parameterName = name;
            if (!parameterName.StartsWith("@", StringComparison.Ordinal)) {
                parameterName = "@" + parameterName;
            }
            SqlParameter sp = new SqlParameter() {
                ParameterName = parameterName,
                Direction = direction
            };
            bool settype = false;
            if (value == null) {
                sp.Value = DBNull.Value;
                if (dbType == null && dataType != null) {
                    if (ConvertDbType(dataType, out SqlDbType sqlType)) {
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
            if (!settype && !string.IsNullOrEmpty(dbType)) {
                if (ParseSqlDbType(dbType, out System.Data.SqlDbType sqltype)) {
                    sp.SqlDbType = sqltype;
                }
                else if (Utility.ParseDbType(dbType, out DbType dType)) {
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

        }

        #endregion

        bool ConvertDbType(Type type, out SqlDbType sqlType)
        {
            bool ret = true;
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
            else if (type == typeof(Decimal)) {
                sqlType = SqlDbType.Decimal;
            }
            else if (type == typeof(Double)) {
                sqlType = SqlDbType.Float;
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
            int index = dbType.IndexOf('(');
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
    }
}

