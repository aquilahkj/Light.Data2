namespace Light.Data
{
	/// <summary>
	/// Query expression.
	/// </summary>
	class QueryExpression : LightExpression
	{
		QueryExpression _expression1;

		QueryExpression _expression2;

		ConcatOperatorType _operatorType = ConcatOperatorType.AND;

		internal QueryExpression (DataEntityMapping tableMapping)
		{
			TableMapping = tableMapping;
		}

		/// <summary>
		/// Creates the sql string.
		/// </summary>
		/// <returns>The sql string.</returns>
		/// <param name="factory">Factory.</param>
		/// <param name="isFullName">If set to <c>true</c> is full name.</param>
		/// <param name="state">State.</param>
		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string expressionString1 = _expression1.CreateSqlString (factory, isFullName, state);
			string expressionString2 = _expression2.CreateSqlString (factory, isFullName, state);
			return factory.CreateConcatExpressionSql (expressionString1, expressionString2, _operatorType);
		}

		/// <summary>
		/// Concat the specified expression1, operatorType and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="operatorType">Operator type.</param>
		/// <param name="expression2">Expression2.</param>
		internal static QueryExpression Concat (QueryExpression expression1, ConcatOperatorType operatorType, QueryExpression expression2)
		{
			if (expression1 == null && expression2 == null) {
				return null;
			}
			else if (expression1 == null && expression2 != null) {
				return expression2;
			}
			else if (expression1 != null && expression2 == null) {
				return expression1;
			}
			DataEntityMapping demapping = null;
			if (expression1.TableMapping != null) {
				demapping = expression1.TableMapping;
			}
			else if (expression2.TableMapping != null) {
				demapping = expression2.TableMapping;
			}
			QueryExpression newExpression = new QueryExpression (demapping);
			newExpression._expression1 = expression1;
			newExpression._expression2 = expression2;
			newExpression._operatorType = operatorType;
			newExpression.mutliQuery = expression1.mutliQuery | expression2.mutliQuery;
			return newExpression;
		}

		/// <summary>
		/// And the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static QueryExpression And (QueryExpression expression1, QueryExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.AND, expression2);
		}

		/// <summary>
		/// Or the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static QueryExpression Or (QueryExpression expression1, QueryExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.OR, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static QueryExpression operator & (QueryExpression expression1, QueryExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.AND, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static QueryExpression operator | (QueryExpression expression1, QueryExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.OR, expression2);
		}

		///// <summary>
		///// Exists specified expression.
		///// </summary>
		///// <param name="expression">Expression.</param>
		//public static QueryExpression Exists (QueryExpression expression)
		//{
		//	return new ExistsQueryExpression (expression, false);
		//}

		///// <summary>
		///// Not exists specified expression.
		///// </summary>
		///// <returns>The exists.</returns>
		///// <param name="expression">Expression.</param>
		//public static QueryExpression NotExists (QueryExpression expression)
		//{
		//	return new ExistsQueryExpression (expression, true);
		//}

		///// <summary>
		///// Not the specified expression.
		///// </summary>
		///// <param name="expression">Expression.</param>
		//public static QueryExpression Not (QueryExpression expression)
		//{
		//	return new LambdaNotQueryExpression (expression);
		//}

		bool mutliQuery;

		internal bool MutliQuery {
			get {
				return mutliQuery;
			}

			set {
				mutliQuery = value;
			}
		}
	}
}
