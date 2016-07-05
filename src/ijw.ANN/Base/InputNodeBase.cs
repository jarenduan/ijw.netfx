using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Maths.Functions;
using System.Runtime.Serialization;

namespace ijw.ANN.Base {
    public abstract class InputNodeBase : NodeBase, ISend {
        public InputNodeBase(IMathFunctionWithDerivative calculationCore)
            : base(calculationCore) {
            this.outConnections = new List<IConnection>();
        }

        public override double Input { get; set; }

        public IEnumerable<IConnection> OutConnections {
            get { return this.outConnections; }
        }

        public void AddSend(IConnection connection) {
            this.outConnections.Add(connection);
        }
        public void RemoveSend(IConnection connection) {
            this.outConnections.Remove(connection);
        }

        protected List<IConnection> outConnections;

        public double GetValueByConn(IConnection connection) {
            if (!this.OutConnections.Contains(connection)) {
                throw new NullReferenceException();
            }
            return this.Output;
        }
    }
}