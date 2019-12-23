using System;

namespace Light.Data
{
	/// <summary>
	/// Command output event arguments.
	/// </summary>
	public class CommandOutputEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the command info.
		/// </summary>
		/// <value>The command info.</value>
		public string CommandInfo { get; set; }

		/// <summary>
		/// Gets or sets the runnable command.
		/// </summary>
		/// <value>The runnable command.</value>
		public string RunnableCommand { get; set; }
	}


	
}

