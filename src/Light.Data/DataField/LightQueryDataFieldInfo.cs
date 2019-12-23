namespace Light.Data
{
	internal class LightQueryDataFieldInfo : LightDataFieldInfo
	{
		private readonly QueryExpression _query;

		public LightQueryDataFieldInfo (QueryExpression query)
			: base (query.TableMapping)
		{
			_query = query;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			sql = _query.CreateSqlString (factory, isFullName, state);

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

