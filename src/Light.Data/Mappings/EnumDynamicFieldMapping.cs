using System;

namespace Light.Data
{
	internal class EnumDynamicFieldMapping : DynamicFieldMapping
	{
		public EnumDynamicFieldMapping (Type type, string fieldName, DynamicDataMapping mapping)
			: base (type, fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}

			value = Enum.ToObject (ObjectType, value);
			return value;
		}
	}
}

