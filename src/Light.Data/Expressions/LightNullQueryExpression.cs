namespace Light.Data
{
	internal class LightNullQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightNullDataFieldInfo _fieldInfo;

		public LightNullQueryExpression (LightNullDataFieldInfo fieldInfo)
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
