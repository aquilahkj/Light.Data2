namespace Light.Data
{
	class LightExistsQueryExpression : QueryExpression, ISupportNotDefine
	{
		readonly LightExistsDataFieldInfo _fieldInfo;

		public LightExistsQueryExpression (LightExistsDataFieldInfo fieldInfo)
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
