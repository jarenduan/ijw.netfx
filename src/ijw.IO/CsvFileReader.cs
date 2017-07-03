using ijw.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace ijw.IO {
    /// <summary>
    /// CSV文件读取
    /// </summary>
    public class CsvFileReader {
        public string CsvFilePath { get; set; }
        public Encoding Encoding { get; set; } = Encoding.Unicode;
        public bool IsFirstLineHeader { get; set; } = true;
        /// <summary>
        /// 分隔符
        /// </summary>
        public char[] Separators { get; set; } = new char[] { ',' };

        /// <summary>
        /// 将每行分隔后，返回字符串数组和行号
        /// </summary>
        public IEnumerable<Tuple<string[], int>> ReadStringsWithLineNumber() {
            this.CsvFilePath.ShouldExistSuchFile();
            this.Separators.ShouldBeNotNullArgument();
            this.Encoding.ShouldBeNotNullReference();

            using (var reader = StreamReaderHelper.NewStreamReaderFrom(this.CsvFilePath, this.Encoding)) {
                foreach (var t in reader.ReadLinesWithLineNumber()) {
                    if (t.Item2 == 1 && this.IsFirstLineHeader) {
                        continue;
                    }
                    char[] with = this.Separators ?? new char[] { ',' };
                    string[] values = t.Item1.Split(with);
                    yield return new Tuple<string[], int>(values, t.Item2);
                }
            }
        }
    }
}
