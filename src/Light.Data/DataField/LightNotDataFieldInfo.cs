namespace Light.Data
{
	internal class LightNotDataFieldInfo : LightDataFieldInfo
	{
		private readonly DataFieldInfo _baseFieldInfo;
		private readonly bool _query;

		public LightNotDataFieldInfo (DataFieldInfo info, bool query)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
			_query = query;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			sql = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = _query ? factory.CreateNotSql(sql) : factory.CreateOutputNotSql(sql);

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

