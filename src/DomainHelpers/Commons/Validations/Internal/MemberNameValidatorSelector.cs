namespace DomainHelpers.Core.Validations.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Selects validators that are associated with a particular property.
/// </summary>
public class MemberNameValidatorSelector : IValidatorSelector {
    internal const string DisableCascadeKey = "_FV_DisableSelectorCascadeForChildRules";
    readonly IEnumerable<string> _memberNames;

    /// <summary>
    /// Creates a new instance of MemberNameValidatorSelector.
    /// </summary>
    public MemberNameValidatorSelector(IEnumerable<string> memberNames) {
        _memberNames = memberNames;
    }

    /// <summary>
    /// Member names that are validated.
    /// </summary>
    public IEnumerable<string> MemberNames => _memberNames;

    /// <summary>
    /// Determines whether or not a rule should execute.
    /// </summary>
    /// <param name="rule">The rule</param>
    /// <param name="propertyPath">Property path (eg Customer.Address.Line1)</param>
    /// <param name="context">Contextual information</param>
    /// <returns>Whether or not the validator can execute.</returns>
    public bool CanExecute(IValidationRule rule, string propertyPath, IValidationContext context) {
        // Validator selector only applies to the top level.
        // If we're running in a child context then this means that the child validator has already been selected
        // Because of this, we assume that the rule should continue (ie if the parent rule is valid, all children are valid)
        bool isChildContext = context.IsChildContext;
        bool cascadeEnabled = !context.RootContextData.ContainsKey(DisableCascadeKey);

        return (isChildContext && cascadeEnabled && !_memberNames.Any(x => x.Contains(".")))
               || rule is IIncludeRule
               || (_memberNames.Any(x => x == propertyPath || propertyPath.StartsWith(x + ".") || x.StartsWith(propertyPath + ".")));
    }

    /// <summary>
    /// Gets member names from expressions
    /// </summary>
    /// <param name="propertyExpressions"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string[] MemberNamesFromExpressions<T>(params Expression<Func<T, object>>[] propertyExpressions) {
        var members = propertyExpressions.Select(MemberFromExpression).ToArray();
        return members;
    }

    private static string MemberFromExpression<T>(Expression<Func<T, object>> expression) {
        var chain = PropertyChain.FromExpression(expression);

        if (chain.Count == 0) {
            throw new ArgumentException($"Expression '{expression}' does not specify a valid property or field.");
        }

        return chain.ToString();
    }
}
