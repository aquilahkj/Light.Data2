using System;

namespace Light.Data
{
	internal class BytesDynamicFieldMapping : DynamicFieldMapping
	{
		public BytesDynamicFieldMapping (string fieldName, DynamicDataMapping mapping)
			: base (typeof(byte[]), fieldName, mapping, true)
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

