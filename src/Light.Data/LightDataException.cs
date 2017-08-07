using System;

namespace Light.Data
{
	/// <summary>
	/// Light data exception.
	/// </summary>
	public class LightDataException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LightDataException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public LightDataException(string message)
			: base(message) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LightDataException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
		public LightDataException(string message, Exception innerException)
			: base(message, innerException) {

		}
	}
}
