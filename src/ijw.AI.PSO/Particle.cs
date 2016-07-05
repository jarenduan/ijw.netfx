using System;
using ijw.Maths.Structures;
using ijw.Maths.Models;

namespace ijw.AI.PSO {
    public class Particle {
        /// <summary>
        /// 速度
        /// </summary>
        public VectorDouble Velocity { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public VectorDouble Position { get; set; }

        /// <summary>
        /// 个体最佳位置
        /// </summary>
        public VectorDouble BestPosition { get; set; }

        /// <summary>
        /// 个体最佳适应度
        /// </summary>
        public double BestFitness { get; protected set; }

        /// <summary>
        /// 目标函数
        /// </summary>
        public ISimpleMathModel TargetFunction { get; protected set; }

        /// <summary>
        /// 构造函数
        /// 指定维度和目标函数, 粒子默认将向目标函数的极小值飞行
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="targetFunction"></param>
        public Particle(ISimpleMathModel targetFunction, bool isSeekingMinimal = true) {
            //initialize parameters
            var dimension = targetFunction.InputDimension;
            this.Velocity = VectorDouble.RandomNew(dimension);
            this.Position = VectorDouble.RandomNew(dimension);
            this.BestPosition = new VectorDouble(dimension);
            this.BestFitness = 10.0f;
            this.TargetFunction = targetFunction;
            this.UpdateBestFitnessAndPosition();
        }

        /// <summary>
        /// 更新自身的位置和速度, 使用标准PSO更新算法, 
        /// </summary>
        /// <param name="c1">自身学习因子</param>
        /// <param name="c2">群体学习因子</param>
        /// <param name="w">取0时退化为原始PSO更新算法</param>
        public void UpdateState(VectorDouble globalBestPos, double c1, double c2, double w = 1) {
            Random r = new Random();
            this.Velocity = this.Velocity * w + (this.BestPosition - this.Position) * c1 * r.NextDouble() + (globalBestPos - this.Position) * c2 * r.NextDouble();
            this.Position += this.Velocity;
        }

        /// <summary>
        /// 更新最新适应度和最佳位置
        /// </summary>
        public void UpdateBestFitnessAndPosition() {
            double fitness = this.GetCurrentFitness();
            if (fitness < BestFitness) {
                this.BestFitness = fitness;
                this.BestPosition = Position;
            }
        }

        /// <summary>
        /// 获取当前位置的适应度
        /// 对于目标函数, 只能返回输出向量中的第一个值
        /// </summary>
        /// <returns>输出向量中的第一个值</returns>
        public double GetCurrentFitness() {
            this.TargetFunction.Input = this.Position;
            this.TargetFunction.Calculate();
            return this.TargetFunction.Output;
        }
    }
}
