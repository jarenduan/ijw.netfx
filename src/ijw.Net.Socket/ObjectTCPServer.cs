//TODO: support netstandard when netcore support binaryserialization
using ijw.Log;
using ijw.Collection;
using ijw.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ijw.Diagnostic;

namespace ijw.Net.Socket {
    /// <summary>
    /// ObjectTCPServer&lt;T&gt; 基于TCP协议, 通过监听指定端口, 实现了一个泛型对象远程接收/通知处理服务器. 
    /// 调用者可通过指定ObjectRecievedHandlerAsync委托, 方便地以异步的方式处理新接收到的对象.
    /// 调用者也可以通过挂接事件处理器, 方便地同步处理接收到的对象.
    /// </summary>
    /// <typeparam name="T">获取对象的类型</typeparam>
    /// <remarks>
    /// 调用StartAsync()方法, 内部启动了两个Task. 一个是负责监听TCP端口. 另一个负责发出通知事件.
    /// 两个Task共同维护了内部的一个线程安全的泛型集合. 
    /// </remarks>
    public class ObjectTCPServer<T> : IDisposable {
        /// <summary>
        /// 初始化服务器实例
        /// </summary>
        /// <param name="ifSupportUIThreading">是否对UI线程操作提供支持. true 则每次接收到对象将会封送至ObjectHandlerAsync所在线程进而可以更改控件. 反之ObjectHandlerAsync将在后台线程运行, 从而无法更改UI线程中的控件.</param>
        public ObjectTCPServer(bool ifSupportUIThreading = false) {
            this._withUIThreadingSupport = ifSupportUIThreading;
            if (this._withUIThreadingSupport) {
                //注: 此种方式调用就可以进行异步操作了, 弥补了同步事件的不足
                this._objectAppendedProgess = new Progress<T>(async (o) => {
                    await invokeObjectRecievedAsync(o);
                });
            }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose() {
            this._are.Dispose();
        }

        #region Properties
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 监听的端口
        /// </summary>
        public int PortNum { get; set; }

        /// <summary>
        /// 服务器是否停止监听
        /// </summary>
        public bool IsListenerRunning {
            get {
                return this._isListenerRunning;
            }
        }
        //TDDO: remove cts and ct!
        #endregion

        #region Events
        /// <summary>
        /// 接收到对象后进行调用的委托, 此委托将会异步执行.
        /// 注意: 即使调用Stop方法停止了接收, 但是如果对象池仍有对象没有被处理, 此委托仍会被调用, 直至所有对象被成功处理.
        /// </summary>
        public Func<T, Task> ObjectHandlerAsync { get; set; }

        /// <summary>
        /// 服务器停止后, 且全部对象被成功处理后, 激活此事件.
        /// </summary>
        public event EventHandler AllItemsHandled;

        /// <summary>
        /// 当有数据接受到, 追加到数据池中, 会触发此事件. EventArg 是对象池的引用.
        /// </summary>
        /// <remarks>
        /// 一般地, 事件处理器应调用集合的TryGetItem方法取出下一个需要处理的对象, 成功处理后, 应该调用Remove方法将其移出对象池.
        /// 如果处理失败, 应该调用Return方法还回对象池. 对象集合是线程安全的.
        /// 另外, 此事件将会运行在通知任务的线程中, 并不会阻塞端口监听.
        /// 但此事件是同步调用, 因此会导致诸如无法更改前台界面线程中的窗体控件等线程同步问题.
        /// </remarks>
        public event EventHandler<ObjectAvailabeEventArgs<T>> ObjectAvailable;

        /// <summary>
        /// 接收到损坏对象
        /// </summary>
        public event EventHandler BadObjectRecieved;
        #endregion
        
        #region Start & Stop
        /// <summary>
        /// 启动服务器
        /// 此操作将在内部启动两个Task. 分别负责端口监听和事件激发.
        /// 启动监听后, 代码会立即返回. 
        /// 可通过注册<see cref="ObjectAvailable"/>事件来处理接收到的对象.
        /// </summary>
        /// <remarks>
        /// 未提供此方法的同步版本.
        /// 以await方式调用, 可用TryCatch捕捉内部Task中的异常.
        /// </remarks>
        public async Task StartAsync() {
            if(this._isListenerRunning) {
                DebugHelper.WriteLine("[Main] Server is running.");
                logger.WriteError("[Main] Server is running.");
                return;
            }
            startOrResumeNotifyLoop();
            try {
                await startNewListenerLoopAsync();
            }
            catch(Exception ex) {
                throw ex;
            }
            finally {
                if(this._isListenerRunning) {
                    this.Stop();
                }
            }
        }

        /// <summary>
        /// 停止服务器.
        /// 注意: Stop方法只停止了监听线程. 内部的通知处理线程仍会继续循环, 直至所有对象都被处理和移出集合.
        /// </summary>
        public void Stop() {
            DebugHelper.WriteLine("[Listener] Got STOP signal.");
            if(!this._isListenerRunning) {
                DebugHelper.WriteLine("[Listener] Listener is not running.");
                return;
            }
            //发出停止信号
            stopListening();
            
        }
        #endregion

        #region Internal logic

        #region Listening Jobs
        /// <summary>
        /// 启动监听线程
        /// </summary>
        /// <returns></returns>
        private async Task startNewListenerLoopAsync() {
            try {
                establishListener();
                await Task.Run(() => {
                    listenLoop();
                });
                closeListenerIfThereIs();
            }
            catch(Exception e) {
                DebugHelper.WriteLine(e.ToString());
                throw e;
            }
            finally {
                closeClientIfThereIs();
                if(this._isListenerRunning) {
                    closeListenerIfThereIs();
                }
                this._isListenerRunning = false;
            }
        }
        /// <summary>
        /// 循环监听TcpClient, 并从中读取对象, 直至用户取消监听.
        /// 一旦读到对象, 将会向内部数据池中追加对象, 并send信号有数据追加.
        /// </summary>
        private void listenLoop() {
            //更改监听线程的状态 => 运行
            this._isListenerRunning = true;
            this._shouldContinueListen = true;
            //开始循环
            while(this._shouldContinueListen) {
                waitingForTCPClient();
                try {
                    if(retrieve_and_append_object()) {
                        wake_notifyLoop();
                    }
                }
                catch {
                    DebugHelper.WriteLine("[Listener] Bad object, dropped. Or stop signal ");
                    this.BadObjectRecieved.InvokeIfNotNull(this, null);
                }
                finally {
                    closeClientIfThereIs();
                }
            }
            DebugHelper.WriteLine("[Listener] Should stop.");
            this._isListenerRunning = false;//监听线程状态 => 停止
            DebugHelper.WriteLine("[Listener] Stopped.");
            this._shouldContinueNotify = false; //通知线程 => 应该停止
            wake_notifyLoop();
        }
        /// <summary>
        /// 发出停止监听的信号
        /// </summary>
        private void stopListening() {
            DebugHelper.WriteLine("[Listener] Try to stop...");
            lock(this._notifierSyncRoot) {
                //设置监听令牌
                this._shouldContinueListen = false;
                //设置通知令牌
                this._shouldContinueNotify = false;
            }
            sendFakeClient();
        }
        #endregion

        #region Network Jobs
        /// <summary>
        /// 建立监听器连接
        /// </summary>
        private void establishListener() {
            this._localAddr = IPAddress.Parse(this.HostName);
            this._listener = new TcpListener(this._localAddr, this.PortNum);
            this._listener.Start();
            DebugHelper.WriteLine("[Listener] New listener Started.");
        }
        private void closeListenerIfThereIs() {
            if(this._listener != null) {
                this._listener.Stop();
                DebugHelper.WriteLine("[Main] Listener stopped.");
                this._listener = null;
            }
        }
        private void closeClientIfThereIs() {
            if(this._client != null) {
                this._client.Close();
                DebugHelper.WriteLine("[Listener] TCP Connection closed.");
                this._client = null;
            }
        }
        /// <summary>
        /// 开一个tcp连接, 防止监听线程处在阻塞之中.
        /// </summary>
        private void sendFakeClient() {
            DebugHelper.WriteLine("[Listener] Send fake client.");
            TcpClient client = null;
            try {
                client = new TcpClient(this.HostName, this.PortNum);
            }
            catch {
            }
            finally {
                if(client != null)
                    client.Close();
            }
        }
        /// <summary>
        /// 会一直阻塞, 直至有连接进入.
        /// </summary>
        private void waitingForTCPClient() {
            DebugHelper.WriteLine("[Listener] Waiting for TCP client...");
            this._client = _listener.AcceptTcpClient();
            DebugHelper.WriteLine("[Listener] TCP client accepted.");
        }
        #endregion

        #region Notify Jobs
        /// <summary>
        /// 启动通知线程
        /// </summary>
        private void startOrResumeNotifyLoop() {
            lock(this._notifierSyncRoot) {
                if (this._isNotifierRunning) {
                    //notifyLoop在运行, 通知应该继续循环.
                    DebugHelper.WriteLine("[Main] Current notifier should continue.");
                    //stopListening方法有可能将_shouldContinueNotify设为false. 重设为true会保证notifyLoop即使在最后时刻也可以恢复循环.
                    this._shouldContinueNotify = true; 
                }
                else {
                    //如果notifyloop没在运行, 开始新线程进行新的loop.
                    DebugHelper.WriteLine("[Main] Should start a new notifier.");
                    startNewNotifyLoop();
                }
            }
        }
        private void startNewNotifyLoop() {
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
        /// <summary>
        /// 通知循环. 有未处理或者新接到的数据, 会激发事件处理器. 没有的话暂停循环.
        /// </summary>
        private void notifyLoop() {
            //设置一下循环状态
            this._isNotifierRunning = true;
            this._shouldContinueNotify = true;
            //开始循环
            while(true) {
                //如果发现对象池中有对象需要处理:
                if(this._objectPool.HasNonConsumingItem) { 
                    DebugHelper.WriteLine("[Notifier] Found non-consuming object(s).");
                    T obj;
                    if (this._objectPool.TryGetItemNotConsuming(out obj)) {
                        invokeAsyncObjectHandlerIfPossible(obj);
                        invokeObjectHandlerIfPossible();
                    }
                    continue; //处理完, 进入下次迭代.
                }
                //没有对象需要处理, 用户没终止监听:
                if(this._shouldContinueNotify) {
                    DebugHelper.WriteLine("[Notifier] Sleeping...");
                    sleepUntilNextSignal(); //进入睡眠状态.
                    DebugHelper.WriteLine("[Notifier] Wake up.");
                    continue; //被唤醒, 进入下次迭代.
                }
                //没有对象需要处理, 线程不需要继续:
                DebugHelper.WriteLine("[Notifier] Should stop. ");
                if(this._objectPool.HasItem) {
                    //但是还存在一些对象, 都在处理中
                    System.Threading.Thread.Sleep(500); //等一会
                    continue; //再迭代一次.
                }
                //所有对象都被处理了, 对象池中没对象了
                DebugHelper.WriteLine("[Notifier] All items are handled. Try to invoke AllItemsHandled EventHandler.");
                this.AllItemsHandled.InvokeIfNotNull(this, null);
                //即将结束循环
                DebugHelper.WriteLine("[Notifier] Should stop notify loop");
                lock(this._notifierSyncRoot) {
                    //锁住状态, 最后检查一下是否应该继续
                    DebugHelper.Write("[Notifier] Last check...");
                    //用户指示需要继续.
                    if(this._shouldContinueNotify) {
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
        /// 尝试invoke同步进度报告事件
        /// </summary>
        private void invokeObjectHandlerIfPossible() {
            DebugHelper.WriteLine("Try to Invoke ObjectAppended event.");
            this.ObjectAvailable.InvokeIfNotNull(this, null);
        }
        /// <summary>
        /// 尝试invoke异步进度处理委托
        /// </summary>
        /// <param name="obj"></param>
        private void invokeAsyncObjectHandlerIfPossible(T obj) {
            //_objectAppendedReport总不为null, 因此只需要检查ObjectRecievedHandlerAsync即可.
            if(this.ObjectHandlerAsync != null) {
                //支持界面控件线程的话, 就使用Progress.Report
                if(this._withUIThreadingSupport) {
                    _objectAppendedProgess.Report(obj);
                }
                //不需要UI线程支持的话, 就直接调用注册的委托
                else {
#pragma  warning disable 4014
                    invokeObjectRecievedAsync(obj);
#pragma  warning restore 4014
                }
            }
        }
        /// <summary>
        /// 尝试获取并解析到对象
        /// </summary>
        /// <returns>成功解析到对象, 返回true; 反之返回false.</returns>
        private bool retrieve_and_append_object() {
            var networkStream = _client.GetStream();
            DebugHelper.WriteLine("[Listener] Try to retrieve object...");
            var obj = networkStream.RetrieveBinaryObjectAndDispose<T>();
            //有可能是此类自己发出的唤醒client的假数据, 那样什么也不会解析到
            if(obj != null) {
                this._objectPool.Append(obj);
                DebugHelper.WriteLine("[Listener] An object appended.");
            }
            else {
                DebugHelper.WriteLine("[Listener] No object retrieved.");
                //Console.WriteLine("No object retrieved!");
            }
            return obj != null;
        }
        /// <summary>
        /// 取出对象, 交由ObjectRecievedHandlerAsync委托处理, 之后会移除对象. 
        /// ObjectRecievedHandlerAsync委托抛出异常的话则将对象还回对象池, 供下次使用
        /// </summary>
        /// <param name="obj">取出的对象</param>
        /// <returns></returns>
        private async Task invokeObjectRecievedAsync(T obj) {
            try {
                DebugHelper.WriteLine("Invoke ObjectRecievedHandlerAsync delegate asyncly.");
                await this.ObjectHandlerAsync(obj);
                _objectPool.Remove(obj);
                logger.WriteInfo("Object handled successfully.");
            }
            catch (Exception ex) {
                DebugHelper.WriteLine("ObjectRecievedHandlerAsync exception. Should return object.");
                _objectPool.Return(obj);
                wake_notifyLoop();
                logger.WriteError(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Control between the listen and notify jobs
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

        #endregion

        #region Internal Members
        /// <summary>
        /// 本地地址
        /// </summary>
        protected IPAddress _localAddr;
        /// <summary>
        /// 监听器
        /// </summary>
        protected TcpListener _listener;
        /// <summary>
        /// 客户端
        /// </summary>
        protected TcpClient _client;
        /// <summary>
        /// 信号量
        /// </summary>
        protected AutoResetEvent _are = new AutoResetEvent(false);
        /// <summary>
        /// 进度通知委托
        /// </summary>
        protected IProgress<T> _objectAppendedProgess;
        /// <summary>
        /// 接受到的对象池
        /// </summary>
        protected LongTimeConsumerCollection<T> _objectPool = new LongTimeConsumerCollection<T>();
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
        /// 是否应该继续监听
        /// </summary>
        protected bool _shouldContinueListen = false;
        /// <summary>
        /// 监听线程是否在运行
        /// </summary>
        protected bool _isListenerRunning = false;
        /// <summary>
        /// 是否支持UI界面线程
        /// </summary>
        protected bool _withUIThreadingSupport;
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected LogHelper logger = new LogHelper();
        #endregion
    }
}
