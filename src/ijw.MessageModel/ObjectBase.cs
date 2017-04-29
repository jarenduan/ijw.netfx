using ijw.Diagnostic;
using ijw.Net.Socket;
using ijw.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.MessageModel {
    public abstract class ObjectBase : TcpReceivingServer<Message> {
        public Guid Id { get; protected set; } = new Guid();

        protected TcpSender<Message> Sender = new TcpSender<Message>();

        public ObjectBase(string hostName = "127.0.0.1", int portNum = 8080) : base(hostName, portNum) {
            this.ItemHandler = new Action<Message>(this.processing);
            this.Sender.HostName = "sys";
            this.Sender.PortNum = 80;
        }

        protected virtual void processing(Message message) {
            var (_, messageName, messageBody, _) = message;
            respond(messageName, messageBody);
        }

        protected void respond(string messageName, string messageBody) {
            bool hasHandled = this.TryInvokeMethod(messageName + "Handler", out var _, messageBody);
            if (!hasHandled) {
                DebugHelper.WriteLine($"Messsage: {messageName} not handled, with body: {messageBody}");
            }
        }

        protected void SendMessage(string msg) {
            Message message = buildMessagee(msg);
#if !NET40
            this.Sender.TrySendDataAsync(message);
#else
            this.Sender.TrySendData(message);
#endif
        }

        private Message buildMessagee(string message) {
            throw new NotImplementedException();
        }
    }
}