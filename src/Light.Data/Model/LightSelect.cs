using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class LightSelect<T, K> : SelectBase<K>
    {
        #region IEnumerable implementation

        public override IEnumerator<K> GetEnumerator()
        {
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, Selector, _query, _order, false, _region);
            return _context.QueryDataDefineReader<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele).GetEnumerator();
        }

        #endregion

        protected QueryExpression _query;

        public override QueryExpression QueryExpression => _query;

        protected OrderExpression _order;

        public override OrderExpression OrderExpression => _order;

        protected bool _distinct;

        public override bool Distinct => _distinct;

        protected JoinSetting _joinSetting;

        public override JoinSetting JoinSetting => _joinSetting;

        protected Region _region;

        public override Region Region => _region;

        protected SafeLevel _level;

        public override SafeLevel Level => _level;

        public LightSelect(DataContext context, Expression<Func<T, K>> expression, QueryExpression query, OrderExpression order, bool distinct, JoinSetting joinSetting, Region region, SafeLevel level)
            : base(context, expression, typeof(T))
        {
            _query = query;
            _order = order;
            _distinct = distinct;
            _joinSetting = joinSetting;
            _region = region;
            _level = level;
        }

        public override List<K> ToList()
        {
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, Selector, _query, _order, false, _region);
            return _context.QueryDataDefineList<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele);
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
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, Selector, _query, _order, false, region);
            return _context.QueryDataDefineSingle<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var lightQuery = new LightQuery<T1>(Context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<K, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(Context);
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
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, Selector, _query, _order, false, _region);
            return await _context.QueryDataDefineListAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, Dele, cancellationToken);
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
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, Selector, _query, _order, false, region);
            return await _context.QueryDataDefineSingleAsync<K>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, Dele, cancellationToken);
        }


        #endregion
    }
}

