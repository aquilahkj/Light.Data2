namespace Light.Data
{
	class LightInQueryExpression : QueryExpression, ISupportNotDefine
	{
		readonly LightInQueryDataFieldInfo _fieldInfo;

		public LightInQueryExpression (LightInQueryDataFieldInfo fieldInfo)
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
