using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Data Entity Set.
    /// </summary>
    public class DataEntitySet<T>
    {
        private readonly DataContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Light.Data.DataEntitySet`1"/> class.
        /// </summary>
        /// <param name="context">Context.</param>
        public DataEntitySet(DataContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public int BatchDelete(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchDelete(datas, index, count);
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        public int BatchDelete(IEnumerable<T> datas)
        {
            return context.BatchDelete(datas);
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas, CancellationToken cancellationToken = default)
        {
            return await context.BatchDeleteAsync(datas, cancellationToken);
        }

        /// <summary>
        /// Batch delete datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken = default)
        {
            return await context.BatchDeleteAsync(datas, index, count, cancellationToken);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        public int BatchInsert(IEnumerable<T> datas)
        {
            return context.BatchInsert(datas);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public int BatchInsert(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchInsert(datas, index, count);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken = default)
        {
            return await context.BatchInsertAsync(datas, index, count, cancellationToken);
        }

        /// <summary>
        /// Batch insert datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, CancellationToken cancellationToken = default)
        {
            return await context.BatchInsertAsync(datas, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public int BatchUpdate(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchUpdate(datas, index, count);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        public int BatchUpdate(IEnumerable<T> datas)
        {
            return context.BatchUpdate(datas);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, CancellationToken cancellationToken = default)
        {
            return await context.BatchUpdateAsync(datas, cancellationToken);
        }

        /// <summary>
        /// Batch update datas.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="datas">Datas.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken = default)
        {
            return await context.BatchUpdateAsync(datas, index, count, cancellationToken);
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="data">Data.</param>
        public int Delete(T data)
        {
            return context.Delete(data);
        }

        /// <summary>
        /// Delete the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> DeleteAsync(T data, CancellationToken cancellationToken = default)
        {
            return await context.DeleteAsync(data, cancellationToken);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>The insert count.</returns>
        /// <param name="data">Data.</param>
        public int Insert(T data)
        {
            return context.Insert(data);
        }

        /// <summary>
        /// Insert the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertAsync(T data, CancellationToken cancellationToken = default)
        {
            return await context.InsertAsync(data, cancellationToken);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        public int InsertOrUpdate(T data)
        {
            return context.InsertOrUpdate(data);
        }

        /// <summary>
        /// Insert or update the specified data.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> InsertOrUpdateAsync(T data, CancellationToken cancellationToken = default)
        {
            return await context.InsertOrUpdateAsync(data, cancellationToken);
        }

        /// <summary>
        /// Select the single object by id.
        /// </summary>
        /// <returns>object.</returns>
        /// <param name="id">Identifier.</param>
        public T SelectById(object id)
        {
            return context.SelectById<T>(id);
        }

        /// <summary>
        /// Select data by identifier.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="id">Identifier.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<T> SelectByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        public T SelectByKey(params object[] primaryKeys)
        {
            return context.SelectByKey<T>(primaryKeys);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<T> SelectByKeyAsync(object primaryKey, CancellationToken cancellationToken = default)
        {
            return await context.SelectByKeyAsync<T>(primaryKey, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<T> SelectByKeyAsync(object primaryKey1, object primaryKey2, CancellationToken cancellationToken = default)
        {
            return await context.SelectByKeyAsync<T>(primaryKey1, primaryKey2, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<T> SelectByKeyAsync(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken = default)
        {
            return await context.SelectByKeyAsync<T>(primaryKey1, primaryKey2, primaryKey3, cancellationToken);
        }

        /// <summary>
        /// Select the single object by keys.
        /// </summary>
        /// <returns>result.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<T> SelectByKeyAsync(object[] primaryKeys, CancellationToken cancellationToken = default)
        {
            return await context.SelectByKeyAsync<T>(primaryKeys, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        public bool Exists(params object[] primaryKeys)
        {
            return context.Exists<T>(primaryKeys);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey">Primary key.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<bool> ExistsAsync(object primaryKey, CancellationToken cancellationToken = default)
        {
            return await context.ExistsAsync<T>(primaryKey, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<bool> ExistsAsync(object primaryKey1, object primaryKey2, CancellationToken cancellationToken = default)
        {
            return await context.ExistsAsync<T>(primaryKey1, primaryKey2, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKey1">Primary key 1.</param>
        /// <param name="primaryKey2">Primary key 2.</param>
        /// <param name="primaryKey3">Primary key 3.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<bool> ExistsAsync(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken = default)
        {
            return await context.ExistsAsync<T>(primaryKey1, primaryKey2, primaryKey3, cancellationToken);
        }

        /// <summary>
        /// Check exist the object by keys.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="primaryKeys">Primary keys.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<bool> ExistsAsync(object[] primaryKeys, CancellationToken cancellationToken = default)
        {
            return await context.ExistsAsync<T>(primaryKeys, cancellationToken);
        }

        /// <summary>
        /// Update the specified data.
        /// </summary>
        /// <returns>The update count.</returns>
        /// <param name="data">Data.</param>
        public int Update(T data)
        {
            return context.Update(data);
        }

        /// <summary>
        /// Updates the.
        /// </summary>
        /// <returns>exists or not.</returns>
        /// <param name="data">Data.</param>
        /// <param name="cancellationToken">CancellationToken.</param>
        public async Task<int> UpdateAsync(T data, CancellationToken cancellationToken = default)
        {
            return await context.UpdateAsync(data, cancellationToken);
        }

        /// <summary>
        /// Create query expression.
        /// </summary>
        /// <returns>The queryable.</returns>
        public IQuery<T> Query()
        {
            return context.Query<T>();
        }

        /// <summary>
        /// Create query expression with where condition.
        /// </summary>
        /// <returns>The queryable.</returns>
        /// <param name="expression">Expression.</param>
        public IQuery<T> Where(Expression<Func<T, bool>> expression)
        {
            return context.Query<T>().Where(expression);
        }

        /// <summary>
        /// Create query expression with order asc.
        /// </summary>
        /// <returns>The queryable.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">The field type.</typeparam>
        public IQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            return context.Query<T>().OrderBy(expression);
        }

        /// <summary>
        /// Create query expression with order desc.
        /// </summary>
        /// <returns>The queryable.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="TKey">The field type.</typeparam>
        public IQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> expression)
        {
            return context.Query<T>().OrderByDescending(expression);
        }

        /// <summary>
        /// Create query expression with order random.
        /// </summary>
        /// <returns>The by random.</returns>
        public IQuery<T> OrderByRandom()
        {
            return context.Query<T>().OrderByRandom();
        }

        /// <summary>
        /// Create Selector.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        public ISelect<K> Select<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().Select(expression);
        }

        /// <summary>
        /// Create group by aggregator
        /// </summary>
        /// <returns>The by.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        public IAggregate<K> Aggregate<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().Aggregate(expression);
        }

        /// <summary>
        /// Select special field.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">Data type.</typeparam>
        public ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().SelectField(expression);
        }

    }
}
