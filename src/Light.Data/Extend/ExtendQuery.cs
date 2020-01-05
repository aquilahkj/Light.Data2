using System;
using System.Linq.Expressions;

namespace Light.Data
{
	/// <summary>
	/// Extend query.
	/// </summary>
	public static class ExtendQuery
	{
		/// <summary>
		/// Exists expression.
		/// </summary>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		public static bool Exists<T> (Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field in the collection.
		/// </summary>
		/// <returns>The in.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool In<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field in the collection.
		/// </summary>
		/// <returns>The in.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool In<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if all was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtAll<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if all was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtAll<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if all was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtAll<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if all was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtAll<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if any was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtAny<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if any was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtAny<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if any was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtAny<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if any was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtAny<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than or equal all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq all was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtEqAll<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than or equal all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq all was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtEqAll<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than or equal all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq all was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtEqAll<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than or equal all elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq all was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtEqAll<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than or equal any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq any was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtEqAny<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field greater than or equal any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq any was gt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool GtEqAny<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than or equal any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq any was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtEqAny<T, K> (K field, Expression<Func<T, K>> selectField) 
		{
			return true;
		}

		/// <summary>
		/// The specified field less than or equal any elements in the collection.
		/// </summary>
		/// <returns><c>true</c>, if eq any was lt, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="selectField">Select field.</param>
		/// <param name="expression">Expression.</param>
		/// <typeparam name="T">Data type.</typeparam>
		/// <typeparam name="K">The 2nd type parameter.</typeparam>
		public static bool LtEqAny<T, K> (K field, Expression<Func<T, K>> selectField, Expression<Func<T, bool>> expression) 
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (bool field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (byte field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (short field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (int field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (long field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (ushort field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (uint field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (ulong field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (double field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (float field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (decimal field)
		{
			return true;
		}

		/// <summary>
		/// Is null.
		/// </summary>
		/// <param name="field">If set to <c>true</c> field.</param>
		public static bool IsNull (DateTime field)
		{
			return true;
		}
	}
}
