using System;

namespace Light.Data
{
	/// <summary>
	/// Data entity.
	/// </summary>
	public abstract class DataEntity
	{
		DataContext _context;

		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>The context.</value>
		protected DataContext Context {
			get {
				return _context;
			}
		}

		/// <summary>
		/// Sets the context.
		/// </summary>
		/// <param name="context">Context.</param>
		/// <exception cref="T:System.ArgumentNullException"></exception>
		public void SetContext(DataContext context) {
			if (context == null) {
				throw new ArgumentNullException(nameof(context));
			}
			_context = context;
		}

		internal virtual void LoadDataComplete() {

		}

	}
}
