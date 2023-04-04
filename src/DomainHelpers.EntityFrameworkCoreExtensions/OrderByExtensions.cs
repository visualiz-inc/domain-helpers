using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.EntityFrameworkCoreExtensions;

public enum SortDirection {
    None,
    Asc,
    Desc,
}

public static class OrderByExtensions {
    public static IQueryable<T> Sort<T,TSortProperty>(this IQueryable< T> query,IEnumerable<(TSortProperty, SortDirection)> sorts) {

        var q = query;
        foreach(var s in sorts) {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, "name");
            var orderByLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            q = q.OrderBy(orderByLambda);
        }

        return q;
    }
}
