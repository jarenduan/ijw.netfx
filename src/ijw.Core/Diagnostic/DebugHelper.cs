using System;
using System.Diagnostics;
using System.Threading;
#if !(NET35 || NET40)
    using System.Runtime.CompilerServices;
#endif

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

#if !(NET35 || NET40)
        private static string GetFormattedMessage(string message, [CallerMemberName] string methodName = "") {
            return getFormattedMessage(methodName, message);  
        } 
#else
        private static string GetFormattedMessage(string message) {
            var name = new StackTrace().GetFrame(2).GetMethod().DeclaringType.Name;
            return getFormattedMessage(name, message);
        }
#endif
        private static string getFormattedMessage(string name, string message) {
            string formatted = string.Format("[{3}][ThreadId: {0}][{1}]: {2}", Thread.CurrentThread.ManagedThreadId, name, message, DateTime.Now.ToLocalTime());
            return formatted;
        }
    }
}
