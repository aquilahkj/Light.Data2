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

        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas, int index, int count)
        {
            return await context.BatchDeleteAsync(datas, index, count);
        }

        public async Task<int> BatchDeleteAsync(IEnumerable<T> datas)
        {
            return await context.BatchDeleteAsync(datas);
        }

        public int BatchInsert(IEnumerable<T> datas)
        {
            return context.BatchInsert(datas);
        }

        public int BatchInsert(IEnumerable<T> datas, int index, int count)
        {
            return context.BatchInsert(datas, index, count);
        }

        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, int index, int count)
        {
            return await context.BatchInsertAsync(datas, index, count);
        }

        public async Task<int> BatchInsertAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await context.BatchInsertAsync(datas, index, count, cancellationToken);
        }

        public async Task<int> BatchInsertAsync(IEnumerable<T> datas)
        {
            return await context.BatchInsertAsync(datas);
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

        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, int index, int count)
        {
            return await context.BatchUpdateAsync(datas, index, count);
        }

        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas)
        {
            return await context.BatchUpdateAsync(datas);
        }

        public async Task<int> BatchUpdateAsync(IEnumerable<T> datas, int index, int count, CancellationToken cancellationToken)
        {
            return await context.BatchUpdateAsync(datas, index, count, cancellationToken);
        }

        public int Delete(T data)
        {
            return context.Delete(data);
        }

        public async Task<int> DeleteAsync(T data)
        {
            return await context.DeleteAsync(data);
        }

        public async Task<int> DeleteAsync(T data, CancellationToken cancellationToken)
        {
            return await context.DeleteAsync(data, cancellationToken);
        }

        public int Insert(T data)
        {
            return context.Insert(data);
        }

        public async Task<int> InsertAsync(T data)
        {
            return await context.InsertAsync(data);
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

        public async Task<int> InsertOrUpdateAsync(T data)
        {
            return await context.InsertOrUpdateAsync(data);
        }

        public T SelectById(uint id)
        {
            return context.SelectById<T>(id);
        }

        public T SelectById(int id)
        {
            return context.SelectById<T>(id);
        }

        public T SelectById(long id)
        {
            return context.SelectById<T>(id);
        }

        public T SelectById(ulong id)
        {
            return context.SelectById<T>(id);
        }

        public async Task<T> SelectByIdAsync(ulong id)
        {
            return await context.SelectByIdAsync<T>(id);
        }

        public async Task<T> SelectByIdAsync(ulong id, CancellationToken cancellationToken)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        public async Task<T> SelectByIdAsync(long id, CancellationToken cancellationToken)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        public async Task<T> SelectByIdAsync(uint id)
        {
            return await context.SelectByIdAsync<T>(id);
        }

        public async Task<T> SelectByIdAsync(long id)
        {
            return await context.SelectByIdAsync<T>(id);
        }

        public async Task<T> SelectByIdAsync(uint id, CancellationToken cancellationToken)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        public async Task<T> SelectByIdAsync(int id)
        {
            return await context.SelectByIdAsync<T>(id);
        }

        public async Task<T> SelectByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await context.SelectByIdAsync<T>(id, cancellationToken);
        }

        public T SelectByKey(params object[] primaryKeys)
        {
            return context.SelectByKey<T>(primaryKeys);
        }

        public async Task<T> SelectByKeyAsync(object[] primaryKeys, CancellationToken cancellationToken)
        {
            return await context.SelectByKeyAsync<T>(primaryKeys, cancellationToken);
        }

        public async Task<T> SelectByKeyAsync(params object[] primaryKeys)
        {
            return await context.SelectByKeyAsync<T>(primaryKeys);
        }

        public int Update(T data)
        {
            return context.Update(data);
        }

        public async Task<int> UpdateAsync(T data, CancellationToken cancellationToken)
        {
            return await context.UpdateAsync(data, cancellationToken);
        }

        public async Task<int> UpdateAsync(T data)
        {
            return await context.UpdateAsync(data);
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
            return context.Query<T>().GroupBy(expression);
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
