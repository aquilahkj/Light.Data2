using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    internal class CreateSqlState
    {
        //readonly CommandFactory _factory;

        //public CreateSqlState(CommandFactory factory)
        //{
        //    this._factory = factory;
        //}

        private readonly DataContext context;

        public CreateSqlState(DataContext context)
        {
            this.context = context;
        }

        public CreateSqlState(DataContext context, bool useDirectNull)
        {
            this.context = context;
            UseDirectNull = useDirectNull;
        }

        public bool TryGetAliasTableName(DataEntityMapping mapping, out string name)
        {
            return context.TryGetAliasTableName(mapping, out name);
        }

        private class ObjectData
        {
            public string Full;

            public string Normal;
        }

        public int Seed { get; private set; }

        private string GetNextParameterName(CommandFactory factory)
        {
            Seed++;
            return factory.CreateParamName("P" + Seed);
        }

        private Dictionary<object, ObjectData> dict = new Dictionary<object, ObjectData>();

        private List<DataParameter> parameters = new List<DataParameter>();

        private Dictionary<object, string> aliasDict = new Dictionary<object, string>();

        public bool UseFieldAlias { get; set; }

        public void SetAliasData(object obj, string alias)
        {
            if (!aliasDict.ContainsKey(obj)) {
                aliasDict.Add(obj, alias);
            }
        }

        public string GetDataSql(object obj, bool isFullName)
        {
            if (UseFieldAlias) {
                if (aliasDict.TryGetValue(obj, out var sql)) {
                    return sql;
                }
            }

            if (dict.TryGetValue(obj, out var data)) {
                if (isFullName) {
                    return data.Full;
                }
                else {
                    return data.Normal;
                }
            }
            else {
                return null;
            }
        }

        public void SetDataSql(object obj, bool isFullName, string sql)
        {
            if (dict.TryGetValue(obj, out var data)) {
                if (isFullName) {
                    if (data.Full != null)
                        data.Full = sql;
                }
                else {
                    if (data.Normal != null)
                        data.Normal = sql;
                }
            }
            else {
                data = new ObjectData();
                if (isFullName) {
                    data.Full = sql;
                }
                else {
                    data.Normal = sql;
                }
                dict[obj] = data;
            }
        }

        public bool UseDirectNull { get; } = true;

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        /// <param name="dataType">Data type.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType, ParameterDirection direction, Type dataType)
        {
            if (UseDirectNull) {
                if (Equals(paramValue, null)) {
                    return factory.Null;
                }
            }
            var paramName = GetNextParameterName(factory);
            var dataParameter = new DataParameter(paramName, paramValue, dbType, direction, dataType);
            parameters.Add(dataParameter);
            return paramName;
        }

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="dataType">Data type.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType, Type dataType)
        {
            if (UseDirectNull) {
                if (Equals(paramValue, null)) {
                    return factory.Null;
                }
            }
            var paramName = GetNextParameterName(factory);
            var dataParameter = new DataParameter(paramName, paramValue, dbType, ParameterDirection.Input, dataType);
            parameters.Add(dataParameter);
            return paramName;
        }

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType, ParameterDirection direction)
        {
            return AddDataParameter(factory, paramValue, dbType, direction, null);
        }

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue)
        {
            return AddDataParameter(factory, paramValue, null, ParameterDirection.Input, null);
        }

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="direction">Direction.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, ParameterDirection direction)
        {
            return AddDataParameter(factory, paramValue, null, direction);
        }

        /// <summary>
        /// Add the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType)
        {
            return AddDataParameter(factory, paramValue, dbType, ParameterDirection.Input);
        }

        public DataParameter[] GetDataParameters()
        {
            return parameters.ToArray();
        }
    }
}

