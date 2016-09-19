using System.Text;

namespace ijw {
    /// <summary>
    /// 提供StringBuilder的扩展方法
    /// </summary>
    public static class StringBuilderExt {
        /// <summary>
        /// 移除尾部的指定字符串, 如果不符合将不更动。常用于更动字符串中的文件扩展名。
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="toRemove">指定的一系列字符串，如果尾部符合，将被移除</param>
        /// <returns>移除尾部指定字符串的结果</returns>
        public static void RemoveLast(this StringBuilder sb, params string[] toRemove) {
            foreach (var endString in toRemove) {
                if (sb.ToString().EndsWith(endString)) {
                    sb.RemoveLast(endString.Length);
                }
            }
        }

        /// <summary>
        /// 从后向前删除指定数量的字符
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="number">删除数量</param>
        public static void RemoveLast(this StringBuilder sb, int number = 1) {
            sb.Remove(sb.Length - number, number);
        }
    }
}
