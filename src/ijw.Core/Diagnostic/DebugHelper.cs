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
        /// 向输出窗口写入包含时间/线程id/调用类(或方法名)/指定消息的debug信息, 并换行.
        /// </summary>
        /// <param name="message">输出信息</param>
        [Conditional("DEBUG")]
#if (NET35 || NET40)
        public static void Write(string message, string callerName = "") {
            if (callerName == "") callerName = GetCallerName();
#else
        public static void Write(string message, [CallerMemberName] string callerName = "") {
#endif
            string info = getFormattedMessage(callerName, message);
            Debug.Write(info);
        }

        /// <summary>
        /// 向输出窗口写入包含时间/线程id/调用类(或方法名)/指定消息的debug信息, 并换行.
        /// </summary>
        /// <param name="message">输出信息</param>
        [Conditional("DEBUG")]
#if (NET35 || NET40)
        public static void WriteLine(string message, string callerName = "") {
            if (callerName == "") callerName = GetCallerName();
#else
        public static void WriteLine(string message, [CallerMemberName] string callerName = "") {
#endif
            string info = getFormattedMessage(callerName, message);
            Debug.WriteLine(info);
        }

#if !NETSTANDARD1_4
        /// <summary>
        /// 获取调用者的方法名称。注意:获取的是调用GetCallerName()所在方法的方法的名称。
        /// </summary>
        /// <returns>返回方法名称，形式如：类型名.方法名称。</returns>
        public static string GetCallerName() {
            var mi = new StackTrace().GetFrame(2).GetMethod();
            return $"{mi.DeclaringType.Name}.{mi.Name}";
        }

        /// <summary>
        /// 获取调用者的方法信息。注意:获取的是调用GetCallerName()所在方法的方法的信息。
        /// </summary>
        /// <returns>返回方法信息。</returns>
        public static System.Reflection.MethodBase GetCallerMethod() {
            return new StackTrace().GetFrame(2).GetMethod();
        }
#endif

        private static string getFormattedMessage(string name, string message) {
            return $"[{DateTime.Now.ToLocalTime()}][ThreadId: {Thread.CurrentThread.ManagedThreadId}][{name}]: {message}";
        }
    }
}
