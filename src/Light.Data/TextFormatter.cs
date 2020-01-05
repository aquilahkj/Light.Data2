using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Custom text template parameter name, parameter name '{name}' format, such as '{param1}', parameter name only allow case of English and Numbers and '_' number, parameter names corresponding object properties, such as to be in the text printed '{}' or ' 'no, two successive input as an escape, as "{{", if you want to specify the parameter value is not empty, need to put a '+' before the parameter name
    /// </summary>
    public class TextFormatter
    {
        private class Section
        {
            public string Name;
            public SectionType Type;
            public string Value;
            public bool Nullable;
            public bool ExtendFormat;
        }

        private class GetPropertyHandler
        {
            public GetValueHandler Get { get; }

            public GetPropertyHandler(PropertyInfo property)
            {
                if (property.CanRead)
                {
                    Get = ReflectionHandlerFactory.PropertyGetHandler(property);
                }
            }
        }

        private enum SectionType
        {
            NormalText,
            FormatText
        }

        private static readonly Dictionary<Type, Dictionary<string, GetPropertyHandler>> TypeDict =
            new Dictionary<Type, Dictionary<string, GetPropertyHandler>>();

        private static readonly Dictionary<string, Section[]> SectionDict = new Dictionary<string, Section[]>();

        private static readonly TextFormatProvider textFormatProvider = new TextFormatProvider();

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Format(string pattern, object obj)
        {
            return Format(pattern, obj, TextTemplateOptions.None);
        }

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string Format(string pattern, object obj, TextTemplateOptions options)
        {
            var template = new TextFormatter(pattern, options);
            return template.Format(obj);
        }

        private readonly Section[] sectionList;

        private readonly bool notAllowNullValue;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public TextFormatter(string pattern)
            : this(pattern, TextTemplateOptions.None)
        {
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        public TextFormatter(string pattern, TextTemplateOptions options)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var extend = (options & TextTemplateOptions.NotAllowNullValue) !=
                         TextTemplateOptions.NotAllowNullValue;
            if ((options & TextTemplateOptions.Compiled) == TextTemplateOptions.Compiled)
            {
                if (!SectionDict.TryGetValue(pattern, out var array))
                {
                    lock (SectionDict)
                    {
                        if (!SectionDict.TryGetValue(pattern, out array))
                        {
                            array = LoadSections(pattern, extend);
                            SectionDict[pattern] = array;
                        }
                    }
                }

                sectionList = array;
            }
            else
            {
                sectionList = LoadSections(pattern, extend);
            }

            notAllowNullValue = (options & TextTemplateOptions.NotAllowNullValue) ==
                                TextTemplateOptions.NotAllowNullValue;
        }

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Format(object obj)
        {
            if (Equals(obj, null))
            {
                throw new ArgumentNullException(nameof(obj));
            }

            var sb = new StringBuilder();
            foreach (var s in sectionList)
            {
                if (s.Type == SectionType.NormalText)
                {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText)
                {
                    var data = LoadObject(obj, s.Name);
                    if (!Equals(data, null))
                    {
                        if (data is string)
                        {
                            sb.AppendFormat(textFormatProvider, s.Value, data);
                        }
                        else
                        {
                            sb.AppendFormat(s.Value, data);
                        }
                    }
                    else
                    {
                        if (notAllowNullValue || !s.Nullable)
                        {
                            throw new FormatException($"The value of \'{s.Name}\' is null.");
                        }

                        sb.AppendFormat(s.Value, string.Empty);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Format string
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public string Format(Dictionary<string, object> dict)
        {
            if (Equals(dict, null))
            {
                throw new ArgumentNullException(nameof(dict));
            }

            var sb = new StringBuilder();
            foreach (var s in sectionList)
            {
                if (s.Type == SectionType.NormalText)
                {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText)
                {
                    dict.TryGetValue(s.Name, out var data);
                    if (!Equals(data, null))
                    {
                        if (data is string)
                        {
                            sb.AppendFormat(textFormatProvider, s.Value, data);
                        }
                        else
                        {
                            sb.AppendFormat(s.Value, data);
                        }
                    }
                    else
                    {
                        if (notAllowNullValue || !s.Nullable)
                        {
                            throw new FormatException($"The value of \'{s.Name}\' is null.");
                        }

                        sb.AppendFormat(s.Value, string.Empty);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Format sql string
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="prefix"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string FormatSql(object obj, string prefix, out DataParameter[] parameters)
        {
            //if (object.Equals(obj, null)) {
            //    throw new ArgumentNullException(nameof(obj));
            //}
            var sb = new StringBuilder();
            var dict = new Dictionary<string, DataParameter>();
            foreach (var s in sectionList)
            {
                if (s.Type == SectionType.NormalText)
                {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText)
                {
                    if (s.ExtendFormat)
                    {
                        throw new FormatException($"Not support extend format in \'{s.Name}\'.");
                    }

                    if (!dict.TryGetValue(s.Name, out var parameter))
                    {
                        var data = LoadObject(obj, s.Name);
                        var name = string.Concat(prefix, "P", dict.Count + 1);
                        if (!Equals(data, null))
                        {
                            parameter = new DataParameter(name, data);
                            dict.Add(s.Name, parameter);
                        }
                        else
                        {
                            if (notAllowNullValue || !s.Nullable)
                            {
                                throw new FormatException($"The value of \'{s.Name}\' is null.");
                            }

                            parameter = new DataParameter(name, null);
                            dict.Add(s.Name, parameter);
                        }
                    }

                    sb.Append(parameter.ParameterName);
                }
            }

            parameters = new DataParameter[dict.Count];
            var i = 0;
            foreach (var item in dict.Values)
            {
                parameters[i] = item;
                i++;
            }

            return sb.ToString();
        }

        private static object LoadObject(object obj, string name)
        {
            if (Equals(obj, null))
            {
                return null;
            }

            var type = obj.GetType();
            if (!TypeDict.TryGetValue(type, out var dict))
            {
                lock (TypeDict)
                {
                    if (!TypeDict.TryGetValue(type, out dict))
                    {
                        var typeInfo = type.GetTypeInfo();
                        var properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        dict = new Dictionary<string, GetPropertyHandler>();
                        foreach (var property in properties)
                        {
                            dict[property.Name] = new GetPropertyHandler(property);
                        }

                        TypeDict[type] = dict;
                    }
                }
            }

            var index = name.IndexOf(".", StringComparison.Ordinal);
            if (index == -1)
            {
                if (dict.TryGetValue(name, out var handler))
                {
                    return handler.Get(obj);
                }

                return null;
            }
            else
            {
                var typeName = name.Substring(0, index);
                if (dict.TryGetValue(typeName, out var handler))
                {
                    var subObj = handler.Get(obj);
                    if (subObj == null)
                    {
                        return null;
                    }

                    return LoadObject(subObj, name.Substring(index + 1));
                }

                return null;
            }
        }

        private static Section[] LoadSections(string pattern, bool supportExtend)
        {
            var prev = 0;

            var chars = pattern.ToCharArray();
            var realLen = chars.Length;
            var safeLen = realLen - 1;

            var endFlag = false;
            var list = new List<Section>();
            for (var i = 0; i < safeLen; i++)
            {
                var c = chars[i];
                if (c == '{')
                {
                    if (chars[i + 1] == '{')
                    {
                        i++;
                    }
                    else
                    {
                        if (i > 0)
                        {
                            var normalSection = new Section
                            {
                                Type = SectionType.NormalText,
                                Value = string.Format(new string(chars, prev, i - prev))
                            };
                            list.Add(normalSection);
                        }

                        bool nullable;
                        if (chars[i + 1] == '+')
                        {
                            nullable = false;
                            i++;
                        }
                        else
                        {
                            nullable = true;
                        }

                        if (chars[i + 1] == '.')
                        {
                            throw new FormatException(
                                $"Input param name was not a valid word, index is {i + 1}, char is '{'.'}'");
                        }

                        var start = i + 1;
                        var end = -1;
                        var split = -1;
                        for (var j = start; j < realLen; j++)
                        {
                            var e = chars[j];
                            if (e == '}')
                            {
                                if (j == start)
                                {
                                    throw new FormatException(
                                        $"Input string was not in a correct format, index is {j}, char is '{e}'");
                                }

                                if (j < safeLen && chars[j + 1] == '}')
                                {
                                    if (split == -1)
                                    {
                                        throw new FormatException(
                                            $"Input string was not in a correct format, index is {j}, char is '{e}'");
                                    }

                                    j++;
                                    continue;
                                }

                                if (chars[j - 1] == '.')
                                {
                                    throw new FormatException(
                                        $"Input param name was not a valid word, index is {j - 1}, char is '{'.'}'");
                                }

                                end = j;
                                break;
                            }

                            if (e == '{')
                            {
                                if (split > -1 && j < safeLen && chars[j + 1] == '{')
                                {
                                    j++;
                                }
                                else
                                {
                                    throw new FormatException(
                                        $"Input string was not in a correct format, index is {j}, char is '{e}'");
                                }
                            }
                            else if (split == -1)
                            {
                                if (e == ':' || e == ',')
                                {
                                    if (supportExtend)
                                    {
                                        if (j == start)
                                        {
                                            throw new FormatException(
                                                $"Input string was not a correct format, index is {j}, char is '{e}'");
                                        }

                                        if (chars[j - 1] == '.')
                                        {
                                            throw new FormatException(
                                                $"Input param name was not a valid word, index is {j - 1}, char is '{'.'}'");
                                        }

                                        split = j;
                                    }
                                    else
                                    {
                                        throw new FormatException(
                                            $"Input string was not support extend format, index is {j}, char is '{e}'");
                                    }
                                }
                                else if ((e >= 48 && e <= 57) || (e >= 65 && e <= 90) || (e >= 97 && e <= 122) ||
                                         e == '_')
                                {
                                }
                                else if (e == '.')
                                {
                                    if (chars[j + 1] == '.')
                                    {
                                        throw new FormatException(
                                            $"Input param name was not a valid word, index is {j + 1}, char is '{'.'}'");
                                    }
                                }
                                else
                                {
                                    throw new FormatException(
                                        $"Input param name was not a valid word, index is {j}, char is '{e}'");
                                }
                            }
                        }

                        if (end == -1)
                        {
                            throw new FormatException(
                                $"Input string was not in a correct format, index is {i}, '{c}' not valid close char");
                        }

                        endFlag |= end == safeLen;
                        string name;
                        string value;
                        bool extendFormat;
                        if (split > -1)
                        {
                            name = new string(chars, start, split - start);
                            var format = new string(chars, split, end - split);
                            value = string.Concat("{0", format, "}");
                            extendFormat = true;
                        }
                        else
                        {
                            name = new string(chars, start, end - start);
                            value = "{0}";
                            extendFormat = false;
                        }

                        var formatSection = new Section
                        {
                            Type = SectionType.FormatText,
                            Name = name,
                            Value = value,
                            Nullable = nullable,
                            ExtendFormat = extendFormat
                        };
                        list.Add(formatSection);
                        prev = end + 1;
                        i = end;
                    }
                }
                else if (c == '}')
                {
                    if (chars[i + 1] == '}')
                    {
                        i++;
                    }
                    else
                    {
                        throw new FormatException(
                            $"Input string was not in a correct format, index is {i}, char is '{c}'");
                    }
                }
            }

            if (!endFlag)
            {
                var c = chars[safeLen];
                if (c == '{' || c == '}')
                {
                    throw new FormatException(
                        $"Input string was not in a correct format, Index is {safeLen}, char is '{c}'");
                }

                var normalSection = new Section
                {
                    Type = SectionType.NormalText,
                    Value = string.Format(new string(chars, prev, realLen - prev))
                };
                list.Add(normalSection);
            }

            return list.ToArray();
        }
    }

    /// <summary>
    /// Text template options
    /// </summary>
    [Flags]
    public enum TextTemplateOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Compile template
        /// </summary>
        Compiled = 1,

        /// <summary>
        /// Not allow null value
        /// </summary>
        NotAllowNullValue = 2,

        /// <summary>
        /// Not allow extend format
        /// </summary>
        NotAllowExtend = 3
    }

    internal class TextFormatProvider : ICustomFormatter, IFormatProvider
    {
        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));
            if (format == null)
            {
                if (arg is IFormattable formattable)
                {
                    return formattable.ToString(null, provider);
                }

                return arg.ToString();
            }

            var result = new StringBuilder();
            var i = 0;
            var flag = false;
            while (i < format.Length)
            {
                var ch = format[i];
                int tokenLen;
                switch (ch)
                {
                    case '#':
                        tokenLen = ParseRepeatPattern(format, i, ch);
                        var data = TransData(arg, tokenLen);
                        result.Append(data);
                        flag = true;
                        break;
                    case '\\':
                        var nextChar = ParseNextChar(format, i);
                        if (nextChar >= 0)
                        {
                            result.Append(((char) nextChar));
                            tokenLen = 2;
                        }
                        else
                        {
                            throw new FormatException("Input string was not in a correct format.");
                        }

                        break;
                    default:
                        result.Append(ch);
                        tokenLen = 1;
                        break;
                }

                i += tokenLen;
            }

            if (flag)
            {
                return result.ToString();
            }

            {
                if (arg is IFormattable formattable)
                {
                    return formattable.ToString(format, provider);
                }

                return arg.ToString();
            }
        }

        private static string TransData(object arg, int tokenLen)
        {
            if (!(arg is string data))
            {
                data = arg.ToString();
            }

            if (data.Length >= tokenLen)
            {
                return data;
            }

            return data.PadRight(tokenLen);
        }

        internal static int ParseNextChar(string format, int pos)
        {
            if (pos >= format.Length - 1)
            {
                return (-1);
            }

            return format[pos + 1];
        }

        internal static int ParseRepeatPattern(string format, int pos, char patternChar)
        {
            var len = format.Length;
            var index = pos + 1;
            while ((index < len) && (format[index] == patternChar))
            {
                index++;
            }

            return (index - pos);
        }

        public object GetFormat(Type format)
        {
            if (format == typeof(ICustomFormatter))
            {
                return this;
            }

            return null;
        }
    }
}