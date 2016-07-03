using System;
using System.Runtime.Serialization;
//TODO: support .net standard binary

namespace ijw.Maths.Functions {
    public class SigmodFunction  : IMathFunctionWithDerivative{
        /// <summary>
        /// 计算导数
        /// </summary>
        /// <param name="input"></param>
        public double CalculateDerivative(double input) {
            double fx = Calculate(input);
            return  fx * (1 - fx);
        }

        /// <summary>
        /// 计算输入对应的输出值
        /// </summary>
        public double Calculate(double input) {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }
    }
}
