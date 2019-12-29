using System;

namespace Light.Data
{
	internal class PrimitiveDynamicFieldMapping : DynamicFieldMapping
	{
		public PrimitiveDynamicFieldMapping(Type type, string fieldName, DynamicDataMapping mapping)
			: base(type, fieldName, mapping, true) {
		}

		public override object ToProperty(object value)
		{
			if (Equals(value, DBNull.Value) || Equals(value, null)) {
				return null;
			}

			if (value is IConvertible ic)
			{
				if (ic.GetTypeCode() != _typeCode) {
					return Convert.ChangeType(value, _typeCode, null);
				}

				return value;
			}

			return value;
		}
	}
}

