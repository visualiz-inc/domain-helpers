using DomainHelpers.Core.Validations.Results;
using DomainHelpers.Core.Validations.Validators;
using System.Linq.Expressions;
using System.Reflection;

namespace DomainHelpers.Core.Validations.Internal;

internal abstract class RuleBase<T, TProperty, TValue> : IValidationRule<T, TValue> {
    private Func<CascadeMode> _cascadeModeThunk;
    private string _displayName;
    private Func<ValidationContext<T>, string> _displayNameFactory;
    private string _propertyDisplayName;
    private string _propertyName;

    /// <summary>
    ///     Creates a new property rule.
    /// </summary>
    /// <param name="member">Property</param>
    /// <param name="propertyFunc">Function to get the property value</param>
    /// <param name="expression">Lambda expression used to create the rule</param>
    /// <param name="cascadeModeThunk">Function to get the cascade mode.</param>
    /// <param name="typeToValidate">Type to validate</param>
    public RuleBase(MemberInfo member, Func<T, TProperty> propertyFunc, LambdaExpression expression,
        Func<CascadeMode> cascadeModeThunk, Type typeToValidate) {
        Member = member;
        PropertyFunc = propertyFunc;
        Expression = expression;
        TypeToValidate = typeToValidate;
        _cascadeModeThunk = cascadeModeThunk;

        Type containerType = typeof(T);
        PropertyName = ValidatorOptions.Global.PropertyNameResolver(containerType, member, expression);
        _displayNameFactory = context =>
            ValidatorOptions.Global.DisplayNameResolver(containerType, member, expression);
    }

    public List<RuleComponent<T, TValue>> Components { get; } = new();

    /// <summary>
    ///     Condition for all validators in this rule.
    /// </summary>
    internal Func<ValidationContext<T>, bool> Condition { get; private set; }

    /// <summary>
    ///     Asynchronous condition for all validators in this rule.
    /// </summary>
    internal Func<ValidationContext<T>, CancellationToken, Task<bool>> AsyncCondition { get; private set; }

    /// <summary>
    ///     Function that can be invoked to retrieve the value of the property.
    /// </summary>
    public Func<T, TProperty> PropertyFunc { get; }

    /// <summary>
    ///     Dependent rules
    /// </summary>
    internal List<IValidationRuleInternal<T>> DependentRules { get; private protected set; }

    /// <inheritdoc />
    IEnumerable<IRuleComponent> IValidationRule.Components => Components;

    /// <summary>
    ///     Property associated with this rule.
    /// </summary>
    public MemberInfo Member { get; }

    /// <summary>
    ///     Expression that was used to create the rule.
    /// </summary>
    public LambdaExpression Expression { get; }

    /// <summary>
    ///     Sets the display name for the property.
    /// </summary>
    /// <param name="name">The property's display name</param>
    public void SetDisplayName(string name) {
        _displayName = name;
        _displayNameFactory = null;
    }

    /// <summary>
    ///     Sets the display name for the property using a function.
    /// </summary>
    /// <param name="factory">The function for building the display name</param>
    public void SetDisplayName(Func<ValidationContext<T>, string> factory) {
        if (factory == null) {
            throw new ArgumentNullException(nameof(factory));
        }

        _displayNameFactory = factory;
        _displayName = null;
    }

    /// <summary>
    ///     Rule set that this rule belongs to (if specified)
    /// </summary>
    public string[] RuleSets { get; set; }

    /// <summary>
    ///     The current rule component.
    /// </summary>
    public IRuleComponent<T, TValue> Current => Components.LastOrDefault();

    /// <summary>
    ///     Type of the property being validated
    /// </summary>
    public Type TypeToValidate { get; }

    /// <inheritdoc />
    public bool HasCondition => Condition != null;

    /// <inheritdoc />
    public bool HasAsyncCondition => AsyncCondition != null;

    /// <summary>
    ///     Cascade mode for this rule.
    /// </summary>
    public CascadeMode CascadeMode {
        get => _cascadeModeThunk();
        set => _cascadeModeThunk = () => value;
    }

    public void AddValidator(IPropertyValidator<T, TValue> validator) {
        RuleComponent<T, TValue> component = new(validator);
        Components.Add(component);
    }

    /// <summary>
    ///     Returns the property name for the property being validated.
    ///     Returns null if it is not a property being validated (eg a method call)
    /// </summary>
    public string PropertyName {
        get => _propertyName;
        set {
            _propertyName = value;
            _propertyDisplayName = _propertyName.SplitPascalCase();
        }
    }

    /// <summary>
    ///     Allows custom creation of an error message
    /// </summary>
    public Func<IMessageBuilderContext<T, TValue>, string> MessageBuilder { get; set; }

    IEnumerable<IValidationRule> IValidationRule.DependentRules => DependentRules;

    string IValidationRule.GetDisplayName(IValidationContext context) {
        return GetDisplayName(context != null ? ValidationContext<T>.GetFromNonGenericContext(context) : null);
    }

    /// <summary>
    ///     Applies a condition to the rule
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="applyConditionTo"></param>
    public void ApplyCondition(Func<ValidationContext<T>, bool> predicate,
        ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators) {
        // Default behaviour for When/Unless as of v1.3 is to apply the condition to all previous validators in the chain.
        if (applyConditionTo == ApplyConditionTo.AllValidators) {
            foreach (RuleComponent<T, TValue> validator in Components) {
                validator.ApplyCondition(predicate);
            }

            if (DependentRules != null) {
                foreach (IValidationRuleInternal<T> dependentRule in DependentRules) {
                    dependentRule.ApplyCondition(predicate, applyConditionTo);
                }
            }
        }
        else {
            Current.ApplyCondition(predicate);
        }
    }

    /// <summary>
    ///     Applies the condition to the rule asynchronously
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="applyConditionTo"></param>
    public void ApplyAsyncCondition(Func<ValidationContext<T>, CancellationToken, Task<bool>> predicate,
        ApplyConditionTo applyConditionTo = ApplyConditionTo.AllValidators) {
        // Default behaviour for When/Unless as of v1.3 is to apply the condition to all previous validators in the chain.
        if (applyConditionTo == ApplyConditionTo.AllValidators) {
            foreach (RuleComponent<T, TValue> validator in Components) {
                validator.ApplyAsyncCondition(predicate);
            }

            if (DependentRules != null) {
                foreach (IValidationRuleInternal<T> dependentRule in DependentRules) {
                    dependentRule.ApplyAsyncCondition(predicate, applyConditionTo);
                }
            }
        }
        else {
            Current.ApplyAsyncCondition(predicate);
        }
    }

    public void ApplySharedCondition(Func<ValidationContext<T>, bool> condition) {
        if (Condition == null) {
            Condition = condition;
        }
        else {
            Func<ValidationContext<T>, bool> original = Condition;
            Condition = ctx => condition(ctx) && original(ctx);
        }
    }

    public void ApplySharedAsyncCondition(Func<ValidationContext<T>, CancellationToken, Task<bool>> condition) {
        if (AsyncCondition == null) {
            AsyncCondition = condition;
        }
        else {
            Func<ValidationContext<T>, CancellationToken, Task<bool>> original = AsyncCondition;
            AsyncCondition = async (ctx, ct) => await condition(ctx, ct) && await original(ctx, ct);
        }
    }

    object IValidationRule<T>.GetPropertyValue(T instance) {
        return PropertyFunc(instance);
    }

    public void AddAsyncValidator(IAsyncPropertyValidator<T, TValue> asyncValidator,
        IPropertyValidator<T, TValue> fallback = null) {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Clear all validators from this rule.
    /// </summary>
    public void ClearValidators() {
        Components.Clear();
    }

    /// <summary>
    ///     Display name for the property.
    /// </summary>
    public string GetDisplayName(ValidationContext<T> context) {
        return _displayNameFactory?.Invoke(context) ?? _displayName ?? _propertyDisplayName;
    }

    /// <summary>
    ///     Prepares the <see cref="MessageFormatter" /> of <paramref name="context" /> for an upcoming
    ///     <see cref="ValidationFailure" />.
    /// </summary>
    /// <param name="context">The validator context</param>
    /// <param name="value">Property value.</param>
    protected void PrepareMessageFormatterForValidationError(ValidationContext<T> context, TValue value) {
        context.MessageFormatter.AppendPropertyName(context.DisplayName);
        context.MessageFormatter.AppendPropertyValue(value);

        // If there's a collection index cached in the root context data then add it
        // to the message formatter. This happens when a child validator is executed
        // as part of a call to RuleForEach. Usually parameters are not flowed through to
        // child validators, but we make an exception for collection indices.
        if (context.RootContextData.TryGetValue("__FV_CollectionIndex", out object? index)) {
            // If our property validator has explicitly added a placeholder for the collection index
            // don't overwrite it with the cached version.
            if (!context.MessageFormatter.PlaceholderValues.ContainsKey("CollectionIndex")) {
                context.MessageFormatter.AppendArgument("CollectionIndex", index);
            }
        }
    }

    /// <summary>
    ///     Creates an error validation result for this validator.
    /// </summary>
    /// <param name="context">The validator context</param>
    /// <param name="value">The property value</param>
    /// <param name="component">The current rule component.</param>
    /// <returns>Returns an error validation result.</returns>
    protected ValidationFailure CreateValidationError(ValidationContext<T> context, TValue value,
        RuleComponent<T, TValue> component) {
        string error = MessageBuilder != null
            ? MessageBuilder(new MessageBuilderContext<T, TValue>(context, value, component))
            : component.GetErrorMessage(context, value);

        ValidationFailure failure = new(context.PropertyName, error, value);

        failure.FormattedMessagePlaceholderValues =
            new Dictionary<string, object>(context.MessageFormatter.PlaceholderValues);
        failure.ErrorCode = component.ErrorCode ?? ValidatorOptions.Global.ErrorCodeResolver(component.Validator);

        failure.Severity = component.SeverityProvider != null
            ? component.SeverityProvider(context, value)
            : ValidatorOptions.Global.Severity;

        if (component.CustomStateProvider != null) {
            failure.CustomState = component.CustomStateProvider(context, value);
        }

        return failure;
    }
}