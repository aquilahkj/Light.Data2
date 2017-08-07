using System;
using System.Collections.Generic;

namespace Light.Data
{
	/// <summary>
	/// Order expression.
	/// </summary>
	class OrderExpression : LightExpression
	{
		List<OrderExpression> _orderExpressions;

		internal OrderExpression (DataEntityMapping tableMapping)
		{
			TableMapping = tableMapping;
		}

		/// <summary>
		/// Catch the specified expression1 and expression2.
		/// </summary>
		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		internal static OrderExpression Catch (OrderExpression expression1, OrderExpression expression2)
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
			else if (Object.ReferenceEquals (expression1, expression2)) {
				return expression1;
			}
			else if (expression1 is RandomOrderExpression || expression2 is RandomOrderExpression) {
				return expression2;
			}
			DataEntityMapping demapping = null;
			OrderExpression newExpression = new OrderExpression (demapping);
			List<OrderExpression> list = new List<OrderExpression> ();
			if (expression1._orderExpressions == null) {
				list.Add (expression1);
			}
			else {
				list.AddRange (expression1._orderExpressions);
			}
			if (expression2._orderExpressions == null) {
				list.Add (expression2);
			}
			else {
				list.AddRange (expression2._orderExpressions);
			}
			newExpression._orderExpressions = list;
			newExpression.mutliOrder = expression1.mutliOrder | expression2.mutliOrder;
			return newExpression;
		}

		/// <param name="expression1">Expression1.</param>
		/// <param name="expression2">Expression2.</param>
		public static OrderExpression operator & (OrderExpression expression1, OrderExpression expression2)
		{
			return Catch (expression1, expression2);
		}

		internal virtual OrderExpression CreateAliasTableNameOrder (string aliasTableName)
		{
			OrderExpression newExpression = new OrderExpression (TableMapping);
			List<OrderExpression> list = new List<OrderExpression> ();
			foreach (OrderExpression item in list) {
				OrderExpression newitem = item.CreateAliasTableNameOrder (aliasTableName);
				list.Add (newitem);
			}
			newExpression._orderExpressions = list;
			return newExpression;
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
			string [] array = new string [_orderExpressions.Count];
			int len = array.Length;
			for (int i = 0; i < len; i++) {
				array [i] = _orderExpressions [i].CreateSqlString (factory, isFullName, state);
			}
			return factory.CreateCatchExpressionSql (array);
		}

		bool mutliOrder;

		internal bool MutliOrder {
			get {
				return mutliOrder;
			}

			set {
				mutliOrder = value;
			}
		}
	}
}
