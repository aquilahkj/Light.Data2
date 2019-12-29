
using System;

namespace Light.Data
{
	/// <summary>
	/// Custom field mapping.
	/// </summary>
	internal class CustomFieldMapping : DataFieldMapping
	{
		public override bool IsPrimaryKey => false;
		public override bool IsIdentity => false;
		public override bool IsAutoUpdate => false;
		public CustomFieldMapping (string fieldName, DataEntityMapping mapping)
			: base (null, fieldName, null, mapping, true, null)
		{
			
		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value)) {
				return null;
			}

			return value;
		}

		public override object ToParameter (object value)
		{
			if (Equals (value, DBNull.Value)) {
				return null;
			}

			return value;
		}
		
		public override object ToUpdate(object entity, bool refreshField)
		{
			var value = Handler.Get(entity);
			return value;
		}
		
		public override object ToInsert(object entity, bool refreshField)
        {
            var value = Handler.Get(entity);
            return value;
        }
	}
}
