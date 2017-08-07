namespace Light.Data
{
	class LightStringLengthDataFieldInfo : LightDataFieldInfo
	{
		readonly DataFieldInfo _baseFieldInfo;

		public LightStringLengthDataFieldInfo (DataFieldInfo info)
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
			sql = factory.CreateLengthSql (field);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

