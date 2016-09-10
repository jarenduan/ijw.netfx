using ijw.Diagnostic;
using ijw.IO;
using ijw.Log;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    public class TcpSender<T> {
        /// <summary>
        /// 服务器端的端口号
        /// </summary>
        public int PortNum { get; set; }

        /// <summary>
        /// 服务器的IP地址
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 发送某个对象遇到错误时的最大尝试次数
        /// </summary>
        public int MaxRetryTimes { get; set; } = 4;

        /// <summary>
        /// 重试发送的间隔，单位是毫秒。
        /// </summary>
        public TimeSpan RetryInterval { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// 负责完成将一个Item写入网络流中，并关闭流。默认实现是向流中写入item的ToString()。
        /// </summary>
        public Action<NetworkStream, T> WriteItemAndDisposeAction = (s, i) => s.WriteStringAndDispose(i.ToString());

#if !NETSTANDARD1_4
        /// <summary>
        /// 发送一条数据，尝试重试
        /// </summary>
        /// <param name="item">要发送的数据</param>
        /// <returns>发送成功，返回真。尝试制定次数仍然发送失败，返回假</returns>

        public bool TrySendDataWithRetry(T item) {
            int i = 1;
            while (i <= this.MaxRetryTimes) {
                if (i > 1) {
                    DebugHelper.WriteLine($"The {i.ToOrdinalString()} retrying failed, wait {this.RetryInterval.TotalSeconds} seconds...");
                    Thread.Sleep(this.RetryInterval);
                }
                bool sentSuccessfully = TrySendData(item);
                if (sentSuccessfully) return true;
                i++;
            }
            return false;
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="item">要发送的数据</param>
        /// <returns>发送成功，返回真。发送失败，返回假</returns>

        public bool TrySendData(T item) {
            DebugHelper.WriteLine("Start sending item...");
            bool isConnected = tryConnect();
            if (!isConnected) {
                return false;
            }
            else {
                bool hasSent = trySendData(item);
                if (hasSent) DebugHelper.WriteLine("Item Sent.");
                return hasSent;
            }
        }

        private bool tryConnect() {
            try {
                _client = new TcpClient(AddressFamily.InterNetwork);
                DebugHelper.WriteLine($"Try connecting {this.HostName}:{this.PortNum}...");
                _client.Connect(this.HostName, this.PortNum);
                Thread.Sleep(_CLIENT_CONNECTION_TIME_WAIT);
                DebugHelper.WriteLine($"{this.HostName}:{this.PortNum} connected.");
                return true;
            }
#pragma warning disable 168
            catch (Exception ex) {
                _client.CloseIfNotNull();
                DebugHelper.WriteLine($"Cannot connect {this.HostName}:{this.PortNum}!");
                _logger?.WriteError("无法连接, 可能是网络问题/服务器没有开启/地址端口不对! Error:" + ex.Message);
                return false;
            }
#pragma warning restore 168
        }
#endif
#if !NET35
        /// <summary>
        /// 发送一条数据，尝试重试
        /// </summary>
        /// <param name="item">要发送的数据</param>
        /// <returns>发送成功，返回真。尝试制定次数仍然发送失败，返回假</returns>
        public async Task<bool> TrySendDataWithRetryAsync(T item) {
            int i = 1;
            while (i <= this.MaxRetryTimes) {
                if (i > 1) {
                    DebugHelper.WriteLine($"The {i.ToOrdinalString()} retrying failed, wait {this.RetryInterval.TotalSeconds} seconds...");
                    Thread.Sleep(this.RetryInterval);
                }
                bool sentSuccessfully = await TrySendDataAsync(item);
                if (sentSuccessfully) return true;
                i++;
            }
            return false;
        }

        /// <summary>
        /// 发送一条数据
        /// </summary>
        /// <param name="item">要发送的数据</param>
        /// <returns>发送成功，返回真。发送失败，返回假</returns>
        public async Task<bool> TrySendDataAsync(T item) {
            DebugHelper.WriteLine("Start sending item...");
            bool isConnected = await tryConnectAsync();
            if (!isConnected) {
                return false;
            }
            else {
                bool hasSent = trySendData(item);
                if (hasSent) DebugHelper.WriteLine("Item Sent.");
                return hasSent;
            }
        }

        private async Task<bool> tryConnectAsync() {
            try {
                _client = new TcpClient(AddressFamily.InterNetwork);
                DebugHelper.WriteLine($"Try connecting {this.HostName}:{this.PortNum}...");
                await _client.ConnectAsync(this.HostName, this.PortNum);
                Thread.Sleep(_CLIENT_CONNECTION_TIME_WAIT);
                DebugHelper.WriteLine($"{this.HostName}:{this.PortNum} connected.");
                return true;
            }
#pragma warning disable 168
            catch (Exception ex){
                _client.CloseIfNotNull();
                DebugHelper.WriteLine($"Cannot connect {this.HostName}:{this.PortNum}!");
                _logger?.WriteError("无法连接, 可能是网络问题/服务器没有开启/地址端口不对! Error:" + ex.Message);
                return false;
            }
#pragma warning restore 168
        }

#endif
        private bool trySendData(T item) {
            try {
                using (NetworkStream ns = _client.GetStream()) {
                    this.WriteItemAndDisposeAction?.Invoke(ns, item);
                }
                _logger?.WriteInfo("数据发送成功:" + item.ToString());
                return true;
            }
#pragma warning disable 168
            catch {
                _logger?.WriteError("数据发送失败.");
                return false;
            }
            finally {
                _client.CloseIfNotNull();
            }
#pragma warning restore 168
        }

        protected TcpClient _client;
        protected LogHelper _logger;
        protected const int _CLIENT_CONNECTION_TIME_WAIT = 10;
    }
}
