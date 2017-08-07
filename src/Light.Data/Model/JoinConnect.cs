
namespace Light.Data
{
	class JoinConnect
	{
		JoinType _type = JoinType.InnerJoin;

		public JoinType Type {
			get {
				return _type;
			}
			set {
				_type = value;
			}
		}

		DataFieldExpression _on;

		public DataFieldExpression On {
			get {
				return _on;
			}
			set {
				_on = value;
			}
		}

		public JoinConnect (JoinType type, DataFieldExpression on)
		{
			_type = type;
			_on = on;
		}
	}
}

