namespace Light.Data
{
	internal class ExistsSelector : ISelector
	{
		internal static ExistsSelector Value { get; } = new ExistsSelector ();

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return factory.CreateSelectExistsSql ();
		}

		public string [] GetSelectFieldNames ()
		{
			return new string [0];
		}
	}
}
