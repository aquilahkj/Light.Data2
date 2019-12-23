using System;
using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// Config parameter set.
    /// </summary>
    public class ConfigParamSet
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// Gets the parameter value.
        /// </summary>
        /// <returns>The parameter value.</returns>
        /// <param name="name">Name.</param>
        public string GetParamValue(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (dict.TryGetValue(name, out var value)) {
                return value;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Sets the parameter value.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public void SetParamValue(string name, string value)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            dict[name] = value;
        }
    }
}
