namespace Light.Data
{
	internal class LightNotQueryExpression : QueryExpression
	{
		private readonly QueryExpression _queryExpression;

		public LightNotQueryExpression (QueryExpression expression)
			: base (expression.TableMapping)
		{
			_queryExpression = expression;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var queryString = _queryExpression.CreateSqlString (factory, isFullName, state);
			return factory.CreateNotQuerySql (queryString);
		}
	}
}

