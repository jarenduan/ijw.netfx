using ijw.Diagnostic;
using ijw.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    public class TcpReceivingServer<T> where T : class {
        public TcpReceivingServer(string hostName, int portNum) {
            this._receiver = new TcpReceiver<T>(hostName, portNum);
        }

        /// <summary>
        /// 服务器IP
        /// </summary>
        public IPAddress HostName => this._receiver.LocalAddress;

        /// <summary>
        /// 监听的端口
        /// </summary>
        public int PortNum => this._receiver.PortNum;

        /// <summary>
        /// 服务器是否停止监听
        /// </summary>
        public bool IsListenerRunning => this._isListenerRunning;

        /// <summary>
        /// 接收到流后，从流中解析取回数据项，并关闭流. 没有任何数据返回空. 有数据但解析发生错误应该抛出异常.
        /// </summary>
        public Func<NetworkStream, T> RetrieveItemAndDispose {
            get { return this._receiver.RetrieveItemAndDispose; }
            set { this._receiver.RetrieveItemAndDispose = value; }
        }
        public Action<T> ItemHandler { get; set; }

        /// <summary>
        /// 异步启动监听. 将在内部启动两个Task. 分别负责端口监听和事件激发.
        /// 可通过注册<see cref="ItemRecieved"/>事件来处理接收到的对象.
        /// </summary>
        /// <remarks>
        /// 未提供此方法的同步版本.
        /// </remarks>
        public async Task StartListeningAsync() {
            if (this._isListenerRunning) {
                DebugHelper.WriteLine("[Main] Server is already running.");
                _logger.WriteError("[Main] Server is already running.");
                return;
            }
            try {
                _receiver.Establish();
                //更改监听线程的状态 => 运行
                this._isListenerRunning = true;
                this._shouldContinueListen = true;
                //开始循环
                while (this._shouldContinueListen) {
                    T item = null;
                    try {
                        item = await _receiver.ReceiveData();
                        if (item != null) {
                            ItemHandler?.Invoke(item);
                        }
                        else {
                            DebugHelper.WriteLine("[Listener] Null item retrieved.");
                        }
                    }
                    catch {
                        DebugHelper.WriteLine("[Listener] Bad item or stop signal ");
                    }
                }
                DebugHelper.WriteLine("[Listener] Stopped.");
            }
            catch (Exception e) {
                DebugHelper.WriteLine(e.ToString());
                throw e;
            }
            finally {
                _receiver.Stop();
                this._isListenerRunning = false;
                this._shouldContinueListen = false;
            }
        }

        /// <summary>
        /// 停止服务器.
        /// 注意: Stop方法只停止了监听线程. 内部的通知处理线程仍会继续循环, 直至所有对象都被处理和移出集合.
        /// </summary>
        public void StopListening() {
            DebugHelper.WriteLine("[Listener] Got STOP signal.");
            if (!this._isListenerRunning) {
                DebugHelper.WriteLine("[Listener] Listener is not running.");
                return;
            }
            //发出停止信号
            DebugHelper.WriteLine("[Listener] Try to stop...");
            this._receiver.Stop();
            //设置监听令牌
            this._shouldContinueListen = false;
        }

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected LogHelper _logger = new LogHelper();
        /// <summary>
        /// 监听线程是否在运行
        /// </summary>
        protected bool _isListenerRunning = false;
        /// <summary>
        /// 是否应该继续监听
        /// </summary>
        protected bool _shouldContinueListen = false;

        protected TcpReceiver<T> _receiver;
    }
}
