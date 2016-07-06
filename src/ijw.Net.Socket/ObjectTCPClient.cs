//TODO: support netstandard when netcore support binaryserialization
using ijw.Collection;
using ijw.Diagnostic;
using ijw.Log;
using ijw.Serialization.Binary;
using ijw.Threading.Tasks; 
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    /// <summary>
    /// 负责通过TCP发送对象到指定地址/端口.
    /// </summary>
    /// <typeparam name="T">发送对象的类型</typeparam>
    /// <remarks>
    /// ObjectTCPClient类内部使用了两个线程共同维护了一个线程安全的数据池.
    /// 数据追加线程通过负责向池中追加对象.
    /// 工作线程根据既定策略取出对象并进行发送, 发送后将负责从池中删除该对象.
    /// </remarks>
    public class ObjectTCPClient<T> {
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
        public int MaxRetryTimes { get; set; }

        
        /// <summary>
        /// 成功发送一个对象后会调用此委托
        /// </summary>
        public Action<T> ItemsSentAction { get; set; }

        /// <summary>s
        /// 数量发生变更时会调用此委托.
        /// </summary>
        public Action<int> ItemCountChangedAction { get; set; }

        /// <summary>
        /// 构造一个对象发送客户端
        /// </summary>
        /// <param name="getStratrgy">获取对象的策略</param>
        /// <param name="hostName">主机IP, 默认 127.0.0.1</param>
        /// <param name="portNum">端口号, 默认15210(obj三个字母的序号)</param>
        /// <param name="logOn">是否开启日志</param>
        public ObjectTCPClient(FetchStrategies getStratrgy, string hostName = "127.0.0.1", int portNum = 15210, bool logOn = true) {
            this._dataPool.ItemGettngStrategy = getStratrgy;
            this.HostName = hostName;
            this.PortNum = portNum;
            this.MaxRetryTimes = 5;
            this._dataPool.ItemCountChanged += DataPool_ItemCountChanged;
            this._ItemsCountChanged = new Progress<int>( (count) => {
                this.ItemCountChangedAction?.Invoke(count);
            });
            this._ItemSent = new Progress<T>((obj) => {
                this.ItemsSentAction?.Invoke(obj);
            });
            if(logOn) {
                this._logger = new LogHelper();
            }
        }

        private void DataPool_ItemCountChanged(object sender, ItemCountChangedEventArgs e) {
            this._ItemsCountChanged.Report(e.ItemCount);
        }

        /// <summary>
        /// 将欲发送的对象放入数据池, 并通知发送操作所在线程继续.
        /// </summary>
        /// <param name="data"></param>
        public void PuttingDataAndNotifySending(T data) {
            Task.Run(() => {
                DebugHelper.WriteLine("Try to append data.");
                this._dataPool.Append(data);
                DebugHelper.WriteLine("Wake up the loop.");
                this.ContinueSendingIfWaiting();
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
                T curr;
                if (this._dataPool.TryGetItem(out curr)) {
                    DebugHelper.WriteLine("Start sending object.");
                    int i = 1;
                    while (i < MaxRetryTimes) {
                        if (i > 1) {
                            DebugHelper.WriteLine("Transfering error detected.");
                            DebugHelper.WriteLine("Waiting 1 second to try again.");
                            System.Threading.Thread.Sleep(1000);
                            DebugHelper.WriteLine("Try sending again.");
                        }

                        if (sendData(curr)) {
                            DebugHelper.WriteLine("Start removing object.");
                            _dataPool.Remove(curr);
                            this._ItemSent.Report(curr);
                            return;
                        }
                        i++;
                    }
                    _dataPool.Return(curr);
                    throw new Exception(string.Format("Sending fail: has retry {0} times. 有可能服务器未开启或者网络问题.", MaxRetryTimes));
                }
            };
            await _bgLoopwork.StartAsync();
        }

        /// <summary>
        /// 批处理之前的剩余数据
        /// </summary>
        private void BatchHandle() {
            throw new NotImplementedException();
        }

        private bool sendData(T obj) {
            TcpClient client = null;
            try {
                client = new TcpClient(HostName, PortNum);
                System.Threading.Thread.Sleep(_CLIENT_CONNECTION_TIME_WAIT);
            }
#pragma warning disable 168
            catch {
                DebugHelper.WriteLine("无法连接, 可能是网络问题/服务器没有开启/地址端口不对!");
                client?.Close();
                return false;
            }
#pragma warning restore 168
            DebugHelper.WriteLine("Tcp connected.");
            try {
                NetworkStream ns = client.GetStream();
                ns.WriteBinaryObjectAndDispose(obj);
                DebugHelper.WriteLine("Object transfered.");
                _logger.WriteInfo("数据发送成功.");
            }
#pragma warning disable 168
            catch {
                _logger.WriteError("数据发送失败.");
                client?.Close();
                DebugHelper.WriteLine("Tcp closed.");
                return false;
            }
#pragma warning restore 168
            client?.Close();
            DebugHelper.WriteLine("Tcp closed.");
            return true;
        }

        /// <summary>
        /// 停止发送,结束任务.
        /// </summary>
        public void ExitSending() {
            this._bgLoopwork.Exit();
        }

        /// <summary>
        /// 通知发送器从等待中恢复, 继续取对象数据并进行发送.
        /// </summary>
        protected void ContinueSendingIfWaiting() {
            this._bgLoopwork.ContinueIfWaiting();
        }

        private LongTimeConsumerCollection<T> _dataPool = new LongTimeConsumerCollection<T>();
        private BackgroundLooper _bgLoopwork = new BackgroundLooper();
        private IProgress<int> _ItemsCountChanged;
        private LogHelper _logger;
        private const int _CLIENT_CONNECTION_TIME_WAIT = 10;
        private IProgress<T> _ItemSent;
    }
}
