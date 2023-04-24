using DomainHelpers.Domain;
using DomainHelpers.Domain.Indentifier;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MoriFlocky.Application.Domain.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public static class UseImmutableEntityOnModelBuilderExtensions {
    public static EntityTypeBuilder<TEntity> UseDomainEntity<TEntity, TId>(this EntityTypeBuilder<TEntity> entity)
        where TEntity : Entity
        where TId : PrefixedUlid, new() {
        entity.HasKey(nameof(Entity<TEntity, TId>.Id));
        entity.Property<TId>(nameof(Entity<TEntity, TId>.Id))
            .HasConversion(id => id.ToString(), id => PrefixedUlid.Parse<TId>(id))
            .HasValueGenerator<IdGenerator<TId>>();

        return entity;
    }

    static int TotalLength(Type type) {
        var totalFieldInfo = type.GetField("TotalLength", BindingFlags.Public | BindingFlags.Static);
        if (totalFieldInfo is { } info && totalFieldInfo.IsLiteral && totalFieldInfo.IsInitOnly is false) {
            return (int)info.GetValue(null);
        }

        return 0;
    }
}

public class IdGenerator<TId> : ValueGenerator<TId> where TId : PrefixedUlid, new() {
    public override TId Next(EntityEntry entry) {
        if (entry == null) {
            throw new ArgumentNullException(nameof(entry));
        }

        return PrefixedUlid.NewPrefixedUlid<TId>();
    }

    public override bool GeneratesTemporaryValues { get; }
}