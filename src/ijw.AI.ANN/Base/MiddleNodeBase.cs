using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Maths.Functions;

namespace ijw.AI.ANN.Base {
    public abstract class MiddleNodeBase : NodeBase, ISend, IRecieve {
        public MiddleNodeBase(IMathFunctionWithDerivative calculationCore)
            : base(calculationCore) { 
            this.inConnections = new List<IConnection>();
            this.outConnections = new List<IConnection>();
        }

        /// <summary>
        /// 输入,来自输入连接的加权和
        /// 注意, 将一个节点的输入端连接到其他节点后, input是自动获取的, 将无法set
        /// </summary>
        public override double Input {
            get {
                return this.InConnections.Sum((x) => {
                    return x.Value * x.Weight;
                });
            }
            set {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IConnection> OutConnections {
            get { return this.outConnections; }
        }
        public IEnumerable<IConnection> InConnections {
            get { return this.inConnections; }
        }

        public void AddSend(IConnection connection) {
            this.outConnections.Add(connection);
        }
        public void RemoveSend(IConnection connection) {
            this.outConnections.Remove(connection);
        }

        public void AddRecieve(IConnection connection) {
            this.inConnections.Add(connection);
        }
        public void RemoveRecieve(IConnection connection) {
            this.inConnections.Remove(connection);
        }

        public double GetValueByConn(IConnection connection) {
            if (!this.OutConnections.Contains(connection)) {
                throw new NullReferenceException();
            }
            return this.Output;
        }

        protected List<IConnection> outConnections;
        protected List<IConnection> inConnections;
    }
}