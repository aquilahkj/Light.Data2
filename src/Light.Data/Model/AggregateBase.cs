using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    abstract class AggregateBase<K> : IAggregate<K>
    {
        protected readonly DataContext _context;

        public DataContext Context {
            get {
                return _context;
            }
        }

        AggregateModel _model;

        LambdaExpression _expression;

        public AggregateModel Model {
            get {
                if (_model == null) {
                    _model = LambdaExpressionExtend.CreateAggregateModel(_expression);
                }
                return _model;
            }
        }

        public abstract QueryExpression QueryExpression {
            get;
        }

        public abstract QueryExpression HavingExpression {
            get;
        }

        public abstract OrderExpression OrderExpression {
            get;
        }

        public abstract Region Region {
            get;
        }

        public abstract SafeLevel SafeLevel {
            get;
        }


        protected AggregateBase(DataContext context, LambdaExpression expression)
        {
            _context = context;
            _expression = expression;
            //_model = LambdaExpressionExtend.CreateAggregateModel(expression);
        }

        public abstract IEnumerator<K> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public abstract K First();

        public abstract K ElementAt(int index);

        public abstract IAggregate<K> Having(Expression<Func<K, bool>> expression);

        public abstract IAggregate<K> HavingReset();

        public abstract IAggregate<K> HavingWithAnd(Expression<Func<K, bool>> expression);

        public abstract IAggregate<K> HavingWithOr(Expression<Func<K, bool>> expression);

        public abstract IAggregate<K> OrderBy<TKey>(Expression<Func<K, TKey>> expression);

        public abstract IAggregate<K> OrderByCatch<TKey>(Expression<Func<K, TKey>> expression);

        public abstract IAggregate<K> OrderByDescending<TKey>(Expression<Func<K, TKey>> expression);

        public abstract IAggregate<K> OrderByDescendingCatch<TKey>(Expression<Func<K, TKey>> expression);

        public abstract IAggregate<K> OrderByRandom();

        public abstract IAggregate<K> OrderByReset();

        public abstract IAggregate<K> PageSize(int page, int size);

        public abstract IAggregate<K> Range(int from, int to);

        public abstract IAggregate<K> RangeReset();

        public abstract IAggregate<K> SafeMode(SafeLevel level);

        public abstract IAggregate<K> Skip(int index);

        public abstract IAggregate<K> Take(int count);

        public abstract int SelectInsert<P>(Expression<Func<K, P>> expression); //where P : class, new();

        public abstract List<K> ToList();

        public abstract K[] ToArray();

        public abstract IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        public abstract Task<List<K>> ToListAsync(CancellationToken cancellationToken);

        public abstract Task<K[]> ToArrayAsync(CancellationToken cancellationToken);

        public abstract Task<K> FirstAsync(CancellationToken cancellationToken);

        public abstract Task<K> ElementAtAsync(int index, CancellationToken cancellationToken);

        public abstract Task<int> SelectInsertAsync<P>(Expression<Func<K, P>> expression, CancellationToken cancellationToken) where P : class;
    }
}
