using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.Domain;

public class Entity {
}

public class Entity<TId>: Entity
    where TId : PrefixedUlid {
    public virtual  TId Id { get; init; }

    public Entity(TId id) {
        Id = id;
    }
}

