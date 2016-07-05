using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ijw.Maths.Functions;

namespace ijw.ANN.BP {
    public class BPNetBuilder {

        protected Random _randomMaker;

        //输入层节点数
        public int InputNodeCount { get; private set; }
        //隐含层数
        public int hiddenLayerCount { get { return this.HiddenNodeCount.Count(); } }
        //各隐含层的节点数
        public int[] HiddenNodeCount { get; private set; }
        //输出层节点数
        public int OutputNodeCount { get; private set; }

        public OutputNodeActivationFunction OutputFunction { get; private set; }
        public WeightGenerating  WeightGen { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inputNodeCount">输入层节点数</param>
        /// <param name="hiddenNodeCount">各隐含层的节点数</param>
        /// <param name="outputNodeCount">输出层节点数</param>
        public BPNetBuilder(int inputNodeCount, int[] hiddenNodeCount, int outputNodeCount, OutputNodeActivationFunction outputFunction = OutputNodeActivationFunction.Linear, WeightGenerating weightGen = WeightGenerating.ZeroToOne)
        {
            //没有输入层节点是异常
            if (inputNodeCount == 0) throw new NoInputNodeException();

            //没有输出层节点是异常
            if (outputNodeCount == 0) throw new NoOutputNodeException();

            //隐含层节点数的数组不能为空
            if (hiddenNodeCount == null) throw new NullReferenceException();

            //赋值
            this.InputNodeCount = inputNodeCount;
            this.HiddenNodeCount = hiddenNodeCount;
            this.OutputNodeCount = outputNodeCount;
            this.OutputFunction = outputFunction;
            this.WeightGen = weightGen;

            //初始化随机数生成器
            this._randomMaker = new Random();
        }

        /// <summary>
        /// 创建BP网络
        /// </summary>
        /// <returns>创建好的BP网络</returns>
        public BPNet Build() {
            BPNet net = new BPNet();
            CreateInputLayer(net);
            CreateHiddenLayers(net);
            CreateOutputLayer(net);
            ConnectAllNodesBetweenLayers(net);
            return net;
        }

        /// <summary>
        /// 为BP网络创建输入层
        /// </summary>
        /// <param name="net">BP网络</param>
        private void CreateInputLayer(BPNet net) {
            BPInputNode[] inputNodes = new BPInputNode[InputNodeCount];
            for (int i = 0; i < InputNodeCount; i++) {
                inputNodes[i] = new BPInputNode();
            }
            net.InputLayer = inputNodes;
        }

        /// <summary>
        /// 为BP网络创建隐含层
        /// </summary>
        /// <param name="net">BP网络</param>
        private void CreateHiddenLayers(BPNet net) {
            BPHiddenNode[][] hiddennodes = new BPHiddenNode[hiddenLayerCount][];
            for(int i = 0; i < this.hiddenLayerCount; i++) {
                int NodeCountOfCurrentHiddenLayer = this.HiddenNodeCount[i];
                hiddennodes[i] = new BPHiddenNode[NodeCountOfCurrentHiddenLayer];
                for(int j = 0; j < this.HiddenNodeCount[i]; j++) {
                    //设置sigmod函数和随机阈值的节点
                    hiddennodes[i][j] = new BPHiddenNode(new SigmodFunction(), this._randomMaker.NextDouble());
                }
            }
            net.HiddenLayers = hiddennodes;
        }

        /// <summary>
        /// 为BP网络创建输出层
        /// </summary>
        /// <param name="net">bp网络</param>
        private void CreateOutputLayer(BPNet net) {
            BPOutputNode[] outputNodes = new BPOutputNode[this.OutputNodeCount];
            for (int i = 0; i < this.OutputNodeCount; i++) {
                IMathFunctionWithDerivative m = null;
                switch (this.OutputFunction)
                {
                    case OutputNodeActivationFunction.Linear:
                        m = new LinearFunction();
                        break;
                    case OutputNodeActivationFunction.Sigmod:
                        m = new SigmodFunction();
                        break;
                    default:
                        break;
                }
                outputNodes[i] = new BPOutputNode(m,this._randomMaker.NextDouble());
            }
            net.OutputLayer = outputNodes;
        }

        /// <summary>
        /// 将BP网络每相邻两层之间的节点进行连接
        /// </summary>
        /// <param name="net">尚未进行节点连接的BP网络</param>
        private void ConnectAllNodesBetweenLayers(BPNet net) {
            //组织一个发送层集合
            List<IEnumerable<ISend>> sendLayers = new List<IEnumerable<ISend>>();
            sendLayers.Add(net.InputLayer);
            sendLayers.AddRange(net.HiddenLayers);

            //组织一个接收层集合
            List<IEnumerable<IRecieve>> recieveLayers = new List<IEnumerable<IRecieve>>();
            recieveLayers.AddRange(net.HiddenLayers);
            recieveLayers.Add(net.OutputLayer);

            //将发送层和接受层中的节点连接起来
            for (int i = 0; i < this.hiddenLayerCount + 1; i++) { //共 ( this.hiddenLayerCount + 1) 对组合
                foreach (var send in sendLayers[i]) {
                    foreach (var recieve in recieveLayers[i]) {
                        BPConnection conn = new BPConnection();
                        switch (this.WeightGen)
                        {
                            case WeightGenerating.ZeroToOne:
                                conn.Weight = this._randomMaker.NextDouble();
                                break;
                            case WeightGenerating.BetweenPositiveAndNegativePoint7:
                                conn.Weight = (this._randomMaker.NextDouble() - 0.5) * 1.4;
                                break;
                            default:
                                break;
                        }
                        conn.ConnectNodes(send, recieve);  //加入到网络中
                        net._allConnections.Add(conn);
                    }
                }
            }
        }
    }
}