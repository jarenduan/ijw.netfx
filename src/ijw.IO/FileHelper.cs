using System.IO;
using System.Text;

namespace ijw.IO {
    public class FileHelper {
        /// <summary>
        /// 使用Unicode编码向指定的文件写入字符串，可选追加或者创建/覆盖.
        /// </summary>
        /// <param name="filepath">写入文件</param>
        /// <param name="content">写入的内容</param>
        /// <param name="append">是否追加. true为追加, false为创建或覆盖</param>
        public static void WriteStringToFile(string filepath, string content, bool append = false) {
            WriteStringToFile(filepath, content, Encoding.Unicode, append);
        }

        /// <summary>
        /// 使用指定的编码向制定的文件写入字符串,可选追加或者创建/覆盖.
        /// </summary>
        /// <param name="filepath">写入文件</param>
        /// <param name="content">写入的内容</param>
        /// <param name="encoding">指定的编码方法</param>
        /// <param name="append">是否追加. true为追加, false为创建或覆盖</param>
        public static void WriteStringToFile(string filepath, string content, Encoding encoding, bool append = false) {
            using (StreamWriter writer = StreamWriterHelper.NewStreamWriterByFilepath(filepath, encoding, append)) {
                writer.Write(content);
            }
        }
               
       
        /// <summary>
        /// 按通配符拷贝多个文件.
        /// </summary>
        /// <param name="sourceDir">源文件夹</param>
        /// <param name="destDir">目标文件夹</param>
        /// <param name="pattern">通配符</param>
        /// <param name="copyOption">是否复制所有子目录中的文件</param>
        /// <param name="overwrite">是否覆盖, 设为false后遇到同名文件会抛出异常</param>
        /// <returns>字符串数组, 包含拷贝文件的源路径全名称</returns>
        public static string[] CopyFiles(string sourceDir, string destDir, string pattern = "*.*", SearchOption copyOption = SearchOption.TopDirectoryOnly, bool overwrite = true) {
            var files = Directory.GetFiles(sourceDir, pattern, copyOption);
            foreach (var f in files) {
                FileInfo fi = new FileInfo(f);
                File.Copy(f, Path.Combine(destDir, fi.Name), overwrite);
                //Console.WriteLine("  copy from " + fi.DirectoryName);
            }
            return files;
        }

        /// <summary>
        /// 删除指定目录的所有符合通配符的文件。
        /// </summary>
        /// <param name="dir">目录名</param>
        /// <param name="pattern">通配符</param>
        /// <returns>删除的文件数量</returns>
        public static int DeleteFiles(string dir, string pattern = "*.*") {
            var files = Directory.GetFiles(dir, pattern);
            int deleted = 0;
            foreach (var f in files) {
                try {
                    File.Delete(f);
                    deleted++;
                }
                catch{
                    continue;
                }
            }
            return deleted;
        }
    }
}
