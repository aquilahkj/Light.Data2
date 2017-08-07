using System;
namespace Light.Data
{
	interface IAliasDataFieldInfo
	{
		string CreateAliasDataFieldSql (CommandFactory factory, bool isFullName, CreateSqlState state);
	}
}

