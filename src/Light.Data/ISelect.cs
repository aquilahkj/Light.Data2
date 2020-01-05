using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Select.
    /// </summary>
    public interface ISelect<K> : IEnumerable<K>
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <returns>The list.</returns>
        List<K> ToList();

        /// <summary>
        /// Get data array.
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
        /// Get data list.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>The list.</returns>
        Task<List<K>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get data array.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <returns>The array.</returns>
        Task<K[]> ToArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get first instance.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken.</param>
        Task<K> FirstAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets specified element at index.
        /// </summary>
        /// <returns>The <see cref="!:K"/>.</returns>
        /// <param name="index">Index.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        Task<K> ElementAtAsync(int index, CancellationToken cancellationToken = default);

        #endregion
    }
}

