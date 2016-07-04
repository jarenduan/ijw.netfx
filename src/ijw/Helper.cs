using System;
using ijw.Contract;

namespace ijw
{
    public static class Helper
       
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="length">总长度</param>
        /// <param name="startAtPython"></param>
        /// <param name="endAtPython"></param>
        /// <param name="startAt">启始索引. 该处字符将包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null等同于0. 默认值是0</param>
        /// <param name="endAt">结束索引. 该处字符将不包括在返回结果中. 0代表第一个字符, 负数代表倒数第几个字符(-1表示倒数第一个字符), null代表结尾. 默认值为null.</param>
        public static void PythonStartEndCalculator(int length, out int startAtPython, out int endAtPython, int? startAt = 0, int? endAt = null) {
            //endAt.ShouldNotEquals(0);

            if (startAt == null) {
                startAt = 0;
            }
            else if (startAt < 0) {
                startAt = length + startAt;
            }

            if (endAt == null) {
                endAt = length - 1;
            }
            else if (endAt < 0) {
                endAt = length + endAt - 1;
            }
            else {
                endAt--;
            }

            //if (startAt > endAt) {
            //    throw new Exception("start index > end index.");
            //}

            startAtPython = startAt.Value;
            endAtPython = endAt.Value;
        }
    }
}
