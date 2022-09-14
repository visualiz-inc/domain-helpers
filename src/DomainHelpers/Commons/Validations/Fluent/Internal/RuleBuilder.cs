using DomainHelpers.Core.Validations.Validators;

namespace DomainHelpers.Core.Validations.Internal; 

/// <summary>
///     Builds a validation rule and constructs a validator.
/// </summary>
/// <typeparam name="T">Type of object being validated</typeparam>
/// <typeparam name="TProperty">Type of property being validated</typeparam>
internal class RuleBuilder<T, TProperty> : IRuleBuilderOptions<T, TProperty>, IRuleBuilderInitial<T, TProperty>,
    IRuleBuilderInitialCollection<T, TProperty>, IRuleBuilderOptionsConditions<T, TProperty>,
    IRuleBuilderInternal<T, TProperty> {
    /// <summary>
    ///     Creates a new instance of the <see cref="RuleBuilder{T,TProperty}">RuleBuilder</see> class.
    /// </summary>
    public RuleBuilder(IValidationRuleInternal<T, TProperty> rule, AbstractValidator<T> parent) {
        Rule = rule;
        ParentValidator = parent;
    }

    /// <summary>
    ///     The rule being created by this RuleBuilder.
    /// </summary>
    public IValidationRuleInternal<T, TProperty> Rule { get; }

    /// <summary>
    ///     Parent validator
    /// </summary>
    public AbstractValidator<T> ParentValidator { get; }

    IValidationRule<T, TProperty> IRuleBuilderInternal<T, TProperty>.Rule => Rule;

    public IRuleBuilderOptions<T, TProperty> SetValidator(IPropertyValidator<T, TProperty> validator) {
        if (validator == null) {
            throw new ArgumentNullException(nameof(validator));
        }

        Rule.AddValidator(validator);
        return this;
    }

    public IRuleBuilderOptions<T, TProperty> SetAsyncValidator(IAsyncPropertyValidator<T, TProperty> validator) {
        if (validator == null) {
            throw new ArgumentNullException(nameof(validator));
        }

        // See if the async validator supports synchronous execution too.
        IPropertyValidator<T, TProperty> fallback = validator as IPropertyValidator<T, TProperty>;
        Rule.AddAsyncValidator(validator, fallback);
        return this;
    }

    public IRuleBuilderOptions<T, TProperty>
        SetValidator(IValidator<TProperty> validator, params string[] ruleSets) {
        validator.Guard("Cannot pass a null validator to SetValidator", nameof(validator));
        ChildValidatorAdaptor<T, TProperty> adaptor =
            new ChildValidatorAdaptor<T, TProperty>(validator, validator.GetType()) { RuleSets = ruleSets };
        // ChildValidatorAdaptor supports both sync and async execution.
        Rule.AddAsyncValidator(adaptor, adaptor);
        return this;
    }

    public IRuleBuilderOptions<T, TProperty> SetValidator<TValidator>(Func<T, TValidator> validatorProvider,
        params string[] ruleSets) where TValidator : IValidator<TProperty> {
        validatorProvider.Guard("Cannot pass a null validatorProvider to SetValidator", nameof(validatorProvider));
        ChildValidatorAdaptor<T, TProperty> adaptor =
            new ChildValidatorAdaptor<T, TProperty>((context, _) => validatorProvider(context.InstanceToValidate),
                typeof(TValidator)) { RuleSets = ruleSets };
        // ChildValidatorAdaptor supports both sync and async execution.
        Rule.AddAsyncValidator(adaptor, adaptor);
        return this;
    }

    public IRuleBuilderOptions<T, TProperty> SetValidator<TValidator>(
        Func<T, TProperty, TValidator> validatorProvider, params string[] ruleSets)
        where TValidator : IValidator<TProperty> {
        validatorProvider.Guard("Cannot pass a null validatorProvider to SetValidator", nameof(validatorProvider));
        ChildValidatorAdaptor<T, TProperty> adaptor =
            new ChildValidatorAdaptor<T, TProperty>(
                (context, val) => validatorProvider(context.InstanceToValidate, val), typeof(TValidator)) {
                RuleSets = ruleSets
            };
        // ChildValidatorAdaptor supports both sync and async execution.
        Rule.AddAsyncValidator(adaptor, adaptor);
        return this;
    }

    IRuleBuilderOptions<T, TProperty> IRuleBuilderOptions<T, TProperty>.DependentRules(Action action) {
        List<IValidationRuleInternal<T>> dependencyContainer = new List<IValidationRuleInternal<T>>();
        // Capture any rules added to the parent validator inside this delegate.
        using (ParentValidator.Rules.Capture(dependencyContainer.Add)) {
            action();
        }

        if (Rule.RuleSets != null && Rule.RuleSets.Length > 0) {
            foreach (IValidationRuleInternal<T> dependentRule in dependencyContainer) {
                if (dependentRule.RuleSets == null) {
                    dependentRule.RuleSets = Rule.RuleSets;
                }
            }
        }

        Rule.AddDependentRules(dependencyContainer);
        return this;
    }

    public void AddComponent(RuleComponent<T, TProperty> component) {
        Rule.Components.Add(component);
    }
}