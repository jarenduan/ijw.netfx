using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.AI.ANN.BP {
    public class BPTrainedEventArgs: EventArgs {
        private double error;

        public double RelativeError {
            get { return error; }
            set { error = value; }
        }

        public BPTrainedEventArgs(double error) {
            this.error = error;
        }
    }
}
