using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ijw.Threading.Tasks;
using System.Threading;

namespace ijw.MessageModel {
    public class IntelligentMessageProcessor : MessageProcessor {
        public override async Task ProcessAsync(Message msg) {
            this.sendSignal();
            await base.ProcessAsync(msg);
        }

        public override void StartProcessing() {
            base.StartProcessing();
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            TaskHelper.Run(() => {
                while (true) {
                    var thought = decision();
                    reaction(thought);
                    waitforsignal();
                }
            });
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }

        private object decision() {
            throw new NotImplementedException();
        }

        private void reaction(object thought) {
            throw new NotImplementedException();
        }

        private void waitforsignal() {
            this._signal.WaitOne();
        }

        private void sendSignal() {
            this._signal.Set();
        }

        protected AutoResetEvent _signal = new AutoResetEvent(false);
    }
}
