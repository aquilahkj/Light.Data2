namespace Light.Data
{
	internal sealed class AllSelector : ISelector
	{
		internal static AllSelector Value { get; } = new AllSelector ();

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			return factory.CreateSelectAllSql ();
		}

		public string [] GetSelectFieldNames ()
		{
			return new string [0];
		}
	}
}
