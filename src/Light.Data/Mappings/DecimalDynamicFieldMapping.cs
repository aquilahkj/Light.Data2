using System;

namespace Light.Data
{
	internal class DecimalDynamicFieldMapping : DynamicFieldMapping
	{
		public DecimalDynamicFieldMapping (string fieldName, DynamicDataMapping mapping)
			: base (typeof(decimal), fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}
  
			if (value.GetType() != ObjectType)
			{
				value = Convert.ToDecimal(value);
			}

			return value;
		}
	}
}

