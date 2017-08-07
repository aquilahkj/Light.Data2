using System;

namespace Light.Data
{
	/// <summary>
	/// Data field match expression.
	/// </summary>
	class DataFieldMatchExpression : DataFieldExpression
	{
		readonly DataFieldInfo leftField;

		readonly DataFieldInfo rightField;

		QueryPredicate predicate;

		/// <summary>
		/// Initializes a new instance of the <see cref="DataFieldMatchExpression"/> class.
		/// </summary>
		/// <param name="leftField">Left field.</param>
		/// <param name="rightField">Right field.</param>
		/// <param name="predicate">Predicate.</param>
		internal DataFieldMatchExpression (DataFieldInfo leftField, DataFieldInfo rightField, QueryPredicate predicate)
		{
			this.leftField = leftField;
			this.rightField = rightField;
			this.predicate = predicate;
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
			string leftFieldSql = leftField.CreateSqlString (factory, true, state);
			string rightFieldSql = rightField.CreateSqlString (factory, true, state);
			string sql = factory.CreateJoinOnMatchSql (leftFieldSql, predicate, rightFieldSql);
			return sql;
		}

		/// <summary>
		/// Converts the query expression.
		/// </summary>
		/// <returns>The query expression.</returns>
		protected override QueryExpression ConvertQueryExpression ()
		{
			//QueryExpression expression;
			//if ((predicate == QueryPredicate.Eq || predicate == QueryPredicate.NotEq) && Object.Equals (rightField, null)) {
			//	expression = new NullQueryExpression (leftField, predicate == QueryPredicate.Eq);
			//}
			//else {
			//	expression = new DataFieldQueryExpression (leftField, predicate, rightField, false);
			//}
			QueryExpression expression = new LightBinaryQueryExpression (leftField.TableMapping, QueryPredicate.Eq, leftField, rightField);
			return expression;
		}
	}
}

