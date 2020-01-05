namespace Light.Data
{
	/// <summary>
	/// Query expression.
	/// </summary>
	internal class QueryExpression : LightExpression
	{
		private QueryExpression _expression1;

		private QueryExpression _expression2;

		private ConcatOperatorType _operatorType = ConcatOperatorType.AND;

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
		internal virtual string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var expressionString1 = _expression1.CreateSqlString (factory, isFullName, state);
			var expressionString2 = _expression2.CreateSqlString (factory, isFullName, state);
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

			if (expression1 == null) {
				return expression2;
			}

			if (expression2 == null) {
				return expression1;
			}
			DataEntityMapping deMapping = null;
			if (expression1.TableMapping != null) {
				deMapping = expression1.TableMapping;
			}
			else if (expression2.TableMapping != null) {
				deMapping = expression2.TableMapping;
			}

			var newExpression = new QueryExpression(deMapping)
			{
				_expression1 = expression1,
				_expression2 = expression2,
				_operatorType = operatorType,
				MultiQuery = expression1.MultiQuery | expression2.MultiQuery
			};
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

		internal bool MultiQuery { get; set; }
	}
}
