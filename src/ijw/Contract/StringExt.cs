using System.IO;

namespace ijw.Contract {
    public static class StringExt
    {
        public static bool ShouldExistSuchFile(this string path) {
            FileInfo fi = new FileInfo(path);
            if (!fi.Exists) {
                throw new FileNotFoundException("File doesn't exist.", fi.FullName);
            }
            return true;
        }

        public static bool ShouldBeValidAbsoluteName(this string path) {
            var result = path.Length > 3 && path[1] == ':' && path[2] == '\\';
            if (!result) {
                throw new ContractBreakException($"{path} is not an absolute path.");
            }
            return path.ShouldExistSuchFile();
        }
    }
}
