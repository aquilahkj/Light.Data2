namespace Light.Data
{
    internal class SpecifiedDataFieldInfo : LightDataFieldInfo, IAliasDataFieldInfo
    {
        public SpecifiedDataFieldInfo(DataFieldInfo fieldInfo, string name)
            : base(fieldInfo.TableMapping, true, name)
        {
            FieldInfo = fieldInfo;
        }

        public DataFieldInfo FieldInfo { get; }

        public override DataFieldInfo CreateAliasTableInfo(string aliasTableName)
        {
			var info = new DataFieldInfo(FieldInfo.TableMapping, true, FieldName, aliasTableName);
			return info;

		}

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (isFullName) {
                if (_aliasTableName != null) {
                    return factory.CreateFullDataFieldSql(_aliasTableName, FieldName);
                }
                else {
                    return factory.CreateFullDataFieldSql(TableMapping, FieldName, state);
                }
            }
            else {
                return factory.CreateDataFieldSql(FieldName);
            }
        }

        public string CreateAliasDataFieldSql(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var fieldSql = FieldInfo.CreateSqlString(factory, isFullName, state);
            var sql = factory.CreateAliasFieldSql(fieldSql, FieldName);
            return sql;
        }
    }
}
