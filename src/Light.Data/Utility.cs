using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace Light.Data
{
    internal static class Utility
    {
        private const sbyte MIN_SBYTE = 0;

        private const byte MIN_BYTE = 0;

        private const short MIN_SHORT = 0;

        private const ushort MIN_USHORT = 0;

        public static bool ParseDbType(string dbType, out DbType type)
        {
            type = DbType.Object;
            var index = dbType.IndexOf('(');
            string typeString;
            if (index < 0) {
                typeString = dbType;
            }
            else if (index == 0) {
                return false;
            }
            else {
                typeString = dbType.Substring(0, index);
            }
            var result = false;
            if (Enum.TryParse(typeString, true, out type)) {
                result = true;
            }
            else {
                if (typeString.Equals("int", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.Int32;
                    result = true;
                }
                else if (typeString.Equals("short", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.Int16;
                    result = true;
                }
                else if (typeString.Equals("long", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.Int64;
                    result = true;
                }
                else if (typeString.Equals("uint", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.UInt32;
                    result = true;
                }
                else if (typeString.Equals("ushort", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.UInt16;
                    result = true;
                }
                else if (typeString.Equals("ulong", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.UInt64;
                    result = true;
                }
                else if (typeString.Equals("float", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.Double;
                    result = true;
                }
                else if (typeString.Equals("bool", StringComparison.OrdinalIgnoreCase)) {
                    type = DbType.Boolean;
                    result = true;
                }
            }
            return result;
        }

        public static bool ParseSize(string dbType, out int size, out byte? scale)
        {
            size = 0;
            scale = null;
            var result = Regex.Match(dbType, "(?<=\\u0028).*?(?=\\u0029)").Value;
            if (string.IsNullOrEmpty(result)) {
                return false;
            }
            var arr = result.Split(',');
            if (int.TryParse(arr[0], out size)) {
                if (arr.Length == 2) {
                    if (byte.TryParse(arr[1], out var s)) {
                        scale = s;
                    }
                    else {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public static bool EnumableObjectEquals(object value1, object value2)
        {
            if (Equals(value1, value2)) {
                return true;
            }
            var objType1 = value1.GetType();
            var objType2 = value2.GetType();
            if (objType1 != objType2) {
                return false;
            }
            if (objType1 == typeof(string)) {
                return (value1 as string) == (value2 as string);
            }
            if (value1 is IEnumerable enumerable) {
                var e1 = enumerable.GetEnumerator();
                var e2 = (value2 as IEnumerable).GetEnumerator();

                while (true) {
                    var b1 = e1.MoveNext();
                    var b2 = e2.MoveNext();
                    if (b1 && b2) {
                        if (!Equals(e1.Current, e2.Current)) {
                            return false;
                        }
                    }
                    else if (!b1 && !b2) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            else {
                return false;
            }
        }

        public static int EnumableHashCode(IEnumerable e)
        {
            var result = 0;
            foreach (var obj in e) {
                result ^= obj.GetHashCode();
            }
            return result;
        }

        public static object GetDefaultValue(TypeCode typeCode)
        {
            object obj;
            switch (typeCode) {
                case TypeCode.String:
                    obj = string.Empty;
                    break;
                case TypeCode.Boolean:
                    obj = false;
                    break;
                case TypeCode.Char:
                    obj = Char.MinValue;
                    break;
                case TypeCode.SByte:
                    obj = MIN_SBYTE;
                    break;
                case TypeCode.Byte:
                    obj = MIN_BYTE;
                    break;
                case TypeCode.Int16:
                    obj = MIN_SHORT;
                    break;
                case TypeCode.UInt16:
                    obj = MIN_USHORT;
                    break;
                case TypeCode.Int32:
                    obj = 0;
                    break;
                case TypeCode.UInt32:
                    obj = 0u;
                    break;
                case TypeCode.Int64:
                    obj = 0L;
                    break;
                case TypeCode.UInt64:
                    obj = 0uL;
                    break;
                case TypeCode.Single:
                    obj = 0f;
                    break;
                case TypeCode.Double:
                    obj = 0d;
                    break;
                case TypeCode.Decimal:
                    obj = 0m;
                    break;
                case TypeCode.DateTime:
                    obj = DateTime.MinValue;
                    break;
                case TypeCode.Object:
                    obj = null;
                    break;
                default:
                    obj = null;
                    break;
            }
            return obj;
        }
    }
}
