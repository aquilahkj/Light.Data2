using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Light data options.
    /// </summary>
	public class LightDataOptions
    {
        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>The connections.</value>
		public ConnectionSection[] Connections { get; set; }
    }

    /// <summary>
    /// Connection section.
    /// </summary>
	public class ConnectionSection
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
		public string Name { get; set; }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
		public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
		public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the config parameters.
        /// </summary>
        /// <value>The config parameters.</value>
        public Dictionary<string, string> ConfigParams { get; set; }
    }
}