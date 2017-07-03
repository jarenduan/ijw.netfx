using ijw.Net.Socket;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ijw.MessageModel {
    public abstract class IntelligentObject : SmartObject {
        public IntelligentObject(MessageTransceiver transceiver, IntelligentMessageProcessor imp) : base(transceiver, imp) {
        }
    }
}