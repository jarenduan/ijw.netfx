using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ijw.IO {
    public static class BytesExt {
        public static void WriteToFile(this byte[] content, string filename, bool append = false) {
            content.WriteToFile(filename, Encoding.Unicode);
        }

        public static void WriteToFile(this byte[] content, string filename, Encoding encoding, bool append = false) {
            FileMode filemode = append ? FileMode.Append : FileMode.Create;
            FileStream file = new FileStream(filename, filemode);
            using (BinaryWriter writer = new BinaryWriter(file, encoding)) {
                writer.Write(content);
                writer.Flush();
            }
        }
    }
}
