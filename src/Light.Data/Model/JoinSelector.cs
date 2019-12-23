using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class JoinSelector : ISelector
	{
		private Dictionary<string, DataFieldInfo> infoDict = new Dictionary<string, DataFieldInfo> ();

		private HashSet<string> aliasTableHash = new HashSet<string> ();

		public void SetDataEntity (DataEntityMapping entityMapping)
		{
			if (entityMapping == null)
				throw new ArgumentNullException (nameof (entityMapping));
			foreach (var fieldMapping in entityMapping.DataEntityFields) {
				if (fieldMapping != null) {
					var field = new DataFieldInfo (fieldMapping);
					var aliasField = new AliasDataFieldInfo (field, field.FieldName);
					infoDict [aliasField.AliasName] = aliasField;
				}
			}
		}

		public void SetDataField (DataFieldInfo field)
		{
			if (Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			var aliasField = new AliasDataFieldInfo (field, field.FieldName);
			infoDict [aliasField.AliasName] = aliasField;
			if (field.AliasTableName != null) {
				aliasTableHash.Add (field.AliasTableName);
			}
		}

		public void SetAliasDataField (AliasDataFieldInfo aliasField)
		{
			if (Equals (aliasField, null))
				throw new ArgumentNullException (nameof (aliasField));
			infoDict [aliasField.AliasName] = aliasField;
			if (aliasField.AliasTableName != null) {
				aliasTableHash.Add (aliasField.AliasTableName);
			}
		}

		public string [] GetSelectFieldNames ()
		{
			var fields = new string [infoDict.Count + aliasTableHash.Count];
			var index = 0;
			foreach (var fieldInfo in infoDict.Values) {
				var aliasInfo = fieldInfo as AliasDataFieldInfo;
				if (!Equals (aliasInfo, null)) {
					fields [index] = aliasInfo.AliasName;
				}
				else {
					fields [index] = fieldInfo.FieldName;
				}
				index++;
			}
			foreach (var alias in aliasTableHash) {
				fields [index] = alias;
				index++;
			}
			return fields;
		}

		public string CreateSelectString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var selectList = new string [infoDict.Count];
			var index = 0;
			foreach (var fieldInfo in infoDict.Values) {
				if (fieldInfo is IAliasDataFieldInfo aliasInfo) {
					selectList [index] = aliasInfo.CreateAliasDataFieldSql (factory, true, state);
				}
				else {
					selectList [index] = fieldInfo.CreateSqlString (factory, true, state);
				}
				index++;
			}

			var customSelect = factory.CreateSelectFieldConcat(selectList);
			return customSelect;
		}
	}
}

