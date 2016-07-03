using System.Collections.Generic;

namespace ijw.Maths.Models {
    public interface ISimpleMathModel : IModel<IEnumerable<double>, double> {
        int InputDimension { get; }
    }
}
