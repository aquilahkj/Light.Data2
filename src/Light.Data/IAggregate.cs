using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Aggregate.
    /// </summary>
    public interface IAggregate<K> : IEnumerable<K>
    {
        /// <summary>
        /// Set the specified having expression
        /// </summary>
        /// <param name="expression">Expression.</param>
        IAggregate<K> Having(Expression<Func<K, bool>> expression);

        /// <summary>
        /// Concat the specified having expression with and.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IAggregate<K> HavingWithAnd(Expression<Func<K, bool>> expression);

        /// <summary>
        /// Concat the specified having expression with or.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IAggregate<K> HavingWithOr(Expression<Func<K, bool>> expression);

        /// <summary>
        /// Reset the specified having expression.
        /// </summary>
        IAggregate<K> HavingReset();

        /// <summary>
        /// Concat the specified asc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IAggregate<K> OrderByConcat<TKey>(Expression<Func<K, TKey>> expression);

        /// <summary>
        /// Concat the specified desc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IAggregate<K> OrderByDescendingConcat<TKey>(Expression<Func<K, TKey>> expression);

        /// <summary>
        /// Set the specified asc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">Data type.</typeparam>
        IAggregate<K> OrderBy<TKey>(Expression<Func<K, TKey>> expression);

        /// <summary>
        /// Set the specified desc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">Data type.</typeparam>
        IAggregate<K> OrderByDescending<TKey>(Expression<Func<K, TKey>> expression);

        /// <summary>
        /// Reset the specified order by expression.
        /// </summary>
        IAggregate<K> OrderByReset();

        /// <summary>
        /// Set order by random.
        /// </summary>
        IAggregate<K> OrderByRandom();

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        List<K> ToList();

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        K[] ToArray();

        /// <summary>
        /// Get first instance.
        /// </summary>
        K First();

        /// <summary>
        /// Gets specified element at index.
        /// </summary>
        /// <returns>The <see cref="!:K"/>.</returns>
        /// <param name="index">Index.</param>
        K ElementAt(int index);

        /// <summary>
        /// Select insert aggregate data to specified table
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        int SelectInsert<P>(Expression<Func<K, P>> expression);

        /// <summary>
        /// Set take datas count.
        /// </summary>
        /// <param name="count">Count.</param>
        IAggregate<K> Take(int count);

        /// <summary>
        /// Set from datas index.
        /// </summary>
        /// <returns>JoinTable.</returns>
        /// <param name="index">Index.</param>
        IAggregate<K> Skip(int index);

        /// <summary>
        /// Set take datas range.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        IAggregate<K> Range(int from, int to);

        /// <summary>
        /// Reset take datas range.
        /// </summary>
        IAggregate<K> RangeReset();

        /// <summary>
        /// Sets page size.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <param name="size">Size.</param>
        IAggregate<K> PageSize(int page, int size);

        /// <summary>
        /// Set the SafeLevel.
        /// </summary>
        IAggregate<K> SafeMode(SafeLevel level);

        /// <summary>
        /// Sets the join setting.
        /// </summary>
        IAggregate<K> SetJoinSetting(JoinSetting setting);

        /// <summary>
        /// Inner Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join table with query and onExpression.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Left Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Right Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Left Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Right Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join table with query and onExpression.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> Join<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<K, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<K, T1, bool>> onExpression, JoinSetting joinSetting);


        #region async

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        Task<List<K>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get first instance.
        /// </summary>
        Task<K> FirstAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets specified element at index.
        /// </summary>
        /// <returns>The <see cref="!:K"/>.</returns>
        /// <param name="index">Index.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default);

        /// <summary>
        /// Select insert aggregate data to specified table
        /// </summary>
        /// <typeparam name="P"></typeparam>
        /// <param name="expression"></param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns></returns>
        Task<int> SelectInsertAsync<P>(Expression<Func<K, P>> expression, CancellationToken cancellationToken = default) where P : class;
        #endregion
    }
}

