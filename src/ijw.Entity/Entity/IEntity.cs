using System;

namespace ijw.Entity {
    public interface IEntity<TKey> {
        TKey Id { get; }
    }
}