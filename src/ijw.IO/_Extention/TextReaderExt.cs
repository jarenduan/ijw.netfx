using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ijw.IO {
    /// <summary>
    /// 文本/字符流的扩展方法
    /// </summary>
    public static class TextReaderExt {
        /// <summary>
        /// 扩展方法, 读取流中的一行
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>行集合</returns>
        public static IEnumerable<string> ReadLines(this TextReader reader) {
            string line = reader.ReadLine();
            while (null != line) {
                yield return line;
                line = reader.ReadLine();
            }
        }
       
    }
}
