
namespace Light.Data
{
	internal class JoinConnect
	{
		public JoinType Type { get; set; } = JoinType.InnerJoin;

		public DataFieldExpression On { get; set; }

		public JoinConnect (JoinType type, DataFieldExpression on)
		{
			Type = type;
			On = on;
		}
	}
}

