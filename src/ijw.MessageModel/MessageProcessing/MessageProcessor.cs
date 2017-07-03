using ijw.Diagnostic;
using ijw.Reflection;
using System;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public class MessageProcessor {
        protected bool _running = false;

        public virtual void StartProcessing() {
            this._running = true;
        }

        public virtual void StopProcessing() {
            this._running = false;
        }

        /// <summary>
        /// 处理一条消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual async Task ProcessAsync(Message msg) {
            if (!this._running) return;
            var (_, messageName, messageBody, _) = msg;
            await respond();

            async Task respond()
            {
                string handlerName = messageName + "Handler";
                var mi = this.GetType().GetMethodInfo(handlerName);
                if (mi == null) {
                    DebugHelper.WriteLine($"Messsage: {messageName} not handled, with body: {messageBody}");
                }
                else {
                    try {
                        var taskResult = this.InvokeMethod(handlerName, messageBody) as Task;
                        if (taskResult == null) {
                            DebugHelper.WriteLine($"Messsage: {messageName} handler is not a async Task.");
                        }
                        await taskResult;
                    }
                    catch (Exception ex) {
                        DebugHelper.WriteLine($"Messsage: {messageName} handler exception, {ex.Message}");
                    }
                }
            }
        }
    }
}