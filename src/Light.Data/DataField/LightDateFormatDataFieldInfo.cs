namespace Light.Data
{
	internal class LightDateFormatDataFieldInfo : LightDataFieldInfo
	{
		private readonly string _format;

		private readonly DataFieldInfo _baseFieldInfo;

		internal LightDateFormatDataFieldInfo (DataFieldInfo info, string format)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
			_format = format;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			var field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = factory.CreateDateTimeFormatSql (field, _format);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

