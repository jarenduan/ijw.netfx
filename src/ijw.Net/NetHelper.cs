using System.Net;
using System.Net.NetworkInformation;
#if !NET35
using System.Threading.Tasks;
#endif

namespace ijw.Net {
    /// <summary>
    /// 网络帮助类
    /// </summary>
    public class NetHelper {
#if NET35 || NET40 || NET45
        /// <summary>
        /// Ping指定地址
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static PingReply Ping(string hostName, int timeout = 120) {
            Ping pingSender = new Ping();
            return pingSender.Send(hostName, timeout);
        }
#endif
#if NET45 || NETSTANDARD1_4
        /// <summary>
        /// Ping指定地址
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        public static async Task<PingReply> PingAsync(string hostName, int timeout = 120) {
            Ping pingSender = new Ping();
            return await pingSender.SendPingAsync(hostName, timeout);
        }
#endif

#if NET35 || NET40 || NET45
        /// <summary>
        /// 取本地第一个IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string GetFirstLocalIPV4() {
            var ips = Dns.GetHostAddresses(string.Empty);
            string localIP = string.Empty;
            foreach(var ip in ips) {
                if(ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                    localIP = ip.ToString();
                    var first = localIP.Split('.')[0];
                    if(first != "192" && first != "127" & first != "0") {
                        break;
                    }
                }
            }
            return localIP;
        }
#endif
#if NET45 || NETSTANDARD1_4
        /// <summary>
        /// 取本地第一个IPV4地址
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetFirstLocalIPV4Async() {
            var ips = await Dns.GetHostAddressesAsync(string.Empty);
            string localIP = string.Empty;
            foreach (var ip in ips) {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
                    localIP = ip.ToString();
                    var first = localIP.Split('.')[0];
                    if (first != "192" && first != "127" && first != "0") {
                        break;
                    }
                }
            }
            return localIP;
        }
#endif

    }
}
