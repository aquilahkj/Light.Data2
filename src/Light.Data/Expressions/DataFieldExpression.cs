﻿namespace Light.Data
{
	/// <summary>
	/// Data field expression.
	/// </summary>
	internal class DataFieldExpression : LightExpression
	{
		private DataFieldExpression _expression1;

		private DataFieldExpression _expression2;

		private ConcatOperatorType _operatorType = ConcatOperatorType.AND;

		internal virtual string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var expressionString1 = _expression1.CreateSqlString (factory, isFullName, state);

			var expressionString2 = _expression2.CreateSqlString (factory, isFullName, state);

			return factory.CreateConcatQueryExpressionSql (expressionString1, expressionString2, _operatorType);
		}

		internal static DataFieldExpression Concat (DataFieldExpression expression1, ConcatOperatorType operatorType, DataFieldExpression expression2)
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

			var newExpression = new DataFieldExpression
			{
				_expression1 = expression1, 
				_expression2 = expression2, 
				_operatorType = operatorType
			};
			return newExpression;
		}

		/// <summary>
		/// And the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static DataFieldExpression And (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.AND, expression2);
		}

		/// <summary>
		/// Or the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static DataFieldExpression Or (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.OR, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static DataFieldExpression operator & (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.AND, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static DataFieldExpression operator | (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Concat (expression1, ConcatOperatorType.OR, expression2);
		}

		/// <summary>
		/// Converts the query expression.
		/// </summary>
		/// <returns>The query expression.</returns>
		protected virtual QueryExpression ConvertQueryExpression ()
		{
			var query1 = _expression1.ConvertQueryExpression ();
			var query2 = _expression2.ConvertQueryExpression ();
			return QueryExpression.Concat (query1, _operatorType, query2);
		}

		/// <param name="expression">Expression.</param>
		public static implicit operator QueryExpression (DataFieldExpression expression)
		{
			return expression.ConvertQueryExpression ();
		}

	}
}

