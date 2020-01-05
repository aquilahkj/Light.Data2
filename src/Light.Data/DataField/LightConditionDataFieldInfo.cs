namespace Light.Data
{
	internal class LightConditionDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
	{
		private readonly object _ifTrue;

		private readonly object _ifFalse;

		private readonly QueryExpression _query;

		private bool _isNot;

		public LightConditionDataFieldInfo (QueryExpression query, object ifTrue, object ifFalse)
			: base (query.TableMapping)
		{
			_query = query;
			_ifTrue = ifTrue;
			_ifFalse = ifFalse;
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

			var query = _query.CreateSqlString (factory, isFullName, state);

			object ifTrue;
			object ifFalse;
			var ifTrueInfo = _ifTrue as DataFieldInfo;
			var ifFalseInfo = _ifFalse as DataFieldInfo;
			if (!Equals (ifTrueInfo, null) && !Equals (ifFalseInfo, null)) {
				ifTrue = ifTrueInfo.CreateSqlString (factory, isFullName, state);
				ifFalse = ifFalseInfo.CreateSqlString (factory, isFullName, state);
			}
			else if (!Equals (ifTrueInfo, null)) {
				ifTrue = ifTrueInfo.CreateSqlString (factory, isFullName, state);
				var ifFalseObject = LambdaExpressionExtend.ConvertLambdaObject (_ifFalse).AdjustValue();
				ifFalse = state.AddDataParameter (factory, ifFalseObject);
			}
			else if (!Equals (ifFalseInfo, null)) {
				ifFalse = ifFalseInfo.CreateSqlString (factory, isFullName, state);
				var ifTrueObject = LambdaExpressionExtend.ConvertLambdaObject (_ifTrue).AdjustValue();
				ifTrue = state.AddDataParameter (factory, ifTrueObject);
			}
			else {
				var ifTrueObject = LambdaExpressionExtend.ConvertLambdaObject (_ifTrue).AdjustValue();
				var ifFalseObject = LambdaExpressionExtend.ConvertLambdaObject (_ifFalse).AdjustValue();
				ifTrue = state.AddDataParameter (factory, ifTrueObject);
				ifFalse = state.AddDataParameter (factory, ifFalseObject);
			}

			sql = factory.CreateConditionSql (query, ifTrue, ifFalse);
			state.SetDataSql (this, isFullName, sql);
			return sql;
		}

		public QueryExpression ConvertToExpression ()
		{
			return new LightConditionQueryExpression (this);
		}
	}
}

