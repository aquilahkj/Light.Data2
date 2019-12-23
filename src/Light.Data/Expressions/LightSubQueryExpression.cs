namespace Light.Data
{
	internal class LightSubQueryExpression : QueryExpression
	{
		private readonly LightSubQueryDataFieldInfo _fieldInfo;

		public LightSubQueryExpression (LightSubQueryDataFieldInfo fieldInfo)
			: base (fieldInfo.TableMapping)
		{
			_fieldInfo = fieldInfo;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return _fieldInfo.CreateSqlString (factory, isFullName, state);
		}
	}
}
