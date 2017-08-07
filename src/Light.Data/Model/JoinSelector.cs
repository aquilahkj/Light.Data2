using System;
using System.Collections.Generic;

namespace Light.Data
{
	class JoinSelector : ISelector
	{
		Dictionary<string, DataFieldInfo> infoDict = new Dictionary<string, DataFieldInfo> ();

		HashSet<string> aliasTableHash = new HashSet<string> ();

		public void SetDataEntity (DataEntityMapping entityMapping)
		{
			if (entityMapping == null)
				throw new ArgumentNullException (nameof (entityMapping));
			foreach (DataFieldMapping fieldMapping in entityMapping.DataEntityFields) {
				if (fieldMapping != null) {
					DataFieldInfo field = new DataFieldInfo (fieldMapping);
					AliasDataFieldInfo aliasField = new AliasDataFieldInfo (field, field.FieldName);
					this.infoDict [aliasField.AliasName] = aliasField;
				}
			}
		}

		public void SetDataField (DataFieldInfo field)
		{
			if (Object.Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			AliasDataFieldInfo aliasField = new AliasDataFieldInfo (field, field.FieldName);
			this.infoDict [aliasField.AliasName] = aliasField;
			if (field.AliasTableName != null) {
				aliasTableHash.Add (field.AliasTableName);
			}
		}

		public void SetAliasDataField (AliasDataFieldInfo aliasField)
		{
			if (Object.Equals (aliasField, null))
				throw new ArgumentNullException (nameof (aliasField));
			this.infoDict [aliasField.AliasName] = aliasField;
			if (aliasField.AliasTableName != null) {
				aliasTableHash.Add (aliasField.AliasTableName);
			}
		}

		public string [] GetSelectFieldNames ()
		{
			string [] fields = new string [this.infoDict.Count + aliasTableHash.Count];
			int index = 0;
			foreach (DataFieldInfo fieldInfo in this.infoDict.Values) {
				AliasDataFieldInfo aliasInfo = fieldInfo as AliasDataFieldInfo;
				if (!Object.Equals (aliasInfo, null)) {
					fields [index] = aliasInfo.AliasName;
				}
				else {
					fields [index] = fieldInfo.FieldName;
				}
				index++;
			}
			foreach (string alias in aliasTableHash) {
				fields [index] = alias;
				index++;
			}
			return fields;
		}

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			string [] selectList = new string [this.infoDict.Count];
			int index = 0;
			foreach (DataFieldInfo fieldInfo in this.infoDict.Values) {
				IAliasDataFieldInfo aliasInfo = fieldInfo as IAliasDataFieldInfo;
				if (!Object.Equals (aliasInfo, null)) {
					selectList [index] = aliasInfo.CreateAliasDataFieldSql (factory, true, state);
				}
				else {
					selectList [index] = fieldInfo.CreateSqlString (factory, true, state);
				}
				index++;
			}
			string customSelect = string.Join (",", selectList);
			return customSelect;
		}
	}
}

