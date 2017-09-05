using System;
using System.Data;

namespace Light.Data
{
	public class CommandOutputInfo
	{
		string action;

        /// <summary>
        /// Get command action name
        /// </summary>
		public string Action {
			get {
				return action;
			}

			set {
				action = value;
			}
		}

		string command;

        /// <summary>
        /// Get sql command text
        /// </summary>
		public string Command {
			get {
				return command;
			}

			set {
				command = value;
			}
		}

		IDataParameter[] datas;

        /// <summary>
        /// Get sql parameters
        /// </summary>
		public IDataParameter[] Datas {
			get {
				return datas;
			}

			set {
				datas = value;
			}
		}

		CommandType commandType;

        /// <summary>
        /// Get sql command type, text or stored procedure
        /// </summary>
		public CommandType CommandType {
			get {
				return commandType;
			}

			set {
				commandType = value;
			}
		}

		bool isTransaction;

        /// <summary>
        /// Get the action is use transaction
        /// </summary>
		public bool IsTransaction {
			get {
				return isTransaction;
			}

			set {
				isTransaction = value;
			}
		}

		SafeLevel level;

        /// <summary>
        /// Get the action safe level
        /// </summary>
		public SafeLevel Level {
			get {
				return level;
			}

			set {
				level = value;
			}
		}

		DateTime startTime;

        /// <summary>
        /// Get the action start time
        /// </summary>
        public DateTime StartTime {
			get {
				return startTime;
			}

			set {
				startTime = value;
			}
		}

		DateTime endTime;

        /// <summary>
        /// Get the action end time
        /// </summary>
		public DateTime EndTime {
			get {
				return endTime;
			}

			set {
				endTime = value;
			}
		}

		int start;

        /// <summary>
        /// get data query region offset
        /// </summary>
		public int Start {
			get {
				return start;
			}

			set {
				start = value;
			}
		}

		int size;

        /// <summary>
        /// get data query region max size
        /// </summary>
		public int Size {
			get {
				return size;
			}

			set {
				size = value;
			}
		}

		bool success;

        /// <summary>
        /// Get the action is success
        /// </summary>
		public bool Success {
			get {
				return success;
			}

			set {
				success = value;
			}
		}

		string exceptionMessage;

        /// <summary>
        /// If the action throw exception, get the exception message
        /// </summary>
		public string ExceptionMessage {
			get {
				return exceptionMessage;
			}

			set {
				exceptionMessage = value;
			}
		}

		object result;

        /// <summary>
        /// Get the action result
        /// </summary>
		public object Result {
			get {
				return result;
			}

			set {
				result = value;
			}
		}
	}
}
