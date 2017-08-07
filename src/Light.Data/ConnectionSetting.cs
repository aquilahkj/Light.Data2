using System;

namespace Light.Data
{
	public class ConnectionSetting : IConnectionSetting
	{
		public string ConnectionString { get; set; }

		public ConfigParamSet ConfigParam { get; set; }

		public string Name { get; set; }

		public string ProviderName { get; set; }
	}
}