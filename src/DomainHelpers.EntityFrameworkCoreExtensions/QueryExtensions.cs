using Microsoft.EntityFrameworkCore;
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
    public static IQueryable<T> FilterOr<T, U>(this IQueryable<T> q, Expression<Func<T, U>> selector, IReadOnlyCollection<U>? values) {
        if (values == null || values.Count == 0) {
            return q;
        }

        var parameter = selector.Parameters.Single();
        var lambda = Expression.Lambda<Func<T, bool>>(
            values
                .Select(value => Expression.Equal(selector.Body, Expression.Constant(value, typeof(U))))
                .Aggregate(Expression.OrElse),
            parameter
        );

        return q.Where(lambda);
    }

    public static IQueryable<T> LikeOr<T>(
          this IQueryable<T> query,
          Expression<Func<T, string>> selector,
          IReadOnlyCollection<string>? values) {
        if (values == null || !values.Any()) {
            return query;
        }

        // Create the initial predicate with the first value to ensure a non-null seed for Aggregate.
        var predicate = values
            .Aggregate(
                (Expression<Func<T, bool>>)(_ => false),
                (current, value) => current.Or(GetLikePredicate(selector, value))
            );

        return query.Where(predicate);
    }

    private static Expression<Func<T, bool>> GetLikePredicate<T>(
        Expression<Func<T, string>> selector,
        string value
    ) {
        var parameter = selector.Parameters.First();
        var body = Expression.Call(
            null,
            typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like),
                [
                    typeof(DbFunctions),
                    typeof(string),
                    typeof(string)
                ]
            )!,
            Expression.Property(null, typeof(EF), nameof(EF.Functions)),
            selector.Body,
            Expression.Constant($"%{value}%")
        );

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) {
        var parameter = Expression.Parameter(typeof(T));

        var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
        var left = leftVisitor.Visit(expr1.Body);

        var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
        var right = rightVisitor.Visit(expr2.Body);

        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);
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

    private class ReplaceExpressionVisitor(Expression oldValue, Expression newValue) : ExpressionVisitor {
        public override Expression Visit(Expression node) {
            if (node == oldValue)
                return newValue;

            return base.Visit(node);
        }
    }
}