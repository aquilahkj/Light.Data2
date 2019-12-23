namespace Light.Data
{
    /// <summary>
    /// The Connection Setting
    /// </summary>
	public class ConnectionSetting : IConnectionSetting
	{
        /// <summary>
        /// Connection String
        /// </summary>
		public string ConnectionString { get; set; }

        /// <summary>
        /// Config Param
        /// </summary>
		public ConfigParamSet ConfigParam { get; set; }

        /// <summary>
        /// Name
        /// </summary>
		public string Name { get; set; }

        /// <summary>
        /// Provider Name
        /// </summary>
		public string ProviderName { get; set; }
	}
}