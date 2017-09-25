using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Light.Data
{
	public class LightDataOptions
    {
		public ConnectionSection[] Connections { get; set; }
    }

	public class ConnectionSection
	{
		public string Name { get; set; }

		public string ConnectionString { get; set; }

		public string ProviderName { get; set; }

        public Dictionary<string,string> ConfigParams { get; set; }
    }
}