namespace Light.Data
{
	/// <summary>
	/// Data field expression.
	/// </summary>
	class DataFieldExpression : LightExpression
	{
		DataFieldExpression _expression1;

		DataFieldExpression _expression2;

		CatchOperatorsType _operatorType = CatchOperatorsType.AND;

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string expressionString1 = _expression1.CreateSqlString (factory, isFullName, state);

			string expressionString2 = _expression2.CreateSqlString (factory, isFullName, state);

			return factory.CreateCatchExpressionSql (expressionString1, expressionString2, _operatorType);
		}

		internal static DataFieldExpression Catch (DataFieldExpression expression1, CatchOperatorsType operatorType, DataFieldExpression expression2)
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
			DataFieldExpression newExpression = new DataFieldExpression ();
			newExpression._expression1 = expression1;
			newExpression._expression2 = expression2;
			newExpression._operatorType = operatorType;
			return newExpression;
		}

		/// <summary>
		/// And the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static DataFieldExpression And (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Catch (expression1, CatchOperatorsType.AND, expression2);
		}

		/// <summary>
		/// Or the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static DataFieldExpression Or (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Catch (expression1, CatchOperatorsType.OR, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static DataFieldExpression operator & (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Catch (expression1, CatchOperatorsType.AND, expression2);
		}
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static DataFieldExpression operator | (DataFieldExpression expression1, DataFieldExpression expression2)
		{
			return Catch (expression1, CatchOperatorsType.OR, expression2);
		}

		/// <summary>
		/// Converts the query expression.
		/// </summary>
		/// <returns>The query expression.</returns>
		protected virtual QueryExpression ConvertQueryExpression ()
		{
			QueryExpression query1 = _expression1.ConvertQueryExpression ();
			QueryExpression query2 = _expression2.ConvertQueryExpression ();
			return QueryExpression.Catch (query1, _operatorType, query2);
		}

		/// <param name="expression">Expression.</param>
		public static implicit operator QueryExpression (DataFieldExpression expression)
		{
			return expression.ConvertQueryExpression ();
		}

	}
}

