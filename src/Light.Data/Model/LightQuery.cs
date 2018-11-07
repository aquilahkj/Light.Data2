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
            QueryCommand queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return _context.QueryDataDefineReader<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null).GetEnumerator();
            //return _context.QueryEntityDataReader<T>(_mapping, null, _query, _order, _distinct, _region, _level, null).GetEnumerator();
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
            if (_region == null) {
                start = 0;
            }
            else {
                start = _region.Start;
            }
            _region = new Region(start, size);
            return this;
        }

        public override IQuery<T> Skip(int index)
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
            QueryCommand queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            object value = _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt32(value);
            //return Convert.ToInt32(_context.AggregateCount(_mapping, _query, _level));
        }

        public override long LongCount()
        {
            QueryCommand queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            object value = _context.ExecuteScalar(queryCommand.Command, _level);
            return Convert.ToInt64(value);
            //return Convert.ToInt64(_context.AggregateCount(_mapping, _query, _level));
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
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryEntityData(_context, _mapping, null, _query, _order, false, region);
            return _context.QueryDataDefineSingle<T>(_mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
            //target = _context.QueryEntityDataSingle<T>(_mapping, null, _query, _order, false, region, _level, null);
            //return target;
        }

        public override bool Exists()
        {
            QueryCommand queryCommand = _context.Database.Exists(_context, _mapping, _query);
            DataDefine define = DataDefine.GetDefine(typeof(int?));
            int? obj = _context.QueryDataDefineSingle<int?>(define, _level, queryCommand.Command, 0, null, null);
            return obj.HasValue;
            //return _context.Exists(_mapping, _query, _level);
        }

        public override List<T> ToList()
        {
            QueryCommand queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return _context.QueryDataDefineList<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null);
            //List<T> list = _context.QueryEntityDataList<T>(_mapping, null, _query, _order, _distinct, _region, _level, null);
            //return list;
        }

        public override T[] ToArray()
        {
            return ToList().ToArray();
        }

        #endregion

        public override int Insert<K>()
        {
            DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            QueryCommand queryCommand = _context.Database.SelectInsert(_context, insertMapping, _mapping, _query, _order);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
            //return _context.SelectInsert(insertMapping, _mapping, _query, _order, _level);
        }

        public override int SelectInsert<K>(Expression<Func<T, K>> expression)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            QueryCommand queryCommand = _context.Database.SelectInsert(_context, selector, _mapping, _query, _order, _distinct);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
            //return _context.SelectInsert(selector, _mapping, _query, _order, _distinct, _level);
        }

        public override int Update(Expression<Func<T, T>> expression)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            MassUpdator updator = LambdaExpressionExtend.CreateMassUpdator(expression);
            QueryCommand queryCommand = _context.Database.QueryUpdate(_context, mapping, updator, _query);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override int Delete()
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _context.Database.QueryDelete(_context, mapping, _query);
            return _context.ExecuteNonQuery(queryCommand.Command, _level);
        }

        public override ISelect<K> Select<K>(Expression<Func<T, K>> expression)
        {
            LightSelect<T, K> selectable = new LightSelect<T, K>(_context, expression, _query, _order, _distinct, _region, _level);
            return selectable;
        }

        public override IAggregate<K> Aggregate<K>(Expression<Func<T, K>> expression)
        {
            return new LightAggregate<T, K>(this, expression);
        }

        public override IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
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
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, queryBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression)
        {
            LightQuery<T1> lightQuery = new LightQuery<T1>(_context);
            if (queryExpression != null) {
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
            if (queryExpression != null) {
                lightQuery.Where(queryExpression);
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, lightQuery, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression)
        {
            QueryBase<T1> queryBase = query as QueryBase<T1>;
            if (queryBase == null) {
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
            if (queryBase == null) {
                throw new ArgumentException(nameof(query));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, queryBase, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression)
        {
            AggregateBase<T1> aggregateBase = aggregate as AggregateBase<T1>;
            if (aggregateBase == null) {
                throw new ArgumentException(nameof(aggregate));
            }
            return new LightJoinTable<T, T1>(this, JoinType.RightJoin, aggregateBase, onExpression);
        }

        public override IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.InnerJoin, selectBase, onExpression);
        }

        public override IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
                throw new ArgumentException(nameof(select));
            }
            return new LightJoinTable<T, T1>(this, JoinType.LeftJoin, selectBase, onExpression);
        }

        public override IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression)
        {
            SelectBase<T1> selectBase = select as SelectBase<T1>;
            if (selectBase == null) {
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
            Region region = new Region(0, 1);
            //target = _context.QueryDynamicAggregateSingle<K>(model, _query, null, _order, region, _level, null);
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, model, _query, null, _order, region);
            K target = _context.QueryDataDefineSingle<K>(model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null);
            if (target == null) {
                object obj = model.OutputMapping.InitialData();
                target = (K)obj;
            }
            return target;
        }



        #region async
        public async override Task<List<T>> ToListAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.QueryEntityData(_context, Mapping, null, _query, _order, false, _region);
            return await _context.QueryDataDefineListAsync<T>(Mapping, _level, queryCommand.Command, queryCommand.InnerPage ? null : _region, queryCommand.State, null, cancellationToken);
            //List<T> list = await _context.QueryEntityDataListAsync<T>(_mapping, null, _query, _order, _distinct, _region, _level, null, cancellationToken);
            //return list;
        }

        public async override Task<T[]> ToArrayAsync(CancellationToken cancellationToken)
        {
            List<T> list = await ToListAsync(CancellationToken.None);
            return list.ToArray();
        }

        public async override Task<T> FirstAsync(CancellationToken cancellationToken)
        {
            return await ElementAtAsync(0, cancellationToken);
        }

        public async override Task<T> ElementAtAsync(int index, CancellationToken cancellationToken)
        {
            Region region = new Region(index, 1);
            QueryCommand queryCommand = _context.Database.QueryEntityData(_context, _mapping, null, _query, _order, false, region);
            return await _context.QueryDataDefineSingleAsync<T>(_mapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);

            //T target = default(T);
            //Region region = new Region(index, 1);
            //target = await _context.QueryEntityDataSingleAsync<T>(_mapping, null, _query, _order, false, region, _level, null, cancellationToken);
            //return target;
        }

        public async override Task<int> InsertAsync<K>(CancellationToken cancellationToken)
        {
            DataTableEntityMapping insertMapping = DataEntityMapping.GetTableMapping(typeof(K));
            QueryCommand queryCommand = _context.Database.SelectInsert(_context, insertMapping, _mapping, _query, _order);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
            //return await _context.SelectInsertAsync(insertMapping, _mapping, _query, _order, _level, cancellationToken);
        }

        public async override Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken)
        {
            InsertSelector selector = LambdaExpressionExtend.CreateInsertSelector(expression);
            QueryCommand queryCommand = _context.Database.SelectInsert(_context, selector, _mapping, _query, _order, _distinct);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
            //return await _context.SelectInsertAsync(selector, _mapping, _query, _order, _distinct, _level, cancellationToken);
        }
        
        public async override Task<int> UpdateAsync(Expression<Func<T, T>> expression, CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            MassUpdator updator = LambdaExpressionExtend.CreateMassUpdator(expression);
            QueryCommand queryCommand = _context.Database.QueryUpdate(_context, mapping, updator, _query);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public async override Task<int> DeleteAsync(CancellationToken cancellationToken)
        {
            DataTableEntityMapping mapping = DataEntityMapping.GetTableMapping(typeof(T));
            QueryCommand queryCommand = _context.Database.QueryDelete(_context, mapping, _query);
            return await _context.ExecuteNonQueryAsync(queryCommand.Command, _level, cancellationToken);
        }

        public async override Task<int> CountAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt32(value);
            //return Convert.ToInt32(await _context.AggregateCountAsync(_mapping, _query, _level, cancellationToken));
        }

        public async override Task<long> LongCountAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.AggregateCount(_context, _mapping, _query);
            object value = await _context.ExecuteScalarAsync(queryCommand.Command, _level, cancellationToken);
            return Convert.ToInt64(value);
            //return Convert.ToInt64(await _context.AggregateCountAsync(_mapping, _query, _level, cancellationToken));
        }

        public async override Task<bool> ExistsAsync(CancellationToken cancellationToken)
        {
            QueryCommand queryCommand = _context.Database.Exists(_context, _mapping, _query);
            DataDefine define = DataDefine.GetDefine(typeof(int?));
            int? obj = await _context.QueryDataDefineSingleAsync<int?>(define, _level, queryCommand.Command, 0, null, null, cancellationToken);
            return obj.HasValue;
            //return await _context.ExistsAsync(_mapping, _query, _level, cancellationToken);
        }

        public async override Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken)
        {
            AggregateModel model = LambdaExpressionExtend.CreateAggregateModel(expression);
            model.OnlyAggregate = true;
            Region region = new Region(0, 1);
            QueryCommand queryCommand = _context.Database.QueryDynamicAggregate(_context, model, _query, null, _order, region);
            K target = await _context.QueryDataDefineSingleAsync<K>(model.OutputMapping, _level, queryCommand.Command, queryCommand.InnerPage ? 0 : region.Start, queryCommand.State, null, cancellationToken);
            if (target == null) {
                object obj = model.OutputMapping.InitialData();
                target = (K)obj;
            }
            return target;
            //AggregateModel model = LambdaExpressionExtend.CreateAggregateModel(expression);
            //model.OnlyAggregate = true;
            //K target = default(K);
            //Region region = new Region(0, 1);
            //target = await _context.QueryDynamicAggregateSingleAsync<K>(model, _query, null, _order, region, _level, null, cancellationToken);
            //if (target == null) {
            //    object obj = model.OutputMapping.InitialData();
            //    target = (K)obj;
            //}
            //return target;
        }
        #endregion
    }
}

