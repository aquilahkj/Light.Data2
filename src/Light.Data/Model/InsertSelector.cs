using System;
using System.Collections.Generic;

namespace Light.Data
{
	class InsertSelector : Selector
	{
		readonly DataTableEntityMapping insertMapping;

		readonly DataEntityMapping selectMapping;

		readonly List<DataFieldInfo> insertList = new List<DataFieldInfo> ();

		public InsertSelector (DataTableEntityMapping insertMapping, DataEntityMapping selectMapping)
		{
			this.selectMapping = selectMapping;
			this.insertMapping = insertMapping;
		}

		public InsertSelector (DataTableEntityMapping insertMapping)
		{
			this.insertMapping = insertMapping;
		}

		internal DataTableEntityMapping InsertMapping {
			get {
				return insertMapping;
			}
		}

		internal DataEntityMapping SelectMapping {
			get {
				return selectMapping;
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

