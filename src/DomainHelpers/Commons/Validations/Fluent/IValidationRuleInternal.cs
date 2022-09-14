using DomainHelpers.Core.Validations.Internal;

namespace DomainHelpers.Core.Validations; 

internal interface IValidationRuleInternal<T> : IValidationRule<T> {
    ValueTask ValidateAsync(ValidationContext<T> context, bool useAsync, CancellationToken cancellation);

    void AddDependentRules(IEnumerable<IValidationRuleInternal<T>> rules);
}

internal interface
    IValidationRuleInternal<T, TProperty> : IValidationRule<T, TProperty>, IValidationRuleInternal<T> {
    new List<RuleComponent<T, TProperty>> Components { get; }
}