namespace Light.Data
{
	internal class LightInQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightInQueryDataFieldInfo _fieldInfo;

		public LightInQueryExpression (LightInQueryDataFieldInfo fieldInfo)
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
