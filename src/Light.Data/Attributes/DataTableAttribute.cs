using System;

namespace Light.Data
{
	/// <summary>
	/// Data table attribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class DataTableAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableAttribute"/> class.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		public DataTableAttribute(string tableName) : this()
		{
			this.TableName = tableName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableAttribute"/> class.
		/// </summary>
		public DataTableAttribute()
		{
			IsEntityTable = true;
		}

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set; }

		/// <summary>
		/// Gets or sets the extend parameters.
		/// </summary>
		/// <value>The extend parameters.</value>
		/// <summary>
		/// Gets or sets a value indicating whether this instance is entity table.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsEntityTable { get; set; }
	}
}
