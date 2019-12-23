namespace Light.Data
{
	internal interface IAliasDataFieldInfo
	{
		string CreateAliasDataFieldSql (CommandFactory factory, bool isFullName, CreateSqlState state);
	}
}

