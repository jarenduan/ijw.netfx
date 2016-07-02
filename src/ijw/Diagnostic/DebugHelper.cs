using System;
using System.Diagnostics;
using System.Threading;

namespace ijw.Diagnostic {
    /// <summary>
    /// 提供对Debug的一系列Helper方法
    /// </summary>
    public static class DebugHelper {
        /// <summary>
        /// 对于NET35及以上的桌面平台，向输出窗口写入包含时间/线程id/调用类/指定字符串的debug信息
        /// 对于其他平台，相当于Debug.Write.
        /// </summary>
        /// <param name="message">输出信息</param>
        [Conditional("DEBUG")]
        public static void Write(string message) {
            Debug.Write(GetFormattedMessage(message));
        }

        /// <summary>
        /// 对于NET35及以上的桌面平台，向输出窗口写入包含时间/线程id/调用类/指定消息的debug信息, 并换行.
        /// 对于其他平台，相当于Debug.WriteLine.
        /// </summary>
        /// <param name="message">输出信息</param>
        [Conditional("DEBUG")]
        public static void WriteLine(string message) {
            Debug.WriteLine(GetFormattedMessage(message));
        }

        private static string GetFormattedMessage(string message) {
#if NET35 || NET40 || NET45
            StackTrace st = new StackTrace();
            var sf = st.GetFrame(2);
            //Debug.WriteLine(sf.GetMethod());
            Type t = sf.GetMethod().DeclaringType;
            string typeName = t.Name;
            string formatted = string.Format("[{3}][ThreadId: {0}][{1}]: {2}", Thread.CurrentThread.ManagedThreadId, typeName, message, DateTime.Now.ToLocalTime());
#else
            string formatted = message;
#endif
            return formatted;
        }

    }
}
