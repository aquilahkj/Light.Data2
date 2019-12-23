using System;

namespace Light.Data
{
	/// <summary>
	/// Data table config.
	/// </summary>
	internal class DataTableMapperConfig
	{
		/// <summary>
		/// Gets or sets the extend parameters.
		/// </summary>
		/// <value>The extend parameters.</value>
		public ConfigParamSet ConfigParams { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableMapperConfig"/> class.
		/// </summary>
		/// <param name="dataType">Data type.</param>
		public DataTableMapperConfig (Type dataType)
		{
			if (dataType == null) {
				throw new ArgumentNullException (nameof (dataType));
			}
			DataType = dataType;
		}

		/// <summary>
		/// Gets the type of the data.
		/// </summary>
		/// <value>The type of the data.</value>
		public Type DataType { get; private set; }

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is entity table.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsEntityTable { get; set; }
	}
}
