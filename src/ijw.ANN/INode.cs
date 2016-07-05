using System;
namespace ijw.ANN {
    public interface INode {
        double Input { get; set; }
        double Output { get; }
        double Derivative { get; }
    }
}
