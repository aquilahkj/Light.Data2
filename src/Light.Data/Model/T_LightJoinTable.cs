using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Light.Data
{
	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1> : IJoinTable<T, T1>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (QueryBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			var entityMapping1 = left.Mapping;
			var entityMapping2 = right.Mapping;
			Maps.Add (entityMapping1.GetRelationMap ());
			Maps.Add (entityMapping2.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new EntityJoinModel (entityMapping1, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new EntityJoinModel (entityMapping2, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (QueryBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			var entityMapping1 = left.Mapping;
			Maps.Add (entityMapping1.GetRelationMap ());
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new EntityJoinModel (entityMapping1, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			var entityMapping1 = right.Mapping;
			Maps.Add (new AggregateMap (left.Model));
			Maps.Add (entityMapping1.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression, leftSetting);
			var model2 = new EntityJoinModel (entityMapping1, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			Maps.Add (new AggregateMap (left.Model));
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression, leftSetting);
			var model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			var entityMapping = right.Mapping;
			Maps.Add (new SelectMap (left.Model));
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new EntityJoinModel (entityMapping, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (QueryBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			var entityMapping = left.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new EntityJoinModel (entityMapping, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			Maps.Add (new SelectMap (left.Model));
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			Maps.Add (new AggregateMap (left.Model));
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression, leftSetting);
			var model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression, JoinSetting leftSetting, JoinSetting rightSetting)
		{
			Context = left.Context;
			Maps.Add (new SelectMap (left.Model));
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression, leftSetting);
			var model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model1);
			ModelList.Add (model2);
		}

		public IJoinTable<T, T1> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1> Where (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1> WhereWithAnd (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1> WhereWithOr (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderByConcat<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderBy<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1> OrderByDescending<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T2> (Context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> Join<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2> : IJoinTable<T, T1, T2>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, QueryBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T2", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, AggregateBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T2", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, SelectBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T2", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2> Where (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> WhereWithAnd (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> WhereWithOr (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByConcat<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderBy<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByDescending<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T3> (Context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3> : IJoinTable<T, T1, T2, T3>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, QueryBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T3", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, AggregateBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T3", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, SelectBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T3", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Where (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> WhereWithAnd (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> WhereWithOr (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T4> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4> : IJoinTable<T, T1, T2, T3, T4>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, QueryBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T4", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, AggregateBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T4", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, SelectBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T4", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Where (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T5> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4, T5> : IJoinTable<T, T1, T2, T3, T4, T5>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, QueryBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T5", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, AggregateBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T5", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, SelectBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T5", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Where (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T6> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4, T5, T6> : IJoinTable<T, T1, T2, T3, T4, T5, T6>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, QueryBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T6", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, AggregateBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T6", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, SelectBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T6", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T7> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, QueryBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T7", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, AggregateBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T7", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, SelectBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T7", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T8> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, QueryBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T8", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, AggregateBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T8", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, SelectBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T8", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression, JoinSetting.None);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, queryBase, onExpression, queryBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, aggregateBase, onExpression, aggregateBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, selectBase, onExpression, selectBase.JoinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var lightQuery = new LightQuery<T9> (Context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, queryBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, aggregateBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, selectBase, onExpression, joinSetting);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting)
		{
			var selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, selectBase, onExpression, joinSetting);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	internal class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		public QueryExpression Query { get; private set; }

		public OrderExpression Order { get; private set; }

		public Region Region { get; private set; }

		public DataContext Context { get; }

		public SafeLevel Level { get; private set; } = SafeLevel.None;

		internal bool Distinct { get; private set; }

		internal List<IJoinModel> ModelList { get; } = new List<IJoinModel> ();

		internal List<IMap> Maps { get; } = new List<IMap> ();

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, QueryBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			var entityMapping = right.Mapping;
			Maps.Add (entityMapping.GetRelationMap ());
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new EntityJoinModel (entityMapping, "T9", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, AggregateBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new AggregateMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new AggregateJoinModel (right.Model, "T9", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, SelectBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting rightSetting)
		{
			Query = left.Query;
			Order = left.Order;
			Context = left.Context;
			ModelList.AddRange (left.ModelList);
			Maps.AddRange (left.Maps);
			Maps.Add (new SelectMap (right.Model));
			var on = LambdaExpressionExtend.ResolveLambdaOnExpression (onExpression, Maps);
			var connect = new JoinConnect (joinType, on);
			var model = new SelectJoinModel (right.Model, "T9", connect, right.QueryExpression, right.OrderExpression, rightSetting);
			ModelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereReset ()
		{
			Query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.And (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMultiQueryExpression (expression, Maps);
			Query = QueryExpression.Or (Query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescendingConcat<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = OrderExpression.Concat (Order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.ASC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMultiOrderByExpression (expression, OrderType.DESC, Maps);
			Order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByReset ()
		{
			Order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Take (int count)
		{
			int start;
			var size = count;
			if (Region == null) {
				start = 0;
			}
			else {
				start = Region.Start;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Skip (int index)
		{
			var start = index;
			int size;
			if (Region == null) {
				size = int.MaxValue;
			}
			else {
				size = Region.Size;
			}
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Range (int from, int to)
		{
			var start = from;
			var size = to - from;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RangeReset ()
		{
			Region = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> PageSize (int page, int size)
		{
			if (page < 1) {
				throw new ArgumentOutOfRangeException (nameof (page));
			}
			if (size < 1) {
				throw new ArgumentOutOfRangeException (nameof (size));
			}
			page--;
			var start = page * size;
			Region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SafeMode (SafeLevel level)
		{
			Level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SetDistinct (bool distinct)
		{
			Distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression) 
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K> (Context, expression, ModelList, Maps, Query, Order, Distinct, Region, Level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression)
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return Context.ExecuteNonQuery(queryCommand.Command, Level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value =  Context.ExecuteScalar(queryCommand.Command, Level);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> CountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt32(value);
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <param name="cancellationToken">CancellationToken.</param>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			var queryCommand = Context.Database.AggregateJoinTableCount(Context, ModelList, Query);
            var value = await Context.ExecuteScalarAsync(queryCommand.Command, Level, cancellationToken);
            return Convert.ToInt64(value);
		}

		/// <summary>
		/// Insert the select data to the special table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression, CancellationToken cancellationToken = default(CancellationToken))
		{
			var selector = LambdaExpressionExtend.CreateMultiInsertSelector (expression, Maps);
			var queryCommand = Context.Database.SelectInsertWithJoinTable(Context, selector, ModelList, Query, Order, Distinct);
            return await Context.ExecuteNonQueryAsync(queryCommand.Command, Level, cancellationToken);
		}

	}

}


