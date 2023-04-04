using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public static class DbContextExtension {
    public static async Task<TEntity?> FindByIdAndDetachAsync<TEntity>(this DbSet<TEntity> dbset, string id) where TEntity : class {
        if (await dbset.FindAsync(id) is { } e) {
            dbset.Entry(e).State = EntityState.Detached;
            return e;
        }

        return null;
    }
}
