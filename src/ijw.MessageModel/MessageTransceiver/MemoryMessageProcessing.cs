using ijw.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public class MemoryMessageTranscevier : MessageTransceiver {
        protected object _syncLock = new object();
        protected Queue<Message> _messageQueue = new Queue<Message>();
        protected AutoResetEvent _signal = new AutoResetEvent(false);
        private MessageSystem _messageSystem;

        public MemoryMessageTranscevier(MessageSystem system) {
            this._messageSystem = system;
        }

        public override async Task StartMessageListeningAsync() {
            while (true) {
                if (this._messageQueue.Count != 0) {
                    Message msg = null;
                    lock (this._syncLock) {
                        if (this._messageQueue.Count != 0) {
                            msg = this._messageQueue.Dequeue();
                        }
                    }
                    if (msg != null) {
                        await this.MessageHandlerAsync?.Invoke(msg);
                    }
                }
                sleepUntilSignal();
            }
        }

        protected override void sendMessage(Message msg) {
            throw new NotImplementedException();
        }

        internal void PutMessage(Message msg) {
            lock (this._syncLock) {
                this._messageQueue.Enqueue(msg);
            }
        }

        private void sleepUntilSignal() {
            this._signal.WaitOne();
        }
    }
}
