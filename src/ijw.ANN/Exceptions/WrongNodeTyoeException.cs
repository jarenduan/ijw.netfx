using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.ANN {
    public class WrongNodeTyoeException : Exception {
        private IRecieve recieve;

        public WrongNodeTyoeException(IRecieve recieve) {
            this.recieve = recieve;
        }
    }
}
