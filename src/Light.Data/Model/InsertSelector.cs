using System;
using System.Collections.Generic;

namespace Light.Data
{
	internal class InsertSelector : Selector
	{
		private readonly List<DataFieldInfo> insertList = new List<DataFieldInfo> ();

		public InsertSelector (DataTableEntityMapping insertMapping, DataEntityMapping selectMapping)
		{
			SelectMapping = selectMapping;
			InsertMapping = insertMapping;
		}

		public InsertSelector (DataTableEntityMapping insertMapping)
		{
			InsertMapping = insertMapping;
		}

		internal DataTableEntityMapping InsertMapping { get; }

		internal DataEntityMapping SelectMapping { get; }

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

