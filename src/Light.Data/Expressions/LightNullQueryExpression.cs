namespace Light.Data
{
	class LightNullQueryExpression : QueryExpression, ISupportNotDefine
	{
		readonly LightNullDataFieldInfo _fieldInfo;

		public LightNullQueryExpression (LightNullDataFieldInfo fieldInfo)
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
