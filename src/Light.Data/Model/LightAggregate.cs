using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    class LightAggregate<T, K> : AggregateBase<K>
    {
        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
            return _context.QueryDataDefineReader<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null).GetEnumerator();
        }

        #endregion

        private QueryExpression _query;

        public override QueryExpression QueryExpression {
            get {
                return _query;
            }
        }

        private QueryExpression _having;

        public override QueryExpression HavingExpression {
            get {
                return _having;
            }
        }

        private OrderExpression _order;

        public override OrderExpression OrderExpression {
            get {
                return _order;
            }
        }

        private Region _region;

        public override Region Region {
            get {
                return _region;
            }
        }

        private SafeLevel _level;

        public override SafeLevel SafeLevel {
            get {
                return _level;
            }
        }

        protected JoinSetting _joinSetting;

        public override JoinSetting JoinSetting {
            get {
                return _joinSetting;
            }
        }

        public LightAggregate(LightQuery<T> query, Expression<Func<T, K>> expression)
            : base(query.Context, expression)
        {
            _query = query.QueryExpression;
            _level = query.Level;
        }

        public override IAggregate<K> Having(Expression<Func<K, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaHavingExpression(expression, Model);
            _having = queryExpression;
            return this;
        }

        public override IAggregate<K> HavingReset()
        {
            _having = null;
            return this;
        }

        public override IAggregate<K> HavingWithAnd(Expression<Func<K, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaHavingExpression(expression, Model);
            _having = QueryExpression.And(_having, queryExpression);
            return this;
        }

        public override IAggregate<K> HavingWithOr(Expression<Func<K, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaHavingExpression(expression, Model);
            _having = QueryExpression.Or(_having, queryExpression);
            return this;
        }

        public override IAggregate<K> OrderBy<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.ASC, Model);
            _order = orderExpression;
            return this;
        }

        public override IAggregate<K> OrderByCatch<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.ASC, Model);
            _order = OrderExpression.Catch(_order, orderExpression);
            return this;
        }

        public override IAggregate<K> OrderByDescending<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.DESC, Model);
            _order = orderExpression;
            return this;
        }

        public override IAggregate<K> OrderByDescendingCatch<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.DESC, Model);
            _order = OrderExpression.Catch(_order, orderExpression);
            return this;
        }

        public override IAggregate<K> OrderByRandom()
        {
            _order = new RandomOrderExpression(DataEntityMapping.GetEntityMapping(typeof(T)));
            return this;
        }

        public override IAggregate<K> OrderByReset()
        {
            _order = null;
            return this;
        }

        public override List<K> ToList()
        {
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
            return _context.QueryDataDefineList<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null);
        }

        public override K[] ToArray()
        {
            return ToList().ToArray();
        }

        public override K First()
        {
            return ElementAt(0);
        }

        public override K ElementAt(int index)
        {
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, region);
            return _context.QueryDataDefineSingle<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
        }

        public override int SelectInsert<P>(Expression<Func<K, P>> expression)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateAggregateInsertSelector(expression, Model);
            QueryCommand queryCommand = _context.Database.SelectInsertWithAggregate(_context, selector, Model, _query, _having, _order);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override IAggregate<K> Take(int count)
        {
            int start;
            int size = count;
            if (_region == null) {
                start = 0;
            }
            else {
                start = _region.Start;
            }
            _region = new Region(start, size);
            return this;
        }

        public override IAggregate<K> Skip(int index)
        {
            int start = index;
            int size;
            if (_region == null) {
                size = int.MaxValue;
            }
            else {
                size = _region.Size;
            }
            _region = new Region(start, size);
            return this;
        }

        public override IAggregate<K> Range(int from, int to)
        {
            int start = from;
            int size = to - from;
            _region = new Region(start, size);
            return this;
        }

        public override IAggregate<K> RangeReset()
        {
            _region = null;
            return this;
        }

        public override IAggregate<K> PageSize(int page, int size)
        {
            if (page < 1) {
                throw new ArgumentOutOfRangeException(nameof(page));
            }
            if (size < 1) {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            page--;
            int start = page * size;
            _region = new Region(start, size);
            return this;
        }

        public override IAggregate<K> SafeMode(SafeLevel level)
        {
            _level = level;
            return this;
        }

        public override IAggregate<K> SetJoinSetting(JoinSetting setting)
        {
            _joinSetting = setting;
            return this;
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
            return await _context.QueryDataDefineListAsync<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null, cancellationToken);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<K> list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, region);
            return await _context.QueryDataDefineSingleAsync<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);
        }

        public async override Task<int> SelectInsertAsync<P>(Expression<Func<K, P>> expression, CancellationToken cancellationToken)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateAggregateInsertSelector(expression, Model);
            QueryCommand queryCommand = _context.Database.SelectInsertWithAggregate(_context, selector, Model, _query, _having, _order);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        #endregion


    }
}

