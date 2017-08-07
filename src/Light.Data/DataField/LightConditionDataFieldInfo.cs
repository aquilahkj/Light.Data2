using System;
namespace Light.Data
{
	class LightConditionDataFieldInfo : LightDataFieldInfo, ISupportNotDefine, IDataFieldInfoConvert
	{
		readonly object _ifTrue;

		readonly object _ifFalse;

		readonly QueryExpression _query;

		bool _isNot;

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
			string sql = state.GetDataSql (this, isFullName);
			if (sql != null) {
				return sql;
			}

			string query = _query.CreateSqlString (factory, isFullName, state);

			object ifTrue;
			object ifFalse;
			DataFieldInfo ifTrueInfo = _ifTrue as DataFieldInfo;
			DataFieldInfo ifFalseInfo = _ifFalse as DataFieldInfo;
			if (!Object.Equals (ifTrueInfo, null) && !Object.Equals (ifFalseInfo, null)) {
				ifTrue = ifTrueInfo.CreateSqlString (factory, isFullName, state);
				ifFalse = ifFalseInfo.CreateSqlString (factory, isFullName, state);
			}
			else if (!Object.Equals (ifTrueInfo, null)) {
				ifTrue = ifTrueInfo.CreateSqlString (factory, isFullName, state);
				object ifFalseObject = LambdaExpressionExtend.ConvertLambdaObject (_ifFalse);
				ifFalse = state.AddDataParameter (factory, ifFalseObject);
			}
			else if (!Object.Equals (ifFalseInfo, null)) {
				ifFalse = ifFalseInfo.CreateSqlString (factory, isFullName, state);
				object ifTrueObject = LambdaExpressionExtend.ConvertLambdaObject (_ifTrue);
				ifTrue = state.AddDataParameter (factory, ifTrueObject);
			}
			else {
				object ifTrueObject = LambdaExpressionExtend.ConvertLambdaObject (_ifTrue);
				object ifFalseObject = LambdaExpressionExtend.ConvertLambdaObject (_ifFalse);
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

