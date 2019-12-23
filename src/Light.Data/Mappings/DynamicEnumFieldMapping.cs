using System;

namespace Light.Data
{
	internal class DynamicEnumFieldMapping : DynamicFieldMapping
	{
		public DynamicEnumFieldMapping (Type type, string fieldName, DynamicCustomMapping mapping)
			: base (type, fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}
			else {
				value = Enum.ToObject (_objectType, value);
				return value;
			}
		}
	}
}

