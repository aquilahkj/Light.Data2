using System;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

namespace Light.Data
{
    internal static class Utility
    {
        public static bool ParseDbType(string dbType, out DbType type)
        {
            type = DbType.Object;
            var index = dbType.IndexOf('(');
            string typeString;
            if (index < 0)
            {
                typeString = dbType;
            }
            else if (index == 0)
            {
                return false;
            }
            else
            {
                typeString = dbType.Substring(0, index);
            }

            var result = false;
            if (Enum.TryParse(typeString, true, out type))
            {
                result = true;
            }
            else
            {
                if (typeString.Equals("int", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.Int32;
                    result = true;
                }
                else if (typeString.Equals("short", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.Int16;
                    result = true;
                }
                else if (typeString.Equals("long", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.Int64;
                    result = true;
                }
                else if (typeString.Equals("uint", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.UInt32;
                    result = true;
                }
                else if (typeString.Equals("ushort", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.UInt16;
                    result = true;
                }
                else if (typeString.Equals("ulong", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.UInt64;
                    result = true;
                }
                else if (typeString.Equals("float", StringComparison.OrdinalIgnoreCase))
                {
                    type = DbType.Double;
                    result = true;
                }
                else if (typeString.Equals("bool", StringComparison.OrdinalIgnoreCase))
                {
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
            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            var arr = result.Split(',');
            if (int.TryParse(arr[0], out size))
            {
                if (arr.Length == 2)
                {
                    if (byte.TryParse(arr[1], out var s))
                    {
                        scale = s;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        public static int EnumerableHashCode(IEnumerable e)
        {
            var result = 0;
            foreach (var obj in e)
            {
                result ^= obj.GetHashCode();
            }

            return result;
        }
    }
}