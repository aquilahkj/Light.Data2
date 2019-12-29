using System;

namespace Light.Data
{
	internal class GuidDynamicFieldMapping : DynamicFieldMapping
	{
		public GuidDynamicFieldMapping (string fieldName, DynamicDataMapping mapping)
			: base (typeof(Guid), fieldName, mapping, true)
		{

		}

		public override object ToProperty (object value)
		{
			if (Equals (value, DBNull.Value) || Equals (value, null)) {
				return null;
			}
  
			if (value is string valueString)
			{
				value = Guid.Parse(valueString);
			}
			else if (value is byte[] valueBuffer)
			{
				value = new Guid(valueBuffer);
			}

			return value;
		}
	}
}

