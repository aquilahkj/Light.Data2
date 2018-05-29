
using System;

namespace Light.Data
{
	/// <summary>
	/// Custom field mapping.
	/// </summary>
	class CustomFieldMapping : DataFieldMapping
	{
		public CustomFieldMapping (string fieldName, DataEntityMapping mapping)
			: base (null, fieldName, null, mapping, true, null)
		{

		}

		#region implemented abstract members of FieldMapping

		public override object ToProperty (object value)
		{
			if (Object.Equals (value, DBNull.Value)) {
				return null;
			}
			else {
				return value;
			}
		}

		public override object ToParameter (object value)
		{
			if (Object.Equals (value, DBNull.Value)) {
				return null;
			}
			else {
				return value;
			}
		}

		#endregion

		#region implemented abstract members of DataFieldMapping

		//public override object ToColumn (object value)
		//{
		//	return value;
		//}

        public override object GetInsertData(object entity, bool refreshField)
        {
            object value = Handler.Get(entity);
            return value;
        }
            #endregion
        }
}
