using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.MessageModel {
    public class Robot : IntelligentObject {
        public Robot(MessageTransceiver transceiver, IntelligentMessageProcessor imp) : base(transceiver, imp) {
        }

        public int Health { get; protected set; }
        public int Strength { get; protected set; }

        private void AttackHandler(string messageBody) {
            var strength = messageBody.ToIntAnyway();
            this.Health -= strength;
        }

        private void Attack() {
            if (this.Strength > 0) {
                this.Strength--;
                string message = "attack,objB,1";
                //sendMessage(message);
            }
        }
    }
}
