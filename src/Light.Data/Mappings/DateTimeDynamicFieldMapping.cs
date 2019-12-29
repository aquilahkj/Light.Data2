using System;

namespace Light.Data
{
	internal class DateTimeDynamicFieldMapping : DynamicFieldMapping
	{
		public DateTimeDynamicFieldMapping (string fieldName, DynamicDataMapping mapping)
			: base (typeof(DateTime), fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}
  
			if (value.GetType() != ObjectType)
			{
				return Convert.ToDateTime(value);
			}

			return value;
		}
	}
}

