using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ijw;
using ijw.Data.Samples;
using ijw.Collection;
using System.Diagnostics;
using ijw.Data.Filter;

namespace ijw.ANN.BP {
    public class BPTrainer {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BPTrainer() {
            this.DesireError = 0.001;
            this.LearningRate = 0.5;
            this.TrainingTimes = 1;
        }

        /// <summary>
        /// 欲训练的BP网络
        /// </summary>
        public BPNet Network { get; set; }

        /// <summary>
        /// 训练样本集
        /// </summary>
        public SampleCollection TrainingSamples { get; set; }

        /// <summary>
        /// 泛化样本集
        /// </summary>
        public IEnumerable<Sample> GeneralizingSamples { get; set; }

        /// <summary>
        /// 期望误差
        /// </summary>
        public double DesireError { get; set; }

        /// <summary>
        /// 学习速率
        /// </summary>
        public double LearningRate { get; set; }

        /// <summary>
        /// 训练次数
        /// </summary>
        public int TrainingTimes { get; set; }

        /// <summary>
        /// 是否是批训练模式
        /// </summary>
        public bool IsBatchMode { get; set; }

        /// <summary>
        /// 训练进度报告事件
        /// </summary>
        public event EventHandler<BPTrainedEventArgs> TrainedEvent;

        public MaxMinNormalizer Normalizer { get; set; }

        /// <summary>
        /// 训练网络
        /// </summary>
        public void Train() {
            CheckSamplesCountAndDimension(this.TrainingSamples);
            //训练预定的次数
            for (int i = 0; i < this.TrainingTimes; i++) {
                if (IsConveged(Train(TrainingSamples))) { //如果收敛, 停止训练
                    //if (IsConveged(Train(TrainingSamples.GetRandomSequence()))) { //如果收敛, 停止训练
                    return;
                }
            }
        }

        /// <summary>
        /// 训练网络，异步进行
        /// </summary>
        /// <param name="cancellationToken">取消标记</param>
        /// <param name="progress">进度报告</param>
        /// <returns></returns>
        public Task TrainAsync(CancellationToken cancellationToken, IProgress<double> progress) {
            CheckSamplesCountAndDimension(this.TrainingSamples);

            var task = Task.Run(() => {
                //Stopwatch st = new Stopwatch();
                //训练预定的次数
                for (int i = 0; i < this.TrainingTimes; i++) {
                    //Debug.WriteLine("Start training " + i.ToString() + "/" + this.TrainingTimes.ToString());
                    //st.Start();
                    var overallError = Train(TrainingSamples, cancellationToken, progress);
                    if (IsConveged(overallError) || cancellationToken.IsCancellationRequested) { //如果收敛, 停止训练
                        return;
                    }
                    //st.Stop();
                    //Debug.WriteLine(st.Elapsed.ToString());
                    //st.Reset();
                }
            });

            return task;
        }

        /// <summary>
        /// 泛化
        /// </summary>
        public IEnumerable<double> Generalize() {
            foreach (var item in this.GeneralizingSamples) {
                this.Network.Input = item.Input;
                this.Network.DesireOutput = item.Output;
                yield return this.Network.GetRelativeError();
            }
        }

        /// <summary>
        /// 用一个样本集合进行一次网络训练
        /// </summary>
        /// <param name="samples">样本集合</param>
        protected double Train(SampleCollection samples, CancellationToken cancellationToken = default(CancellationToken), IProgress<double> progress = null) {
            Debug.Write("start training sample collection...");
            // Debug.Write("checking sampless dimension...");
            //Stopwatch st = new Stopwatch();
            //st.Reset();
            //  st.Start();

            //检查样本集合没问题
            CheckSamplesCountAndDimension(samples);
            //   st.Stop();
            //   Debug.WriteLine("done." + st.Elapsed.ToString());

            //误差初始化为 0
            double error = 0;
            //清空缓冲区
            this.DELTAs.Clear();
            int index = -1;
            //遍历每个样本
            foreach (var s in samples) {
                //st.Reset();
                //  st.Start();
                index++;

                // Debug.WriteLine("train sample " + index.ToString());

                //如果收敛, 停止训练
                if (cancellationToken.IsCancellationRequested) return 0;

                // Debug.Write("   get weights deltas...");
                //   st.Reset();
                //   st.Start();
                //没有的话就要开始训练, 先获取权值调整量
                var deltas = GetWeightsDeltas(s.Input, s.Output);
                //  st.Stop();
                // Debug.WriteLine("done." + st.Elapsed.ToString());

                //如果是批训练模式
                if (this.IsBatchMode) {
                    //  Debug.WriteLine("   save weights deltas");

                    //就先累加起来, 更新修改量
                    this.DELTAs = CollectionHelper.ForEachPair(this.DELTAs, deltas, (D, d) => { D += d; return D; });
                }
                //如果不是
                else {
                    //     Debug.Write("   modify weights with deltas...");
                    //      st.Reset();
                    //      st.Start();
                    //立即进行权值修改
                    CollectionHelper.ForEachPair(this.Network.AllConnections, deltas, (conn, d) => { conn.Weight += d; });
                    //      st.Stop();
                    //      Debug.WriteLine("done." + st.Elapsed.ToString());

                    //CollectionHelper.ForEachPair(this.Network.AllConnections, deltas, (conn, d) =>  conn.Weight += d );
                    var errorDelta = this.Network.GetRelativeError();
                    Debug.WriteLine(errorDelta.ToString());
                    error += errorDelta;
                }
                //  st.Stop();
                //  Debug.WriteLine("done." + st.Elapsed.ToString());
            }
            //是否是批量训练模式?
            if (this.IsBatchMode) {
                //     st.Reset();
                //      st.Start();
                //      Debug.Write("modify weights with deltas...");
                //如果是, 这时需要调整权值, 并计算一下最终误差
                CollectionHelper.ForEachPair(this.Network.AllConnections, this.DELTAs, (conn, d) => { conn.Weight += d; });
                //      st.Stop();
                //      Debug.WriteLine("done." + st.Elapsed.ToString());


                //     st.Reset();
                //      st.Start();
                //      Debug.Write("get errro...");
                error = this.Network.GetRelativeError();
                //     st.Stop();
                //     Debug.WriteLine("done." + st.Elapsed.ToString());
            }
            else {
                //否则就计算一下平均误差
                error /= samples.Count();
            }


            if (double.IsNaN(error) || error > 100000) {
                error = 50000;
            }

            //    st.Reset();
            //    st.Start();
            //    Debug.Write("report error...");

            //报告一下进度(异步)
            if (progress != null) progress.Report(error);
            //   st.Stop();
            //   Debug.WriteLine("done." + st.Elapsed.ToString());

            //触发一下事件(同步)
            RaiseTrainedEvent(error);

            //返回误差
            //st.Stop();
            //      Debug.WriteLine("sc training done." + st.Elapsed.ToString());
            return error;
        }

        private IEnumerable<double> GetWeightsDeltas(IEnumerable<double> inputSample, IEnumerable<double> outputSample) {
            //设定输入值和期望的输出值
            this.Network.Input = inputSample;
            this.Network.DesireOutput = outputSample;

            List<double> d = new List<double>();

            //得到每条连接权值的调整量, 暂不修改
            foreach (var conn in this.Network.AllConnections) {
                d.Add(getDELTA(conn));
            }

            return d;
        }

        /// <summary>
        /// 计算网络是否收敛
        /// </summary>
        /// <returns></returns>
        private bool IsConveged(double overallError) {
            return overallError < DesireError;
        }

        /// <summary>
        /// 节点调整量缓存区,小delta
        /// </summary>
        protected Dictionary<IBPTraining, double> deltaMemory = new Dictionary<IBPTraining, double>();

        /// <summary>
        /// 连接权值调整量缓存,大delta
        /// </summary>
        protected List<double> DELTAs = new List<double>();

        /// <summary>
        /// 检查样本集, 确保样本集中有样本, 并且输入输出的维度和待训练网络一致
        /// </summary>
        /// <param name="samples"></param>
        private void CheckSamplesCountAndDimension(SampleCollection samples) {
            if (samples == null || samples.Count() == 0) {
                throw new NoSamplesException();
            }
            if (samples.InputDimension == 0 || samples.InputDimension != this.Network.InputDimension) {
                throw new InputCountWrongException();
            }
            if (samples.OutputDimension == 0 || samples.OutputDimension != this.Network.OutputDimension) {
                throw new OutputCountWrongException();
            }
        }

        private void RaiseTrainedEvent(double error) {
            if (this.TrainedEvent != null) {
                //   this.TrainedEvent(this, new TrainedEventArgs(this.Network.GetRelativeError()));
                //this.TrainedEvent(this, new BPTrainedEventArgs(this.Network.Output.First()));
                this.TrainedEvent(this, new BPTrainedEventArgs(error));
            }
        }

        /// <summary>
        /// 得到权值调整量DELTA
        /// </summary>
        /// <param name="conn">连接</param>
        /// <returns></returns>
        private double getDELTA(BPConnection conn) {
            //var model = conn.To as ISimpleCalculationModelWithDerivative;
            //if (model == null) {
            //    throw new Exception();
            //}
            //model.CalculateDerivative(model.Input);
            //return -(getDelta(conn.To) * model.Derivative * conn.Value * this.LearningRate);

            var nextNode = conn.To as IBPTraining;
            if (nextNode == null) {
                throw new WrongNodeTyoeException(conn.To);
            }
            double delta = nextNode.GetDelta();
            //double delta = getDelta(nextNode);
            return delta * conn.Value * this.LearningRate;
        }

        //private double getDelta(BPNonInputNodeBase nextNode) {
        //    double delta;
        //    if (this.deltaMemory.TryGetValue(nextNode, out delta)) {
        //        return delta;
        //    }
        //    delta = nextNode.GetDelta();
        //    this.deltaMemory.Add(nextNode, delta);
        //    return delta;
        //}
    }
}
