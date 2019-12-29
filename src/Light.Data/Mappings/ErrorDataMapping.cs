using System;
using System.Data;

namespace Light.Data
{
	internal class ErrorDataMapping : DataEntityMapping
	{
		public ErrorDataMapping (Type type, Exception ex) : base (type, type.Name, false)
		{
			InnerException = ex;
		}

		#region implemented abstract members of DataMapping

		public override object LoadData (DataContext context, IDataReader dataReader, object state)
		{
			throw new NotSupportedException ();
		}

		public override object InitialData ()
		{
			throw new NotSupportedException ();
		}

		#endregion

		public Exception InnerException { get; }
	}
}

