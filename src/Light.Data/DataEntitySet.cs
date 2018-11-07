using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    public class DataEntitySet<T> //where T : class, new()
    {
        private readonly DataContext context;

        public DataEntitySet(DataContext context)
        {
            this.context = context;
        }

        public int BatchDelete(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchDelete(datas, index, count);
        }

        public int BatchDelete(IEnumerable<T> datas)
        {
            return context.BatchDelete(datas);
        }

        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            return await context.BatchDeleteAsync(datas, cancellationToken);
        }

        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await context.BatchDeleteAsync(datas, index, count, cancellationToken);
        }

        public int BatchInsert(IEnumerable<T> datas)
        {
            return context.BatchInsert(datas);
        }

        public int BatchInsert(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchInsert(datas, index, count);
        }

        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await context.BatchInsertAsync(datas, index, count, cancellationToken);
        }

        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            return await context.BatchInsertAsync(datas, cancellationToken);
        }

        public int BatchUpdate(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchUpdate(datas, index, count);
        }

        public int BatchUpdate(IEnumerable<T> datas)
        {
            return context.BatchUpdate(datas);
        }

        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, CancellationToken cancellationToken)
        {
            return await context.BatchUpdateAsync(datas, cancellationToken);
        }

        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await context.BatchUpdateAsync(datas, index, count, cancellationToken);
        }

        public int Delete(T data)
        {
            return context.Delete(data);
        }

        public async Task<int> DeleteAsync(T data, CancellationToken cancellationToken)
        {
            return await context.DeleteAsync(data, cancellationToken);
        }

        public int Insert(T data)
        {
            return context.Insert(data);
        }

        public async Task<int> InsertAsync(T data, CancellationToken cancellationToken)
        {
            return await context.InsertAsync(data, cancellationToken);
        }

        public int InsertOrUpdate(T data)
        {
            return context.InsertOrUpdate(data);
        }

        public async Task<int> InsertOrUpdateAsync(T data, CancellationToken cancellationToken)
        {
            return await context.InsertOrUpdateAsync(data, cancellationToken);
        }

        public T SelectById(object id)
        {
            return context.SelectById<T>(id);
        }

        public async Task<T> SelectByIdAsync(object id, CancellationToken cancellationToken)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        public T SelectByKey(params object[] primaryKeys)
        {
            return context.SelectByKey<T>(primaryKeys);
        }

        public async Task<T> SelectByKeyAsync(object primaryKey, CancellationToken cancellationToken)
        {
            return await context.SelectByKeyAsync<T>(primaryKey, cancellationToken);
        }

        public async Task<T> SelectByKeyAsync(object primaryKey1, object primaryKey2, CancellationToken cancellationToken)
        {
            return await context.SelectByKeyAsync<T>(primaryKey1, primaryKey2, cancellationToken);
        }

        public async Task<T> SelectByKeyAsync(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken)
        {
            return await context.SelectByKeyAsync<T>(primaryKey1, primaryKey2, primaryKey3, cancellationToken);
        }

        public async Task<T> SelectByKeyAsync(object[] primaryKeys, CancellationToken cancellationToken)
        {
            return await context.SelectByKeyAsync<T>(primaryKeys, cancellationToken);
        }

        public bool Exists(params object[] primaryKeys)
        {
            return context.Exists<T>(primaryKeys);
        }

        public async Task<bool> ExistsAsync(object primaryKey, CancellationToken cancellationToken)
        {
            return await context.ExistsAsync<T>(primaryKey, cancellationToken);
        }

        public async Task<bool> ExistsAsync(object primaryKey1, object primaryKey2, CancellationToken cancellationToken)
        {
            return await context.ExistsAsync<T>(primaryKey1, primaryKey2, cancellationToken);
        }

        public async Task<bool> ExistsAsync(object primaryKey1, object primaryKey2, object primaryKey3, CancellationToken cancellationToken)
        {
            return await context.ExistsAsync<T>(primaryKey1, primaryKey2, primaryKey3, cancellationToken);
        }

        public async Task<bool> ExistsAsync(object[] primaryKeys, CancellationToken cancellationToken)
        {
            return await context.ExistsAsync<T>(primaryKeys, cancellationToken);
        }

        public int Update(T data)
        {
            return context.Update(data);
        }

        public async Task<int> UpdateAsync(T data, CancellationToken cancellationToken)
        {
            return await context.UpdateAsync(data, cancellationToken);
        }

        public IQuery<T> Query()
        {
            return context.Query<T>();
        }

        public IQuery<T> Where(Expression<Func<T, bool>> expression)
        {
            return context.Query<T>().Where(expression);
        }

        public IQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> expression)
        {
            return context.Query<T>().OrderBy<TKey>(expression);
        }

        public IQuery<T> OrderByRandom()
        {
            return context.Query<T>().OrderByRandom();
        }

        /// <summary>
        /// Create Selector.
        /// </summary>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public ISelect<K> Select<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().Select(expression);
        }

        /// <summary>
        /// Create group by aggregator
        /// </summary>
        /// <returns>The by.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public IAggregate<K> GroupBy<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().Aggregate(expression);
        }

        /// <summary>
        /// Select special field.
        /// </summary>
        /// <returns>The field.</returns>
        /// <param name="expression">Expression.</param>
        /// <typeparam name="K">The 1st type parameter.</typeparam>
        public ISelectField<K> SelectField<K>(Expression<Func<T, K>> expression)
        {
            return context.Query<T>().SelectField(expression);
        }

    }
}
