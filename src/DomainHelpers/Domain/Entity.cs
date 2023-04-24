using DomainHelpers.Domain.Indentifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Domain;

public class Entity {
}

public class Entity<TEntity, TId>: Entity
    where TEntity : class
    where TId : PrefixedUlid {
    public virtual  TId Id { get; init; }

    public Entity(TId id) {
        Id = id;
    }

    public Entity() { }
}

