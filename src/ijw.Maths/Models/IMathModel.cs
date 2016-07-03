using System.Collections.Generic;

namespace ijw.Maths.Models {
    public interface IMathModel : IModel<IEnumerable<double>, IEnumerable<double>> {
        int InputDimension { get; }
        int OutputDimension { get; }
    }
}
