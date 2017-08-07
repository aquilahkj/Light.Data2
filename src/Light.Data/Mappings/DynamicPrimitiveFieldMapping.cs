using System;
namespace Light.Data
{
	class DynamicPrimitiveFieldMapping : DynamicFieldMapping
	{
		public DynamicPrimitiveFieldMapping(Type type, string fieldName, DynamicCustomMapping mapping)
			: base(type, fieldName, mapping, true) {
		}

		public override object ToProperty(object value) {
			if (Object.Equals(value, DBNull.Value) || Object.Equals(value, null)) {
				return null;
			}
			else {
                if (value is IConvertible ic) {
                    if (ic.GetTypeCode() != _typeCode) {
                        return Convert.ChangeType(value, _typeCode, null);
                    }
                    else {
                        return value;
                    }
                }
                else {
                    return value;
                }
            }
		}
	}
}

