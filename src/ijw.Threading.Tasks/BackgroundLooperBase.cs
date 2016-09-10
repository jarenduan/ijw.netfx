using ijw.Diagnostic;
using System.Threading;
using System.Threading.Tasks; //for net40, KB2468871 should be installed.

namespace ijw.Threading.Tasks {
    public enum LooperState { NotRunning, Awaken, Sleeping, Suspending};

    /// <summary>
    /// 封装异步执行循环任务的通用基类, 提供启动, 暂停和退出的功能. 
    /// 可以执行指定的循环体, 循环中每次迭代会检查指定的停止条件和暂停条件.
    /// 循环体由子类实现, 一般情况不需要直接使用此类. 可以实例化BackgroundLooper子类方便地封装指定循环任务.
    /// 如果需要进度报告,可以使用BackgroundLooperWithReport&lt;TReport&gt;子类
    /// 如果需要自定义一些行为, 可以从此类继承, 以获得基本的任务启停等功能.
    /// </summary>
    public abstract class BackgroundLooperBase {
        public LooperState State { get; protected set; } = LooperState.NotRunning;

        /// <summary>
        /// 停止循环的条件, 为真则停止循环. 默认值是一直为假，即一直循环。
        /// </summary>
        public BooleanCondition ExitCondition { get; set; } = () => false;

        /// <summary>
        /// 每次迭代时候检查, 为真则进入睡眠状态, 睡眠后调用<see cref="WakeUpIfSleeping"/>方法唤醒.
        /// </summary>
        public BooleanCondition SleepCondition { get; set; } = () => false;

        /// <summary>
        /// 异步开始执行循环.
        /// </summary>
        /// <returns>封装了循环的Task.</returns>
        public async Task StartAsync() {
            if (this.State == LooperState.NotRunning) {
                this._cts = new CancellationTokenSource();
                await TaskHelper.Run(() => loop(), this._cts.Token);
            }
            else {
                DebugHelper.WriteLine("Loop is already running!");
            }
        }

        /// <summary>
        /// 从睡眠/暂停中恢复或者执行完当前迭代后停止循环。
        /// </summary>
        public void Exit() {
            DebugHelper.WriteLine("Try send the EXIT signal...");
            if (this.State == LooperState.NotRunning) {
                DebugHelper.WriteLine("Loop is not running!");
            }
            else {
                this._cts.Cancel();
                if (this.State == LooperState.Sleeping || this.State == LooperState.Suspending) {
                    DebugHelper.WriteLine("Need try to wake/resume loop first...");
                    _are.Set();
                }
            }
        }

        /// <summary>
        /// 暂停迭代循环。将会执行完当前迭代, 待下次迭代开始时暂停.
        /// </summary>
        public void Suspend() {
            DebugHelper.WriteLine("Try suspend loop...");
            switch (this.State) {
                case LooperState.NotRunning:
                    DebugHelper.WriteLine("Loop is not running.");
                    break;
                case LooperState.Awaken:
                    DebugHelper.WriteLine("Loop is going to suspend...");
                    this._shouldSuspend = true;
                    break;
                case LooperState.Sleeping:
                    DebugHelper.WriteLine("Loop is sleeping. Try to wake and suspend...");
                    this._shouldSuspend = true;
                    _are.Set();
                    break;
                case LooperState.Suspending:
                    DebugHelper.WriteLine("Loop is already suspended.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 通知循环继续
        /// </summary>
        public void Resume() {
            DebugHelper.WriteLine("Try to resume loop...");
            if (this.State == LooperState.Suspending) {
                this._shouldSuspend = false;
                _are.Set();
            }
            else {
                DebugHelper.WriteLine("Loop is not suspended!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void WakeUpIfSleeping() {
            DebugHelper.WriteLine("Try to wake up loop...");
            if (this.State == LooperState.Sleeping) {
                _are.Set();
            }
            else {
                DebugHelper.WriteLine("Loop is not sleeping!");
            }
        }

        /// <summary>
        /// 开始循环
        /// </summary>
        protected void loop() {
            DebugHelper.WriteLine("Loop started.");
            while (this.ExitCondition == null || this.ExitCondition() == false) {
                if (this._cts.Token.IsCancellationRequested) {
                    DebugHelper.WriteLine("EXIT signal recieved.");
                    break;
                }
                if (this._shouldSuspend) {
                    DebugHelper.WriteLine("Loop suspended!");
                    this.State = LooperState.Suspending;
                    _are.WaitOne();
                    DebugHelper.WriteLine("Loop resumed!");
                    this.State = LooperState.Awaken;
                }
                else if (SleepCondition?.Invoke() == true) {
                    DebugHelper.WriteLine("Should sleep, loop is sleeping now!");
                    this.State = LooperState.Sleeping;
                    _are.WaitOne();
                    DebugHelper.WriteLine("Loop waking up!");
                    this.State = LooperState.Awaken;
                }
                else {
                    LoopBody();
                }
            }
            DebugHelper.WriteLine("Loop exit.");
            this.State = LooperState.NotRunning;
        }

        /// <summary>
        /// 循环体, 必须实现.
        /// </summary>
        protected abstract void LoopBody();

        /// <summary>
        /// 任务停止令牌
        /// </summary>
        protected CancellationTokenSource _cts;
        /// <summary>
        /// 同步信号量
        /// </summary>
        protected AutoResetEvent _are = new AutoResetEvent(false);
        /// <summary>
        /// 是否应该暂停
        /// </summary>
        protected bool _shouldSuspend = false;
    }
}
