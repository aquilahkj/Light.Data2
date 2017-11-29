using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    partial class LightQuery<T> : QueryBase<T>
    {
        #region IEnumerable implementation

        public override IEnumerator<T> GetEnumerator()
        {
            return _context.QueryEntityDataReader<T>(_mapping, null, _query, _order, _distinct, _region, null).GetEnumerator();
        }

        #endregion

        QueryExpression _query;

        public override QueryExpression QueryExpression {
            get {
                return _query;
            }
        }

        OrderExpression _order;

        public override OrderExpression OrderExpression {
            get {
                return _order;
            }
        }

        Region _region;

        public override Region Region {
            get {
                return _region;
            }
        }

        bool _distinct;

        public override bool Distinct {
            get {
                return _distinct;
            }
        }

        SafeLevel _level = SafeLevel.None;

        public override SafeLevel Level {
            get {
                return _level;
            }
        }

        internal LightQuery(DataContext dataContext)
            : base(dataContext)
        {

        }

        #region LQuery<T> 成员

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

        public override IQuery<T> OrderByCatch<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.ASC);
            _order = OrderExpression.Catch(_order, orderExpression);
            return this;
        }

        public override IQuery<T> OrderByDescendingCatch<TKey>(Expression<Func<T, TKey>> expression)
        {
            var orderExpression = LambdaExpressionExtend.ResolveLambdaOrderByExpression(expression, OrderType.DESC);
            _order = OrderExpression.Catch(_order, orderExpression);
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
            int start;
            int size = count;
            if (_region == null)
            {
                start = 0;
            }
            else
            {
                start = _region.Start;
            }
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> Skip(int index)
        {
            int start = index;
            int size;
            if (_region == null)
            {
                size = int.MaxValue;
            }
            else
            {
                size = _region.Size;
            }
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> Range(int from, int to)
        {
            int start = from;
            int size = to - from;
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
            if (page < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(page));
            }
            if (size < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }
            page--;
            int start = page * size;
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

        #region aggregate function

        public override int Count()
        {
            return Convert.ToInt32(_context.AggregateCount(_mapping, _query, _level));
        }

        public override long LongCount()
        {
            return Convert.ToInt64(_context.AggregateCount(_mapping, _query, _level));
        }

        //public override IAggregateField<T> AggregateField()
        //{
        //    LightAggregateField<T> aggregate = new LightAggregateField<T>(_context, _query, _distinct, _level);
        //    return aggregate;
        //}

        #endregion

        public override T First()
        {
            return ElementAt(0);
        }

        public override T ElementAt(int index)
        {
            T target = default(T);
            Region region = new Region(index, 1);
            target = _context.QueryEntityDataSingle<T>(_mapping, null, _query, _order, false, region, null);
            return target;
        }

        public override bool Exists()
        {
            return _context.Exists(_mapping, _query);
        }

        public override List<T> ToList()
        {
            List<T> list = _context.QueryEntityDataList<T>(_mapping, null, _query, _order, _distinct, _region, null);
            return list;
        }

        public override T[] ToArray()
        {
            return ToList().ToArray();
        }

        #endregion

        public override int Insert<K>()
        {
            DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            return _context.SelectInsert(insertMapping, _mapping, _query, _order, _level);
        }

        public override int SelectInsert<K>(Expression<Func<T, K>> expression)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            return _context.SelectInsert(selector, _mapping, _query, _order, _distinct, _level);
        }

        public override int Update(Expression<Func<T, T>> expression)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            MassUpdator updator = LambdaExpressionExtend.CreateMassUpdator(expression);
            return _context.Update(mapping, updator, _query, _level);
        }

        public override int Delete()
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return _context.Delete(mapping, _query, _level);
        }

        public override ISelect<K> Select<K>(Expression<Func<T, K>> expression)
        {
            LightSelect<T, K> selectable = new LightSelect<T, K>(_context, expression, _query, _order, _distinct, _region, _level);
            return selectable;
        }

        public override IAggregate<K> GroupBy<K>(Expression<Func<T, K>> expression)
        {
            return new LightAggregate<T, K>(this, expression);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null)
            {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null)
            {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, queryBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null)
            {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null)
            {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null)
            {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, queryBase, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null)
            {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, queryBase, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null)
            {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null)
            {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null)
            {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null)
            {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, selectBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null)
            {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, selectBase, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null)
            {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, selectBase, onExpression);
        }

        public override ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression)
        {
            LightSelectField<K> selectField = new LightSelectField<K>(Context, expression, _query, _order, _distinct, _region, _level);
            return selectField;
        }


        public override K AggregateField<K>(Expression<Func<T, K>> expression)
        {
            AggregateModel model = LambdaExpressionExtend.CreateAggregateModel(expression);
            model.OnlyAggregate = true;
            K target = default(K);
            Region region = new Region(0, 1);
            target = _context.QueryDynamicAggregateSingle<K>(model, _query, null, _order, region, null);
            if (target == null)
            {
                object obj = model.OutputMapping.InitialData();
                target = (K)obj;
            }
            return target;
        }



        #region async
        public async override Task<List<T>> ToListAsync(CancellationToken cancellationToken)
        {
            List<T> list = await _context.QueryEntityDataListAsync<T>(_mapping, null, _query, _order, _distinct, _region, null, cancellationToken);
            return list;
        }

        public async override Task<List<T>> ToListAsync()
        {
            return await ToListAsync(CancellationToken.None);
        }

        public async override Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<T> list = await ToListAsync();
            return list.ToArray();
        }

        public async override Task<T[]> ToArrayAsync()
        {
            return await ToArrayAsync(CancellationToken.None);
        }

        public async override Task<T> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<T> FirstAsync()
        {
            return await FirstAsync(CancellationToken.None);
        }

        public async override Task<T> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            T target = default(T);
            Region region = new Region(index, 1);
            target = await _context.QueryEntityDataSingleAsync<T>(_mapping, null, _query, _order, false, region, null, cancellationToken);
            return target;
        }

        public async override Task<T> ElementAtAsync(int index)
        {
            return await ElementAtAsync(index, CancellationToken.None);
        }

        public async override Task<int> InsertAsync<K>(CancellationToken cancellationToken)
        {
            DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            return await _context.SelectInsertAsync(insertMapping, _mapping, _query, _order, _level, cancellationToken);
        }

        public async override Task<int> InsertAsync<K>()
        {
            return await InsertAsync<K>(CancellationToken.None);
        }

        public async override Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            return await _context.SelectInsertAsync(selector, _mapping, _query, _order, _distinct, _level, cancellationToken);
        }

        public async override Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression)
        {
            return await SelectInsertAsync(expression, CancellationToken.None);
        }

        public async override Task<int> UpdateAsync(Expression<Func<T, T>> expression, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            MassUpdator updator = LambdaExpressionExtend.CreateMassUpdator(expression);
            return await _context.UpdateAsync(mapping, updator, _query, _level, cancellationToken);
        }

        public async override Task<int> UpdateAsync(Expression<Func<T, T>> expression)
        {
            return await UpdateAsync(expression, CancellationToken.None);
        }

        public async override Task<int> DeleteAsync(CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            return await _context.DeleteAsync(mapping, _query, _level, cancellationToken);
        }

        public async override Task<int> DeleteAsync()
        {
            return await DeleteAsync(CancellationToken.None);
        }

        public async override Task<int> CountAsync(CancellationToken cancellationToken)
        {
            return Convert.ToInt32(await _context.AggregateCountAsync(_mapping, _query, _level, cancellationToken));
        }

        public async override Task<int> CountAsync()
        {
            return await CountAsync(CancellationToken.None);
        }

        public async override Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            return Convert.ToInt64(await _context.AggregateCountAsync(_mapping, _query, _level, cancellationToken));
        }

        public async override Task<long> LongCountAsync()
        {
            return await LongCountAsync(CancellationToken.None);
        }

        public async override Task<bool> ExistsAsync(CancellationToken cancellationToken)
        {
            return await _context.ExistsAsync(_mapping, _query, cancellationToken);
        }

        public async override Task<bool> ExistsAsync()
        {
            return await ExistsAsync(CancellationToken.None);
        }

        public async override Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken)
        {
            AggregateModel model = LambdaExpressionExtend.CreateAggregateModel(expression);
            model.OnlyAggregate = true;
            K target = default(K);
            Region region = new Region(0, 1);
            target = await _context.QueryDynamicAggregateSingleAsync<K>(model, _query, null, _order, region, null, cancellationToken);
            if (target == null)
            {
                object obj = model.OutputMapping.InitialData();
                target = (K)obj;
            }
            return target;
        }

        public async override Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression)
        {
            return await AggregateFieldAsync(expression, CancellationToken.None);
        }
        #endregion
    }
}

