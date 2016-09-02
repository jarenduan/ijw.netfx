using System.IO;
using System.Text;

namespace ijw.IO {
    public class StreamReaderHelper {
        public static StreamReader NewStreamReaderFromFilepath(string filepath) {
            return NewStreamReaderFromFilepath(filepath, Encoding.UTF8);
        }
        public static StreamReader NewStreamReaderFromFilepath(string filepath, Encoding encoding) {
            FileStream fs = new FileStream(filepath, FileMode.Open);
            return new StreamReader(fs, encoding);
        }
    }
}
