using Newtonsoft.Json;

namespace Light.Data
{
	class LightDataOptions
	{
		public ConnectionSection[] Connections { get; set; }
	}

	class ConnectionSection
	{
		public string Name { get; set; }

		public string ConnectionString { get; set; }

		public string ProviderName { get; set; }

		public ConfigParamSection[] ConfigParams { get; set; }
	}
}