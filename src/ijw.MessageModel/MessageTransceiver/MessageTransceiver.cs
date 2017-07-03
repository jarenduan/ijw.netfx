using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public abstract class MessageTransceiver {
        public Func<Message, Task> MessageHandlerAsync { get; set; }

        /// <summary>
        /// 启动消息监听
        /// </summary>
        /// <returns></returns>
        public abstract Task StartMessageListeningAsync();

        /// <summary>
        /// 对外发送一条消息
        /// </summary>
        /// <param name="msg"></param>
        protected abstract void sendMessage(Message msg);
    }
}
