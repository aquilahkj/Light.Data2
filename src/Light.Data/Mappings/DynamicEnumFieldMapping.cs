using System;
using System.Collections;

namespace Light.Data
{
	class DynamicEnumFieldMapping : DynamicFieldMapping
	{
		public DynamicEnumFieldMapping (Type type, string fieldName, DynamicCustomMapping mapping)
			: base (type, fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Object.Equals (value, DBNull.Value) || Object.Equals (value, null)) {
				return null;
			}
			else {
				value = Enum.ToObject (_objectType, value);
				return value;
			}
		}
	}
}

