using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
    /// <summary>
    /// Select field.
    /// </summary>
    public interface ISelectField<K> : IEnumerable<K>
    {
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
        #endregion
    }
}
