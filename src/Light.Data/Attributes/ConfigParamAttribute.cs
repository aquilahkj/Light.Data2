using System;

namespace Light.Data
{
	/// <summary>
	/// Extend parameter attribute.
	/// </summary>
	[AttributeUsage (AttributeTargets.Class, AllowMultiple = true)]
	public abstract class ConfigParamAttribute: Attribute
	{
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the value.
		/// </summary>
		/// <value>The value.</value>
		public string Value { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigParamAttribute"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		protected ConfigParamAttribute (string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}

