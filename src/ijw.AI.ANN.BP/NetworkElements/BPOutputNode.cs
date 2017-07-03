using ijw.Maths.Functions;
using ijw.AI.ANN.Base;

namespace ijw.AI.ANN.BP
{
    public class BPOutputNode : OutputNodeBase, IBPTraining {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="activationFunction">指定的激活函数</param>
        /// <param name="thresholdValue">阈值初始值</param>
        public BPOutputNode(IMathFunctionWithDerivative activationFunction, double thresholdValue)
            : base(new BPNodeCalculationCore(activationFunction, thresholdValue)) { }

        /// <summary>
        /// 获取节点输出对总输出误差的delta值
        /// </summary>
        /// <returns></returns>
        public double GetDelta() {
            return this.Derivative * (this.DesireOutput - this.Output);
        }
    }
}
