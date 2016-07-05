using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.ANN {
    public interface IRecieve {
        IEnumerable<IConnection> InConnections { get; }
        void AddRecieve(IConnection connection);
        void RemoveRecieve(IConnection connection);
    }
}