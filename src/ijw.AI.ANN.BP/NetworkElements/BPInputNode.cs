using System;
using ijw.Maths.Functions;
using ijw.AI.ANN.Base;
using System.Runtime.Serialization;

namespace ijw.AI.ANN.BP {
    public class BPInputNode : InputNodeBase {
        public BPInputNode()
            : base(new LinearFunction()) { }
    }
}