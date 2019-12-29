using System;
using System.Collections;
using System.Collections.Generic;

namespace Light.Data
{
    /// <summary>
    /// L collection.
    /// </summary>
    public sealed class LCollection<T> : ICollection<T>
    {
        private List<T> list;

        private readonly QueryExpression query;

        private readonly DataContextOptions options;

        private readonly ICommandOutput output;

        private readonly object owner;

        private readonly string[] fieldPaths;

        internal LCollection(DataContext context, object owner, QueryExpression query, string[] fieldPaths)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            options = context.Options;
            output = context.Output;
            this.owner = owner;
            this.query = query;
            this.fieldPaths = fieldPaths;
        }

        #region ICollection implementation

        private void InitialList()
        {
            if (list == null) {
                var context = new DataContext(options);
                context.SetCommandOutput(output);
                list = context.QueryCollectionRelateData<T>(query, owner, fieldPaths);
                context.Dispose();
            }
        }

        /// <Docs>The item to add to the current collection.</Docs>
        /// <para>Adds an item to the current collection.</para>
        /// <remarks>To be added.</remarks>
        /// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
        /// <summary>
        /// Add the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Clear this instance.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <Docs>The object to locate in the current collection.</Docs>
        /// <para>Determines whether the current collection contains a specific value.</para>
        /// <summary>
        /// Contains the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            InitialList();
            return list.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="arrayIndex">Array index.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            InitialList();
            list.CopyTo(array, arrayIndex);
        }

        /// <Docs>The item to remove from the current collection.</Docs>
        /// <para>Removes the first occurrence of an item from the current collection.</para>
        /// <summary>
        /// Remove the specified item.
        /// </summary>
        /// <param name="item">Item.</param>
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get {
                InitialList();
                return list.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly => false;

        #endregion

        #region IEnumerable implementation

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            InitialList();
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            InitialList();
            return list.GetEnumerator();
        }

        #endregion
    }
}

