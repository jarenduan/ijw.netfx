using ijw.Collection;
using ijw.Diagnostic;
using ijw.Log;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Net.Socket {
    /// <summary>
    /// CachedTcpServer&lt;T&gt;实现了一个泛型对象TCP接收/通知处理服务器. 
    /// 调用者可通过指定ItemRecievedHandlerAsync委托, 方便地以异步的方式处理新接收到的对象.
    /// 调用者也可以通过挂接事件处理器, 方便地同步处理接收到的对象.
    /// </summary>
    /// <typeparam name="T">获取对象的类型</typeparam>
    /// <remarks>
    /// 调用StartAsync()方法, 内部启动了两个Task. 一个是负责监听TCP端口. 另一个负责发出通知事件.
    /// 两个Task共同维护了内部的一个线程安全的泛型集合. 
    /// </remarks>
    public class CachedTcpRecievingServer<T> : IDisposable where T: class {
        /// <summary>
        /// 服务器IP
        /// </summary>
        public IPAddress HostName => this._server.HostName;

        /// <summary>
        /// 监听的端口
        /// </summary>
        public int PortNum => this._server.PortNum;

        /// <summary>
        /// 服务器是否停止监听
        /// </summary>
        public bool IsListening => this._server.IsListenerRunning;

        public bool IsNotifying => this._isNotifierRunning;

        /// <summary>
        /// 接收到流后，从流中解析取回数据项，并关闭流. 没有任何数据返回空. 有数据但解析发生错误应该抛出异常.
        /// </summary>
        public Func<NetworkStream, T> RetrieveItemAndDispose {
            get { return this._server.RetrieveItemAndDispose; }
            set { this._server.RetrieveItemAndDispose = value; }
        }

        /// <summary>
        /// 初始化服务器实例
        /// </summary>
        /// <param name="ifSupportUIThreading">是否对UI线程操作提供支持. true 则每次接收到对象将会封送至ItemHandlerAsync所在线程进而可以更改控件. </param>
        public CachedTcpRecievingServer(string hostName, int portNum, bool ifSupportUIThreading = false) {
            this._server = new TcpReceivingServer<T>(hostName, portNum);
            this._server.ItemHandler = (item) => {
                this._dataPool.Append(item);
                DebugHelper.WriteLine("[Listener] A data item appended.");
                wake_notifyLoop();
            };
            this._withUIThreadingSupport = ifSupportUIThreading;
            if (this._withUIThreadingSupport) {
                //注: 此种方式调用就可以进行异步操作了, 弥补了同步事件的不足
                this._itemAppendedProgess = new Progress<T>(async (item) => {
                    await invokeItemRecievedAsync(item);
                });
            }
        }

        /// <summary>
        /// 接收到对象后进行调用的委托, 此委托将会异步执行.
        /// 注意: 即使调用Stop方法停止了接收, 但是如果对象池仍有对象没有被处理, 此委托仍会被调用, 直至所有对象被成功处理.
        /// </summary>
        public Func<T, Task> ItemRecievedHandlerAsync { get; set; }

        /// <summary>
        /// 服务器停止后, 且全部对象被成功处理后, 激活此事件.
        /// </summary>
        public event EventHandler AllItemsHandled;

        /// <summary>
        /// 启动服务器
        /// 此操作将在内部启动两个Task. 分别负责端口监听和事件激发.
        /// 启动监听后, 代码会立即返回. 
        /// 可通过注册<see cref="ItemRecieved"/>事件来处理接收到的对象.
        /// </summary>
        public async Task StartListeningAsync() {
            startOrResumeNotifyLoop();
            await this._server.StartListeningAsync();
            wake_notifyLoop();
        }

        /// <summary>
        /// 停止监听. 内部仍继续通知处理, 直至所有对象都被处理和移出集合.
        /// </summary>
        public void StopListening() {
            this._server.StopListening();
        }

        public void TryStopServer() {
            lock (this._notifierSyncRoot) {
                //设置通知令牌
                this._shouldContinueNotify = false;
            }
        }

        #region Internal logic

        /// <summary>
        /// 启动通知线程
        /// </summary>
        private void startOrResumeNotifyLoop() {
            lock (this._notifierSyncRoot) {
                if (this._isNotifierRunning) {
                    //notifyLoop在运行, 通知应该继续循环.
                    DebugHelper.WriteLine("[Main] Current notifier should continue.");
                    //stopListening方法有可能将_shouldContinueNotify设为false. 重设为true会保证notifyLoop即使在最后时刻也可以恢复循环.
                    this._shouldContinueNotify = true;
                }
                else {
                    //如果notifyloop没在运行, 开始新线程进行新的loop.
                    DebugHelper.WriteLine("[Main] Should start a new notifier.");
                    Task.Run(() => {
                        DebugHelper.WriteLine("[Notifier] A new notifier started.");
                        try {
                            notifyLoop();
                        }
                        catch (Exception ex) {
                            DebugHelper.WriteLine(ex.ToString());
                            this._isNotifierRunning = false;
                            throw;
                        }
                    });
                }
            }
        }
        /// <summary>
        /// 通知循环. 有未处理或者新接到的数据, 会激发事件处理器. 没有的话暂停循环.
        /// </summary>
        private void notifyLoop() {
            //设置一下循环状态
            this._isNotifierRunning = true;
            this._shouldContinueNotify = true;

            //开始循环
            while (true) {
                //如果发现对象池中有对象需要处理:
                if (this._dataPool.HasAvailableItems) {
                    DebugHelper.WriteLine("[Notifier] Found non-consuming item.");
                    T item;
                    if (this._dataPool.TryBorrowAvailable(out item)) {
                        invokeAsyncItemHandlerIfPossible(item);
                    }
                    continue; //处理完, 进入下次迭代.
                }
                //缓存中还存在一些在处理中的对象
                if (this._dataPool.HasItem) {
                    Thread.Sleep(500); //等一会
                    continue; //再迭代一次， 以防有处理失败
                }
                //没有任何对象, 用户没终止监听:
                if (this._shouldContinueNotify) {
                    DebugHelper.WriteLine("[Notifier] Sleeping...");
                    sleepUntilNextSignal(); //进入睡眠状态.
                    DebugHelper.WriteLine("[Notifier] Wake up.");
                    continue; //被唤醒, 进入下次迭代.
                }
                //所有对象都被处理了, 对象池中没对象了，监听也停止了:
                DebugHelper.WriteLine("[Notifier] Listening stopped and all items are handled. Try to invoke AllItemsHandled EventHandler.");
                this.AllItemsHandled.InvokeIfNotNull(this, null);

                //即将结束循环
                DebugHelper.WriteLine("[Notifier] Should stop.");
                lock (this._notifierSyncRoot) {
                    //锁住状态, 最后检查一下是否应该继续
                    DebugHelper.Write("[Notifier] Last check...");
                    //用户指示需要继续.
                    if (this._shouldContinueNotify) {
                        DebugHelper.WriteLine("[Notifier] Resumed.");
                        continue; //循环需要继续, 重新进入迭代.
                    }
                    //仍不需要继续, 结束循环
                    break;
                }
            }

            //结束循环
            this._isNotifierRunning = false;
            DebugHelper.WriteLine("[Notifier] Stopped.");
        }

        /// <summary>
        /// 尝试激活异步进度处理委托
        /// </summary>
        /// <param name="item">需要处理的数据</param>
        private void invokeAsyncItemHandlerIfPossible(T item) {
            //_itemAppendedReport总不为null, 因此只需要检查ItemRecievedHandlerAsync即可.
            if (this.ItemRecievedHandlerAsync != null) {
                //支持界面控件线程的话, 就使用Progress.Report
                if (this._withUIThreadingSupport) {
                    _itemAppendedProgess.Report(item);
                }
                //不需要UI线程支持的话, 就直接调用注册的委托
                else {
#pragma  warning disable 4014
                    invokeItemRecievedAsync(item);
#pragma  warning restore 4014
                }
            }
        }

        /// <summary>
        /// 取出对象, 交由ItemRecievedHandlerAsync 委托处理, 之后会移除对象. 
        /// ItemRecievedHandlerAsync 委托抛出异常的话则将对象还回对象池, 供下次使用
        /// </summary>
        /// <param name="item">取出的对象</param>
        /// <returns></returns>
        private async Task invokeItemRecievedAsync(T item) {
            try {
                DebugHelper.WriteLine("Invoke ItemRecievedHandlerAsync asyncly.");
                await this.ItemRecievedHandlerAsync(item);
                _dataPool.Remove(item);
                _logger.WriteInfo("Item handled successfully.");
            }
            catch (Exception ex) {
                DebugHelper.WriteLine($"{nameof(ItemRecievedHandlerAsync)} exception. Should return item.");
                _dataPool.Return(item);
                wake_notifyLoop(); //returned, so there is item available again, notify.
                _logger.WriteError(ex.Message);
                throw ex;
            }
        }

        private void sleepUntilNextSignal() {
            _are.WaitOne();
        }
        /// <summary>
        /// 唤醒事件激发线程
        /// </summary>
        private void wake_notifyLoop() {
            this._are.Set();
        }
        #endregion

        #region Internal Members
        /// <summary>
        /// 信号量
        /// </summary>
        protected AutoResetEvent _are = new AutoResetEvent(false);
        /// <summary>
        /// 进度通知委托
        /// </summary>
        protected IProgress<T> _itemAppendedProgess;
        /// <summary>
        /// 接受到的对象池
        /// </summary>
        protected LongTimeConsumerCollection<T> _dataPool = new LongTimeConsumerCollection<T>();
        /// <summary>
        /// 通知线程是否在运行
        /// </summary>
        protected bool _isNotifierRunning = false;
        /// <summary>
        /// 通知线程同步锁对象
        /// </summary>
        protected object _notifierSyncRoot = new object();
        /// <summary>
        /// 是否应该停止通知线程
        /// </summary>
        protected bool _shouldContinueNotify = false;
        /// <summary>
        /// 是否支持UI界面线程
        /// </summary>
        protected bool _withUIThreadingSupport;
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected LogHelper _logger = new LogHelper();
        protected TcpReceivingServer<T> _server;
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    this._are.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CachedTcpServer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
