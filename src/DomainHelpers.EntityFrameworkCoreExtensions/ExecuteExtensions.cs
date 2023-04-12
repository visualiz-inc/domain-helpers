using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public static class ExecuteExtensions {
    public static async Task<ImmutableArray<T>> ExecuteQueryAsync<T>(this IQueryable<T> table) {
        var arr = await table.ToArrayAsync();
        return arr.ToImmutableArray();
    }

    public static async Task<ImmutableArray<U>> ExecuteQueryAsync<T, U>(
        this IQueryable<T> table,
        Func<T, U> func
    ) {
        var arr = await table.ToArrayAsync();
        return arr.Select(func).ToImmutableArray();
    }

    public static async Task<ImmutableArray<U>> ExecuteQueryAsync<T, U>(
        this IQueryable<T> table,
        Func<T, Task<U>> func
    ) {
        var arr = await table.ToArrayAsync();

        var results = new List<U>();
        foreach (var item in arr) {
            results.Add(await func(item));
        }

        return results.ToImmutableArray();
    }
}
