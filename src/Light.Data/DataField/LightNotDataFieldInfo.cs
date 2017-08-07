namespace Light.Data
{
	class LightNotDataFieldInfo : LightDataFieldInfo
	{
		readonly DataFieldInfo _baseFieldInfo;

		public LightNotDataFieldInfo (DataFieldInfo info)
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

			sql = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = factory.CreateNotSql (sql);

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

