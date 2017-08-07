using Newtonsoft.Json;

namespace Light.Data
{
	class ConfigParamSection
	{
		[JsonProperty("name", Required = Required.Always)]
		public string Name { get; set; }

		[JsonProperty("value", Required = Required.Always)]
		public string Value { get; set; }
	}
}