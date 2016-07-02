namespace ijw {
    /// <summary>
    /// 提供对Double类型的若干扩展方法
    /// </summary>
    public static class DoubleExt {
        /// <summary>
        /// 使用指定的最大值/最小值进行归一化
        /// </summary>
        /// <param name="x"></param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns>归一化后的值</returns>
        public static double NormalizeMaxMin(this double x, double min, double max, double minOut = 0.1, double maxOut = 0.9) {
            return (x - min) / (max - min) * (maxOut - minOut) + minOut;
        }

        /// <summary>
        /// 使用指定的最大值/最小值进行反归一化
        /// </summary>
        /// <param name="x"></param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns></returns>
        public static double DenormalizeMaxMin(this double x, double min, double max, double minOut = 0.1, double maxOut = 0.9) {
            return (x - minOut) / (maxOut - minOut) * (max - min) + min;
        }
    }
}
