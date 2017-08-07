﻿namespace Light.Data
{
	class LightAggregateCountDataFieldInfo : LightAggregateDataFieldInfo
	{
		readonly QueryExpression _expression;

		public LightAggregateCountDataFieldInfo ()
			: base (DataEntityMapping.Default)
		{
		}

		public LightAggregateCountDataFieldInfo (QueryExpression expression)
			: base (DataEntityMapping.Default)
		{
			_expression = expression;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}
			if (_expression == null) {
				sql = factory.CreateCountAllSql ();
			}
			else {
				string expressionSql = _expression.CreateSqlString (factory, isFullName, state);
				sql = factory.CreateCountAllSql (expressionSql);
			}

			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

