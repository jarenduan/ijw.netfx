using ijw.Diagnostic;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.Threading.Tasks {
    /// <summary>
    /// 封装异步执行循环任务的通用基类, 提供启动, 暂停和退出的功能. 
    /// 可以执行指定的循环体, 循环中每次迭代会检查指定的停止条件和暂停条件.
    /// 循环体由子类实现, 一般情况不需要直接使用此类. 可以实例化BackgroundLooper子类方便地封装指定循环任务.
    /// 如果需要进度报告,可以使用BackgroundLooperWithReport&lt;TReport&gt;子类
    /// 如果需要自定义一些行为, 可以从此类继承, 以获得基本的任务启停等功能.
    /// </summary>
    public abstract class BackgroundLooperBase {
        /// <summary>
        /// 任务停止令牌
        /// </summary>
        protected CancellationTokenSource cts;
        /// <summary>
        /// 同步信号量
        /// </summary>
        protected static AutoResetEvent are = new AutoResetEvent(false);
        /// <summary>
        /// 是否应该等待
        /// </summary>
        protected bool ShouldWait = false;

        /// <summary>
        /// 循环体, 必须实现.
        /// </summary>
        protected abstract void LoopBody();
        /// <summary>
        /// 检查是否存在循环体, 必须实现.
        /// </summary>
        /// <returns>存在返回真, 不存在返回否</returns>
        protected abstract bool CheckLoopBody();

        /// <summary>
        /// 停止循环的条件, 为真则停止循环.
        /// </summary>
        public BooleanCondition StopCondition { get; set; }

        /// <summary>
        /// 每次迭代时候检查, 为真则暂停一次迭代.
        /// 调用<see cref="ContinueIfWaiting()"/>方法, 恢复迭代.
        /// </summary>
        public BooleanCondition WaitCondition { get; set; }

        /// <summary>
        /// 异步开始执行循环.
        /// </summary>
        /// <returns>封装了循环的Task.</returns>
        public async Task StartAsync() {
            this.cts = new CancellationTokenSource();
            await System.Threading.Tasks.Task.Run(() => Start(), this.cts.Token);
        }

        /// <summary>
        /// 开始执行Loop
        /// </summary>
        protected void Start() {
            if(!CheckLoopBody()) {
                DebugHelper.WriteLine("No loop action.");
                return;
            }
            DebugHelper.WriteLine("Loop started.");
            while(this.StopCondition == null ? true : this.StopCondition() == false) {
                if(this.cts.Token.IsCancellationRequested) {
                    DebugHelper.WriteLine("Exit notification recieved.");
                    break;
                }
                if((WaitCondition != null && WaitCondition() == true) || this.ShouldWait == true) {
                    this.ShouldWait = false;
                    DebugHelper.WriteLine("Condition not satisfied, loop sleeping!");
                    are.WaitOne();
                    if(this.cts.Token.IsCancellationRequested) {
                        DebugHelper.WriteLine("Exit notification recieved.");
                        break;
                    }
                    else {
                        DebugHelper.WriteLine("Notification recieved, loop waking up!");
                    }
                }
                else {
                    LoopBody();
                }
            }
            DebugHelper.WriteLine("Loop exit.");
        }

        /// <summary>
        /// 通知循环体暂停一次迭代. 待接受到通知再继续循环.
        /// 需要注意的是, 如果代码正处在循环体内, 将不会立即暂停, 而是会执行完当前的迭代, 待下次迭代开始的时候才会进入暂停.
        /// </summary>
        public void LoopWaitOnce() {
            this.ShouldWait = true;
        }

        /// <summary>
        /// 通知循环体停止迭代.
        /// 需要注意的是, 如果代码正处在循环体内, 将不会立即停止返回, 而是会执行完当前的迭代, 待下次迭代开始的时候才会停止循环.
        /// </summary>
        public void Exit() {
            this.cts.Cancel();
            DebugHelper.WriteLine("Loop exit signal sended...");
            are.Set();
        }

        /// <summary>
        /// 通知循环体继续迭代
        /// </summary>
        public void ContinueIfWaiting() {
            DebugHelper.WriteLine("Loop waking signal sended...");
            are.Set();
        }
    }
}
