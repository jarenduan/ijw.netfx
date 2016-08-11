using System.IO;
using System.Text;

namespace ijw.IO {
    public class StreamReaderHelper {
        public static StreamReader NewStreamReader(string filepath) {
            return NewStreamReader(filepath, Encoding.UTF8);
        }
        public static StreamReader NewStreamReader(string filepath, Encoding encoding) {
            FileStream fs = new FileStream(filepath, FileMode.Open);
            return new StreamReader(fs, encoding);
        }
    }
}
