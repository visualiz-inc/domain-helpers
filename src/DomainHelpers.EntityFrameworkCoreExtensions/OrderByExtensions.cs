using System.Linq.Expressions;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public enum SortDirection {
    None,
    Asc,
    Desc,
}

public static class OrderByExtensions {
    public static IQueryable<T> SortOrder<T, U, TSortProperty>(
        this IQueryable<T> query,
        Expression<Func<T, U>> selector,
        IEnumerable<(TSortProperty, SortDirection)>? sorts
    ) where TSortProperty : Enum {
        if(sorts?.Count() is null or 0) {
            return query;
        }

        var q = query;
        var parameter = selector.Parameters.Single();
        foreach (var (prop, direction) in sorts) {
            var orderByLambda = Expression.Lambda<Func<T, object>>(
                Expression.Convert(
                    Expression.PropertyOrField(selector.Body, prop.ToString()),
                    typeof(object)
                ),
                parameter
            );

            q = direction switch {
                SortDirection.Asc => q.OrderBy(orderByLambda),
                SortDirection.Desc => q.OrderByDescending(orderByLambda),
                _ => q,
            };
        }

        return q;
    }
}
