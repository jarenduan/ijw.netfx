namespace ijw {
    /// <summary>
    /// Long型的扩展方法
    /// </summary>
    public static class LongExt {
        /// <summary>
        /// 转换成序数词字符串
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToOrdinalString(this long number) {
            return number.ToString().AppendOrdinalPostfix();
        }
    }
}
