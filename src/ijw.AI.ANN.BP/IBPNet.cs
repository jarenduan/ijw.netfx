using ijw.Data.Filter;
using System;
namespace ijw.AI.ANN.BP {
    interface IBPNet {
        System.Collections.Generic.IEnumerable<BPConnection> AllConnections { get; }
        void Calculate();
        System.Collections.Generic.IEnumerable<double> DesireOutput { get; set; }
        double GetError();
        double GetRelativeError();
        int[] HiddenCount { get; }
        System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<BPHiddenNode>> HiddenLayers { get; }
        System.Collections.Generic.IEnumerable<double> Input { get; set; }
        int InputDimension { get; }
        System.Collections.Generic.IEnumerable<BPInputNode> InputLayer { get; }
        MaxMinNormalizer Normalizer { get; }
        System.Collections.Generic.IEnumerable<double> Output { get; }
        int OutputDimension { get; }
        System.Collections.Generic.IEnumerable<BPOutputNode> OutputLayer { get; }
    }
}
