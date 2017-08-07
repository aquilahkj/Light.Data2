namespace Light.Data
{
    class AggregateDataFieldInfo : LightDataFieldInfo, IAliasDataFieldInfo
    {
        readonly DataFieldInfo _fieldInfo;

        readonly string _aggregateName;

        readonly bool _aggregate;

        public AggregateDataFieldInfo(DataFieldInfo fieldInfo, string name, bool aggregate)
            : base(fieldInfo.TableMapping, true, name)
        {
            _fieldInfo = fieldInfo;
            _aggregateName = name;
            _aggregate = aggregate;
        }

        public DataFieldInfo FieldInfo {
            get {
                return _fieldInfo;
            }
        }

        public string AggregateName {
            get {
                return _aggregateName;
            }
        }

        public bool Aggregate {
            get {
                return _aggregate;
            }
        }

        public override DataFieldInfo CreateAliasTableInfo(string aliasTableName)
        {
            DataFieldInfo info = new DataFieldInfo(_fieldInfo.TableMapping, true, _aggregateName, aliasTableName);
            return info;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (state.UseFieldAlias) {
                return factory.CreateDataFieldSql(_aggregateName);
            }
            else {
                return _fieldInfo.CreateSqlString(factory, isFullName, state);
            }
        }

        internal string CreateGroupBySqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            return _fieldInfo.CreateSqlString(factory, isFullName, state);
        }

        public string CreateAliasDataFieldSql(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            string fieldSql = _fieldInfo.CreateSqlString(factory, isFullName, state);
            string sql = factory.CreateAliasFieldSql(fieldSql, _aggregateName);
            return sql;
        }
    }
}

