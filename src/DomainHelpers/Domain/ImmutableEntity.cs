using DomainHelpers.Domain.Indentifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Domain;

public record ImmutableEntity {
}

public record ImmutableEntity<TEntity, TId>: ImmutableEntity
    where TEntity : class
    where TId : PrefixedUlid {
    public virtual required TId Id { get; init; }
}

