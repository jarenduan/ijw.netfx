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
        /// 循环体, 必须实现.
        /// </summary>
        protected abstract void LoopBody();

        /// <summary>
        /// 停止循环的条件, 为真则停止循环. 默认值是一直为假，即一直循环。
        /// </summary>
        public BooleanCondition StopCondition { get; set; } = () => false;

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
            this._cts = new CancellationTokenSource();
            await Task.Run(() => start(), this._cts.Token);
        }

        /// <summary>
        /// 通知循环体停止迭代.
        /// 需要注意的是, 如果代码正处在循环体内, 将不会立即停止返回, 而是会执行完当前的迭代, 待下次迭代开始的时候才会停止循环.
        /// </summary>
        public void Exit() {
            this._cts.Cancel();
            DebugHelper.WriteLine("Loop exit signal sended...");
            _are.Set();
        }

        /// <summary>
        /// 暂停迭代循环。将会执行完当前迭代, 待下次迭代开始时暂停.
        /// </summary>
        public void Suspend() {
            DebugHelper.WriteLine("Loop is going to suspend...");
            this.ShouldSuspend = true;
            _are.Set();
        }

        /// <summary>
        /// 通知循环继续
        /// </summary>
        public void Resume() {
            if (this.ShouldSuspend) {
                DebugHelper.WriteLine("Loop is going to resume...");
                this.ShouldSuspend = false;
            }
            else {
                DebugHelper.WriteLine("Loop waking signal sended...");
            }
            _are.Set();
        }

        /// <summary>
        /// 开始循环
        /// </summary>
        protected void start() {
            DebugHelper.WriteLine("Loop started.");
            while (this.StopCondition == null ? true : this.StopCondition() == false) {
                if (this._cts.Token.IsCancellationRequested) {
                    DebugHelper.WriteLine("Exiting signal recieved.");
                    break;
                }
                if (this.ShouldSuspend) {
                    DebugHelper.WriteLine("Loop suspended!");
                    _are.WaitOne();
                    if (!ShouldSuspend) {
                        DebugHelper.WriteLine("Loop resumed!");
                    }
                }
                else if (WaitCondition?.Invoke() == true) {
                    DebugHelper.WriteLine("Condition not satisfied, loop sleeping!");
                    _are.WaitOne();
                    if (this._cts.Token.IsCancellationRequested) {
                        DebugHelper.WriteLine("Exit signal recieved.");
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
        /// 任务停止令牌
        /// </summary>
        protected CancellationTokenSource _cts;
        /// <summary>
        /// 同步信号量
        /// </summary>
        protected static AutoResetEvent _are = new AutoResetEvent(false);
        /// <summary>
        /// 是否应该暂停
        /// </summary>
        protected bool ShouldSuspend = false;
    }
}
