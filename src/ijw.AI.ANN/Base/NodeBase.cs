using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ijw.Maths.Functions;

namespace ijw.AI.ANN.Base {
    /// <summary>
    /// 表示一个神经元
    /// </summary>
    public abstract class NodeBase : INode  {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="calculationCore">核心计算逻辑</param>
        public NodeBase(IMathFunctionWithDerivative calculationCore) {
            this._calculationCore = calculationCore;
        }

        /// <summary>
        /// 神经元输入
        /// </summary>
        public abstract double Input { get; set; }

        /// <summary>
        /// 神经元输出
        /// </summary>
        public double Output { 
            get { return  _calculationCore.Calculate(this.Input); }
        }

        /// <summary>
        /// 神经元当前输入的导数
        /// </summary>
        public double Derivative {
            get { return _calculationCore.CalculateDerivative(this.Input); }
        }

        /// <summary>
        /// 神经元的核心计算函数
        /// </summary>
        protected IMathFunctionWithDerivative _calculationCore;
    }
}
