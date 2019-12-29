using System;
using System.Reflection;

namespace Light.Data
{
	internal class LightConstantDataFieldInfo : LightDataFieldInfo
	{
		private readonly object _value;

		public LightConstantDataFieldInfo (object value)
			: base (DataEntityMapping.Default)
		{
			_value = value;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var sql = state.GetDataSql (this, false);
			if (sql != null) {
				return sql;
			}
			var value = LambdaExpressionExtend.ConvertLambdaObject (_value).EnumCheck();
			sql = state.AddDataParameter (factory, value);

			state.SetDataSql (this, false, sql);
			return sql;
		}

	}
}

