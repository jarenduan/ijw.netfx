using ijw.Diagnostic;
using System;

namespace ijw.Threading.Tasks {
    /// <summary>
    /// 封装异步执行循环任务的通用类, 提供启动, 暂停和退出的功能. 
    /// 可以执行指定的循环体, 循环中每次迭代会检查指定的停止条件和暂停条件.
    /// </summary>
    public class BackgroundLooper :BackgroundLooperBase {
        /// <summary>
        /// 循环中每次迭代的循环体
        /// 如果每次迭代中的暂停条件不为真, 则会被调用
        /// </summary>
        public Action LoopAction { get; set; }

        protected override void LoopBody() {
            if(this.LoopAction != null) {
                DebugHelper.WriteLine("Loop body started.");
                this.LoopAction();
                DebugHelper.WriteLine("Loop body ended.");
            }
        }
    }
}
