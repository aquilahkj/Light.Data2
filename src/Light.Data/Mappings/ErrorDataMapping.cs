using System;
using System.Data;

namespace Light.Data
{
	class ErrorDataMapping : DataEntityMapping
	{
		public ErrorDataMapping (Type type, Exception ex) : base (type, type.Name, false)
		{
			this.innerException = ex;
		}

		#region implemented abstract members of DataMapping

		public override object LoadData (DataContext context, IDataReader datareader, object state)
		{
			throw new NotSupportedException ();
		}

		public override object InitialData ()
		{
			throw new NotSupportedException ();
		}

		#endregion

		readonly Exception innerException;

		public Exception InnerException {
			get {
				return innerException;
			}
		}
	}
}

