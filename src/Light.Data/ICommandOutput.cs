namespace Light.Data
{
	/// <summary>
	/// ICommand output.
	/// </summary>
	public interface ICommandOutput
	{
		/// <summary>
		/// Output the specified info.
		/// </summary>
		/// <returns>The output.</returns>
		/// <param name="info">Info.</param>
		void Output (CommandOutputInfo info);
	}
}

