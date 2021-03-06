﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    internal abstract class QueryBase<T> : IQuery<T>
    {
        public abstract QueryExpression QueryExpression {
            get;
        }

        public abstract OrderExpression OrderExpression {
            get;
        }

        public abstract Region Region {
            get;
        }

        public abstract bool Distinct {
            get;
        }

        public abstract JoinSetting JoinSetting {
            get;
        }

        protected readonly DataContext _context;

        public DataContext Context => _context;

        protected readonly DataEntityMapping _mapping;

        public DataEntityMapping Mapping => _mapping;

        protected QueryBase(DataContext dataContext)
        {
            _context = dataContext;
            _mapping = DataEntityMapping.GetEntityMapping(typeof(T));
        }

        public abstract int Count();

        public abstract long LongCount();

        public abstract bool Exists();

        public abstract int Delete();

        public abstract T ElementAt(int index);

        public abstract T First();

        public abstract IEnumerator<T> GetEnumerator();

        public abstract IAggregate<K> Aggregate<K>(Expression<Func<T, K>> expression);

        public abstract int Insert<K>();

        public abstract IQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression);

        public abstract IQuery<T> OrderByConcat<TKey>(Expression<Func<T, TKey>> expression);

        public abstract IQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> expression);

        public abstract IQuery<T> OrderByDescendingConcat<TKey>(Expression<Func<T, TKey>> expression);

        public abstract IQuery<T> OrderByRandom();

        public abstract IQuery<T> OrderByReset();

        public abstract IQuery<T> PageSize(int page, int size);

        public abstract IQuery<T> Range(int from, int to);

        public abstract IQuery<T> RangeReset();

        public abstract IQuery<T> SafeMode(SafeLevel level);

        public abstract ISelect<K> Select<K>(Expression<Func<T, K>> expression);

        public abstract int SelectInsert<K>(Expression<Func<T, K>> expression);

        public abstract IQuery<T> SetDistinct(bool distinct);

        public abstract IQuery<T> SetJoinSetting(JoinSetting setting);

        public abstract IQuery<T> Skip(int index);

        public abstract IQuery<T> Take(int count);

        public abstract T[] ToArray();

        public abstract List<T> ToList();

        public abstract int Update(Expression<Func<T, T>> expression);

        public abstract IQuery<T> Where(Expression<Func<T, bool>> expression);

        public abstract IQuery<T> WhereReset();

        public abstract IQuery<T> WhereWithAnd(Expression<Func<T, bool>> expression);

        public abstract IQuery<T> WhereWithOr(Expression<Func<T, bool>> expression);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);

        public abstract IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        public abstract IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);



        public abstract ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression);

        public abstract K AggregateField<K>(Expression<Func<T, K>> expression);

        public abstract Task<int> CountAsync(CancellationToken cancellationToken = default);

        public abstract Task<long> LongCountAsync(CancellationToken cancellationToken = default);

        public abstract Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

        public abstract Task<T> FirstAsync(CancellationToken cancellationToken = default);

        public abstract Task<T> ElementAtAsync(int index, CancellationToken cancellationToken = default);

        public abstract Task<List<T>> ToListAsync(CancellationToken cancellationToken = default);

        public abstract Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default);

        public abstract Task<int> InsertAsync<K>(CancellationToken cancellationToken = default);

        public abstract Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default);

        public abstract Task<int> UpdateAsync(Expression<Func<T, T>> expression, CancellationToken cancellationToken = default);

        public abstract Task<int> DeleteAsync(CancellationToken cancellationToken = default);

        public abstract Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default);
    }
}