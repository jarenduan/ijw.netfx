using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Maths.Functions;

namespace ijw.AI.ANN.Base {
    public abstract class OutputNodeBase : NodeBase,IRecieve {
        public OutputNodeBase(IMathFunctionWithDerivative calculationCore)
            : base(calculationCore) {
            this.inConnections = new List<IConnection>();
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

        /// <summary>
        /// 期望输出
        /// </summary>
        public double DesireOutput { get; set; }

        public IEnumerable<IConnection> InConnections {
            get { return this.inConnections; }
        }

        public void AddRecieve(IConnection connection) {
            this.inConnections.Add(connection);
        }
        public void RemoveRecieve(IConnection connection) {
            this.inConnections.Remove(connection);
        }

        protected List<IConnection> inConnections;
    }
}