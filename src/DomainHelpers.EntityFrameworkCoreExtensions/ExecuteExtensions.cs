using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public static class ExecuteExtensions {
    public static async Task<IReadOnlyList<T>> ExecuteQueryAsync<T>(this IQueryable<T> table) {
        return await table.ToArrayAsync();
    }

    public static async Task<IReadOnlyList<U>> ExecuteQueryAsync<T, U>(
        this IQueryable<T> table,
        Func<T, U> func
    ) {
        var arr = await table.ToArrayAsync();
        return arr.Select(func).ToArray();
    }

    public static async Task<IReadOnlyList<U>> ExecuteQueryAsync<T, U>(
        this IQueryable<T> table,
        Func<T, Task<U>> func
    ) {
        var arr = await table.ToArrayAsync();

        var results = new List<U>();
        foreach (var item in arr) {
            results.Add(await func(item));
        }

        return results.ToArray();
    }
}