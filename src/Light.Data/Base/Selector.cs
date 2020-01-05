using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class Selector : ISelector
	{
		protected readonly List<DataFieldInfo> selectList = new List<DataFieldInfo> ();

		public virtual void SetSelectField (DataFieldInfo field)
		{
			if (Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			selectList.Add (field);
		}

		public virtual string [] GetSelectFieldNames ()
		{
			var list = new List<string> ();
			foreach (var fieldInfo in selectList) {
				var name = fieldInfo.FieldName;
				if (name != null) {
					list.Add (name);
				}
			}
			return list.ToArray ();
		}

		public virtual string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var list = new string [selectList.Count];
			var index = 0;
			foreach (var fieldInfo in selectList) {
				list [index] = fieldInfo.CreateSqlString (factory, isFullName, state);
				index++;
			}
			var customSelect = string.Join (",", list);
			return customSelect;
		}

		public static JoinSelector ComposeSelector (Dictionary<string, Selector> selectors)
		{
			var joinSelector = new JoinSelector ();
			foreach (var selector in selectors) {
				foreach (var item in selector.Value.selectList) {
					var info = item;
					var aliasName = $"{selector.Key}_{info.FieldName}";
					var alias = new AliasDataFieldInfo (info, aliasName, selector.Key);
					//alias.AliasTableName = selector.Key;
					joinSelector.SetAliasDataField (alias);
				}
			}
			return joinSelector;
		}
	}
}

