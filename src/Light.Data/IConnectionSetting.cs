namespace Light.Data
{
    /// <summary>
    /// Connection setting.
    /// </summary>
	public interface IConnectionSetting
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
		string ConnectionString { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
		string Name { get; }
        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
		string ProviderName { get; }
        /// <summary>
        /// Gets the config parameter.
        /// </summary>
        /// <value>The config parameter.</value>
		ConfigParamSet ConfigParam { get; }
    }
}
