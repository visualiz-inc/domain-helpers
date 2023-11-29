using DomainHelpers.Commons;
using DomainHelpers.Domain;
using Microsoft.EntityFrameworkCore;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public class GeneralEntityRepository<TEntity, TId>(DbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
    where TId : notnull, new() {
    private readonly DbContext _dbContext = dbContext;

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
        if (entity.Id is null) {
            throw GeneralException.WithMessage("Entity Id is null", null);
        }

        try {
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
                return entity;
            }

            return null;
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<TEntity>> FindByIdsAsync(IEnumerable<TId> ids, CancellationToken cancellationToken = default) {
        try {
            var items = await _dbContext.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToArrayAsync();
            return items;
        }
        catch (Exception e) {
            throw GeneralException.WithException(e);
        }
    }
}