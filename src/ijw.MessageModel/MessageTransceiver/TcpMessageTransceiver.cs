using ijw.Net.Socket;
using System;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public abstract class TcpMessageTransceiver : MessageTransceiver {
        protected TcpReceivingServer<Message> _reciever;
        protected TcpSender<Message> _sender = new TcpSender<Message>();

        public TcpMessageTransceiver(string hostName = "127.0.0.1", int portNum = 8080) {
            this._reciever = new TcpReceivingServer<Message>(hostName, portNum);
            this._reciever.ItemHandlerAsync = this.MessageHandlerAsync;
            this._sender.HostName = "127.0.0.1";
            this._sender.PortNum = 80;
        }

        public override async Task StartMessageListeningAsync() {
            await this._reciever.StartListeningAsync();
        }

        protected override void sendMessage(Message msg) {
#if !NET40
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            this._sender.TrySendDataAsync(msg);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
#else
            this._sender.TrySendData(msg);
#endif
        }
    }
}