using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal class LightQuery<T> : QueryBase<T>
    {
        #region IEnumerable implementation

        public override IEnumerator<T> GetEnumerator()
        {
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return _context.QueryDataDefineReader<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null).GetEnumerator();
        }

        #endregion

        private QueryExpression _query;

        public override QueryExpression QueryExpression => _query;

        private OrderExpression _order;

        public override OrderExpression OrderExpression => _order;

        private Region _region;

        public override Region Region => _region;

        private bool _distinct;

        public override bool Distinct => _distinct;

        private JoinSetting _joinSetting;

        public override JoinSetting JoinSetting => _joinSetting;

        private SafeLevel _level = SafeLevel.None;

        public virtual SafeLevel Level => _level;

        internal LightQuery(DataContext dataContext)
            : base(dataContext)
        {

        }

        #region LQuery<T> Member

        public override IQuery<T> WhereReset()
        {
            _query = null;
            return this;
        }

        public override IQuery<T> Where(Expression<Func<T, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaQueryExpression(expression);
            _query = queryExpression;
            return this;
        }

        public override IQuery<T> WhereWithAnd(Expression<Func<T, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaQueryExpression(expression);
            _query = QueryExpression.And(_query, queryExpression);
            return this;
        }

        public override IQuery<T> WhereWithOr(Expression<Func<T, bool>> expression)
        {
            var queryExpression = LambdaExpressionExtend.ResolveLambdaQueryExpression(expression);
            _query = QueryExpression.Or(_query, queryExpression);
            return this;
        }

        public override IQuery<T> OrderByConcat<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.ASC);
            _order = OrderExpression.Concat(_order, orderExpression);
            return this;
        }

        public override IQuery<T> OrderByDescendingConcat<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.DESC);
            _order = OrderExpression.Concat(_order, orderExpression);
            return this;
        }

        public override IQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.ASC);
            _order = orderExpression;
            return this;
        }

        public override IQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.DESC);
            _order = orderExpression;
            return this;
        }

        public override IQuery<T> OrderByReset()
        {
            _order = null;
            return this;
        }

        public override IQuery<T> OrderByRandom()
        {
            _order = new RandomOrderExpression(DataEntityMapping.GetEntityMapping(typeof(T)));
            return this;
        }

        public override IQuery<T> Take(int count)
        {
            var size = count;
            var start = _region?.Start ?? 0;
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> Skip(int index)
        {
            var start = index;
            var size = _region?.Size ?? int.MaxValue;
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> Range(int from, int to)
        {
            var start = from;
            var size = to - from;
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> RangeReset()
        {
            _region = null;
            return this;
        }

        public override IQuery<T> PageSize(int page, int size)
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

        public override IQuery<T> SafeMode(SafeLevel level)
        {
            _level = level;
            return this;
        }

        public override IQuery<T> SetDistinct(bool distinct)
        {
            _distinct = distinct;
            return this;
        }

        public override IQuery<T> SetJoinSetting(JoinSetting setting)
        {
            _joinSetting = setting;
            return this;
        }


        #region aggregate function

        public override int Count()
        {
            var queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            var value = _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
        }

        public override long LongCount()
        {
            var queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            var value = _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
        }


        #endregion

        public override T First()
        {
            return ElementAt(0);
        }

        public override T ElementAt(int index)
        {
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryEntityData(_context, _mapping, null, _query, _order, false, region);
            return _context.QueryDataDefineSingle<T>(_mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
        }

        public override bool Exists()
        {
            var queryCommand = _context.Database.Exists(_context, _mapping, _query);
            var define = DataDefine.GetDefine(typeof(int?));
            var obj = _context.QueryDataDefineSingle<int?>(define, _level, queryCommand.Command, 0, null, null);
            return obj.HasValue;
        }

        public override List<T> ToList()
        {
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return _context.QueryDataDefineList<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null);
        }

        public override T[] ToArray()
        {
            return ToList().ToArray();
        }

        #endregion

        public override int Insert<K>()
        {
            var insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            var queryCommand = _context.Database.SelectInsert(_context, insertMapping, _mapping, _query, _order);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override int SelectInsert<K>(Expression<Func<T, K>> expression)
        {
            var selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            var queryCommand = _context.Database.SelectInsert(_context, selector, _mapping, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override int Update(Expression<Func<T, T>> expression)
        {
            var mapping = DataEntityMapping.GetTableMapping(typeof(T));
            var updator = LambdaExpressionExtend.CreateMassUpdateExpression(expression);
            var queryCommand = _context.Database.QueryUpdate(_context, mapping, updator, _query);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override int Delete()
        {
            var mapping = DataEntityMapping.GetTableMapping(typeof(T));
            var queryCommand = _context.Database.QueryDelete(_context, mapping, _query);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override ISelect<K> Select<K>(Expression<Func<T, K>> expression)
        {
            var selectable = new LightSelect<T, K>(_context, expression, _query, _order, _distinct, _joinSetting, _region, _level);
            return selectable;
        }

        public override IAggregate<K> Aggregate<K>(Expression<Func<T, K>> expression)
        {
            return new LightAggregate<T, K>(this, expression);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, JoinSetting.None);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, queryBase.JoinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, aggregateBase.JoinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, selectBase.JoinSetting);
        }




        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, queryBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, aggregateBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting)
        {
            var selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, selectBase, onExpression, _joinSetting, joinSetting);
        }

        public override ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression)
        {
            var selectField = new LightSelectField<K>(Context, expression, _query, _order, _distinct, _region, _level);
            return selectField;
        }


        public override K AggregateField<K>(Expression<Func<T, K>> expression)
        {
            var model = LambdaExpressionExtend.CreateAggregateModel(expression);
            model.OnlyAggregate = true;
            var region = new Region(0, 1);
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, model, _query, null, _order, region);
            var target = _context.QueryDataDefineSingle<K>(model.OutputDataMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
            if (target == null) {
                var obj = model.OutputDataMapping.InitialData();
                target = (K)obj;
            }
            return target;
        }



        #region async
        public override async Task<List<T>> ToListAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return await _context.QueryDataDefineListAsync<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null, cancellationToken);
        }

        public override async Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default)
        {
            var list = await ToListAsync(CancellationToken.None);
            return list.ToArray();
        }

        public override async Task<T> FirstAsync(CancellationToken cancellationToken = default)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public override async Task<T> ElementAtAsync(int index, CancellationToken cancellationToken = default)
        {
            var region = new Region(index, 1);
            var queryCommand = _context.Database.QueryEntityData(_context, _mapping, null, _query, _order, false, region);
            return await _context.QueryDataDefineSingleAsync<T>(_mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);
        }

        public override async Task<int> InsertAsync<K>(CancellationToken cancellationToken = default)
        {
            var insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            var queryCommand = _context.Database.SelectInsert(_context, insertMapping, _mapping, _query, _order);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public override async Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default)
        {
            var selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            var queryCommand = _context.Database.SelectInsert(_context, selector, _mapping, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public override async Task<int> UpdateAsync(Expression<Func<T, T>> expression, CancellationToken cancellationToken = default)
        {
            var mapping = DataEntityMapping.GetTableMapping(typeof(T));
            var updator = LambdaExpressionExtend.CreateMassUpdateExpression(expression);
            var queryCommand = _context.Database.QueryUpdate(_context, mapping, updator, _query);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public override async Task<int> DeleteAsync(CancellationToken cancellationToken = default)
        {
            var mapping = DataEntityMapping.GetTableMapping(typeof(T));
            var queryCommand = _context.Database.QueryDelete(_context, mapping, _query);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            var value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
        }

        public override async Task<long> LongCountAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            var value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
        }

        public override async Task<bool> ExistsAsync(CancellationToken cancellationToken = default)
        {
            var queryCommand = _context.Database.Exists(_context, _mapping, _query);
            var define = DataDefine.GetDefine(typeof(int?));
            var obj = await _context.QueryDataDefineSingleAsync<int?>(define, _level, queryCommand.Command, 0, null, null, cancellationToken);
            return obj.HasValue;
        }

        public override async Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default)
        {
            var model = LambdaExpressionExtend.CreateAggregateModel(expression);
            model.OnlyAggregate = true;
            var region = new Region(0, 1);
            var queryCommand = _context.Database.QueryDynamicAggregate(_context, model, _query, null, _order, region);
            var target = await _context.QueryDataDefineSingleAsync<K>(model.OutputDataMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);
            if (target == null) {
                var obj = model.OutputDataMapping.InitialData();
                target = (K)obj;
            }
            return target;
        }
        #endregion
    }
}

