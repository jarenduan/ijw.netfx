using System.Diagnostics;
using ijw.Maths.Structures;
using ijw.Maths.Models;
using ijw.Collection;

namespace ijw.AI.PSO {
    public class ParticleSwarm {
        /// <summary>
        /// 粒子数量
        /// </summary>
        public int ParticleCount { get; protected set; }

        /// <summary>
        /// 粒子群
        /// </summary>
        public Particle[] Particles { get; protected set; }

        /// <summary>
        /// 自身学习因子
        /// </summary>
        public double c1 { get; set; }

        /// <summary>
        /// 群体学习因子
        /// </summary>
        public double c2 { get; set; }

        /// <summary>
        /// 惯性因子
        /// </summary>
        public double w { get; set; }

        /// <summary>
        /// 全局最优位置
        /// </summary>
        public VectorDouble GlobalBestPosition { get; protected set; }

        /// <summary>
        /// 全局最优适应度
        /// </summary>
        public double GlobalBestFitness { get; protected set; }

        /// <summary>
        /// 最大速度
        /// </summary>
        public VectorDouble MaxVelocity { get; set; }

        /// <summary>
        /// 目标函数
        /// </summary>
        public ISimpleMathModel TargetFunction { get; protected set; }

        /// <summary>
        /// 迭代次数
        /// </summary>
        private int _loopTime = 1000;
        public int LoopTimes {
            get { return _loopTime; }
            set { _loopTime = value; }
        }

        /// <summary>
        /// 惯性因子初始值
        /// </summary>
        protected double _w_ini = 0.9;
        /// <summary>
        /// 惯性因子结束值
        /// </summary>
        protected double _w_end =0.4;

        /// <summary>
        /// 构造函数, 初始化时候就要确定粒子数目, 一旦建立无法更改
        /// </summary>
        /// <param name="particleCount"></param>
        public ParticleSwarm(int particleCount, ISimpleMathModel targetFunction, double c1 = 2, double c2 = 2) {
            this.ParticleCount = particleCount;
            this.TargetFunction = targetFunction;
            this.Particles = new Particle[this.ParticleCount];
            for (int i = 0; i < this.ParticleCount; i++) {
                this.Particles[i] = new Particle(targetFunction);
            }
            this.c1 = c1;
            this.c2 = c2;
            this.GlobalBestPosition = this.Particles[0].BestPosition;
            this.GlobalBestFitness = this.Particles[0].BestFitness;
        }

        /// <summary>
        /// 使用指定的搜索方式和惯性因子调整策略开始搜索.
        /// 默认使用结合社会和个体认知的正常搜索方法, 默认惯性因子调整策略是惯性递减策略.
        /// 返回后. 检查GlobalBestPosition和GlobalBestFitness属性以获取最优位置和最优适应度.
        /// </summary>
        public void BeginSearching(SearchMethod searchMethod = SearchMethod.Normal, wUsingMethod wUsingMethod = wUsingMethod.Decreased) {
            var c1 = this.c1;
            var c2 = this.c2;
            var w = (this._w_ini - this._w_end) / 2;

            switch (searchMethod) {
                case SearchMethod.Normal:
                    break;
                case SearchMethod.SocialOnly:
                    c2 = 0;
                    break;
                case SearchMethod.CognitionOnly:
                    c1 = 0;
                    break;
            }

            beginSearching(c1, c2, w, wUsingMethod);
        }

        /// <summary>
        /// 实际开始搜索
        /// </summary>
        /// <param name="c1">自身学习因子</param>
        /// <param name="c2">群体学习因子</param>
        /// <param name="w">惯性因子</param>
        /// <param name="wUsingMethod">惯性因子调整策略</param>
        private void beginSearching(double c1, double c2, double w, wUsingMethod wUsingMethod) {
            int i = 0;
            while (i < this._loopTime) {
                Debug.WriteLine(string.Format("searching, loop {0}/{1}", i, this._loopTime));
                UpdateGlobalBestFitnessAndPosition();
                if (wUsingMethod != PSO.wUsingMethod.Fixed) {
                    w = Get_w(wUsingMethod, i);
                }
                UpdatePartcleStates(c1, c2, w);
                i++;
            }
        }

        /// <summary>
        /// 根据一定策略得到w的值
        /// </summary>
        /// <param name="i">迭代次数</param>
        /// <returns></returns>
        private double Get_w(wUsingMethod wUsingMethod, int i) {
            switch (wUsingMethod) {
                case wUsingMethod.Fixed:
                    return this.w;
                case wUsingMethod.NotUse:
                    return 1;
                case wUsingMethod.Decreased:
                    return (this._w_ini - this._w_end) * (this._loopTime - i) / this._loopTime + this._w_end;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 更新全局的最优位置和适应度
        /// </summary>
        protected void UpdateGlobalBestFitnessAndPosition() {
            foreach (var p in this.Particles) {
                p.UpdateBestFitnessAndPosition();
                if (p.BestFitness < this.GlobalBestFitness) {
                    this.GlobalBestPosition = p.BestPosition;
                    this.GlobalBestFitness = p.BestFitness;
                }
            }
            Debug.WriteLine(string.Format("global best fitness:{0}, best position:{1}", this.GlobalBestFitness, this.GlobalBestPosition.ToAllEnumStrings()));
        }

        /// <summary>
        /// 更新粒子的位置和速度, 默认惯性因子为1, 即不使用惯性因子
        /// </summary>
        protected void UpdatePartcleStates(double c1, double c2, double w) {
            foreach (var p in this.Particles) {
                p.UpdateState(this.GlobalBestPosition, c1, c2, w);
            }
        }
    }
}