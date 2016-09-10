using ijw.Diagnostic;
using System.Net.Sockets;

namespace ijw.Net.Socket {
    /// <summary>
    /// 提供对TcpClient的若干扩展方法
    /// </summary>
    public static class TcpClientExt {
        public static void CloseIfNotNull(this TcpClient c) {
#if NETSTANDARD1_4
            c?.Dispose();
#else
            c?.Close();
#endif
            DebugHelper.WriteLine("Tcp client closed.");
        }
        /// <summary>
        /// 检查是否在线
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsOnline(this TcpClient c) {
            return !(!c.Client.Connected || (c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)));
        }
    }
}
