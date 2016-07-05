using System;
using System.Collections.Generic;
using System.Linq;
using ijw.Collection;
using ijw.Maths.Models;
using System.Runtime.Serialization;
using ijw.Data.Filter;

namespace ijw.AI.ANN.BP {
    public class BPNet : IPreidictionModel, IMathModel, IBPNet {
        public int InputDimension {
            get { return this.InputLayer.Count(); }
        }

        public int OutputDimension {
            get { return this.OutputLayer.Count(); }
        }

        public int[] HiddenCount {
            get {
                return (from h in this.HiddenLayers select h.Count()).ToArray();
            }
        }

        public IEnumerable<double> Input {
            get {
                return from node in this.InputLayer select node.Input;
            }
            set {
                CollectionHelper.ForEachPair(this.InputLayer, value, (node, v) => { node.Input = v; });
            }
        }

        public IEnumerable<double> DesireOutput {
            get {
                return from node in this.OutputLayer select node.DesireOutput;
            }
            set {
                CollectionHelper.ForEachPair(this.OutputLayer, value, (node, v) => { node.DesireOutput = v; });
            }
        }

        public IEnumerable<double> Output {
            get {
                return from n in this.OutputLayer select n.Output;
            }
        }

        public IEnumerable<BPInputNode> InputLayer { get; internal set; }

        public IEnumerable<IEnumerable<BPHiddenNode>> HiddenLayers { get; internal set; }

        public IEnumerable<BPOutputNode> OutputLayer { get; internal set; }

        public IEnumerable<BPConnection> AllConnections { get { return this._allConnections; } }

        public MaxMinNormalizer Normalizer { get; internal set; }

        public double GetError() {
            return this.OutputLayer.Sum(node => Math.Pow((node.DesireOutput - node.Output), 2));
        }

        public double GetRelativeError() {
            var e = this.OutputLayer.Sum(node => {
                double adjustDesire = node.DesireOutput == 0 ? 0.000000001 : node.DesireOutput;
                return Math.Pow((node.DesireOutput - node.Output) / adjustDesire, 2);
            }) / this.OutputDimension;
            return Math.Sqrt(e);
        }

        public void Calculate() {
            //this method does nothing here.
            //to get result, use .Output property directly.
        }

        internal List<BPConnection> _allConnections = new List<BPConnection>();
    }
}