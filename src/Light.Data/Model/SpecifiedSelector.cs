namespace Light.Data
{
	internal class SpecifiedSelector : Selector
	{
		public override string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var list = new string [selectList.Count];
			var index = 0;
			foreach (var fieldInfo in selectList) {
                if (fieldInfo is IAliasDataFieldInfo alias) {
                    list[index] = alias.CreateAliasDataFieldSql(factory, isFullName, state);
                }
                else {
                    list[index] = fieldInfo.CreateSqlString(factory, isFullName, state);
                }
                index++;
			}
			var customSelect = string.Join (",", list);
			return customSelect;
		}
	}
}
