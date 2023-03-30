using System.Linq.Expressions;

namespace DomainHelpers.EntityFrameworkCoreExtensions;
public static class QueryExtensions {
    /// <summary>
    /// Filters the elements of an <see cref="IQueryable{T}"/> based on a selector and a collection of values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source.</typeparam>
    /// <typeparam name="U">The type of the result of the selector.</typeparam>
    /// <param name="q">The <see cref="IQueryable{T}"/> to filter.</param>
    /// <param name="selector">An expression to select a property to be used for filtering.</param>
    /// <param name="values">A collection of values to be used for filtering.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements from the input sequence that satisfy the filter condition.</returns>
    public static IQueryable<T> FilterOr<T, U>(this IQueryable<T> q, Expression<Func<T, U>> selector, ICollection<U>? values) {
        if (values is null or { Count: 0 }) {
            return q;
        }

        var parameter = selector.Parameters.Single();
        var orExpression = values
            .Select(value => Expression.Equal(selector.Body, Expression.Constant(value, typeof(U))))
            .Aggregate(Expression.OrElse);
        var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);

        return q.Where(lambda);
    }

    /// <summary>
    /// Filters the elements of an <see cref="IQueryable{T}"/> based on a selector and a collection of values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the source.</typeparam>
    /// <typeparam name="U">The type of the result of the selector.</typeparam>
    /// <param name="q">The <see cref="IQueryable{T}"/> to filter.</param>
    /// <param name="selector">An expression to select a property to be used for filtering.</param>
    /// <param name="values">A collection of values to be used for filtering.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements from the input sequence that satisfy the filter condition.</returns>
    public static IQueryable<T> FilterEqual<T, U>(this IQueryable<T> q, Expression<Func<T, U>> selector, U? value) {
        if (value == null) {
            return q;
        }

        var parameter = selector.Parameters.Single();
        var orExpression = Expression.Equal(selector.Body, Expression.Constant(value, typeof(U)));
        var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);

        return q.Where(lambda);
    }

    public static IQueryable<T> FilterRange<T, U>(this IQueryable<T> q, Expression<Func<T, U>> selector, U? start, U? end) {
        Expression? lowerBound = null, upperBound = null;

        if (start is not null) {
            lowerBound = Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(start, typeof(U)));
        }

        if (end is not null) {
            upperBound = Expression.LessThanOrEqual(selector.Body, Expression.Constant(end, typeof(U)));
        }

        Expression rangeExpression;
        if (lowerBound != null && upperBound != null) {
            rangeExpression = Expression.AndAlso(lowerBound, upperBound);
        }
        else if (lowerBound != null) {
            rangeExpression = lowerBound;
        }
        else if (upperBound != null) {
            rangeExpression = upperBound;
        }
        else {
            return q;
        }

        var lambda = Expression.Lambda<Func<T, bool>>(rangeExpression, selector.Parameters.Single());

        return q.Where(lambda);
    }
}
