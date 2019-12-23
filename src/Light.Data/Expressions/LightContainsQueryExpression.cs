namespace Light.Data
{
	internal class LightContainsQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightContainsDataFieldInfo _fieldInfo;

		public LightContainsQueryExpression (LightContainsDataFieldInfo fieldInfo)
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

