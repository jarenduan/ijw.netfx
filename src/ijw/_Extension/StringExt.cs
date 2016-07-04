using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Contract;

namespace ijw {
    /// <summary>
    /// 提供对string类型的若干扩展方法
    /// </summary>
    public static class StringExt {
        /// <summary>
        /// 添加短格式的当前时间前缀, 使用[20121221 132355]这样的形式.
        /// </summary>
        /// <param name="astring"></param>
        /// <returns></returns>
        public static string PrefixWithNowShortTimeLabel(this string astring) {
            return string.Format("[{0}] {1}", DateTime.Now.ToLocalTime(), astring);
        }

        /// <summary>
        /// python风格的子字符串
        /// </summary>
        /// <param name="astring"></param>
        /// <param name="startIndex">启始索引. 该处字符将包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null等同于0. 默认值是0</param>
        /// <param name="endIndex">结束索引. 该处字符将不包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null代表结尾. 默认值为null.</param>
        /// <returns></returns>
        public static string GetSubStringPythonStyle(this string astring, int? startIndex = null, int? endIndex = null) {
            int startAt, endAt;
            Helper.PythonStartEndCalculator(astring.Length, out startAt, out endAt, startIndex, endIndex);
            if (endAt < 0) {
                return string.Empty;
            }
            else {
                return astring.GetSubString(startAt, endAt);
            }
        }

        public static string GetSubString(this string astring, int startIndex, int endIndex) {
            startIndex.ShouldBeNotLessThanZero();
            endIndex.ShouldBeNotLessThanZero();
            if (endIndex < startIndex) {
                return string.Empty;
            }
            char[] result = new char[endIndex - startIndex + 1];
            int j = 0;
            for (int i = startIndex; i <= endIndex; i++) {
                result[j] = astring[i];
                j++;
            }
            return new string(result);
        }
        /// <summary>
        /// 重复指定次数. 如"Abc".Repeat(3) 返回 "AbcAbcAbc".
        /// </summary>
        /// <param name="astring"></param>
        /// <param name="times">重复次数, 小于1则返回null</param>
        /// <returns></returns>
        public static string Repeat(this string astring, int times) {
            if (times <= 0) {
                return null;
            }
            if (times == 1) {
                return astring;
            }
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < times; i++) {
                result.Append(astring);
            }
            return result.ToString();
        }

        /// <summary>
        /// 表示缩进. 三个空格组成
        /// </summary>
        private static String INDENT = "   ";

        /// <summary>
        /// 返回格式化后的json序列化字符串
        /// </summary>
        /// <param name="json"></param>
        /// <returns>格式化后的json序列化字符串</returns>
        public static String FormatJson(this String json) {
            StringBuilder result = new StringBuilder();
            string newLine = Environment.NewLine;
            int length = json.Length;
            int number = 0;
            bool inQuotion = false;
            char currChar = ' ';
            char? nextChar = ' ';

            //遍历输入字符串。  
            for (int i = 0; i < length; i++) {
                //获取当前字符和下一个字符。  
                currChar = json[i];
                if ((i + 1) < length) {
                    nextChar = json[i + 1];
                }
                else {
                    nextChar = null;
                }

                //双引号需要控制开合, 内部字符直接输出
                if(currChar == '\"') {
                    inQuotion = !inQuotion;
                }

                if(inQuotion) {
                    result.Append(currChar);
                    continue;
                }

                //如果当前字符是前方括号、前花括号做如下处理：  
                if ((currChar == '[') || (currChar == '{')) {
                    //（1）如果前面还有字符，并且字符为“：”，打印：换行和缩进字符字符串。  
                    //if ((i - 1 > 0) && (json[i - 1] == ':')) {
                    //    result.Append('\n');
                    //    result.Append(indent(number));
                    //}

                    //打印当前字符。  
                    result.Append(currChar);

                    //"{"、"["后面必须换行。  
                    result.Append(newLine);

                    //每出现一次开括号, 缩进次数增加一次。  
                    number++;
                    
                    //打印缩进。
                    result.Append(INDENT.Repeat(number));

                    //进行下一次循环。  
                    continue;
                }

                //如果当前字符是闭括号  
                if ((currChar == ']') || (currChar == '}')) {
                    //"]"和"}"前面必须换行。 
                    result.Append(newLine);

                    //比括号出现, 缩进次数减少一次。  
                    number--;

                    //打印缩进。
                    result.Append(INDENT.Repeat(number));

                    //打印当前字符。  
                    result.Append(currChar);

                    //如果当前字符后面还有字符，并且字符不为“，”，打印：换行。  
                    if (nextChar != ',' && nextChar != '}' && nextChar != ']' && nextChar != null) {
                        result.Append(newLine);
                    }

                    //继续下一次循环。  
                    continue;
                }

                //如果当前字符是逗号。逗号后面换行，并缩进，不改变缩进次数。  
                if ((currChar == ',')) {
                    result.Append(currChar);
                    result.Append(newLine);
                    result.Append(INDENT.Repeat(number));
                    continue;
                }

                //如果是":", 在后面输出一个空格
                if ((currChar == ':')) {
                    result.Append(currChar);
                    result.Append(' ');
                    continue;
                }

                //打印当前字符。  
                result.Append(currChar);
            }

            return result.ToString();
        }

        /// <summary>
        /// 删除指定的字符串
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="toRemove">欲删除的字符串</param>
        /// <returns></returns>
        public static string Remove(this string theString, string toRemove) {
            return theString.Replace(toRemove, string.Empty);
        }

        /// <summary>
        /// 移除指定的字符串
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="toRemove">一系列欲移除的字符串</param>
        /// <returns></returns>
        public static string Remove(this string theString, params string[] toRemove) {
            string result = theString;
            foreach (var s in toRemove) {
                result = result.Remove(s);
            }
            return result;
        }
        /// <summary>
        /// 如果尾部是指定字符串之一, 则移除掉, 否则不做更改。常用于更动字符串中的文件扩展名。
        /// </summary>
        /// <param name="aString"></param>
        /// <param name="toRemove">指定的一系列字符串</param>
        /// <returns>移除尾部指定字符串的结果</returns>
        public static string RemoveLast(this string aString, params string[] toRemove) {
            foreach (var endString in toRemove) {
                if (aString.EndsWith(endString)) {
                    return aString.RemoveLast(endString.Length);
                }
            }
            return aString;
        }

        /// <summary>
        /// 从后向前删除指定数量的字符
        /// </summary>
        /// <param name="aString"></param>
        /// <param name="number">删除数量</param>
        /// <returns></returns>
        public static string RemoveLast(this string aString, int number) {
            return aString.Remove(aString.Length - number, number);
        }

        /// <summary>
        /// 尝试转换成int. 如果失败将返回defaultNumer
        /// </summary>
        /// <param name="s"></param>
        /// <param name="defaultNumer">转换失败时返回的值, 默认是0</param>
        /// <returns></returns>
        public static int ToInt(this string s, int defaultNumer = 0) {
            int i;
            if (int.TryParse(s, out i)) {
                return i;
            }
            else {
                return defaultNumer;
            }
        }

        /// <summary>
        /// 对每个字符串进行Trim操作.
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static IEnumerable<string> TrimEach(this IEnumerable<string> stringArray) {
            foreach (var s in stringArray) {
                yield return s.Trim();
            }
        }

        internal static string AppendOrdinalPostfix(this string s) {
            switch(s) {
                case "11":
                    return "11th";
                case "12":
                    return "12th";
                case "13":
                    return "13th";
                default:
                    break;
            }
            if(s.EndsWith("1")) {
                return s + "st";
            }
            else if(s.EndsWith("2")) {
                return s + "nd";
            }
            else if(s.EndsWith("3")) {
                return s + "rd";
            }
            else {
                return s + "th";
            }
        }

        /// <summary>
        /// 统计含有多少指定的子字符串
        /// </summary>
        /// <param name="parentString"></param>
        /// <param name="subString">字符串</param>
        /// <returns>含有的数量</returns>
        public static int ContainsHowMany(this string parentString, string subString) {
            parentString.ShouldBeNotNullArgument();
            subString.ShouldBeNotNullArgument();

            int pLength = parentString.Length;
            if (pLength == 0) {
                return 0;
            }
            int sLength = subString.Length;
            if (sLength == 0) {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < pLength; i += sLength) {
                bool isContains = false;
                for (int j = 0; j < sLength; j++) {
                    if (i + j >= pLength) {
                        break;
                    }
                    isContains = parentString[i + j] == subString[j];
                    if (!isContains) {
                        break;
                    }
                }
                if (isContains) {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 统计含有多少指定的字符
        /// </summary>
        /// <param name="aString"></param>
        /// <param name="aChar">字符</param>
        /// <returns>含有的数量</returns>
        public static int ContainsHowMany(this string aString, char aChar) {
            aString.ShouldBeNotNullArgument();

            int pLength = aString.Length;
            if (pLength == 0) {
                return 0;
            }

            int count = 0;

            for (int i = 0; i < pLength; i++) {
                if (aString[i] == aChar) {
                    count++;
                }
            }

            return count;
        }

        public static bool IsInteger32(this string aString) {
            int i;
            return int.TryParse(aString, out i);
        }

        /// <summary>
        /// 把字符串转换成指定的枚举, 如果转换失败返回指定的缺省值.
        /// </summary>
        /// <typeparam name="T">转换的枚举类型</typeparam>
        /// <param name="aString">此字符串</param>
        /// <param name="ignoreCase">转换时是否忽略大小写，默认忽略</param>
        /// <param name="defaultValue">可选参数, 表示转换失败的时候所取的缺省值, 默认是枚举的0值</param>
        /// <returns>转换后的枚举值</returns>
        /// <exception cref="ArgumentException">
        /// 指定的类型不是枚举类型时, 将抛出此异常. (Wish C# support "where T: Enum", to kill it at compilation)
        /// </exception>
        public static T ToEnum<T>(this string aString, bool ignoreCase = false, T defaultValue = default(T)) where T : struct {
            T result = defaultValue;
#if (NET35)
            Type t = typeof(T);
            if (!t.IsEnum) {
                throw new ArgumentException($"{t.Name} is not a enumeration type.");
            }
            try {
                result = (T)Enum.Parse(typeof(T), aString);
            }
            catch (ArgumentException) {
                result = defaultValue;
            }
#else
            T e;
            if (Enum.TryParse<T>(aString, out e))
            {
                result = e;
            }

#endif
            return result;
        }
    }
}