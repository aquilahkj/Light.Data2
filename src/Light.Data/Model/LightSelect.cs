using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    class LightSelect<T, K> : SelectBase<K>
    {
        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            return Context.QueryEntityDataReader<K>(Mapping, Selector, _query, _order, _distinct, _region, Dele).GetEnumerator();
        }

        #endregion

        protected QueryExpression _query;

        public override QueryExpression QueryExpression {
            get {
                return _query;
            }
        }

        protected OrderExpression _order;

        public override OrderExpression OrderExpression {
            get {
                return _order;
            }
        }

        protected bool _distinct;

        public override bool Distinct {
            get {
                return _distinct;
            }
        }

        protected Region _region;

        public override Region Region {
            get {
                return _region;
            }
        }

        protected SafeLevel _level;

        public override SafeLevel Level {
            get {
                return _level;
            }
        }

        public LightSelect(DataContext context, Expression<Func<T, K>> expression, QueryExpression query, OrderExpression order, bool distinct, Region region, SafeLevel level)
            : base(context, expression, typeof(T))
        {
            _query = query;
            _order = order;
            _distinct = distinct;
            _region = region;
            _level = level;
        }

        public override List<K> ToList()
        {
            List<K> list = Context.QueryEntityDataList<K>(Mapping, Selector, _query, _order, _distinct, _region, Dele);
            return list;
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
            K target = default(K);
            Region region = new Region(index, 1);
            target = Context.QueryEntityDataSingle<K>(Mapping, Selector, _query, _order, false, region, Dele);
            return target;
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, queryBase, onExpression);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, queryBase, onExpression);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(Context);
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, queryBase, onExpression);
        }

        public override IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, selectBase, onExpression);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, selectBase, onExpression);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, selectBase, onExpression);
        }

        #region async

        public async override Task<List<K>> ToListAsync(CancellationToken cancellationToken)
        {
            List<K> list = await Context.QueryEntityDataListAsync<K>(Mapping, Selector, _query, _order, _distinct, _region, Dele, cancellationToken);
            return list;
        }

        public async override Task<List<K>> ToListAsync()
        {
            return await ToListAsync(CancellationToken.None);
        }

        public async override Task<K[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<K> list = await ToListAsync(cancellationToken);
            return list.ToArray();
        }

        public async override Task<K[]> ToArrayAsync()
        {
            return await ToArrayAsync(CancellationToken.None);
        }

        public async override Task<K> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<K> FirstAsync()
        {
            return await FirstAsync(CancellationToken.None);
        }

        public async override Task<K> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            K target = default(K);
            Region region = new Region(index, 1);
            target = await Context.QueryEntityDataSingleAsync<K>(Mapping, Selector, _query, _order, false, region, Dele, cancellationToken);
            return target;
        }

        public async override Task<K> ElementAtAsync(int index)
        {
            return await ElementAtAsync(index, CancellationToken.None);
        }

        #endregion
    }
}

