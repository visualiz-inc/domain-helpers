using Microsoft.EntityFrameworkCore;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public static class DbContextExtension {
    public static async Task<TEntity?> FindByIdAndDetachAsync<TEntity>(this DbSet<TEntity> dbset, string id) where TEntity : class {
        if (await dbset.FindAsync(id) is { } e) {
            dbset.Entry(e).State = EntityState.Detached;
            return e;
        }

        return null;
    }

    public static async Task<TEntity?> FindByIdAsync<TEntity>(this DbSet<TEntity> dbset, object id) where TEntity : class {
        if (await dbset.FindAsync(id) is { } e) {
            return e;
        }

        return null;
    }
}