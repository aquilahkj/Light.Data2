using System;
using System.Collections.Generic;

namespace Light.Data
{
	class MutliInsertSelector : Selector
	{
		readonly DataTableEntityMapping insertMapping;

		readonly List<DataFieldInfo> insertList = new List<DataFieldInfo> ();

		public MutliInsertSelector (DataTableEntityMapping insertMapping)
		{
			this.insertMapping = insertMapping;
		}

		internal DataTableEntityMapping InsertMapping {
			get {
				return insertMapping;
			}
		}

		public void SetInsertField (DataFieldInfo field)
		{
			if (Object.Equals (field, null))
				throw new ArgumentNullException (nameof (field));
			insertList.Add (field);
		}

		public DataFieldInfo [] GetInsertFields ()
		{
			return insertList.ToArray ();
		}
	}
}
