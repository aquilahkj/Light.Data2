using System;

namespace Light.Data
{
	/// <summary>
	/// Data field config.
	/// </summary>
	class DataFieldMapperConfig
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataFieldMapperConfig"/> class.
		/// </summary>
		/// <param name="fieldName">Field name.</param>
		public DataFieldMapperConfig (string fieldName)
		{
			if (string.IsNullOrEmpty (fieldName)) {
				throw new ArgumentNullException (nameof (fieldName));
			}
			FieldName = fieldName;
		}

		/// <summary>
		/// Gets the name of the field.
		/// </summary>
		/// <value>The name of the field.</value>
		public string FieldName {
			get;
			private set;
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is nullable.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsNullable {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is primary key.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsPrimaryKey {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is identity.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsIdentity {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the type of the DB.
		/// </summary>
		/// <value>The type of the DB.</value>
		public string DbType {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the default value.
		/// </summary>
		/// <value>The default value.</value>
		public object DefaultValue {
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the data order.
		/// </summary>
		/// <value>The data order.</value>
		public int DataOrder {
			get;
			set;
		}

        public FunctionControl FunctionControl {
            get;
            set;
        }
	}
}
