namespace ijw.Maths.Functions {
    public interface IMathFunctionWithDerivative : IFunction<double, double> {
        double CalculateDerivative(double input);
    }
}
