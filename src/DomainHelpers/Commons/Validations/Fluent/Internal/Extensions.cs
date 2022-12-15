using System.Linq.Expressions;
using System.Reflection;

namespace DomainHelpers.Core.Validations.Internal;

/// <summary>
///     Useful extensions
/// </summary>
public static class Extensions {
    /// <summary>
    ///     Gets a MemberInfo from a member expression.
    /// </summary>
    public static MemberInfo? GetMember<T, TProperty>(this Expression<Func<T, TProperty>> expression) {
        if (RemoveUnary(expression.Body) is not MemberExpression memberExp) {
            return null;
        }

        var currentExpr = memberExp.Expression;

        // Unwind the expression to get the root object that the expression acts upon.
        while (true) {
            currentExpr = RemoveUnary(currentExpr);

            if (currentExpr != null && currentExpr.NodeType == ExpressionType.MemberAccess) {
                currentExpr = ((MemberExpression)currentExpr).Expression;
            }
            else {
                break;
            }
        }

        if (currentExpr == null || currentExpr.NodeType != ExpressionType.Parameter) {
            return null; // We don't care if we're not acting upon the model instance.
        }

        return memberExp.Member;
    }

    private static Expression? RemoveUnary(Expression? toUnwrap) {
        return toUnwrap is UnaryExpression expression
            ? expression.Operand
            : toUnwrap;
    }
}