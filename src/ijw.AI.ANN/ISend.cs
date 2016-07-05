using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.AI.ANN {
    public interface ISend{
        IEnumerable<IConnection> OutConnections {get;}
        void AddSend(IConnection connection);
        void RemoveSend(IConnection connection);
        double GetValueByConn(IConnection connection);
    }
}
