using System;
using ijw.Contract;

namespace ijw
{
    public static class Helper
       
    {
        /// <summary>
        /// 获取python风格的起止索引值
        /// </summary>
        /// <param name="length">总长度</param>
        /// <param name="start">计算得到的C#风格的起始索引</param>
        /// <param name="end">计算得到的C#风格的结束索引</param>
        /// <param name="startAtPython">启始索引. 该处字符将包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null等同于0. 默认值是0</param>
        /// <param name="endAtPython">结束索引. 该处字符将不包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null代表结尾. 默认值为null.</param>
        public static void PythonStartEndCalculator(int length, out int start, out int end, int? startAtPython = 0, int? endAtPython = null) {
            //endAt.ShouldNotEquals(0);

            if (startAtPython == null) {
                startAtPython = 0;
            }
            else if (startAtPython < 0) {
                startAtPython = length + startAtPython;
            }

            if (endAtPython == null) {
                endAtPython = length - 1;
            }
            else if (endAtPython < 0) {
                endAtPython = length + endAtPython - 1;
            }
            else {
                endAtPython--;
            }

            //if (startAt > endAt) {
            //    throw new Exception("start index > end index.");
            //}

            start = startAtPython.Value;
            end = endAtPython.Value;
        }
    }
}
