namespace Light.Data
{
	internal class LightExistsQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightExistsDataFieldInfo _fieldInfo;

		public LightExistsQueryExpression (LightExistsDataFieldInfo fieldInfo)
			: base (fieldInfo.TableMapping)
		{
			_fieldInfo = fieldInfo;
		}

		public void SetNot ()
		{
			_fieldInfo.SetNot ();
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return _fieldInfo.CreateSqlString (factory, isFullName, state);
		}
	}
}
