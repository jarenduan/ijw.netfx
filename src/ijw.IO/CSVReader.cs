using System;
using System.Collections.Generic;
using ijw.Contract;
using ijw.Reflection;
using System.Text;

namespace ijw.IO {
    /// <summary>
    /// CSV文本读取
    /// </summary>
    [Obsolete]
    public class CSVReader {
#if !NET35
        #region read strings, both using utf-8
        /// <summary>
        /// 将每行以逗号分隔后，返回字符串数组和行号. 使用utf-8读取文件.
        /// </summary>
        /// <param name="csvFilepath">csv文件的路径</param>
        /// <param name="isFirstLineHeader">第一行是否是header</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string[], int>> ReadSeparatedStringsWithLineNumber(string csvFilepath, bool isFirstLineHeader = false) {
            return ReadSeparatedStringsWithLineNumber(csvFilepath, new char[] { ',' }, isFirstLineHeader);
        }

        /// <summary>
        /// 将每行用指定字符分隔后，返回字符串数组和行号.使用utf-8读取文件.
        /// </summary>
        /// <param name="csvFilepath">csv文件的路径</param>
        /// <param name="separators">使用的分隔符</param>
        /// <param name="isFirstLineHeader">第一行是否是header</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string[], int>> ReadSeparatedStringsWithLineNumber(string csvFilepath, char[] separators, bool isFirstLineHeader = false) {
            csvFilepath.ShouldExistSuchFile();
            separators.ShouldBeNotNullArgument();

            var reader = StreamReaderHelper.NewStreamReaderFrom(csvFilepath);
            foreach (var t in reader.ReadLinesWithLineNumber()) {
                if (t.Item2 == 1 && isFirstLineHeader) {
                    continue;
                }
                char[] with = separators ?? new char[] { ',' };
                string[] values = t.Item1.Split(with);
                yield return new Tuple<string[], int>(values, t.Item2);
            }
        }
        #endregion
        public static IEnumerable<T> ReadObjects<T>(string csvFilepath) where T : class, new() {
            string[] headers = null;
            foreach (var strings in ReadSeparatedStringsWithLineNumber(csvFilepath)) {
                if (strings.Item2 == 1) {
                    headers = strings.Item1;
                }
                else {
                    yield return ReflectionHelper.CreateNewInstance<T>(headers, strings.Item1);
                }
            }
        }
#endif
    }
}
