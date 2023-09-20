using DomainHelpers.Domain.Indentifier;

namespace DomainHelpers.Domain;

public class Entity {
}

public class Entity<TId>(TId id) : Entity
    where TId : PrefixedUlid {
    public virtual TId Id { get; init; } = id;
}

