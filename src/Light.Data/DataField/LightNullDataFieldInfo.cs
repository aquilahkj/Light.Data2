namespace Light.Data
{
	internal class LightNullDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
	{
		private bool _isNull;

		private readonly DataFieldInfo _baseFieldInfo;

		public LightNullDataFieldInfo (DataFieldInfo info, bool isNull)
			: base (info.TableMapping)
		{
			_baseFieldInfo = info;
			_isNull = isNull;
		}

		public void SetNot ()
		{
			_isNull = !_isNull;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}
			object obj = _baseFieldInfo.CreateSqlString (factory, isFullName, state);

			sql = factory.CreateNullQuerySql (obj, _isNull);

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}

		public QueryExpression ConvertToExpression ()
		{
			return new LightNullQueryExpression (this);
		}
	}
}
