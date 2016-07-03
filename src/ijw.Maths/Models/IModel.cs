namespace ijw.Maths.Models {
    public interface IModel<TInput, TOutput> {
        TInput Input { get; set; }
        TOutput Output { get; }
        void Calculate();
    }

    //public interface ISimpleCalculationModel : IModel<double, double> { }

    //public interface ISimpleCalculationModelWithDerivative : ISimpleCalculationModel {
    //    double Derivative { get; }
    //    void CalculateDerivative();
    //}
}
