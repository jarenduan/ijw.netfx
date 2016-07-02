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
    }
}
