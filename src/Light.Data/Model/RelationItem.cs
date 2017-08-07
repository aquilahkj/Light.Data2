using System;
namespace Light.Data
{
	class RelationItem
	{
		DataEntityMapping dataMapping;

		SingleRelationFieldMapping fieldMapping;

		string prevFieldPath;

		string currentFieldPath;

		string [] keys;

		string aliasName;

		public string CurrentFieldPath {
			get {
				return currentFieldPath;
			}

			set {
				currentFieldPath = value;
			}
		}

		public string [] Keys {
			get {
				return keys;
			}

			set {
				keys = value;
			}
		}

		public DataEntityMapping DataMapping {
			get {
				return dataMapping;
			}

			set {
				dataMapping = value;
			}
		}

		public SingleRelationFieldMapping FieldMapping {
			get {
				return fieldMapping;
			}

			set {
				fieldMapping = value;
			}
		}

		public string AliasName {
			get {
				return aliasName;
			}

			set {
				aliasName = value;
			}
		}

		public string PrevFieldPath {
			get {
				return prevFieldPath;
			}

			set {
				prevFieldPath = value;
			}
		}
	}
}

