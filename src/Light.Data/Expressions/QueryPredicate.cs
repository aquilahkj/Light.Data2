
namespace Light.Data
{
	/// <summary>
	/// Query predicate.
	/// </summary>
	enum QueryPredicate
	{
		Eq,
		Lt,
		LtEq,
		Gt,
		GtEq,
		NotEq
	}

	/// <summary>
	/// Query collection predicate.
	/// </summary>
	enum QueryCollectionPredicate
	{
		In,
		NotIn,
		GtAll,
		LtAll,
		GtAny,
		LtAny,
		GtEqAll,
		LtEqAll,
		GtEqAny,
		LtEqAny
	}
}
