using System;
using System.Data;

namespace Light.Data
{
	public class CommandOutputInfo
	{
		string action;

		public string Action {
			get {
				return action;
			}

			set {
				action = value;
			}
		}

		string command;

		public string Command {
			get {
				return command;
			}

			set {
				command = value;
			}
		}

		IDataParameter[] datas;

		public IDataParameter[] Datas {
			get {
				return datas;
			}

			set {
				datas = value;
			}
		}

		CommandType commandType;

		public CommandType CommandType {
			get {
				return commandType;
			}

			set {
				commandType = value;
			}
		}

		bool isTransaction;

		public bool IsTransaction {
			get {
				return isTransaction;
			}

			set {
				isTransaction = value;
			}
		}

		SafeLevel level;

		public SafeLevel Level {
			get {
				return level;
			}

			set {
				level = value;
			}
		}

		DateTime startTime;

		public DateTime StartTime {
			get {
				return startTime;
			}

			set {
				startTime = value;
			}
		}

		DateTime endTime;

		public DateTime EndTime {
			get {
				return endTime;
			}

			set {
				endTime = value;
			}
		}

		int start;

		public int Start {
			get {
				return start;
			}

			set {
				start = value;
			}
		}

		int size;

		public int Size {
			get {
				return size;
			}

			set {
				size = value;
			}
		}

		bool success;

		public bool Success {
			get {
				return success;
			}

			set {
				success = value;
			}
		}

		string exceptionMessage;

		public string ExceptionMessage {
			get {
				return exceptionMessage;
			}

			set {
				exceptionMessage = value;
			}
		}

		object result;

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
