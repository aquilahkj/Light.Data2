namespace Light.Data
{
	class LightQueryDataFieldInfo : LightDataFieldInfo
	{
		readonly QueryExpression _query;

		public LightQueryDataFieldInfo (QueryExpression query)
			: base (query.TableMapping)
		{
			_query = query;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			sql = _query.CreateSqlString (factory, isFullName, state);

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

