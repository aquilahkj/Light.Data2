namespace Light.Data
{
	class LightDateDataFieldInfo : LightDataFieldInfo
	{
		readonly DataFieldInfo _baseFieldInfo;

		public LightDateDataFieldInfo (DataFieldInfo info)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			string field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = factory.CreateDateSql (field);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

