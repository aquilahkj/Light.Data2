namespace Light.Data
{
	class LightConstantDataFieldInfo : LightDataFieldInfo
	{
		readonly object _value;

		public LightConstantDataFieldInfo (object value)
			: base (DataEntityMapping.Default)
		{
			_value = value;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, false);
			if (sql != null) {
				return sql;
			}
			object value = LambdaExpressionExtend.ConvertLambdaObject (_value);
			sql = state.AddDataParameter (factory, value);

			state.SetDataSql (this, false, sql);
			return sql;
		}

	}
}

