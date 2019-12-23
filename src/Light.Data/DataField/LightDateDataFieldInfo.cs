namespace Light.Data
{
	internal class LightDateDataFieldInfo : LightDataFieldInfo
	{
		private readonly DataFieldInfo _baseFieldInfo;

		public LightDateDataFieldInfo (DataFieldInfo info)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			var field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = factory.CreateDateSql (field);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

