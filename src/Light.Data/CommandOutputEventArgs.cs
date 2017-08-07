using System;
using System.Data;

namespace Light.Data
{
	/// <summary>
	/// Command output event arguments.
	/// </summary>
	public class CommandOutputEventArgs : EventArgs
	{
		string commandInfo;

		/// <summary>
		/// Gets or sets the command info.
		/// </summary>
		/// <value>The command info.</value>
		public string CommandInfo {
			get {
				return commandInfo;
			}
			set {
				commandInfo = value;
			}
		}

		string runnableCommand;

		/// <summary>
		/// Gets or sets the runnable command.
		/// </summary>
		/// <value>The runnable command.</value>
		public string RunnableCommand {
			get {
				return runnableCommand;
			}
			set {
				runnableCommand = value;
			}
		}
	}


	
}

