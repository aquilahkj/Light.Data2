using System;

namespace Light.Data
{
	/// <summary>
	/// Relation pair.
	/// </summary>
	class RelationPair
	{
		readonly string relateKey;

		/// <summary>
		/// Gets the relate key.
		/// </summary>
		/// <value>The relate key.</value>
		public string RelateKey {
			get {
				return relateKey;
			}
		}

		readonly string masterKey;

		/// <summary>
		/// Gets the master key.
		/// </summary>
		/// <value>The master key.</value>
		public string MasterKey {
			get {
				return masterKey;
			}
		}

		public RelationPair (string masterKey, string relateKey)
		{
			if (string.IsNullOrEmpty (masterKey))
				throw new ArgumentNullException (nameof (masterKey));
			if (string.IsNullOrEmpty (relateKey))
				throw new ArgumentNullException (nameof (relateKey));
			this.masterKey = masterKey;
			this.relateKey = relateKey;
		}
	}
}

