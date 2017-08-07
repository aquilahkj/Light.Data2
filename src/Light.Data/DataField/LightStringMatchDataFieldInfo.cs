using System;
namespace Light.Data
{
	class LightStringMatchDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
	{
		bool _isNot;

		readonly bool _starts;

		readonly bool _ends;

		readonly object _left;

		readonly object _right;

		public LightStringMatchDataFieldInfo (DataEntityMapping mapping, bool starts, bool ends, object left, object right)
			: base (mapping)
		{
			_starts = starts;
			_ends = ends;
			_left = left;
			_right = right;
		}

		public void SetNot ()
		{
			_isNot = !_isNot;
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
			sql = factory.CreateLikeMatchQuerySql (left, right, _starts, _ends, _isNot);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}

		public QueryExpression ConvertToExpression ()
		{
			return new LightMatchQuerryExpression (this);
		}
	}
}

