using System.Text;

namespace ijw.Text {
    /// <summary>
    /// 编码API的辅助类
    /// </summary>
    public static class EncodingHelper {
        /// <summary>
        /// GB2312 编码
        /// </summary>
        public static Encoding GB2312 => Encoding.GetEncoding("GB2312");
    }
}
