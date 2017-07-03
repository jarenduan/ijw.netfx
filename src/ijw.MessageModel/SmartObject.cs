using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public abstract class SmartObject {
        public SmartObject(MessageTransceiver transceiver, MessageProcessor processor) {
            this._transcevier = transceiver;
            this._processor = processor;
            this._transcevier.MessageHandlerAsync = processor.ProcessAsync;
        }

        public async Task StartMessageLoop() {
            this._processor.StartProcessing();
            await this._transcevier.StartMessageListeningAsync();
        }

        /// <summary>
        /// 对象标识符
        /// </summary>
        public Guid Id { get; protected set; } = new Guid();

        protected MessageTransceiver _transcevier;
        private MessageProcessor _processor;
    }
}