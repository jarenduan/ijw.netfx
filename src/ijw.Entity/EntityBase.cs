using System;

namespace ijw.Entity {
    public abstract class EntityBase : NotifyPropertyChangeBase {
        public Guid Id { get; protected set; }

        public EntityBase() => this.Id = new Guid();

        public EntityBase(Guid id) => this.Id = id;
    }
}
