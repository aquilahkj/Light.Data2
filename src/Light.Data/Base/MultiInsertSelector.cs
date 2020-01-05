using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class MultiInsertSelector : Selector
	{
		private readonly List<DataFieldInfo> insertList = new List<DataFieldInfo> ();

		public MultiInsertSelector (DataTableEntityMapping insertMapping)
		{
			InsertMapping = insertMapping;
		}

		internal DataTableEntityMapping InsertMapping { get; }

		public void SetInsertField (DataFieldInfo field)
		{
			if (Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			insertList.Add (field);
		}

		public DataFieldInfo [] GetInsertFields ()
		{
			return insertList.ToArray ();
		}
	}
}
