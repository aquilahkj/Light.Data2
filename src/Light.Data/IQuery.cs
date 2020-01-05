using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Query.
    /// </summary>
    public interface IQuery<T> : IEnumerable<T>
    {
        #region IQuery<T> Member

        /// <summary>
        /// Reset the specified where expression
        /// </summary>
        IQuery<T> WhereReset();

        /// <summary>
        /// Set the specified where expression.
        /// </summary>T1,
        /// <param name="expression">Expression.</param>
        IQuery<T> Where(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Concat the specified where expression with and.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IQuery<T> WhereWithAnd(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Concat the specified where expression with or.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IQuery<T> WhereWithOr(Expression<Func<T, bool>> expression);

        /// <summary>
        /// Concat the specified asc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IQuery<T> OrderByConcat<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        /// Concat the specified desc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        IQuery<T> OrderByDescendingConcat<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        /// Set the specified asc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">Data type.</typeparam>
        IQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        /// Set the specified desc order by expression.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">Data type.</typeparam>
        IQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> expression);

        /// <summary>
        /// Reset the specified order by expression.
        /// </summary>
        IQuery<T> OrderByReset();

        /// <summary>
        /// Set order by random.
        /// </summary>
        /// <returns>LEnumerable.</returns>
        IQuery<T> OrderByRandom();

        /// <summary>
        /// Set take datas count.
        /// </summary>
        /// <param name="count">Count.</param>
        IQuery<T> Take(int count);

        /// <summary>
        /// Set from datas index.
        /// </summary>
        /// <returns>JoinTable.</returns>
        /// <param name="index">Index.</param>
        IQuery<T> Skip(int index);

        /// <summary>
        /// Set take datas range.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        IQuery<T> Range(int from, int to);

        /// <summary>
        /// Reset take datas range.
        /// </summary>
        IQuery<T> RangeReset();

        /// <summary>
        /// Sets page size.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <param name="size">Size.</param>
        IQuery<T> PageSize(int page, int size);

        /// <summary>
        /// Set the SafeLevel.
        /// </summary>
        IQuery<T> SafeMode(SafeLevel level);

        /// <summary>
        /// Sets the distinct.
        /// </summary>
        IQuery<T> SetDistinct(bool distinct);

        /// <summary>
        /// Sets the join setting.
        /// </summary>
        IQuery<T> SetJoinSetting(JoinSetting setting);

        /// <summary>
        /// Gets the datas count.
        /// </summary>
        int Count();

        /// <summary>
        /// Gets the datas long count.
        /// </summary>
        long LongCount();

        /// <summary>
        /// Get single instance.
        /// </summary>
        /// <returns>instance.</returns>
        T First();

        /// <summary>
        /// Gets specified element at index.
        /// </summary>
        /// <returns>instance.</returns>
        /// <param name="index">Index.</param>
        T ElementAt(int index);

        /// <summary>
        /// Gets the data is exists with query expression.
        /// </summary>
        bool Exists();

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        List<T> ToList();

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        T[] ToArray();

        #endregion

        /// <summary>
        /// All fields data insert to the special table K.
        /// </summary>
        /// <typeparam name="K">Data type.</typeparam>
        int Insert<K>();

        /// <summary>
        /// Select fields data insert to the special table K.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        int SelectInsert<K>(Expression<Func<T, K>> expression);

        /// <summary>
        /// Update datas.
        /// </summary>
        /// <param name="expression">Expression.</param>
        int Update(Expression<Func<T, T>> expression);

        /// <summary>
        /// Delete datas
        /// </summary>
        int Delete();

        /// <summary>
        /// Create Selector.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        ISelect<K> Select<K>(Expression<Func<T, K>> expression);

        /// <summary>
        /// Create group by aggregator
        /// </summary>
        /// <returns>The by.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        IAggregate<K> Aggregate<K>(Expression<Func<T, K>> expression);

        /// <summary>
        /// Select special field.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression);

        #region join table

        /// <summary>
        /// Inner Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join table with query and onExpression.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Left Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Right Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Left Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Right Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Inner Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Left Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);

        /// <summary>
        /// Right Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression);


        /// <summary>
        /// Inner Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join table with query and onExpression.
        /// </summary>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with specified queryExpression and onExpression.
        /// </summary>
        /// <param name="queryExpression">Query expression.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T1, bool>> queryExpression, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with specified onExpression.
        /// </summary>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join table with query and onExpression.
        /// </summary>
        /// <returns>The join.</returns>
        /// <param name="query">Query.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(IQuery<T1> query, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join aggregate data with onExpression.
        /// </summary>
        /// <param name="aggregate">Aggregate.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(IAggregate<T1> aggregate, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Inner Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> Join<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Left Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> LeftJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        /// <summary>
        /// Right Join select data with onExpression.
        /// </summary>
        /// <param name="select">Select.</param>
        /// <param name="onExpression">On expression.</param>
        /// <param name="joinSetting">The setting of join table.</param>
        /// <typeparam name="T1">Data type.</typeparam>
        IJoinTable<T, T1> RightJoin<T1>(ISelect<T1> select, Expression<Func<T, T1, bool>> onExpression, JoinSetting joinSetting);

        #endregion


        /// <summary>
        /// Aggregates the fields.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        K AggregateField<K>(Expression<Func<T, K>> expression);


        #region async

        /// <summary>
        /// Gets the datas count.
        /// </summary>
        /// <value>The count.</value>
        Task<int> CountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the datas long count.
        /// </summary>
        /// <value>The long count.</value>
        Task<long> LongCountAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the data is exists with query expression.
        /// </summary>
        /// <value><c>true</c> if exists; otherwise, <c>false</c>.</value>
        Task<bool> ExistsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get single instance.
        /// </summary>
        /// <returns>instance.</returns>
        Task<T> FirstAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets specified element at index.
        /// </summary>
        /// <returns>instance.</returns>
        /// <param name="index">Index.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        Task<T> ElementAtAsync(int index, CancellationToken cancellationToken = default);

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        Task<List<T>> ToListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        Task<T[]> ToArrayAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// All fields data insert to the special table K.
        /// </summary>
        /// <typeparam name="K">Data type.</typeparam>
        Task<int> InsertAsync<K>(CancellationToken cancellationToken = default);

        /// <summary>
        /// Select fields data insert to the special table K.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="K">Data type.</typeparam>
        Task<int> SelectInsertAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update datas.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        Task<int> UpdateAsync(Expression<Func<T, T>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete datas
        /// </summary>
        Task<int> DeleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Aggregates the fields.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        /// <typeparam name="K">Data type.</typeparam>
        Task<K> AggregateFieldAsync<K>(Expression<Func<T, K>> expression, CancellationToken cancellationToken = default);

        #endregion

    }
}


