using System.IO;
using System.Text;

namespace ijw.IO {
    public class StreamWriterHelper {
        public static StreamWriter NewStreamWriterByFilepath(string filepath, Encoding encoding, bool append = false) {
            FileStream fs = new FileStream(filepath, append ? FileMode.Append : FileMode.Create);
            return new StreamWriter(fs, encoding);
        }

        public static StreamWriter NewStreamWriterByFilepath(string filepath) {
            return NewStreamWriterByFilepath(filepath, Encoding.Unicode);
        }
    }
}