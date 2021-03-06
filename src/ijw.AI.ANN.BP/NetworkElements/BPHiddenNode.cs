﻿using System;
using ijw.Maths.Functions;
using ijw.AI.ANN.Base;

namespace ijw.AI.ANN.BP {

    public class BPHiddenNode : MiddleNodeBase, IBPTraining {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="activationFunction">指定的激活函数</param>
        /// <param name="thresholdValue">阈值初始值</param>
        public BPHiddenNode(IMathFunctionWithDerivative activationFunction, double thresholdValue) :
            base(new BPNodeCalculationCore(activationFunction, thresholdValue)) { }

        public double GetDelta() {
            if (lastTimeInput == null || lastTimeInput != this.Input) {
                double sum = 0;
                foreach (var outConn in this.OutConnections) {
                    var nextNode = outConn.To as IBPTraining;
                    if (nextNode == null) {
                        throw new Exception("Training Exception : Not a IBPTraining!");
                    }
                    sum += this.Derivative * outConn.Weight * nextNode.GetDelta();
                }
                this.lastTimeDelta = sum;
                this.lastTimeInput = this.Input;
            }
            return lastTimeDelta;
        }

        private double? lastTimeInput = null;
        private double lastTimeDelta;
    }
}