using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.AI.ANN {
    public interface IConnection {
        ISend From { get; set; }
        IRecieve To { get; set; }
        double Value { get; }
        double Weight { get; set; }
        void ConnectNodes(ISend ni, IRecieve nh);
    }
}