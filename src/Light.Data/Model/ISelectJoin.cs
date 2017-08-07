using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Light.Data
{
	/// <summary>
	/// Join select.
	/// </summary>
	public interface ISelectJoin<K> : IEnumerable<K>
	{
        /// <summary>
        /// set entity null if no any data
        /// </summary>
        /// <param name="entityIndex"></param>
        ISelectJoin<K> NoDataSetEntityNull(int entityIndex);

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        List<K> ToList ();

		/// <summary>
		/// To the array.
		/// </summary>
		/// <returns>The array.</returns>
		K[] ToArray();

		/// <summary>
		/// Get first instance.
		/// </summary>
		K First ();

		/// <summary>
		/// Elements at index.
		/// </summary>
		/// <returns>The <see cref="!:K"/>.</returns>
		/// <param name="index">Index.</param>
		K ElementAt (int index);

        #region async

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        Task<List<K>> ToListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// To the list.
        /// </summary>
        /// <returns>The list.</returns>
        Task<List<K>> ToListAsync();

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        Task<K[]> ToArrayAsync(CancellationToken cancellationToken);

        /// <summary>
        /// To the array.
        /// </summary>
        /// <returns>The array.</returns>
        Task<K[]> ToArrayAsync();

        /// <summary>
        /// Get first instance.
        /// </summary>
        Task<K> FirstAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Get first instance.
        /// </summary>
        Task<K> FirstAsync();


        /// <summary>
        /// Elements at index.
        /// </summary>
        /// <returns>The <see cref="!:K"/>.</returns>
        /// <param name="index">Index.</param>
        Task<K> ElementAtAsync(int index, CancellationToken cancellationToken);

        /// <summary>
        /// Elements at index.
        /// </summary>
        /// <returns>The <see cref="!:K"/>.</returns>
        /// <param name="index">Index.</param>
        Task<K> ElementAtAsync(int index);

        #endregion
    }
}
