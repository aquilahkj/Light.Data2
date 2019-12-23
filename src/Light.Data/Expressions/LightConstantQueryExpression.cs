using System;

namespace Light.Data
{
	internal class LightConstantQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly object _value;

		private bool _not;

		public LightConstantQueryExpression (object value)
			: base (DataEntityMapping.Default)
		{
			_value = value;
		}

		public void SetNot ()
		{
			_not = !_not;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var value = LambdaExpressionExtend.ConvertLambdaObject (_value);
			var ret = Convert.ToBoolean(value);
			if (_not)
			{
				ret = !ret;
			}
			return factory.CreateBooleanConstantSql(ret);
		}
	}
}

