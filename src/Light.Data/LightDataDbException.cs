using System;

namespace Light.Data
{
	/// <summary>
	/// Light data exception.
	/// </summary>
	public class LightDataDbException : LightDataException
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="LightDataException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public LightDataDbException(string message)
			: base(message) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LightDataException"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="innerException">Inner exception.</param>
		public LightDataDbException(string message, Exception innerException)
			: base(message, innerException) {

		}
	}
}
