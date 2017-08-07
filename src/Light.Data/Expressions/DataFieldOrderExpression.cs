using System;

namespace Light.Data
{
	class DataFieldOrderExpression : OrderExpression
	{
		DataFieldInfo _fieldInfo;

		OrderType _orderType = OrderType.ASC;

		public DataFieldOrderExpression (DataFieldInfo fieldInfo, OrderType orderType)
			: base (fieldInfo.TableMapping)
		{
			_fieldInfo = fieldInfo;
			_orderType = orderType;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string fieldSql = _fieldInfo.CreateSqlString (factory, isFullName, state);
			return factory.CreateOrderBySql (fieldSql, _orderType);
		}

		internal override OrderExpression CreateAliasTableNameOrder (string aliasTableName)
		{
			DataFieldInfo info = this._fieldInfo.CreateAliasTableInfo (aliasTableName);
			return new DataFieldOrderExpression (info, this._orderType);
		}
	}
}
