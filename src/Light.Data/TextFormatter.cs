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
    /// 自定义参数名的文本模板,参数名格式"{参数名}",如{param1},参数名只允许大小写英文和数字和'_'号,参数名对应对象的属性,如要在文本中打印'{'或'}'号,需要连续输入两个作为转义,如"{{,如要指定参数值非空，需在参数名前加'+'号"
    /// </summary>
    public class TextFormatter
    {
        class Section
        {
            public string Name;
            public SectionType Type;
            public string Value;
            public bool Nullable;
        }

        delegate object GetValueHandler(object source);

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
                    this.mGetValue = PropertyGetHandler(property);
                }
                this.mProperty = property;
                this.mName = property.Name;
            }

            private static readonly Dictionary<PropertyInfo, GetValueHandler> mPropertyGetHandlers = new Dictionary<PropertyInfo, GetValueHandler>();

            public static GetValueHandler PropertyGetHandler(PropertyInfo property)
            {
                GetValueHandler handler;
                if (mPropertyGetHandlers.ContainsKey(property)) {
                    return mPropertyGetHandlers[property];
                }
                lock (mPropertyGetHandlers) {
                    if (mPropertyGetHandlers.ContainsKey(property)) {
                        return mPropertyGetHandlers[property];
                    }
                    handler = CreatePropertyGetHandler(property);
                    mPropertyGetHandlers.Add(property, handler);
                }
                return handler;
            }

            private static GetValueHandler CreatePropertyGetHandler(PropertyInfo property)
            {
                DynamicMethod method = new DynamicMethod(string.Empty, typeof(object), new Type[] { typeof(object) }, property.DeclaringType.GetTypeInfo().Module);
                ILGenerator iLGenerator = method.GetILGenerator();
                iLGenerator.Emit(OpCodes.Ldarg_0);
                iLGenerator.EmitCall(OpCodes.Callvirt, property.GetMethod, null);
                EmitBoxIfNeeded(iLGenerator, property.PropertyType);
                iLGenerator.Emit(OpCodes.Ret);
                return (GetValueHandler)method.CreateDelegate(typeof(GetValueHandler));
            }

            private static void EmitBoxIfNeeded(ILGenerator il, Type type)
            {
                if (type.GetTypeInfo().IsValueType) {
                    il.Emit(OpCodes.Box, type);
                }
            }
        }

        enum SectionType
        {
            NormalText,
            FormatText
        }

        static Dictionary<Type, Dictionary<string, GetPropertyHandler>> TypeDict = new Dictionary<Type, Dictionary<string, GetPropertyHandler>>();

        static Dictionary<string, Section[]> SectionDict = new Dictionary<string, Section[]>();

        static TextFormatProvider textFormat = new TextFormatProvider();

        public static string Format(string pattern, Object obj)
        {
            return Format(pattern, obj, TextTemplateOptions.None);
        }

        public static string Format(string pattern, Object obj, TextTemplateOptions options)
        {
            TextFormatter template = new TextFormatter(pattern, options);
            return template.Format(obj);
        }

        Section[] sectionList;

        public TextFormatter(string pattern)
            : this(pattern, TextTemplateOptions.None)
        {

        }

        readonly TextTemplateOptions options;

        //readonly StringComparison comparison;

        readonly bool notAllowNullValue;

        public TextFormatter(string pattern, TextTemplateOptions options)
        {
            if (string.IsNullOrEmpty(pattern)) {
                throw new ArgumentNullException(nameof(pattern));
            }
            this.options = options;
            if ((this.options & TextTemplateOptions.Compiled) == TextTemplateOptions.Compiled) {
                if (!SectionDict.TryGetValue(pattern, out Section[] array)) {
                    lock (SectionDict) {
                        array = LoadSections(pattern);
                        SectionDict[pattern] = array;
                    }
                }
                this.sectionList = array;
            }
            else {
                this.sectionList = LoadSections(pattern);
            }
            //if ((this.options & TextTemplateOptions.IgnoreCase) == TextTemplateOptions.IgnoreCase) {
            //    comparison = StringComparison.OrdinalIgnoreCase;
            //}
            //else {
            //    comparison = StringComparison.Ordinal;
            //}
            this.notAllowNullValue = ((this.options & TextTemplateOptions.NotAllowNullValue) == TextTemplateOptions.NotAllowNullValue);
        }

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
                            sb.AppendFormat(textFormat, s.Value, data);
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

        static object LoadObject(object obj, string name)
        {
            Type type = obj.GetType();
            TypeInfo typeInfo = type.GetTypeInfo();
            if (!TypeDict.TryGetValue(type, out Dictionary<string, GetPropertyHandler> dict)) {
                lock (TypeDict) {
                    if (!TypeDict.TryGetValue(type, out dict)) {
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
                dict.TryGetValue(name, out GetPropertyHandler handler);
                if (handler != null) {
                    return handler.Get(obj);
                }
                else {
                    return null;
                }
            }
            else {
                string typeName = name.Substring(0, index);
                dict.TryGetValue(typeName, out GetPropertyHandler handler);
                if (handler != null) {
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

        static Section[] LoadSections(string pattern)
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
                                    if (j == start) {
                                        throw new FormatException(string.Format("Input string was not in a correct format, index is {0}, char is '{1}'", j, e));
                                    }
                                    else if (chars[j - 1] == '.') {
                                        throw new FormatException(string.Format("Input param name was not a valid word, index is {0}, char is '{1}'", j - 1, '.'));
                                    }
                                    split = j;
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
                        if (split > -1) {
                            name = new string(chars, start, split - start);
                            string format = new string(chars, split, end - split);
                            value = string.Concat("{0", format, "}");
                        }
                        else {
                            name = new string(chars, start, end - start);
                            value = "{0}";
                        }
                        Section forrmatSection = new Section {
                            Type = SectionType.FormatText,
                            Name = name,
                            Value = value,
                            Nullable = nullable
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

    [Flags]
    public enum TextTemplateOptions
    {
		None = 0,
		Compiled = 1,
        //IgnoreCase = 2,
        NotAllowNullValue = 2
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
