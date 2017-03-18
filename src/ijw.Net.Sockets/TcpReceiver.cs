using ijw.Diagnostic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    public class TcpReceiver<T> where T : class {
        /// <summary>
        /// 欲监听的本地地址
        /// </summary>
        public IPAddress LocalAddress { get; private set; }

        /// <summary>
        /// 欲监听的端口号
        /// </summary>
        public int PortNum { get; private set; }

        /// <summary>
        /// 接收到流后，从流中解析取回数据项，并关闭流. 没有任何数据返回空. 有数据但解析发生错误应该抛出异常.
        /// </summary>
        public Func<NetworkStream, T> RetrieveItemAndDispose { get; set; }

        public event EventHandler RetrieveItemFailed;

        public TcpReceiver(string hostName, int portNum) {
            portNum.ShouldBeValidPortNumber();
            this.LocalAddress = IPAddress.Parse(hostName);
            this.PortNum = portNum;
        }

        /// <summary>
        /// 建立监听
        /// </summary>
        public void Establish() {
            this._listener = new TcpListener(this.LocalAddress, this.PortNum);
            this._listener.Start();
            DebugHelper.WriteLine("[Listener] New listener Started.");
        }

        /// <summary>
        /// 停止监听
        /// </summary>
        public void Stop() {
            sendFakeClient();
            if (this._listener != null) {
                this._listener.Stop();
                DebugHelper.WriteLine("[Listener] Listener stopped.");
                this._listener = null;
            }
        }
#if !NETSTANDARD1_4
        /// <summary>
        /// 获取客户端发送的数据，会一直阻塞, 直至接受到数据.
        /// </summary>
        public T ReceiveData() {
            TcpClient client = null;
            DebugHelper.WriteLine("[Listener] Waiting for TCP client...");
            client = _listener.AcceptTcpClient();
            try {

                var networkStream = client.GetStream();
                DebugHelper.WriteLine("[Listener] TCP client accepte, try retrieve data item...");
                T item = null;
                using (networkStream) {
                    item = this.RetrieveItemAndDispose?.Invoke(networkStream);
                }
                return item;
            }
            catch {
                this.RetrieveItemFailed.InvokeIfNotNull(this, null);
                throw;
            }
            finally {
                client.CloseIfNotNull();
            }
        }
#endif
#if !NET35 && !NET40

        //TODO: Add support to net35 and net40
        //There are only async version, considering add sync version for net35 and net40

        /// <summary>
        /// 获取客户端发送的数据.
        /// </summary>
        public async Task<T> ReceiveDataAsync() {
            TcpClient client = null;
            DebugHelper.WriteLine("[Listener] Waiting for TCP client...");
            client = await _listener.AcceptTcpClientAsync();
            try {
                
                var networkStream = client.GetStream();
                DebugHelper.WriteLine("[Listener] TCP client accepte, try retrieve data item...");
                T item = null;
                using (networkStream) {
                    item = this.RetrieveItemAndDispose?.Invoke(networkStream);
                }
                return item;
            }
            catch {
                this.RetrieveItemFailed.InvokeIfNotNull(this, null);
                throw;
            }
            finally {
                client.CloseIfNotNull();
            }
        }
#endif
        /// <summary>
        /// 开一个tcp连接, 防止监听线程处在阻塞之中.
        /// </summary>
        private void sendFakeClient() {
            DebugHelper.WriteLine("[Listener] Send fake client.");
            TcpClient client = null;
            try {
                client = new TcpClient(AddressFamily.InterNetwork);
#if NETSTANDARD1_4
                client.ConnectAsync(this.LocalAddress, this.PortNum).Wait();
#else
                client.Connect(this.LocalAddress, this.PortNum);
#endif
            }
            catch {
            }
            finally {
                client.CloseIfNotNull();
            }
        }

        /// <summary>
        /// 监听器
        /// </summary>
        protected TcpListener _listener;
    }
}
