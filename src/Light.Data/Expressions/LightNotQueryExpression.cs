namespace Light.Data
{
	class LightNotQueryExpression : QueryExpression
	{
		readonly QueryExpression _queryExpression;

		public LightNotQueryExpression (QueryExpression expression)
			: base (expression.TableMapping)
		{
			_queryExpression = expression;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string queryString = _queryExpression.CreateSqlString (factory, isFullName, state);
			return factory.CreateNotQuerySql (queryString);
		}
	}
}

