namespace Light.Data
{
    class SpecifiedDataFieldInfo : LightDataFieldInfo, IAliasDataFieldInfo
    {
        readonly DataFieldInfo _fieldInfo;

        public SpecifiedDataFieldInfo(DataFieldInfo fieldInfo, string name)
            : base(fieldInfo.TableMapping, true, name)
        {
            _fieldInfo = fieldInfo;
        }

        public DataFieldInfo FieldInfo {
            get {
                return _fieldInfo;
            }
        }

        public override DataFieldInfo CreateAliasTableInfo(string aliasTableName)
        {
			//DataFieldInfo info = _fieldInfo.CreateAliasTableInfo(aliasTableName);
			//SpecifiedDataFieldInfo newinfo = new SpecifiedDataFieldInfo(info, FieldName);
			//newinfo._aliasTableName = aliasTableName;
			//return newinfo;
			DataFieldInfo info = new DataFieldInfo(_fieldInfo.TableMapping, true, FieldName, aliasTableName);
			return info;

		}

        internal override string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            if (isFullName) {
                if (this._aliasTableName != null) {
                    return factory.CreateFullDataFieldSql(this._aliasTableName, FieldName);
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
            string fieldSql = _fieldInfo.CreateSqlString(factory, isFullName, state);
            string sql = factory.CreateAliasFieldSql(fieldSql, FieldName);
            return sql;
        }
    }
}
