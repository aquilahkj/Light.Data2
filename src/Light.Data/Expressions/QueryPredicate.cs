
namespace Light.Data
{
	/// <summary>
	/// Query predicate.
	/// </summary>
	internal enum QueryPredicate
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
	internal enum QueryCollectionPredicate
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
