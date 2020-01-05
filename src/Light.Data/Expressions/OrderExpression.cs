using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Order expression.
    /// </summary>
    internal class OrderExpression : LightExpression
    {
        private List<OrderExpression> _orderExpressions;

        internal OrderExpression(DataEntityMapping tableMapping)
        {
            TableMapping = tableMapping;
        }

        /// <summary>
        /// Concat the specified expression1 and expression2.
        /// </summary>
        /// <param name="expression1">Expression1.</param>
        /// <param name="expression2">Expression2.</param>
        internal static OrderExpression Concat(OrderExpression expression1, OrderExpression expression2)
        {
            if (expression1 == null && expression2 == null)
            {
                return null;
            }

            if (expression1 == null)
            {
                return expression2;
            }

            if (expression2 == null)
            {
                return expression1;
            }

            if (ReferenceEquals(expression1, expression2))
            {
                return expression1;
            }

            if (expression1 is RandomOrderExpression || expression2 is RandomOrderExpression)
            {
                return expression2;
            }

            var deMapping = expression1.TableMapping ?? expression2.TableMapping;
            var newExpression = new OrderExpression(deMapping);
            var list = new List<OrderExpression>();
            if (expression1._orderExpressions == null)
            {
                list.Add(expression1);
            }
            else
            {
                list.AddRange(expression1._orderExpressions);
            }

            if (expression2._orderExpressions == null)
            {
                list.Add(expression2);
            }
            else
            {
                list.AddRange(expression2._orderExpressions);
            }

            newExpression._orderExpressions = list;
            newExpression.MultiOrder = expression1.MultiOrder | expression2.MultiOrder;
            return newExpression;
        }

        /// <param name="expression1">Expression1.</param>
        /// <param name="expression2">Expression2.</param>
        public static OrderExpression operator &(OrderExpression expression1, OrderExpression expression2)
        {
            return Concat(expression1, expression2);
        }

        internal virtual OrderExpression CreateAliasTableNameOrder(string aliasTableName)
        {
            var newExpression = new OrderExpression(TableMapping);
            var list = new List<OrderExpression>();
            foreach (var item in list)
            {
                var newItem = item.CreateAliasTableNameOrder(aliasTableName);
                list.Add(newItem);
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
        internal virtual string CreateSqlString(CommandFactory factory, bool isFullName, CreateSqlState state)
        {
            var array = new string [_orderExpressions.Count];
            var len = array.Length;
            for (var i = 0; i < len; i++)
            {
                array[i] = _orderExpressions[i].CreateSqlString(factory, isFullName, state);
            }

            return factory.CreateConcatExpressionSql(array);
        }

        internal bool MultiOrder { get; set; }
    }
}