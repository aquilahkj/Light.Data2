namespace Light.Data
{
	class LightContainsQueryExpression : QueryExpression, ISupportNotDefine
	{
		readonly LightContainsDataFieldInfo _fieldInfo;

		public LightContainsQueryExpression (LightContainsDataFieldInfo fieldInfo)
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

