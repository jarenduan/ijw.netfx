using ijw.Net.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.MessageModel {
    public abstract class AgentBase : ObjectBase {
        public AgentBase() : base() {

        }

        protected override void processing(Message message) {
            base.processing(message);
            var nextMove = decision();
            if (nextMove != null) {
                reaction(nextMove);
            }
        }

        protected abstract void reaction(Message nextMove);

        protected abstract Message decision();


    }
}
