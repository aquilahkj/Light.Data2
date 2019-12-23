namespace Light.Data
{
	internal class LightConditionQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightConditionDataFieldInfo _fieldInfo;

		public LightConditionQueryExpression (LightConditionDataFieldInfo fieldInfo)
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

