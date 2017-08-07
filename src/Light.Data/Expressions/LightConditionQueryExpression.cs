namespace Light.Data
{
	class LightConditionQueryExpression : QueryExpression, ISupportNotDefine
	{
		readonly LightConditionDataFieldInfo _fieldInfo;

		public LightConditionQueryExpression (LightConditionDataFieldInfo fieldInfo)
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

