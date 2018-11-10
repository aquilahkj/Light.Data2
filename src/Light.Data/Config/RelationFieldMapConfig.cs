using System;
using System.Collections.Generic;

namespace Light.Data
{
	/// <summary>
	/// Relation field config.
	/// </summary>
	class RelationFieldMapConfig
	{
		readonly List<RelationKey> _relationKeys = new List<RelationKey>();

		/// <summary>
		/// Initializes a new instance of the <see cref="RelationFieldMapConfig"/> class.
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		public RelationFieldMapConfig(string fieldName) {
			if (string.IsNullOrEmpty(fieldName)) {
				throw new ArgumentNullException(nameof(fieldName));
			}
			FieldName = fieldName;
		}

		/// <summary>
		/// Adds the relation keys.
		/// </summary>
		/// <param name="masterKey">Master key.</param>
		/// <param name="relateKey">Relate key.</param>
		public void AddRelationKeys(string masterKey, string relateKey) {
			_relationKeys.Add(new RelationKey(masterKey, relateKey));
		}

		///// <summary>
		///// Gets the relation mode.
		///// </summary>
		///// <value>The relation mode.</value>
		//public RelationMode RelationMode {
		//	get;
		//	set;
		//}

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		public string FieldName {
			get;
			private set;
		}

		/// <summary>
		/// Gets the relation keys.
		/// </summary>
		/// <returns>The relation keys.</returns>
		public RelationKey[] GetRelationKeys() {
			return _relationKeys.ToArray();
		}

        /// <summary>
        ///  Gets the relation keys count
        /// </summary>
        /// <value>The relation key count.</value>
        public int RelationKeyCount {
			get {
				return _relationKeys.Count;
			}
		}
	}
}
