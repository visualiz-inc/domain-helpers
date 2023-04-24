using DomainHelpers.Commons;
using DomainHelpers.Domain;
using DomainHelpers.Domain.Indentifier;
using Microsoft.EntityFrameworkCore;
using MoriFlocky.Domain.Accounts;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public class GeneralEntityRepository<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TEntity, TId>
    where TId : notnull, PrefixedUlid, new() {
    private readonly DbContext _dbContext;
    readonly ConcurrentDictionary<TId, TEntity> _founds = new();

    public GeneralEntityRepository(DbContext dbContext) {
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) {
        try {
            _dbContext.Set<TEntity>().Add(entity);

            await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
        try {
            _dbContext.Set<TEntity>().AddRange(entities);

            await SaveChangesAsync(cancellationToken);

            return entities;
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default) {
        try {
            if (_founds.TryGetValue(entity.Id, out var found)) {
                RecursiveDetach(_dbContext, found);
            }

            _dbContext.Set<TEntity>().Update(entity);
            await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task SaveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
        try {
            _dbContext.Set<TEntity>().UpdateRange(entities);

            await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default) {
        try {
            _dbContext.Set<TEntity>().Remove(entity);

            await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(TId id, CancellationToken cancellationToken = default) {
        try {
            var entity = await FindAsync(id);
            if (entity is not null) {
                _dbContext.Set<TEntity>().Remove(entity);
                await SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) {
        try {
            _dbContext.Set<TEntity>().RemoveRange(entities);

            await SaveChangesAsync(cancellationToken);
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        var entries = await _dbContext.SaveChangesAsync(cancellationToken);

        return entries;
    }

    /// <inheritdoc/>
    public virtual async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default) {
        try {
            if (await _dbContext.Set<TEntity>().FindAsync(
                new object[] { id },
                cancellationToken: cancellationToken
            ) is { } entity) {
                if (_founds.TryAdd(id, entity) is false) {
                    _dbContext.Entry(_founds[id]).State = EntityState.Detached;
                    _founds[id] = entity;
                }

                return entity;
            }

            return null;
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    public static void RecursiveDetach(DbContext context, object entity, bool isTopLevel = true) {
        if (entity == null) return;

        var entityType = entity.GetType();
        var entry = context.Entry(entity);
        if (entry.State is not EntityState.Detached && isTopLevel) {
            entry.State = EntityState.Detached;
        }

        // Get all navigation properties
        var navigationProperties = entityType.GetProperties()
            .Where(p => p.PropertyType.IsGenericType &&
                        p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>));

        // Detach entities in the navigation properties
        foreach (var navProp in navigationProperties) {
            var relatedEntities = (IEnumerable?)navProp.GetValue(entity);
            if (relatedEntities == null) continue;

            var relatedEntityType = navProp.PropertyType.GetGenericArguments()[0];

            foreach (var relatedEntity in relatedEntities) {
                context.Entry(relatedEntity).State = EntityState.Deleted;

                RecursiveDetach(context, entity, false);
            }
        }
    }
}
