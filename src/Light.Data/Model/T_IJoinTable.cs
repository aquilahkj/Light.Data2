using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;

namespace Light.Data
{

	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1> Where (Expression<Func<T, T1, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1> WhereWithAnd (Expression<Func<T, T1, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1> WhereWithOr (Expression<Func<T, T1, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1> OrderByCatch<TKey> (Expression<Func<T, T1, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1> OrderBy<TKey> (Expression<Func<T, T1, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1> OrderByDescending<TKey> (Expression<Func<T, T1, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> Join<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> Join<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> Join<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> Join<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T2, bool>> queryExpression, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> Join<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (IQuery<T2> query, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> Join<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (IAggregate<T2> aggregate, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> Join<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> LeftJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2> RightJoin<T2> (ISelect<T2> select, Expression<Func<T, T1, T2, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2> Where (Expression<Func<T, T1, T2, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2> WhereWithAnd (Expression<Func<T, T1, T2, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2> WhereWithOr (Expression<Func<T, T1, T2, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2> OrderByCatch<TKey> (Expression<Func<T, T1, T2, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2> OrderBy<TKey> (Expression<Func<T, T1, T2, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2> OrderByDescending<TKey> (Expression<Func<T, T1, T2, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T3, bool>> queryExpression, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (IQuery<T3> query, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (IAggregate<T3> aggregate, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> Join<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> LeftJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3> RightJoin<T3> (ISelect<T3> select, Expression<Func<T, T1, T2, T3, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3> Where (Expression<Func<T, T1, T2, T3, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3> WhereWithAnd (Expression<Func<T, T1, T2, T3, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3> WhereWithOr (Expression<Func<T, T1, T2, T3, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T4, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IQuery<T4> query, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (IAggregate<T4> aggregate, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> Join<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> LeftJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4> RightJoin<T4> (ISelect<T4> select, Expression<Func<T, T1, T2, T3, T4, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4> Where (Expression<Func<T, T1, T2, T3, T4, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T5, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IQuery<T5> query, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (IAggregate<T5> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Join<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> LeftJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5> RightJoin<T5> (ISelect<T5> select, Expression<Func<T, T1, T2, T3, T4, T5, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4, T5>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Where (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4, T5> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T6, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IQuery<T6> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (IAggregate<T6> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Join<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> LeftJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RightJoin<T6> (ISelect<T6> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4, T5, T6>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T7, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IQuery<T7> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (IAggregate<T7> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Join<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> LeftJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RightJoin<T7> (ISelect<T7> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4, T5, T6, T7>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T8, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IQuery<T8> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (IAggregate<T8> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Join<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> LeftJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RightJoin<T8> (ISelect<T8> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, K>> expression, CancellationToken cancellationToken);

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression); 

		/// <summary>
		/// Inner Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join table with specified specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified queryExpression and onExpression.
		/// </summary>
		/// <param name="queryExpression">Query expression.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T9, bool>> queryExpression, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join table with specified onExpression.
		/// </summary>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join query data with onExpression.
		/// </summary>
		/// <param name="query">Query.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IQuery<T9> query, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join aggregate data with onExpression.
		/// </summary>
		/// <param name="aggregate">Aggregate.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (IAggregate<T9> aggregate, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Inner Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Join<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Left Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> LeftJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 

		/// <summary>
		/// Right Join select data with onExpression.
		/// </summary>
		/// <param name="select">Select.</param>
		/// <param name="onExpression">On expression.</param>
		/// <param name="joinSetting">Set join setting</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RightJoin<T9> (ISelect<T9> select, Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> onExpression, JoinSetting joinSetting); 


	}
	/// <summary>
	/// Join table.
	/// </summary>
	public interface IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
	{
		/// <summary>
		/// Reset the specified where expression
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereReset ();

		/// <summary>
		/// Set the specified where expression.
		/// </summary>T1,
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Where (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with and.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithAnd (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression);

		/// <summary>
		/// Catch the specified where expression with or.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> WhereWithOr (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, bool>> expression);

		/// <summary>
		/// Catch the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression);

		/// <summary>
		/// Catch the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescendingCatch<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression);

		/// <summary>
		/// Set the specified asc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderBy<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression);

		/// <summary>
		/// Set the specified desc order by expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="TKey">The 1st type parameter.</typeparam>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByDescending<TKey> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, TKey>> expression);

		/// <summary>
		/// Reset the specified order by expression.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> OrderByReset ();

		/// <summary>
		/// Set take datas count.
		/// </summary>
		/// <param name="count">Count.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Take (int count);

		/// <summary>
		/// Set from datas index.
		/// </summary>
		/// <returns>JoinTable.</returns>
		/// <param name="index">Index.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Skip (int index);

		/// <summary>
		/// Set take datas range.
		/// </summary>
		/// <param name="from">From.</param>
		/// <param name="to">To.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> Range (int from, int to);

		/// <summary>
		/// Reset take datas range.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> RangeReset ();

		/// <summary>
		/// Sets page size.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="size">Size.</param>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> PageSize (int page, int size);

		/// <summary>
		/// Set the SafeLevel.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SafeMode (SafeLevel level);

		/// <summary>
		/// Sets the distinct.
		/// </summary>
		IJoinTable<T, T1, T2, T3, T4, T5, T6, T7, T8, T9> SetDistinct (bool distinct);

		/// <summary>
		/// Create Selector.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		ISelectJoin<K> Select<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		int SelectInsert<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression);

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		int Count();

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		long LongCount();

		/// <summary>
		/// Gets the datas count.
		/// </summary>
		/// <value>The count.</value>
		Task<int> CountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Gets the datas long count.
		/// </summary>
		/// <value>The long count.</value>
		Task<long> LongCountAsync(CancellationToken cancellationToken);

		/// <summary>
		/// Select fields data insert to the specified table K.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="K">The 1st type parameter.</typeparam>
		Task<int> SelectInsertAsync<K> (Expression<Func<T, T1, T2, T3, T4, T5, T6, T7, T8, T9, K>> expression, CancellationToken cancellationToken);


	}

}


