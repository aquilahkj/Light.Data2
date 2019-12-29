using System;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;
using System.Collections.Generic;

namespace Light.Data.Postgre
{
    internal class PostgreProvider : DatabaseProvider
    {
        public PostgreProvider(string configName, ConfigParamSet configParams)
            : base(configName, configParams)
        {
            _factory = new PostgreCommandFactory();
            var strictMode = configParams.GetParamValue("strictMode");
            if (strictMode != null)
            {
                if (bool.TryParse(strictMode, out var value))
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
            var command = new NpgsqlCommand
            {
                CommandText = sql,
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DbCommand CreateCommand()
        {
            var command = new NpgsqlCommand
            {
                CommandTimeout = CommandTimeout
            };
            return command;
        }

        public override DataAdapter CreateDataAdapter(DbCommand command)
        {
            return new NpgsqlDataAdapter((NpgsqlCommand) command);
        }

        public override IDataParameter CreateParameter(string name, object value, string dbType,
            System.Data.ParameterDirection direction, Type dataType, CommandType commandType)
        {
            var parameterName = name;
            if (commandType == CommandType.StoredProcedure)
            {
                if (parameterName.StartsWith(ParameterPrefix, StringComparison.Ordinal))
                {
                    parameterName = parameterName.TrimStart(ParameterPrefix[0]);
                }
            }
            else
            {
                if (!parameterName.StartsWith(ParameterPrefix, StringComparison.Ordinal))
                {
                    parameterName = ParameterPrefix + parameterName;
                }
            }

            var sp = new NpgsqlParameter()
            {
                ParameterName = parameterName,
                Direction = direction
            };
            if (value == null)
            {
                sp.Value = DBNull.Value;
                if (string.IsNullOrEmpty(dbType) && dataType != null)
                {
                    if (ConvertDbType(dataType, out var sqlType))
                    {
                        sp.NpgsqlDbType = sqlType;
                    }
                }
            }
            else if (value is UInt16)
            {
                sp.Value = Convert.ToInt32(value);
            }
            else if (value is UInt32)
            {
                sp.Value = Convert.ToInt64(value);
            }
            else if (value is UInt64)
            {
                sp.Value = Convert.ToDecimal(value);
            }
            else
            {
                // var type = value.GetType();
                // if (type.IsEnum)
                // {
                //     var code = Type.GetTypeCode(type);
                //     sp.Value = Convert.ChangeType(value, code);
                // }
                // else
                // {
                //     sp.Value = value;
                // }
                sp.Value = value;
            }

            if (!string.IsNullOrEmpty(dbType))
            {
                if (!dbTypeDict.TryGetValue(dbType, out var info))
                {
                    lock (dbTypeDict)
                    {
                        if (!dbTypeDict.TryGetValue(dbType, out info))
                        {
                            info = new DbTypeInfo();
                            try
                            {
                                if (ParseSqlDbType(dbType, out var sqltype))
                                {
                                    info.NpgsqlDbType = sqltype;
                                }
                                else if (Utility.ParseDbType(dbType, out var dType))
                                {
                                    info.DbType = dType;
                                }

                                if (Utility.ParseSize(dbType, out var size, out var scale))
                                {
                                    info.Size = size;
                                    info.Scale = scale;
                                }
                            }
                            catch (Exception ex)
                            {
                                info.InnerException = ex;
                            }
                            finally
                            {
                                dbTypeDict.Add(dbType, info);
                            }
                        }
                    }
                }

                if (info != null)
                {
                    if (info.InnerException != null)
                    {
                        throw info.InnerException;
                    }

                    if (info.NpgsqlDbType != null)
                    {
                        sp.NpgsqlDbType = info.NpgsqlDbType.Value;
                    }
                    else if (info.DbType != null)
                    {
                        sp.DbType = info.DbType.Value;
                    }

                    if (info.Size != null)
                    {
                        if (sp.NpgsqlDbType == NpgsqlDbType.Numeric)
                        {
                            sp.Precision = (byte) info.Size.Value;
                        }
                        else
                        {
                            sp.Size = info.Size.Value;
                        }
                    }

                    if (info.Scale != null && sp.NpgsqlDbType == NpgsqlDbType.Numeric)
                    {
                        sp.Scale = info.Scale.Value;
                    }
                }
            }

            return sp;
        }

        #endregion

        private bool ParseSqlDbType(string dbType, out NpgsqlDbType type)
        {
            type = NpgsqlDbType.Varchar;
            var index = dbType.IndexOf('(');
            var typeString = string.Empty;
            if (index < 0)
            {
                typeString = dbType;
            }
            else if (index == 0)
            {
                return false;
            }
            else
            {
                typeString = dbType.Substring(0, index);
            }

            return Enum.TryParse(typeString, true, out type);
        }

        private bool ConvertDbType(Type type, out NpgsqlDbType sqlType)
        {
            var ret = true;
            if (type == typeof(Byte[]))
            {
                sqlType = NpgsqlDbType.Bytea;
            }
            else if (type == typeof(String))
            {
                sqlType = NpgsqlDbType.Varchar;
            }
            else if (type == typeof(Boolean))
            {
                sqlType = NpgsqlDbType.Boolean;
            }
            else if (type == typeof(Byte))
            {
                sqlType = NpgsqlDbType.Smallint;
            }
            else if (type == typeof(SByte))
            {
                sqlType = NpgsqlDbType.Smallint;
            }
            else if (type == typeof(Int16))
            {
                sqlType = NpgsqlDbType.Smallint;
            }
            else if (type == typeof(Int32))
            {
                sqlType = NpgsqlDbType.Integer;
            }
            else if (type == typeof(Int64))
            {
                sqlType = NpgsqlDbType.Bigint;
            }
            else if (type == typeof(UInt16))
            {
                sqlType = NpgsqlDbType.Integer;
            }
            else if (type == typeof(UInt32))
            {
                sqlType = NpgsqlDbType.Bigint;
            }
            else if (type == typeof(UInt64))
            {
                sqlType = NpgsqlDbType.Numeric;
            }
            else if (type == typeof(Single))
            {
                sqlType = NpgsqlDbType.Real;
            }
            else if (type == typeof(Double))
            {
                sqlType = NpgsqlDbType.Double;
            }
            else if (type == typeof(Decimal))
            {
                sqlType = NpgsqlDbType.Numeric;
            }
            else if (type == typeof(DateTime))
            {
                sqlType = NpgsqlDbType.Timestamp;
            }
            else
            {
                sqlType = NpgsqlDbType.Varchar;
                ret = false;
            }

            return ret;
        }

        private readonly Dictionary<string, DbTypeInfo> dbTypeDict = new Dictionary<string, DbTypeInfo>();

        private class DbTypeInfo
        {
            public NpgsqlDbType? NpgsqlDbType;
            public DbType? DbType;
            public int? Size;
            public byte? Scale;
            public Exception InnerException;
        }
    }
}