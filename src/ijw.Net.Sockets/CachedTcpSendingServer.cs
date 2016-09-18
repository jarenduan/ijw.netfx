#if !NET35 && !NET40
using ijw.Collection;
using ijw.Diagnostic;
using ijw.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    /// <summary>
    /// C带有内部缓存的TCP数据发送服务器. 
    /// </summary>
    /// <typeparam name="T">发送数据的类型</typeparam>
    /// <remarks>
    /// 内部维护了数据缓冲池: 1)负责向池中追加对象 2)根据既定策略取出一条数据进行发送 3)成功发送后从池中删除该对象.
    /// </remarks>
    public class CachedTcpSendingServer<T> {
        /// <summary>
        /// 缓存中Item数量发生变更时会调用此委托.
        /// </summary>
        public Action<int> ItemCountChangedAction { get; set; }

        /// <summary>
        /// 负责完成将一个Item写入网络流中，并关闭流。默认实现是向流中写入item的ToString()。
                /// </summary>
        public Action<NetworkStream, T> WriteItemAndDisposeAction
        {
            get { return this._sender.WriteItemAndDisposeAction; }
            set { this._sender.WriteItemAndDisposeAction = value; }
        }

        /// <summary>
        /// 成功发送一个对象后会调用此委托
        /// </summary>
        public Action<T> ItemsSentAction { get; set; }

        /// <summary>
        /// 构造一个对象发送客户端
        /// </summary>
        /// <param name="getStratrgy">获取对象的策略</param>
        /// <param name="hostName">主机IP, 默认 127.0.0.1</param>
        /// <param name="portNum">端口号, 默认15210(obj三个字母的序号)</param>
        public CachedTcpSendingServer(string hostName = "127.0.0.1", int portNum = 15210, FetchingStrategies getStratrgy = FetchingStrategies.FirstFirst) {
            this._sender = new TcpSender<T>() { HostName = hostName, PortNum = portNum };
            this._ItemSent = new Progress<T>((obj) => {
                this.ItemsSentAction?.Invoke(obj);
            });

            this._dataPool.ItemGettngStrategy = getStratrgy;
            this._dataPool.ItemCountChanged += (o, e) => {
                this._ItemsCountChanged.Report(e.ItemCount);
            };
            this._ItemsCountChanged = new Progress<int>((count) => {
                this.ItemCountChangedAction?.Invoke(count);
            });

        }

        /// <summary>
        /// 追加数据项到发送缓存。
        /// </summary>
        /// <param name="item">要放入缓存的数据项</param>
        public void AppendingItem(T item) {
            Task.Run(() => {
                DebugHelper.WriteLine("Try to append item.");
                _dataPool.Append(item);
            });
        }

        /// <summary>
        /// 启动循环发送. 
        /// </summary>
        /// <remarks>
        /// 启动后，如果缓存中有数据, 进行TCP发送, 成功发送后从缓存中移除该数据. 
        /// 如果缓存中没有数据或者用户调用<see cref="SuspendSending"/>方法, 迭代将暂停, 等待通知信号后继续进行.
        /// </remarks>
        public async Task StartSendingAsync() {
            //初始化looper
            _bgLoopwork = new BackgroundLooper();
            _bgLoopwork.ExitCondition = () => false;
            _bgLoopwork.SleepCondition = () => !this._dataPool.HasAvailableItems;
            _bgLoopwork.LoopAction = async () => {
                //if(this.DataPool.Count >= 512) {
                //    BatchHandle();
                //}
                T item;
                if (_dataPool.TryBorrowAvailable(out item)) {
                    //有可发数据
                    if (await _sender.TrySendDataWithRetryAsync(item)) {
                        //发送成功
                        DebugHelper.WriteLine("Try removing item from pool.");
                        _dataPool.Remove(item);
                        _ItemSent.Report(item);
                    }
                    else {
                        //发送失败
                        _dataPool.Return(item);
                        this._bgLoopwork.WakeUpIfSleeping(); //归还item后，唤醒looper
                        throw new Exception($"Sending fail: has retry {_sender.MaxRetryTimes.ToString()} times. 有可能服务器未开启或者网络问题.");
                    }
                }
            };

            //开始looper
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
            this._bgLoopwork.Resume();
        }

        /// <summary>
        /// 停止发送
        /// </summary>
        public void StopSending() {
            this._bgLoopwork.Exit();
        }


        /// <summary>
        /// 批处理之前的剩余数据
        /// </summary>
        private void BatchHandle() {
            //TODO: implement;
            throw new NotImplementedException();
        }

        private IProgress<int> _ItemsCountChanged;
        private LongTimeConsumerCollection<T> _dataPool = new LongTimeConsumerCollection<T>();
        private BackgroundLooper _bgLoopwork = new BackgroundLooper();
        private TcpSender<T> _sender;
        private IProgress<T> _ItemSent;
    }
}
#endif