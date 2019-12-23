namespace Light.Data
{
	internal class LightStringMatchDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
	{
		private bool _isNot;

		private readonly bool _starts;

		private readonly bool _ends;

		private readonly object _left;

		private readonly object _right;

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
			var sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			object left;
			object right;
			var leftInfo = _left as DataFieldInfo;
			var rightInfo = _right as DataFieldInfo;
			if (!Equals (leftInfo, null) && !Equals (rightInfo, null)) {
				left = leftInfo.CreateSqlString (factory, isFullName, state);
				right = rightInfo.CreateSqlString (factory, isFullName, state);
			}
			else if (!Equals (leftInfo, null)) {
				left = leftInfo.CreateSqlString (factory, isFullName, state);
				var rightObject = LambdaExpressionExtend.ConvertLambdaObject (_right);
				right = state.AddDataParameter (factory, rightObject);
			}
			else if (!Equals (rightInfo, null)) {
				right = rightInfo.CreateSqlString (factory, isFullName, state);
				var leftObject = LambdaExpressionExtend.ConvertLambdaObject (_left);
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
			return new LightMatchQueryExpression (this);
		}
	}
}

