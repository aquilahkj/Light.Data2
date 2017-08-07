namespace Light.Data
{
	class LightSubQueryExpression : QueryExpression
	{
		readonly LightSubQueryDataFieldInfo _fieldInfo;

		public LightSubQueryExpression (LightSubQueryDataFieldInfo fieldInfo)
			: base (fieldInfo.TableMapping)
		{
			this._fieldInfo = fieldInfo;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return _fieldInfo.CreateSqlString (factory, isFullName, state);
		}
	}
}
