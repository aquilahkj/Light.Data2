using System;
namespace Light.Data
{
	class LightMathCalculateDataFieldInfo : LightDataFieldInfo
	{
		readonly MathOperator _opera;

		readonly object _left;

		readonly object _right;

		public LightMathCalculateDataFieldInfo (DataEntityMapping mapping, MathOperator opera, object left, object right)
			: base (mapping)
		{
			_opera = opera;
			_left = left;
			_right = right;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			object left;
			object right;
			DataFieldInfo leftInfo = _left as DataFieldInfo;
			DataFieldInfo rightInfo = _right as DataFieldInfo;
			if (!Object.Equals (leftInfo, null) && !Object.Equals (rightInfo, null)) {
				left = leftInfo.CreateSqlString (factory, isFullName, state);
				right = rightInfo.CreateSqlString (factory, isFullName, state);
			}
			else if (!Object.Equals (leftInfo, null)) {
				left = leftInfo.CreateSqlString (factory, isFullName, state);
				object rightObject = LambdaExpressionExtend.ConvertLambdaObject (_right);
				right = state.AddDataParameter (factory, rightObject);
			}
			else if (!Object.Equals (rightInfo, null)) {
				right = rightInfo.CreateSqlString (factory, isFullName, state);
				object leftObject = LambdaExpressionExtend.ConvertLambdaObject (_left);
				left = state.AddDataParameter (factory, leftObject);
			}
			else {
				throw new LightDataException (SR.DataFieldContentError);
			}

			switch (_opera) {
			case MathOperator.Puls:
				sql = factory.CreatePlusSql (left, right);
				break;
			case MathOperator.Minus:
				sql = factory.CreateMinusSql (left, right);
				break;
			case MathOperator.Multiply:
				sql = factory.CreateMultiplySql (left, right);
				break;
			case MathOperator.Divided:
				sql = factory.CreateDividedSql (left, right);
				break;
			case MathOperator.Mod:
				sql = factory.CreateModSql (left, right);
				break;
			case MathOperator.Power:
				sql = factory.CreatePowerSql (left, right);
				break;
			}
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}
	}
}

