using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Text {
    public static class StringExt {
        public static byte[] ToBytes(this string aString, Encoding encoding) {
            return encoding.GetBytes(aString);
        }

        /// <summary>
        /// 使用UTF8编码转换成字节数组
        /// </summary>
        /// <param name="aString"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this string aString) {
            return aString.ToBytes(Encoding.Unicode);
        }


    }
}
