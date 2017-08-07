using System;

namespace Light.Data
{
	/// <summary>
	/// Relation field attribute.
	/// </summary>
	[AttributeUsage (AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
	public class RelationFieldAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RelationFieldAttribute"/> class.
		/// </summary>
		/// <param name="masterKey">Master key.</param>
		/// <param name="relateKey">Relate key.</param>
		public RelationFieldAttribute (string masterKey, string relateKey)
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
		string masterKey;

		/// <summary>
		/// Gets the master key.
		/// </summary>
		/// <value>The master key.</value>
		public string MasterKey {
			get {
				return masterKey;
			}

			//set {
			//	masterKey = value;
			//}
		}
		string relateKey;

		/// <summary>
		/// Gets the relate key.
		/// </summary>
		/// <value>The relate key.</value>
		public string RelateKey {
			get {
				return relateKey;
			}

			//set {
			//	relateKey = value;
			//}
		}
	}
}
