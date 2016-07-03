using System.Collections.Generic;
using System.Linq;

namespace ijw.Maths.Models {
    /// <summary>
    /// 默认实现
    /// </summary>
    public abstract class ModelBase : IMathModel {
        private double[] _input;
        public IEnumerable<double> Input {
            get {
                return this._input;
            }
            set {
                if (value.Count() != this._input.Length)
                    throw new DimensionNotMatchException();
                this._input = value.ToArray();
            }
        }

        private double[] _output;
        public IEnumerable<double> Output {
            get { return this._output; }
        }

        public int InputDimension { get { return this._input.Length; } }
        public int OutputDimension { get { return this._output.Length; } }

        public ModelBase(int inputDimension, int outputDimension) {
            this._input = new double[inputDimension];
            this._output = new double[outputDimension];
        }

        public abstract void Calculate();
    }
}
