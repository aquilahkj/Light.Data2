namespace Light.Data
{
    internal class AggregateDataFieldInfo : LightDataFieldInfo, IAliasDataFieldInfo
    {
        public AggregateDataFieldInfo(DataFieldInfo fieldInfo, string name, bool aggregate)
            : base(fieldInfo.TableMapping, true, name)
        {
            FieldInfo = fieldInfo;
            AggregateName = name;
            Aggregate = aggregate;
        }

        public DataFieldInfo FieldInfo { get; }

        public string AggregateName { get; }

        public bool Aggregate { get; }

        public override DataFieldInfo CreateAliasTableInfo(string aliasTableName)
        {
            var info = new DataFieldInfo(FieldInfo.TableMapping, true, AggregateName, aliasTableName);
            return info;
        }

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (state.UseFieldAlias) {
                return factory.CreateDataFieldSql(AggregateName);
            }

            return FieldInfo.CreateSqlString(factory, isFullName, state);
        }

        internal string CreateGroupBySqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            return FieldInfo.CreateSqlString(factory, isFullName, state);
        }

        public string CreateAliasDataFieldSql(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var fieldSql = FieldInfo.CreateSqlString(factory, isFullName, state);
            var sql = factory.CreateAliasFieldSql(fieldSql, AggregateName);
            return sql;
        }
    }
}

