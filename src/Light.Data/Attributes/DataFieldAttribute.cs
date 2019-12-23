using System;

namespace Light.Data
{
    /// <summary>
    /// Data field attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class DataFieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataFieldAttribute"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        public DataFieldAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFieldAttribute"/> class.
        /// </summary>
        public DataFieldAttribute()
        {

        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is identity.
        /// </summary>
        /// <value>true</value>
        /// <c>false</c>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Gets or sets the type of the DB.
        /// </summary>
        /// <value>The type of the DB.</value>
        public string DbType { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>The default value.</value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the data order.
        /// </summary>
        /// <value>The data order.</value>
        public int DataOrder { get; set; }

        /// <summary>
        /// Gets or sets the function control.
        /// </summary>
        public FunctionControl FunctionControl { get; set; }
    }
}
