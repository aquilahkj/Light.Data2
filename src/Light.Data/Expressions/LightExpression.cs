
namespace Light.Data
{
	/// <summary>
	/// Base expression.
	/// </summary>
	abstract class LightExpression
	{
		/// <summary>
		/// Gets or sets the table mapping.
		/// </summary>
		/// <value>The table mapping.</value>
		internal DataEntityMapping TableMapping {
			get;
			set;
		}

		internal abstract string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state);
	}
}
