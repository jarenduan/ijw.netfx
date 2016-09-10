#if !NET35 
using System;

namespace ijw.Threading.Tasks {
    /// <summary>
    /// 封装带有进度通知的异步执行循环任务通用类, 提供启动, 暂停, 进度报告和退出的功能. 
    /// 可以执行指定的循环体, 循环体的每次迭代会检查指定的停止条件和暂停条件.
    /// </summary>
    public class BackgroundLooperWithReport<TReport> : BackgroundLooperBase {
        /// <summary>
        /// 设置进度报告的回调函数
        /// </summary>
        public IProgress<TReport> Progress { get; set; }

        /// <summary>
        /// 循环体, 返回一个值, 用作每次迭代的进度报告参数
        /// </summary>
        public Func<TReport> LoopFunction { get; set; }

        /// <summary>
        /// 循环体
        /// </summary>
        protected override void LoopBody() {
            if (this.LoopFunction != null) {
                TReport r = this.LoopFunction();
                if (this.Progress != null) {
                    this.Progress.Report(r);
                }
            }
        }
    }
}
#endif