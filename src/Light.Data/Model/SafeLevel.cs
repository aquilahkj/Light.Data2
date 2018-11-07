
namespace Light.Data
{
	/// <summary>
	/// Safe level.
	/// </summary>
	public enum SafeLevel
	{
		/// <summary>
		/// Use default transaction level.
		/// </summary>
		Default,
        /// <summary>
        /// Not use transaction level.
        /// </summary>
        None,
        /// <summary>
        /// Use ReadUncommitted transaction level.
        /// </summary>
        Low,
        /// <summary>
        /// Use ReadCommitted transaction level.
        /// </summary>
        Normal,
        /// <summary>
        /// Use RepeatableRead transaction level.
        /// </summary>
        High,
        /// <summary>
        /// Use Serializable transaction level.
        /// </summary>
        Serializable
    }
}
