namespace Light.Data
{
	class LightDateFormatDataFieldInfo : LightDataFieldInfo
	{
		readonly string _format;

		readonly DataFieldInfo _baseFieldInfo;

		internal LightDateFormatDataFieldInfo (DataFieldInfo info, string format)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
			_format = format;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			string field = _baseFieldInfo.CreateSqlString (factory, isFullName, state);
			sql = factory.CreateDateTimeFormatSql (field, _format);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

