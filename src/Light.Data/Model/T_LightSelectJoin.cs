using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Light.Data
{
	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, T5, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, T5, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, T5, T6, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K> : LightSelectJoin<K>
	{
		public LightSelectJoin (DataContext context, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression, List<IJoinModel> models, List<IMap> maps, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
			:base(context, expression, models, maps, query, order, distinct, region, level)
		{

		}
	}

}


