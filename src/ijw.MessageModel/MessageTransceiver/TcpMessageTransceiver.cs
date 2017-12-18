using ijw.Net.Socket;
using System;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public abstract class TcpMessageTransceiver : MessageTransceiver {
        protected TcpReceivingServer<Message> _reciever;
        protected TcpSender<Message> _sender = new TcpSender<Message>();

        public TcpMessageTransceiver(string hostName = "127.0.0.1", int portNum = 13197) {
            //TODO: 端口被占用后，自动获取可用端口号
            _reciever = new TcpReceivingServer<Message>(hostName, portNum);
            _reciever.ItemHandlerAsync = MessageHandlerAsync;
            _sender.HostName = "127.0.0.1";
            _sender.PortNum = portNum + 1;
        }

        public override async Task StartMessageListeningAsync() {
            await _reciever.StartListeningAsync();
        }

        protected override void sendMessage(Message msg) {
#if !NET40
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            _sender.TrySendDataAsync(msg);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
#else
            _sender.TrySendData(msg);
#endif
        }
    }
}