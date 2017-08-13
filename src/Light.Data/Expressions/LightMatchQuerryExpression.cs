using System;
namespace Light.Data
{
	/// <summary>
	/// Lambda match expression.
	/// </summary>
	class LightMatchQuerryExpression : QueryExpression, ISupportNotDefine
	{
		LightStringMatchDataFieldInfo _fieldInfo;

		public LightMatchQuerryExpression (LightStringMatchDataFieldInfo fieldInfo)
			: base (fieldInfo.TableMapping)
		{
			this._fieldInfo = fieldInfo;
		}

		public void SetNot ()
		{
			this._fieldInfo.SetNot ();
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return _fieldInfo.CreateSqlString (factory, isFullName, state);
		}
	}
}

