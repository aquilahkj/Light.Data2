using System;
using System.Collections.Generic;

namespace Light.Data
{
	public class ConfigParamSet
	{
		Dictionary<string, string> dict = new Dictionary<string, string>();

		public string GetParamValue(string name) {
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (dict.TryGetValue(name, out string value)) {
				return value;
			}
			else {
				return null;
			}
		}

		public void SetParamValue(string name, string value) {
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			dict[name] = value;
		}
	}
}
