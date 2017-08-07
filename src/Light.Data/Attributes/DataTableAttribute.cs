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
			this.tableName = tableName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DataTableAttribute"/> class.
		/// </summary>
		public DataTableAttribute()
		{
			this.isEntityTable = true;
		}
		string tableName;

		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName {
			get {
				return tableName;
			}

			set {
				tableName = value;
			}
		}
		bool isEntityTable;

		/// <summary>
		/// Gets or sets the extend parameters.
		/// </summary>
		/// <value>The extend parameters.</value>
		/// <summary>
		/// Gets or sets a value indicating whether this instance is entity table.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsEntityTable {
			get {
				return isEntityTable;
			}

			set {
				isEntityTable = value;
			}
		}
	}
}
