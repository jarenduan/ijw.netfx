using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.MessageModel {
    public class Robot : AgentBase {
        public int Health { get; protected set; }
        public int Strength { get; protected set; }

        protected override Message decision() {
            throw new NotImplementedException();
        }

        protected override void reaction(Message nextMove) {
            throw new NotImplementedException();
        }

        private void AttackHandler(string messageBody) {
            var strength = messageBody.ToIntAnyway();
            this.Health -= strength;
        }

        private void Attack() {
            if (this.Strength > 0) {
                this.Strength--;
                string message = "attack,objB,1";
                SendMessage(message);
            }
        }
    }
}
