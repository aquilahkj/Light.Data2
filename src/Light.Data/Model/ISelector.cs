using System;
namespace Light.Data
{
	interface ISelector
	{
		string [] GetSelectFieldNames ();

		string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state);
	}
}

