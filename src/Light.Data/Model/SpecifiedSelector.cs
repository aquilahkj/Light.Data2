using System;
namespace Light.Data
{
	class SpecifiedSelector : Selector
	{
		public override string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string [] list = new string [this.selectList.Count];
			int index = 0;
			foreach (DataFieldInfo fieldInfo in this.selectList) {
                if (fieldInfo is IAliasDataFieldInfo alias) {
                    list[index] = alias.CreateAliasDataFieldSql(factory, isFullName, state);
                }
                else {
                    list[index] = fieldInfo.CreateSqlString(factory, isFullName, state);
                }
                index++;
			}
			string customSelect = string.Join (",", list);
			return customSelect;
		}
	}
}
