using ijw.Collection;
using ijw.Diagnostic;
using ijw.IO;
using ijw.Log;
using ijw.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    /// <summary>
    /// CachedTcpClient&lt;T&gt;实现了一个带有内部缓存的TCP数据发送客户端. 
    /// </summary>
    /// <typeparam name="T">发送数据的类型</typeparam>
    /// <remarks>
    /// 内部维护了数据缓冲池.1)负责向池中追加对象. 2)根据既定策略取出一条数据进行发送 3)成功发送后从池中删除该对象.
    /// </remarks>
    public class CachedTcpClient<T> {
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
        public int MaxRetryTimes { get; set; } = 5;

        /// <summary>
        /// 成功发送一个对象后会调用此委托
        /// </summary>
        public Action<T> ItemsSentAction { get; set; }

        /// <summary>
        /// Item数量发生变更时会调用此委托.
        /// </summary>
        public Action<int> ItemCountChangedAction { get; set; }

        /// <summary>
        /// 负责完成将一个Item写入网络流中，并关闭流。默认实现是向流中写入item的ToString()。
        /// </summary>
        public Action<NetworkStream, T> WriteItemAndDisposeAction = (s, i) => s.WriteStringAndDispose(i.ToString());

        /// <summary>
        /// 构造一个对象发送客户端
        /// </summary>
        /// <param name="getStratrgy">获取对象的策略</param>
        /// <param name="hostName">主机IP, 默认 127.0.0.1</param>
        /// <param name="portNum">端口号, 默认15210(obj三个字母的序号)</param>
        /// <param name="logOn">是否开启日志</param>
        public CachedTcpClient(FetchStrategies getStratrgy, string hostName = "127.0.0.1", int portNum = 15210, bool logOn = true) {
            this._dataPool.ItemGettngStrategy = getStratrgy;
            this.HostName = hostName;
            this.PortNum = portNum;
            this._dataPool.ItemCountChanged += (o, e) => {
                this._ItemsCountChanged.Report(e.ItemCount);
            };
            this._ItemsCountChanged = new Progress<int>((count) => {
                this.ItemCountChangedAction?.Invoke(count);
            });
            this._ItemSent = new Progress<T>((obj) => {
                this.ItemsSentAction?.Invoke(obj);
            });
            if (logOn) {
                this._logger = new LogHelper();
            }
        }

        /// <summary>
        /// 追加数据项到缓存中
        /// </summary>
        /// <param name="item">要放入缓存的数据项</param>
        /// <param name="isResume">是否立即恢复发送</param>
        public void AppendingItem(T item, bool isResume = false) {
            Task.Run(() => {
                DebugHelper.WriteLine("Try to append item.");
                _dataPool.Append(item);
                if (isResume) {
                    ResumeSending();
                }
            });
        }

        /// <summary>
        /// 启动发送. 将在后台的工作线程中循环执行.
        /// 每次迭代时将会检查StopCondition, 如果为true, 将退出循环, 发送操作结束.
        /// 每次迭代时如果数据池中有数据, 将会进行TCP发送, 发送后会从数据池中移除该数据. 
        /// 如果没有数据, 迭代将暂停, 等待通知信号后继续进行.
        /// </summary>
        public async Task StartSendingAsync() {
            _bgLoopwork = new BackgroundLooper();
            _bgLoopwork.StopCondition = () => false;
            _bgLoopwork.WaitCondition = () => !this._dataPool.HasItem;
            _bgLoopwork.LoopAction = () => {
                //if(this.DataPool.Count >= 512) {
                //    BatchHandle();
                //}
                T item;
                if (_dataPool.TryBorrow(out item)) {
                    DebugHelper.WriteLine("Start sending item...");
                    int i = 1;
                    while (i < MaxRetryTimes) {
                        if (i > 1) {
                            DebugHelper.WriteLine("Sending error detected. Wait 1 second to try again.");
                            Thread.Sleep(1000);
                            DebugHelper.WriteLine("Try sending again...");
                        }

                        if (sendData(item)) {
                            DebugHelper.WriteLine("Item Sent. Try removing item from pool.");
                            _dataPool.Remove(item);
                            _ItemSent.Report(item);
                            _logger?.WriteInfo(item.ToString() + " is sent");
                            return;
                        }
                        i++;
                    }
                    _dataPool.Return(item);
                    throw new Exception(string.Format("Sending fail: has retry {0} times. 有可能服务器未开启或者网络问题.", MaxRetryTimes));
                }
            };
            await _bgLoopwork.StartAsync();
        }

        /// <summary>
        /// 暂停发送，可用<see cref="ResumeSending"/>恢复。
        /// </summary>
        public void SuspendSending() {
            this._bgLoopwork.Suspend();
        }

        /// <summary>
        /// 恢复发送.
        /// </summary>
        public void ResumeSending() {
            DebugHelper.WriteLine("Wake up the loop.");
            this._bgLoopwork.Resume();
        }

        /// <summary>
        /// 停止发送
        /// </summary>
        public void StopSending() {
            this._bgLoopwork.Exit();
        }

        //TODO: change to await
        private bool sendData(T item) {
            TcpClient client = null;
            try {
                client = new TcpClient(HostName, PortNum);
                Thread.Sleep(_CLIENT_CONNECTION_TIME_WAIT);
            }
#pragma warning disable 168
            catch {
                close(client);
                DebugHelper.WriteLine("无法连接, 可能是网络问题/服务器没有开启/地址端口不对!");
                _logger?.WriteError("无法连接, 可能是网络问题/服务器没有开启/地址端口不对!");
                return false;
            }
#pragma warning restore 168
            DebugHelper.WriteLine("Tcp connected.");
            try {
                using (NetworkStream ns = client.GetStream()) { 
                    this.WriteItemAndDisposeAction?.Invoke(ns, item);
                }
                _logger?.WriteInfo("数据发送成功:" + item.ToString());
            }
#pragma warning disable 168
            catch {
                close(client);
                _logger?.WriteError("数据发送失败.");
                return false;
            }
#pragma warning restore 168
            close(client);
            return true;
        }

        /// <summary>
        /// 批处理之前的剩余数据
        /// </summary>
        private void BatchHandle() {
            //TODO: implement;
            throw new NotImplementedException();
        }

        private void close(TcpClient client) {
            if (client != null) {
                client.Close();
                DebugHelper.WriteLine("Tcp closed.");
            }
        }

        private const int _CLIENT_CONNECTION_TIME_WAIT = 10;
        private LongTimeConsumerCollection<T> _dataPool = new LongTimeConsumerCollection<T>();
        private BackgroundLooper _bgLoopwork = new BackgroundLooper();
        private LogHelper _logger;
        private IProgress<int> _ItemsCountChanged;
        private IProgress<T> _ItemSent;
    }
}
