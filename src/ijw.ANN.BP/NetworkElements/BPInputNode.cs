using System;
using ijw.Maths.Functions;
using ijw.ANN.Base;
using System.Runtime.Serialization;

namespace ijw.ANN.BP {
    public class BPInputNode : InputNodeBase {
        public BPInputNode()
            : base(new LinearFunction()) { }
    }
}