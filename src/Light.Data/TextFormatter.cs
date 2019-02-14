using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace Light.Data
{
    /// <summary>
    /// Custom text template parameter name, parameter name '{name}' format, such as '{param1}', parameter name only allow case of English and Numbers and '_' number, parameter names corresponding object properties, such as to be in the text printed '{}' or ' 'no, two successive input as an escape, as "{{", if you want to specify the parameter value is not empty, need to put a '+' before the parameter name
    /// </summary>
    public class TextFormatter
    {
        class Section
        {
            public string Name;
            public SectionType Type;
            public string Value;
            public bool Nullable;
            public bool ExtendFormat;
        }
        
        class GetPropertyHandler
        {
            private GetValueHandler mGetValue;
            private PropertyInfo mProperty;
            private string mName;

            public GetValueHandler Get {
                get {
                    return this.mGetValue;
                }
            }

            public PropertyInfo Property {
                get {
                    return this.mProperty;
                }
            }

            public string Name {
                get {
                    return this.mName;
                }
            }

            public GetPropertyHandler(PropertyInfo property)
            {
                if (property.CanRead) {
                    this.mGetValue = ReflectionHandlerFactory.PropertyGetHandler(property);
                }
                this.mProperty = property;
                this.mName = property.Name;
            }
        }

        enum SectionType
        {
            NormalText,
            FormatText
        }

        static Dictionary<Type, Dictionary<string, GetPropertyHandler>> TypeDict = new Dictionary<Type, Dictionary<string, GetPropertyHandler>>();

        static Dictionary<string, Section[]> SectionDict = new Dictionary<string, Section[]>();

        static readonly TextFormatProvider textFormatProvider = new TextFormatProvider();

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
            TextFormatter template = new TextFormatter(pattern, options);
            return template.Format(obj);
        }

        readonly Section[] sectionList;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        public TextFormatter(string pattern)
            : this(pattern, TextTemplateOptions.None)
        {

        }

        readonly TextTemplateOptions options;

        //readonly StringComparison comparison;

        readonly bool notAllowNullValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        public TextFormatter(string pattern, TextTemplateOptions options)
        {
            if (string.IsNullOrEmpty(pattern)) {
                throw new ArgumentNullException(nameof(pattern));
            }
            this.options = options;
            bool extend = (this.options & TextTemplateOptions.NotAllowNullValue) != TextTemplateOptions.Compiled;
            if ((this.options & TextTemplateOptions.Compiled) == TextTemplateOptions.Compiled) {
                if (!SectionDict.TryGetValue(pattern, out Section[] array)) {
                    lock (SectionDict) {
                        array = LoadSections(pattern, extend);
                        SectionDict[pattern] = array;
                    }
                }
                this.sectionList = array;
            }
            else {
                this.sectionList = LoadSections(pattern, extend);
            }
            this.notAllowNullValue = ((this.options & TextTemplateOptions.NotAllowNullValue) == TextTemplateOptions.NotAllowNullValue);
        }

        /// <summary>
        /// Format
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Format(object obj)
        {
            if (object.Equals(obj, null)) {
                throw new ArgumentNullException(nameof(obj));
            }
            StringBuilder sb = new StringBuilder();
            foreach (Section s in sectionList) {
                if (s.Type == SectionType.NormalText) {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText) {
                    object data = LoadObject(obj, s.Name);
                    if (!Object.Equals(data, null)) {
                        if (data is string) {
                            sb.AppendFormat(textFormatProvider, s.Value, data);
                        }
                        else {
                            sb.AppendFormat(s.Value, data);
                        }
                    }
                    else {
                        if (this.notAllowNullValue || !s.Nullable) {
                            throw new FormatException(string.Format("The value of \'{0}\' is null.", s.Name));
                        }
                        else {
                            sb.AppendFormat(s.Value, string.Empty);
                        }
                    }
                }
            }
            return sb.ToString();
        }

        public string Format(Dictionary<string, object> dict)
        {
            if (Equals(dict, null)) {
                throw new ArgumentNullException(nameof(dict));
            }
            StringBuilder sb = new StringBuilder();
            foreach (Section s in sectionList) {
                if (s.Type == SectionType.NormalText) {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText) {
                    dict.TryGetValue(s.Name, out object data);
                    if (!Equals(data, null)) {
                        if (data is string) {
                            sb.AppendFormat(textFormatProvider, s.Value, data);
                        }
                        else {
                            sb.AppendFormat(s.Value, data);
                        }
                    }
                    else {
                        if (this.notAllowNullValue || !s.Nullable) {
                            throw new FormatException(string.Format("The value of \'{0}\' is null.", s.Name));
                        }
                        else {
                            sb.AppendFormat(s.Value, string.Empty);
                        }
                    }
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Formar sql string
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
            StringBuilder sb = new StringBuilder();
            Dictionary<string, DataParameter> dict = new Dictionary<string, DataParameter>();
            foreach (Section s in sectionList) {
                if (s.Type == SectionType.NormalText) {
                    sb.Append(s.Value);
                }
                else if (s.Type == SectionType.FormatText) {
                    if (s.ExtendFormat) {
                        throw new FormatException(string.Format("Not support extend format in \'{0}\'.", s.Name));
                    }
                    if (!dict.TryGetValue(s.Name, out DataParameter parameter)) {
                        object data = LoadObject(obj, s.Name);
                        string name = string.Concat(prefix, "P", dict.Count + 1);
                        if (!Object.Equals(data, null)) {
                            parameter = new DataParameter(name, data);
                            dict.Add(s.Name, parameter);
                        }
                        else {
                            if (this.notAllowNullValue || !s.Nullable) {
                                throw new FormatException(string.Format("The value of \'{0}\' is null.", s.Name));
                            }
                            else {
                                parameter = new DataParameter(name, data);
                                dict.Add(s.Name, parameter);
                            }
                        }
                    }
                    sb.Append(parameter.ParameterName);
                }
            }
            parameters = new DataParameter[dict.Count];
            int i = 0;
            foreach(var item in dict.Values) {
                parameters[i] = item;
                i++;
            }
            return sb.ToString();
        }

        static object LoadObject(object obj, string name)
        {
            if(object.Equals(obj, null)) {
                return null;
            }
            Type type = obj.GetType();
            if (!TypeDict.TryGetValue(type, out Dictionary<string, GetPropertyHandler> dict)) {
                lock (TypeDict) {
                    if (!TypeDict.TryGetValue(type, out dict)) {
                        TypeInfo typeInfo = type.GetTypeInfo();
                        PropertyInfo[] properties = typeInfo.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        dict = new Dictionary<string, GetPropertyHandler>();
                        foreach (PropertyInfo propertie in properties) {
                            dict[propertie.Name] = new GetPropertyHandler(propertie);
                        }
                    }
                }
            }
            int index = name.IndexOf(".", StringComparison.Ordinal);
            if (index == -1) {
                if (dict.TryGetValue(name, out GetPropertyHandler handler)) {
                    return handler.Get(obj);
                }
                else {
                    return null;
                }
            }
            else {
                string typeName = name.Substring(0, index);
                if (dict.TryGetValue(typeName, out GetPropertyHandler handler)) {
                    object subObj = handler.Get(obj);
                    if (subObj == null) {
                        return null;
                    }
                    else {
                        return LoadObject(subObj, name.Substring(index + 1));
                    }
                }
                else {
                    return null;
                }
            }
        }

        static Section[] LoadSections(string pattern, bool supportExtend)
        {
            int prev = 0;

            char[] chars = pattern.ToCharArray();
            int realLen = chars.Length;
            int safeLen = realLen - 1;

            bool endflag = false;
            List<Section> list = new List<Section>();
            for (int i = 0; i < safeLen; i++) {
                char c = chars[i];
                if (c == '{') {
                    if (chars[i + 1] == '{') {
                        i++;
                        continue;
                    }
                    else {
                        if (i > 0) {
                            Section normalSection = new Section {
                                Type = SectionType.NormalText,
                                Value = string.Format(new string(chars, prev, i - prev))
                            };
                            list.Add(normalSection);
                            prev = i;
                        }
                        bool nullable;
                        if (chars[i + 1] == '+') {
                            nullable = false;
                            i++;
                        }
                        else {
                            nullable = true;
                        }
                        if (chars[i + 1] == '.') {
                            throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", i + 1, '.'));
                        }

                        int start = i + 1;
                        int end = -1;
                        int split = -1;
                        for (int j = start; j < realLen; j++) {
                            char e = chars[j];
                            if (e == '}') {
                                if (j == start) {
                                    throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, char is '{1}'", j, e));
                                }
                                else if (j < safeLen && chars[j + 1] == '}') {
                                    if (split == -1) {
                                        throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, char is '{1}'", j, e));
                                    }
                                    else {
                                        j++;
                                        continue;
                                    }
                                }
                                if (chars[j - 1] == '.') {
                                    throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", j - 1, '.'));
                                }
                                end = j;
                                break;
                            }
                            else if (e == '{') {
                                if (split > -1 && j < safeLen && chars[j + 1] == '{') {
                                    j++;
                                    continue;
                                }
                                else {
                                    throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, char is '{1}'", j, e));
                                }
                            }
                            else if (split == -1) {
                                if (e == ':' || e == ',') {
                                    if (supportExtend) {
                                        if (j == start) {
                                            throw new FormatException(string.Format("Input string was not a correct format, index is {0}, char is '{1}'", j, e));
                                        }
                                        else if (chars[j - 1] == '.') {
                                            throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", j - 1, '.'));
                                        }
                                        split = j;
                                    }
                                    else {
                                        throw new FormatException(string.Format("Input string was not support extend format, index is {0}, char is '{1}'", j, e));
                                    }
                                }
                                else if ((e >= 48 && e <= 57) || (e >= 65 && e <= 90) || (e >= 97 && e <= 122) || e == '_') {
                                    continue;
                                }
                                else if (e == '.') {
                                    if (chars[j + 1] == '.') {
                                        throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", j + 1, '.'));
                                    }
                                    else {
                                        continue;
                                    }
                                }
                                else {
                                    throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", j, e));
                                }
                            }
                        }
                        if (end == -1) {
                            throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, '{1}' not valid close char", i, c));
                        }
                        endflag |= end == safeLen;
                        string name;
                        string value;
                        bool extendFormat;
                        if (split > -1) {
                            name = new string(chars, start, split - start);
                            string format = new string(chars, split, end - split);
                            value = string.Concat("{0", format, "}");
                            extendFormat = true;
                        }
                        else {
                            name = new string(chars, start, end - start);
                            value = "{0}";
                            extendFormat = false;
                        }
                        Section forrmatSection = new Section {
                            Type = SectionType.FormatText,
                            Name = name,
                            Value = value,
                            Nullable = nullable,
                            ExtendFormat = extendFormat
                        };
                        list.Add(forrmatSection);
                        prev = end + 1;
                        i = end;
                    }
                }
                else if (c == '}') {
                    if (chars[i + 1] == '}') {
                        i++;
                        continue;
                    }
                    else {
                        throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, char is '{1}'", i, c));
                    }
                }
            }
            if (!endflag) {
                char c = chars[safeLen];
                if (c == '{' || c == '}') {
                    throw new FormatException(string.Format("Input string was not in a correct format, Index is {0}, char is '{1}'", safeLen, c));
                }
                Section normalSection = new Section {
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

    class TextFormatProvider : ICustomFormatter, IFormatProvider
    {
        public string Format(string format, object arg, IFormatProvider provider)
        {
            if (arg == null)
                throw new ArgumentNullException(nameof(arg));
            if (format == null) {
                if (arg is IFormattable formatteable) {
                    return formatteable.ToString(format, provider);
                }
                else {
                    return arg.ToString();
                }
            }
            else {
                StringBuilder result = new StringBuilder();
                int len = format.Length;
                int i = 0;
                int tokenLen;
                bool flag = false;
                while (i < format.Length) {
                    char ch = format[i];
                    int nextChar;
                    switch (ch) {
                        case '#':
                            tokenLen = ParseRepeatPattern(format, i, ch);
                            string data = TransData(arg, tokenLen);
                            result.Append(data);
                            flag = true;
                            break;
                        case '\\':
                            nextChar = ParseNextChar(format, i);
                            if (nextChar >= 0) {
                                result.Append(((char)nextChar));
                                tokenLen = 2;
                            }
                            else {
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
                if (flag) {
                    return result.ToString();
                }
                else {
                    if (arg is IFormattable formatteable) {
                        return formatteable.ToString(format, provider);
                    }
                    else {
                        return arg.ToString();
                    }
                }
            }
        }

        static string TransData(object arg, int tokenLen)
        {
            string data = arg as string;
            if (data == null) {
                data = arg.ToString();
            }
            if (data.Length >= tokenLen) {
                return data;
            }
            else {
                return data.PadRight(tokenLen);
            }
        }

        internal static int ParseNextChar(string format, int pos)
        {
            if (pos >= format.Length - 1) {
                return (-1);
            }
            return format[pos + 1];
        }

        internal static int ParseRepeatPattern(string format, int pos, char patternChar)
        {
            int len = format.Length;
            int index = pos + 1;
            while ((index < len) && (format[index] == patternChar)) {
                index++;
            }
            return (index - pos);
        }

        public object GetFormat(Type format)
        {
            if (format == typeof(ICustomFormatter)) {
                return this;
            }
            return null;
        }
    }
}
