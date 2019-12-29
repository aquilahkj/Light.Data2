using System;

namespace Light.Data
{
	/// <summary>
	/// Relation pair.
	/// </summary>
	internal class RelationPair
	{
		/// <summary>
		/// Gets the relate key.
		/// </summary>
		/// <value>The relate key.</value>
		public string RelateKey { get; }

		/// <summary>
		/// Gets the master key.
		/// </summary>
		/// <value>The master key.</value>
		public string MasterKey { get; }

		public RelationPair (string masterKey, string relateKey)
		{
			if (string.IsNullOrEmpty (masterKey))
				throw new ArgumentNullException (nameof (masterKey));
			if (string.IsNullOrEmpty (relateKey))
				throw new ArgumentNullException (nameof (relateKey));
			MasterKey = masterKey;
			RelateKey = relateKey;
		}
	}
}

