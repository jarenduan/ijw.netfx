using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Maths.Functions;
using System.Runtime.Serialization;

namespace ijw.ANN.BP {
    /// <summary>
    /// BP神经元的核心计算逻辑
    /// </summary>
    public class BPNodeCalculationCore : IMathFunctionWithDerivative {
        public BPNodeCalculationCore(IMathFunctionWithDerivative activationFunction, double thresholdValue) {
            this._activationFunction = activationFunction;
            this.ThresholdValue = thresholdValue;
        }

        /// <summary>
        /// 激活函数
        /// </summary>
        protected IMathFunctionWithDerivative _activationFunction;

        /// <summary>
        /// 阈值
        /// </summary>
        public double ThresholdValue { get; set; }

        #region CalculationModelImpl
        public double Calculate(double input) {
            return _activationFunction.Calculate(input - this.ThresholdValue);
        }

        public double CalculateDerivative(double input) {
            return _activationFunction.CalculateDerivative(input - this.ThresholdValue);
        }
        #endregion
    }
}