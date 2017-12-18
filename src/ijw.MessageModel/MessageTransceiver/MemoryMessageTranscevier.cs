using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public class MemoryMessageTranscevier : MessageTransceiver {
        protected object _syncLock = new object();
        protected Queue<Message> _messageQueue = new Queue<Message>();
        protected AutoResetEvent _signal = new AutoResetEvent(false);
        private MessageSystem _messageSystem;

        public MemoryMessageTranscevier(MessageSystem system) {
            _messageSystem = system;
        }

        public override async Task StartMessageListeningAsync() {
            while (true) {
                if (_messageQueue.Count != 0) {
                    Message msg = null;
                    lock (_syncLock) {
                        if (_messageQueue.Count != 0) {
                            msg = _messageQueue.Dequeue();
                        }
                    }
                    if (msg != null) {
                        await MessageHandlerAsync?.Invoke(msg);
                    }
                }
                sleepUntilSignal();
            }
        }

        protected override void sendMessage(Message msg) {
            throw new NotImplementedException();
        }

        internal void PutMessage(Message msg) {
            lock (_syncLock) {
                _messageQueue.Enqueue(msg);
            }
        }

        private void sleepUntilSignal() {
            _signal.WaitOne();
        }
    }
}
