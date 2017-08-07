using System;

namespace Light.Data
{
	/// <summary>
	/// Random order expression.
	/// </summary>
	class RandomOrderExpression : OrderExpression
	{
		string _aliasTableName;

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
			return factory.CreateRandomOrderBySql (TableMapping, this._aliasTableName, isFullName);
		}

		internal override OrderExpression CreateAliasTableNameOrder (string aliasTableName)
		{
			RandomOrderExpression expression = new RandomOrderExpression (this.TableMapping);
			expression._aliasTableName = aliasTableName;
			return expression;
		}
	}
}
