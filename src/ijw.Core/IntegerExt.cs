using System;

namespace ijw {
    /// <summary>
    /// 提供对Integer类型的若干扩展方法
    /// </summary>
    public static class IntegerExt {
        /// <summary>
        /// 返回由当前数字开始直到指定数字所组成的递增整数数组
        /// </summary>
        /// <param name="number">当前的数字</param>
        /// <param name="toNumber">结束的数字</param>
        /// <returns></returns>
        public static int[] To(this int number, int toNumber) {
            if (number >= toNumber) {
                throw new ArgumentException("toNumber should be larger.");
            }
            int count = toNumber - number;
            int[] seq = new int[count];

            for (int i = 0; i < count; i++) {
                seq[i] = i + number;
            }
            return seq;
        }

        /// <summary>
        /// 返回由当前数字及指定数目的后续整数一起所组成的递增整数数组. 例如: 11.ToNext(5) 将返回 [11, 12, 13, 14, 15, 16].
        /// </summary>
        /// <param name="number"></param>
        /// <param name="howManyNext"></param>
        /// <returns></returns>
        public static int[] ToNext(this int number, int howManyNext) {
            return number.To(number + howManyNext);
        }

        /// <summary>
        /// 返回从当前数字开始的指定长度的递增整数数组. 例如: 11.ToNext(5) 将返回 [11, 12, 13, 14, 15].
        /// </summary>
        /// <param name="number"></param>
        /// <param name="totalLength"></param>
        /// <returns></returns>
        public static int[] ToTotal(this int number, int totalLength) {
            return number.To(number + totalLength - 1);
        }

        /// <summary>
        /// 幂计算
        /// </summary>
        /// <param name="number"></param>
        /// <param name="power">幂</param>
        /// <returns></returns>
        public static int Pow(this int number, int power) {
            return (int)Math.Pow(number, power);
        }

        /// <summary>
        /// 将整数变成序数字符串, 比如 1.ToOrdinalString() 生成字符串: "1st".
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToOrdinalString(this int number) {
            return number.ToString().AppendOrdinalPostfix();
        }

        /// <summary>
        /// 反复运行委托一定次数
        /// </summary>
        /// <param name="times"></param>
        /// <param name="loopBody"></param>
        public static void Times(this int times, Action loopBody) {
            for (int i = 0; i < times; i++) {
                loopBody();
            }
        }

        /// <summary>
        /// 字符串重复指定次数
        /// </summary>
        /// <param name="times"></param>
        /// <param name="aString">欲重复的字符串</param>
        /// <returns>重复之后的新字符串</returns>
        public static string Times(this int times, string aString) {
            if (times <= 0) {
                throw new ArgumentOutOfRangeException();
            }
            return aString.Repeat(times);
        }
    }
}