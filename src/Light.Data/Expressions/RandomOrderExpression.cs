using System;

namespace Light.Data
{
	/// <summary>
	/// Random order expression.
	/// </summary>
	internal class RandomOrderExpression : OrderExpression
	{
		private string _aliasTableName;

		public RandomOrderExpression (DataEntityMapping tableMapping)
			: base (tableMapping)
		{

		}

		public void SetTableMapping (DataEntityMapping mapping)
		{
			if (mapping == null) {
				throw new ArgumentNullException (nameof (mapping));
			}
			TableMapping = mapping;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return factory.CreateRandomOrderBySql (TableMapping, _aliasTableName, isFullName);
		}

		internal override OrderExpression CreateAliasTableNameOrder (string aliasTableName)
		{
			var expression = new RandomOrderExpression (TableMapping);
			expression._aliasTableName = aliasTableName;
			return expression;
		}
	}
}
