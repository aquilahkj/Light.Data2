using System;

namespace Light.Data
{
	/// <summary>
	/// Extend parameter attribute.
	/// </summary>
	[AttributeUsage (AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public abstract class ConfigParamAttribute: Attribute
	{
		string name;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get {
				return name;
			}

			set {
				name = value;
			}
		}
		string value;

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value {
			get {
				return this.value;
			}

			set {
				this.value = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigParamAttribute"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		protected ConfigParamAttribute (string name, string value)
		{
			this.name = name;
			this.value = value;
		}
	}
}

