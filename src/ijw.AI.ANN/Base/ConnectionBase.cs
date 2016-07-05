using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ijw.AI.ANN.Base {
    public abstract class ConnectionBase : IConnection {
        public ConnectionBase() {
            this.Weight = 1;
        }

        public ISend From {
            get;
            set;
        }

        public IRecieve To {
            get;
            set;
        }

        public double Value {
            get { return From.GetValueByConn(this); }
        }

        public double Weight {
            get;
            set;
        }

       public void ConnectNodes(ISend start, IRecieve end) {
            this.To = end;
            this.From = start;
            start.AddSend(this);
            end.AddRecieve(this);
        }
    }
}