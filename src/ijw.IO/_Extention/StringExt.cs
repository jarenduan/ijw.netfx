using System.Text;

namespace ijw.IO {
    public static class StringExt {
        /// <summary>
        /// 使用指定编码将字符串写入指定文本文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filepath">写入的文件</param>
        /// <param name="encoding">写入使用的编码方式</param>
        /// <param name="append">是否追加, true追加, false新建或覆盖</param>
        public static void WriteToFile(this string content, string filepath, Encoding encoding, bool append = false) {
            FileHelper.WriteStringToFile(filepath, content, encoding, append);
        }

        /// <summary>
        /// 使用Unicode编码将字符串写入指定文本文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filepath">写入的文件</param>
        /// <param name="append">是否追加, true追加, false新建或覆盖</param>
        public static void WriteToFile(this string content, string filepath, bool append = false) {
            FileHelper.WriteStringToFile(filepath, content, append);
        }

    }
}
