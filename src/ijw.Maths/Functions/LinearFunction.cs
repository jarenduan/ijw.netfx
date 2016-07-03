using System;
using System.Runtime.Serialization;

namespace ijw.Maths.Functions {
    public class LinearFunction : IMathFunctionWithDerivative {
        public double Calculate(double input) {
            return input;
        }

        public double CalculateDerivative(double input) {
            return 1;
        }
    }
}
