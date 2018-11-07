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
	class LightJoinTable<T, T1> : IJoinTable<T, T1>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (QueryBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			DataEntityMapping entityMapping1 = left.Mapping;
			DataEntityMapping entityMapping2 = right.Mapping;
			_maps.Add (entityMapping1.GetRelationMap ());
			_maps.Add (entityMapping2.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model1 = new EntityJoinModel (entityMapping1, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			EntityJoinModel model2 = new EntityJoinModel (entityMapping2, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (QueryBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			DataEntityMapping entityMapping1 = left.Mapping;
			_maps.Add (entityMapping1.GetRelationMap ());
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model1 = new EntityJoinModel (entityMapping1, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			AggregateJoinModel model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			DataEntityMapping entityMapping1 = right.Mapping;
			_maps.Add (new AggregateMap (left.Model));
			_maps.Add (entityMapping1.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression);
			EntityJoinModel model2 = new EntityJoinModel (entityMapping1, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			_maps.Add (new AggregateMap (left.Model));
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression);
			AggregateJoinModel model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, QueryBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (new SelectMap (left.Model));
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			EntityJoinModel model2 = new EntityJoinModel (entityMapping, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (QueryBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			DataEntityMapping entityMapping = left.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model1 = new EntityJoinModel (entityMapping, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			SelectJoinModel model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			_maps.Add (new SelectMap (left.Model));
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			SelectJoinModel model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (AggregateBase<T> left, JoinType joinType, SelectBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			_maps.Add (new AggregateMap (left.Model));
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model1 = new AggregateJoinModel (left.Model, "T0", null, left.QueryExpression, left.HavingExpression, left.OrderExpression);
			SelectJoinModel model2 = new SelectJoinModel (right.Model, "T1", connect, right.QueryExpression, right.OrderExpression);
			model2.Distinct = right.Distinct;
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public LightJoinTable (SelectBase<T> left, JoinType joinType, AggregateBase<T1> right, Expression<Func<T, T1, bool>> onExpression)
		{
			_context = left.Context;
			_maps.Add (new SelectMap (left.Model));
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model1 = new SelectJoinModel (left.Model, "T0", null, left.QueryExpression, left.OrderExpression);
			model1.Distinct = left.Distinct;
			AggregateJoinModel model2 = new AggregateJoinModel (right.Model, "T1", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model1);
			_modelList.Add (model2);
		}

		public IJoinTable<T, T1> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1> Where (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1> WhereWithAnd (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1> WhereWithOr (Expression<Func<T, T1, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderByCatch<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1> OrderBy<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1> OrderByDescending<TKey> (Expression<Func<T, T1, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			LightQuery<T2> lightQuery = new LightQuery<T2> (_context);
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			QueryBase<T2> queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			QueryBase<T2> queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			QueryBase<T2> queryBase = query as QueryBase<T2>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2> Join<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			AggregateBase<T2> aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			AggregateBase<T2> aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			AggregateBase<T2> aggregateBase = aggregate as AggregateBase<T2>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2> Join<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			SelectBase<T2> selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2> LeftJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			SelectBase<T2> selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2> RightJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression) //where T2 : class
		{
			SelectBase<T2> selectBase = select as SelectBase<T2>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2> : IJoinTable<T, T1, T2>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, QueryBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T2", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, AggregateBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T2", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1> left, JoinType joinType, SelectBase<T2> right, Expression<Func<T, T1, T2, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T2", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2> Where (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> WhereWithAnd (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> WhereWithOr (Expression<Func<T, T1, T2, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByCatch<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2> OrderBy<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByDescending<TKey> (Expression<Func<T, T1, T2, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			LightQuery<T3> lightQuery = new LightQuery<T3> (_context);
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			QueryBase<T3> queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			QueryBase<T3> queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			QueryBase<T3> queryBase = query as QueryBase<T3>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			AggregateBase<T3> aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			AggregateBase<T3> aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			AggregateBase<T3> aggregateBase = aggregate as AggregateBase<T3>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> Join<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			SelectBase<T3> selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> LeftJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			SelectBase<T3> selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3> RightJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression) //where T3 : class
		{
			SelectBase<T3> selectBase = select as SelectBase<T3>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3> : IJoinTable<T, T1, T2, T3>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, QueryBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T3", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, AggregateBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T3", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2> left, JoinType joinType, SelectBase<T3> right, Expression<Func<T, T1, T2, T3, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T3", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Where (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> WhereWithAnd (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> WhereWithOr (Expression<Func<T, T1, T2, T3, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			LightQuery<T4> lightQuery = new LightQuery<T4> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			QueryBase<T4> queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			QueryBase<T4> queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			QueryBase<T4> queryBase = query as QueryBase<T4>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			AggregateBase<T4> aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			AggregateBase<T4> aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			AggregateBase<T4> aggregateBase = aggregate as AggregateBase<T4>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> Join<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			SelectBase<T4> selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			SelectBase<T4> selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression) //where T4 : class
		{
			SelectBase<T4> selectBase = select as SelectBase<T4>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4> : IJoinTable<T, T1, T2, T3, T4>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, QueryBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T4", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, AggregateBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T4", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3> left, JoinType joinType, SelectBase<T4> right, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T4", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Where (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			LightQuery<T5> lightQuery = new LightQuery<T5> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			QueryBase<T5> queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			QueryBase<T5> queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			QueryBase<T5> queryBase = query as QueryBase<T5>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			AggregateBase<T5> aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			AggregateBase<T5> aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			AggregateBase<T5> aggregateBase = aggregate as AggregateBase<T5>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			SelectBase<T5> selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			SelectBase<T5> selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression) //where T5 : class
		{
			SelectBase<T5> selectBase = select as SelectBase<T5>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4, T5> : IJoinTable<T, T1, T2, T3, T4, T5>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, QueryBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T5", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, AggregateBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T5", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4> left, JoinType joinType, SelectBase<T5> right, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T5", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Where (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			LightQuery<T6> lightQuery = new LightQuery<T6> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			QueryBase<T6> queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			QueryBase<T6> queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			QueryBase<T6> queryBase = query as QueryBase<T6>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			AggregateBase<T6> aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			AggregateBase<T6> aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			AggregateBase<T6> aggregateBase = aggregate as AggregateBase<T6>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			SelectBase<T6> selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			SelectBase<T6> selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression) //where T6 : class
		{
			SelectBase<T6> selectBase = select as SelectBase<T6>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4, T5, T6> : IJoinTable<T, T1, T2, T3, T4, T5, T6>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, QueryBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T6", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, AggregateBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T6", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5> left, JoinType joinType, SelectBase<T6> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T6", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			LightQuery<T7> lightQuery = new LightQuery<T7> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			QueryBase<T7> queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			QueryBase<T7> queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			QueryBase<T7> queryBase = query as QueryBase<T7>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			AggregateBase<T7> aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			AggregateBase<T7> aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			AggregateBase<T7> aggregateBase = aggregate as AggregateBase<T7>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			SelectBase<T7> selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			SelectBase<T7> selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression) //where T7 : class
		{
			SelectBase<T7> selectBase = select as SelectBase<T7>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, QueryBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T7", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, AggregateBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T7", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6> left, JoinType joinType, SelectBase<T7> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T7", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			LightQuery<T8> lightQuery = new LightQuery<T8> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			QueryBase<T8> queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			QueryBase<T8> queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			QueryBase<T8> queryBase = query as QueryBase<T8>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			AggregateBase<T8> aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			AggregateBase<T8> aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			AggregateBase<T8> aggregateBase = aggregate as AggregateBase<T8>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			SelectBase<T8> selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			SelectBase<T8> selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression) //where T8 : class
		{
			SelectBase<T8> selectBase = select as SelectBase<T8>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, QueryBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T8", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, AggregateBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T8", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7> left, JoinType joinType, SelectBase<T8> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T8", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			if (queryExpression != null) {
				lightQuery.Where (queryExpression);
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			LightQuery<T9> lightQuery = new LightQuery<T9> (_context);
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, lightQuery, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			QueryBase<T9> queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			QueryBase<T9> queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			QueryBase<T9> queryBase = query as QueryBase<T9>;
			if (queryBase == null) {
				throw new ArgumentException (nameof (queryBase));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, queryBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			AggregateBase<T9> aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			AggregateBase<T9> aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			AggregateBase<T9> aggregateBase = aggregate as AggregateBase<T9>;
			if (aggregateBase == null) {
				throw new ArgumentException (nameof (aggregate));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, aggregateBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			SelectBase<T9> selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.InnerJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			SelectBase<T9> selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.LeftJoin, selectBase, onExpression);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression) //where T9 : class
		{
			SelectBase<T9> selectBase = select as SelectBase<T9>;
			if (selectBase == null) {
				throw new ArgumentException (nameof (select));
			}
			return new LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> (this, JoinType.RightJoin, selectBase, onExpression);
		}

	}

	/// <summary>
	/// Join table.
	/// </summary>		
	class LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> : IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{

		QueryExpression _query;

		public QueryExpression Query {
			get {
				return _query;
			}
		}

		OrderExpression _order;

		public OrderExpression Order {
			get {
				return _order;
			}
		}

		Region _region;

		public Region Region {
			get {
				return _region;
			}
		}

		readonly DataContext _context;

		public DataContext Context {
			get {
				return _context;
			}
		}

		SafeLevel _level = SafeLevel.None;

		public SafeLevel Level {
			get {
				return _level;
			}
		}

		bool _distinct;

		internal bool Distinct {
			get {
				return _distinct;
			}
		}

		readonly List<IJoinModel> _modelList = new List<IJoinModel> ();

		internal List<IJoinModel> ModelList {
			get {
				return _modelList;
			}
		}

		readonly List<IMap> _maps = new List<IMap> ();

		internal List<IMap> Maps {
			get {
				return _maps;
			}
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, QueryBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			DataEntityMapping entityMapping = right.Mapping;
			_maps.Add (entityMapping.GetRelationMap ());
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			EntityJoinModel model = new EntityJoinModel (entityMapping, "T9", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, AggregateBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new AggregateMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			AggregateJoinModel model = new AggregateJoinModel (right.Model, "T9", connect, right.QueryExpression, right.HavingExpression, right.OrderExpression);
			_modelList.Add (model);
		}

		public LightJoinTable (LightJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> left, JoinType joinType, SelectBase<T9> right, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression)
		{
			_query = left.Query;
			_order = left.Order;
			_context = left.Context;
			_modelList.AddRange (left.ModelList);
			_maps.AddRange (left.Maps);
			_maps.Add (new SelectMap (right.Model));
			DataFieldExpression on = LambdaExpressionExtend.ResolvelambdaOnExpression (onExpression, _maps);
			JoinConnect connect = new JoinConnect (joinType, on);
			SelectJoinModel model = new SelectJoinModel (right.Model, "T9", connect, right.QueryExpression, right.OrderExpression);
			model.Distinct = right.Distinct;
			_modelList.Add (model);
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereReset ()
		{
			_query = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = queryExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.And (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression)
		{
			var queryExpression = LambdaExpressionExtend.ResolveLambdaMutliQueryExpression (expression, _maps);
			_query = QueryExpression.Or (_query, queryExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = OrderExpression.Catch (_order, orderExpression);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.ASC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression)
		{
			var orderExpression = LambdaExpressionExtend.ResolveLambdaMutliOrderByExpression (expression, OrderType.DESC, _maps);
			_order = orderExpression;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByReset ()
		{
			_order = null;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Take (int count)
		{
			int start;
			int size = count;
			if (_region == null) {
				start = 0;
			}
			else {
				start = _region.Start;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Skip (int index)
		{
			int start = index;
			int size;
			if (_region == null) {
				size = int.MaxValue;
			}
			else {
				size = _region.Size;
			}
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Range (int from, int to)
		{
			int start = from;
			int size = to - from;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RangeReset ()
		{
			_region = null;
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
			int start = page * size;
			_region = new Region (start, size);
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SafeMode (SafeLevel level)
		{
			_level = level;
			return this;
		}

		public IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SetDistinct (bool distinct)
		{
			_distinct = distinct;
			return this;
		}

		public ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression) //where K : class
		{
			LightSelectJoin<K> selectable = new LightSelectJoin<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K> (_context, expression, _modelList, _maps, _query, _order, _distinct, _region, _level);
			return selectable;
		}

		public int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
			//return this._context.SelectInsertWithJoinTable (selector, _modelList, _query, _order, _distinct, _level);
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		public int Count() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		public long LongCount() {
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value =  _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(this._context.AggregateJoinTableCount(_modelList, _query, _level));
		}

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		public async Task<int> CountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
			//return Convert.ToInt32(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		public async Task<long> LongCountAsync(CancellationToken cancellationToken)
		{
			QueryCommand queryCommand = _context.Database.AggregateJoinTableCount(_context, _modelList, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
			//return Convert.ToInt64(await this._context.AggregateJoinTableCountAsync(_modelList, _query, _level, cancellationToken));
		}

		/// <summary>
		/// Insert the select data
		/// </summary>
		/// <value>insert count.</value>
		public async Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression, CancellationToken cancellationToken) //where K : class, new()
		{
			InsertSelector selector = LambdaExpressionExtend.CreateMutliInsertSelector (expression, _maps);
			QueryCommand queryCommand = _context.Database.SelectInsertWithJoinTable(_context, selector, _modelList, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
			//return await this._context.SelectInsertWithJoinTableAsync (selector, _modelList, _query, _order, _distinct, _level, cancellationToken);
		}

	}

}


