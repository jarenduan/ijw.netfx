using System.IO;
using System.Text;

namespace ijw.IO {
    public class StreamReaderHelper {
        /// <summary>
        /// 使用Unicode字符集打开指定文件。请使用using。
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>流读取器</returns>
        public static StreamReader NewStreamReaderFrom(string filepath) {
            return NewStreamReaderFrom(filepath, Encoding.Unicode);
        }

        /// <summary>
        /// 使用指定的编码方式打开指定文件。请使用using。
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="encoding">文本编码方式</param>
        /// <returns></returns>
        public static StreamReader NewStreamReaderFrom(string filepath, Encoding encoding) {
            FileStream fs = new FileStream(filepath, FileMode.Open);
            return new StreamReader(fs, encoding);
        }
    }
}
