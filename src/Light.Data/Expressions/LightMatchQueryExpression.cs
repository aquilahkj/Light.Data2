namespace Light.Data
{
	/// <summary>
	/// Lambda match expression.
	/// </summary>
	internal class LightMatchQueryExpression : QueryExpression, ISupportNotDefine
	{
		private readonly LightStringMatchDataFieldInfo _fieldInfo;

		public LightMatchQueryExpression (LightStringMatchDataFieldInfo fieldInfo)
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

