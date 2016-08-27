using System;
using System.Collections.Generic;
using ijw.Contract;
using ijw.Reflection;

namespace ijw.IO {
    /// <summary>
    /// CSV文本读取
    /// </summary>
    public class CSVReader {
#if !NET35
        /// <summary>
        /// 读取进行每行分隔后的字符串数组和行号
        /// </summary>
        /// <param name="csvFilepath">csv文件的路径</param>
        /// <param name="firstLineHeader">第一行是否是header</param>
        /// <returns></returns>
        public static IEnumerable<Tuple<string[], int>> ReadSeparatedStringsWithLineNumber(string csvFilepath) {
            return ReadSeparatedStringsWithLineNumber(csvFilepath, new char[] { ',' });
        }

        public static IEnumerable<Tuple<string[], int>> ReadSeparatedStringsWithLineNumber(string csvFilepath, char[] separators) {
            csvFilepath.ShouldExistSuchFile();
            separators.ShouldBeNotNullArgument();

            var reader = StreamReaderHelper.NewStreamReader(csvFilepath);
            foreach (var t in reader.ReadLinesWithLineNumber()) {
                char[] with = separators ?? new char[] { ',' };
                string[] values = t.Item1.Split(with);
                yield return new Tuple<string[], int>(values, t.Item2);
            }
        }

        public static IEnumerable<T> ReadObjects<T>(string csvFilepath) where T: class , new(){
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
