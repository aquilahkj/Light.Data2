using System;

namespace Light.Data
{
	/// <summary>
	/// Relation key.
	/// </summary>
	internal class RelationKey
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
			MasterKey = masterKey;
			RelateKey = relateKey;
		}

		/// <summary>
		/// Gets the master key.
		/// </summary>
		/// <value>The master key.</value>
		public string MasterKey { get; }

		/// <summary>
		/// Gets the relate key.
		/// </summary>
		/// <value>The relate key.</value>
		public string RelateKey { get; }

		/// <summary>
		/// Determines whether this instance is reverse match the specified target.
		/// </summary>
		/// <returns><c>true</c> if this instance is reverse match the specified target; otherwise, <c>false</c>.</returns>
		/// <param name="target">Target.</param>
		public bool IsReverseMatch (RelationKey target)
		{
			return MasterKey == target.RelateKey && RelateKey == target.MasterKey;
		}

		/// <summary>
		/// Determines whether this instance is match the specified target.
		/// </summary>
		/// <returns><c>true</c> if this instance is match the specified target; otherwise, <c>false</c>.</returns>
		/// <param name="target">Target.</param>
		public bool IsMatch (RelationKey target)
		{
			return MasterKey == target.MasterKey && RelateKey == target.RelateKey;
		}
	}
}
