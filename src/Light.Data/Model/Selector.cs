using System;
using System.Collections.Generic;

namespace Light.Data
{
	class Selector : ISelector
	{
		protected List<DataFieldInfo> selectList = new List<DataFieldInfo> ();

		public virtual void SetSelectField (DataFieldInfo field)
		{
			if (Object.Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			selectList.Add (field);
		}

		public virtual string [] GetSelectFieldNames ()
		{
			List<string> list = new List<string> ();
			foreach (DataFieldInfo fieldInfo in this.selectList) {
				string name = fieldInfo.FieldName;
				if (name != null) {
					list.Add (name);
				}
			}
			return list.ToArray ();
		}

		public virtual string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string [] list = new string [this.selectList.Count];
			int index = 0;
			foreach (DataFieldInfo fieldInfo in this.selectList) {
				list [index] = fieldInfo.CreateSqlString (factory, isFullName, state);
				index++;
			}
			string customSelect = string.Join (",", list);
			return customSelect;
		}

		public static JoinSelector ComposeSelector (Dictionary<string, Selector> selectors)
		{
			JoinSelector joinSelector = new JoinSelector ();
			foreach (KeyValuePair<string, Selector> selector in selectors) {
				foreach (DataFieldInfo item in selector.Value.selectList) {
					DataFieldInfo info = item;
					string aliasName = string.Format ("{0}_{1}", selector.Key, info.FieldName);
					AliasDataFieldInfo alias = new AliasDataFieldInfo (info, aliasName, selector.Key);
					//alias.AliasTableName = selector.Key;
					joinSelector.SetAliasDataField (alias);
				}
			}
			return joinSelector;
		}
	}
}

