using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
    class CreateSqlState
    {
        //readonly CommandFactory _factory;

        //public CreateSqlState(CommandFactory factory)
        //{
        //    this._factory = factory;
        //}

        readonly DataContext context;

        public CreateSqlState(DataContext context)
        {
            this.context = context;
        }

        public bool TryGetAliasTableName(DataEntityMapping mapping, out string name)
        {
            return context.TryGetAliasTableName(mapping, out name);
        }

        class ObjectData
        {
            public string Full;

            public string Normal;
        }

        int seed;

        string GetNextParameterName(CommandFactory factory)
        {
            seed++;
            return factory.CreateParamName("P" + seed);
        }

        Dictionary<object, ObjectData> dict = new Dictionary<object, ObjectData>();

        List<DataParameter> parameters = new List<DataParameter>();

        Dictionary<object, string> aliasDict = new Dictionary<object, string>();

        bool useFieldAlias;

        public bool UseFieldAlias {
            get {
                return useFieldAlias;
            }

            set {
                useFieldAlias = value;
            }
        }

        public void SetAliasData(object obj, string alias)
        {
            if (!aliasDict.ContainsKey(obj)) {
                aliasDict.Add(obj, alias);
            }
        }

        public string GetDataSql(object obj, bool isFullName)
        {
            if (useFieldAlias) {
                if (aliasDict.TryGetValue(obj, out string sql)) {
                    return sql;
                }
            }

            if (dict.TryGetValue(obj, out ObjectData data)) {
                if (isFullName) {
                    return data.Full;
                } else {
                    return data.Normal;
                }
            } else {
                return null;
            }
        }

        public void SetDataSql(object obj, bool isFullName, string sql)
        {
            if (dict.TryGetValue(obj, out ObjectData data)) {
                if (isFullName) {
                    if (data.Full != null)
                        data.Full = sql;
                } else {
                    if (data.Normal != null)
                        data.Normal = sql;
                }
            } else {
                data = new ObjectData();
                if (isFullName) {
                    data.Full = sql;
                } else {
                    data.Normal = sql;
                }
                dict[obj] = data;
            }
        }

        /// <summary>
        /// Adds the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        /// <param name="direction">Direction.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType, DataParameterMode direction)
        {
            if (Object.Equals(paramValue, null)) {
                return factory.Null;
            }
            string paramName = GetNextParameterName(factory);
            DataParameter dataParameter = new DataParameter(paramName, paramValue, dbType, direction);
            parameters.Add(dataParameter);
            return paramName;
        }

        /// <summary>
        /// Adds the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue)
        {
            return AddDataParameter(factory, paramValue, null, DataParameterMode.Input);
        }

        /// <summary>
        /// Adds the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="direction">Direction.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, DataParameterMode direction)
        {
            return AddDataParameter(factory, paramValue, null, direction);
        }

        /// <summary>
        /// Adds the data parameter.
        /// </summary>
        /// <returns>The data parameter.</returns>
        /// <param name="factory">Factory.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <param name="dbType">Db type.</param>
        public string AddDataParameter(CommandFactory factory, object paramValue, string dbType)
        {
            return AddDataParameter(factory, paramValue, dbType, DataParameterMode.Input);
        }

        public DataParameter[] GetDataParameters()
        {
            return parameters.ToArray();
        }
    }
}

