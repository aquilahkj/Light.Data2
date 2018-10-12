using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light.Data.Template
{
    public static class StringUtil
    {
        static char[] splitChars = new char[] { '_', ' ', '	', ':', '-', '+' };

        public static void SetSplitChars(params char[] chars)
        {
            List<char> list = new List<char>(splitChars);
            list.AddRange(chars);
            splitChars = list.ToArray();
        }

        public static string ToPascalCase(string name)
        {
            StringBuilder sb = new StringBuilder();
            string[] parts = name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string part in parts) {
                if (part.Length > 0) {
                    sb.Append(Char.ToUpper(part[0]));
                    if (part.Length > 1) {
                        string o = part.Substring(1);
                        if (o == o.ToUpper()) {
                            o = o.ToLower();
                        }
                        sb.Append(o);
                    }
                }
            }
            return sb.ToString();
        }

        public static string ToCamelCase(string name)
        {
            StringBuilder sb = new StringBuilder();
            string[] parts = name.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            bool f = false;
            foreach (string part in parts) {
                if (part.Length > 0) {
                    if (!f) {
                        sb.Append(Char.ToLower(part[0]));
                        f = true;
                    }
                    else {
                        sb.Append(Char.ToUpper(part[0]));
                    }

                    if (part.Length > 1) {
                        string o = part.Substring(1);
                        if (o == o.ToUpper()) {
                            o = o.ToLower();
                        }
                        sb.Append(o);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
