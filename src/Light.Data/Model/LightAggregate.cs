using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class LightAggregate<T, K> : AggregateBase<K>
    {
        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
            return _context.QueryDataDefineReader<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null).GetEnumerator();
        }

        #endregion

        private QueryExpression _query;

        public override QueryExpression QueryExpression => _query;

        private QueryExpression _having;

        public override QueryExpression HavingExpression => _having;

        private OrderExpression _order;

        public override OrderExpression OrderExpression => _order;

        private Region _region;

        public override Region Region => _region;

        private SafeLevel _level;

        public override SafeLevel SafeLevel => _level;

        protected JoinSetting _joinSetting;

        public override JoinSetting JoinSetting => _joinSetting;

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

        public override IAggregate<K> OrderByConcat<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.ASC, Model);
            _order = OrderExpression.Concat(_order, orderExpression);
            return this;
        }

        public override IAggregate<K> OrderByDescending<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.DESC, Model);
            _order = orderExpression;
            return this;
        }

        public override IAggregate<K> OrderByDescendingConcat<TKey>(Expression<Func<K, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaAggregateOrderByExpression(expression, OrderType.DESC, Model);
            _order = OrderExpression.Concat(_order, orderExpression);
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
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
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
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, region);
            return _context.QueryDataDefineSingle<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
        }

        public override int SelectInsert<P>(Expression<Func<K, P>> expression)
        {
            var selector = LambdaExpressionExtend.CreateAggregateInsertSelector(expression, Model);
            var queryCommand = _context.Database.SelectInsertWithAggregate(_context, selector, Model, _query, _having, _order);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override IAggregate<K> Take(int count)
        {
            int start;
            var size = count;
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
            var start = index;
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
            var start = from;
            var size = to - from;
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
            var start = page * size;
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
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, _region);
            return await _context.QueryDataDefineListAsync<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null, cancellationToken);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default(CancellationToken))
        {
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, Model, _query, _having, _order, region);
            return await _context.QueryDataDefineSingleAsync<K>(Model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);
        }

        public async override Task<int> SelectInsertAsync<P>(Expression<Func<K, P>> expression, CancellationToken cancellationToken = default(CancellationToken))
        {
            var selector = LambdaExpressionExtend.CreateAggregateInsertSelector(expression, Model);
            var queryCommand = _context.Database.SelectInsertWithAggregate(_context, selector, Model, _query, _having, _order);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        #endregion


    }
}

