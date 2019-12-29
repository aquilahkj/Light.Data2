using System;

namespace Light.Data
{
	internal class StringDynamicFieldMapping : DynamicFieldMapping
	{
		public StringDynamicFieldMapping (string fieldName, DynamicDataMapping mapping)
			: base (typeof(string), fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}
  
		 	return value;
		}
	}
}

