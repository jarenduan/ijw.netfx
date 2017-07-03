using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ijw.Entity {
    public abstract class AggregateRootBase : EntityBase, IAggregateRoot<Guid> {
    }
}
