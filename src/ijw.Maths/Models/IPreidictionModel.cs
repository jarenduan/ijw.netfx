using System.Collections.Generic;

namespace ijw.Maths.Models {
    public interface IPreidictionModel : IMathModel {
        IEnumerable<double> DesireOutput { get; set; }
        double GetError();
        double GetRelativeError();
    }
}