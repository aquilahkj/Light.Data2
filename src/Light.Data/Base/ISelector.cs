namespace Light.Data
{
	internal interface ISelector
	{
		string [] GetSelectFieldNames ();

		string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state);
	}
}

