using System;

namespace Light.Data
{
	/// <summary>
	/// Relation key.
	/// </summary>
	class RelationKey
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelationKey"/> class.
		/// </summary>
		/// <param name="masterKey">Master key.</param>
		/// <param name="relateKey">Relate key.</param>
		public RelationKey (string masterKey, string relateKey)
		{
			if (string.IsNullOrEmpty (masterKey)) {
				throw new ArgumentNullException (nameof (masterKey));
			}
			if (string.IsNullOrEmpty (relateKey)) {
				throw new ArgumentNullException (nameof (relateKey));
			}
			this.masterKey = masterKey;
			this.relateKey = relateKey;
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

		/// <summary>
		/// Determines whether this instance is reverse match the specified target.
		/// </summary>
		/// <returns><c>true</c> if this instance is reverse match the specified target; otherwise, <c>false</c>.</returns>
		/// <param name="target">Target.</param>
		public bool IsReverseMatch (RelationKey target)
		{
			return this.masterKey == target.relateKey && this.relateKey == target.masterKey;
		}

		/// <summary>
		/// Determines whether this instance is match the specified target.
		/// </summary>
		/// <returns><c>true</c> if this instance is match the specified target; otherwise, <c>false</c>.</returns>
		/// <param name="target">Target.</param>
		public bool IsMatch (RelationKey target)
		{
			return this.masterKey == target.masterKey && this.relateKey == target.relateKey;
		}
	}
}
