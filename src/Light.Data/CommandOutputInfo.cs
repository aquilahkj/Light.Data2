using System;
using System.Data;

namespace Light.Data
{
    /// <summary>
    /// Command Output Info
    /// </summary>
	public class CommandOutputInfo
	{
		/// <summary>
        /// Get command action name
        /// </summary>
		public string Action { get; set; }

		/// <summary>
        /// Get sql command text
        /// </summary>
		public string Command { get; set; }

		/// <summary>
        /// Get sql parameters
        /// </summary>
		public IDataParameter[] Datas { get; set; }

		/// <summary>
        /// Get sql command type, text or stored procedure
        /// </summary>
		public CommandType CommandType { get; set; }

		/// <summary>
        /// Get the action is use transaction
        /// </summary>
		public bool IsTransaction { get; set; }

		/// <summary>
        /// Get the action safe level
        /// </summary>
		public SafeLevel Level { get; set; }

		/// <summary>
        /// Get the action start time
        /// </summary>
        public DateTime StartTime { get; set; }

		/// <summary>
        /// Get the action end time
        /// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
        /// get data query region offset
        /// </summary>
		public int Start { get; set; }

		/// <summary>
        /// get data query region max size
        /// </summary>
		public int Size { get; set; }

		/// <summary>
        /// Get the action is success
        /// </summary>
		public bool Success { get; set; }

		/// <summary>
        /// If the action throw exception, get the exception message
        /// </summary>
		public string ExceptionMessage { get; set; }

		/// <summary>
        /// Get the action result
        /// </summary>
		public object Result { get; set; }
	}
}
