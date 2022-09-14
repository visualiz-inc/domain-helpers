using DomainHelpers.Core.Validations.Results;
using System.Linq.Expressions;
using System.Reflection;

namespace DomainHelpers.Core.Validations.Internal; 

/// <summary>
///     Defines a rule associated with a property.
/// </summary>
internal class
    PropertyRule<T, TProperty> : RuleBase<T, TProperty, TProperty>, IValidationRuleInternal<T, TProperty> {
    public PropertyRule(MemberInfo member, Func<T, TProperty> propertyFunc, LambdaExpression expression,
        Func<CascadeMode> cascadeModeThunk, Type typeToValidate)
        : base(member, propertyFunc, expression, cascadeModeThunk, typeToValidate) { }

    /// <summary>
    ///     Performs validation using a validation context and adds collected validation failures to the Context.
    /// </summary>
    /// <param name="context">Validation Context</param>
    /// <param name="useAsync">
    ///     Whether asynchronous components are allowed to execute.
    ///     This will be set to True when ValidateAsync is called on the root validator.
    ///     This will be set to False when Validate is called on the root validator.
    ///     When set to True, asynchronous components and asynchronous conditions will be executed.
    ///     When set to False, an exception will be thrown if a component can only be executed asynchronously or if a component
    ///     has an async condition associated with it.
    /// </param>
    /// <param name="cancellation"></param>
    public virtual async ValueTask ValidateAsync(ValidationContext<T> context, bool useAsync,
        CancellationToken cancellation) {
        string displayName = GetDisplayName(context);

        if (PropertyName == null && displayName == null) {
            //No name has been specified. Assume this is a model-level rule, so we should use empty string instead.
            displayName = string.Empty;
        }

        // Construct the full name of the property, taking into account overriden property names and the chain (if we're in a nested validator)
        string propertyName = context.PropertyChain.BuildPropertyName(PropertyName ?? displayName);

        // Ensure that this rule is allowed to run.
        // The validatselector has the opportunity to veto this before any of the validators execute.
        if (!context.Selector.CanExecute(this, propertyName, context)) {
            return;
        }

        if (Condition != null) {
            if (!Condition(context)) {
                return;
            }
        }

        if (AsyncCondition != null) {
            if (useAsync) {
                if (!await AsyncCondition(context, cancellation)) {
                    return;
                }
            }
            else {
                throw new AsyncValidatorInvokedSynchronouslyException();
            }
        }

        CascadeMode cascade = CascadeMode;
        Lazy<TProperty> accessor = new Lazy<TProperty>(() => PropertyFunc(context.InstanceToValidate),
            LazyThreadSafetyMode.None);
        int totalFailures = context.Failures.Count;
        context.InitializeForPropertyValidator(propertyName, GetDisplayName, PropertyName);

        // Invoke each validator and collect its results.
        foreach (RuleComponent<T, TProperty> component in Components) {
            cancellation.ThrowIfCancellationRequested();
            context.MessageFormatter.Reset();

            if (!component.InvokeCondition(context)) {
                continue;
            }

            if (component.HasAsyncCondition) {
                if (useAsync) {
                    if (!await component.InvokeAsyncCondition(context, cancellation)) {
                        continue;
                    }
                }
                else {
                    throw new AsyncValidatorInvokedSynchronouslyException();
                }
            }

            bool valid = await component.ValidateAsync(context, accessor.Value, useAsync, cancellation);

            if (!valid) {
                PrepareMessageFormatterForValidationError(context, accessor.Value);
                ValidationFailure failure = CreateValidationError(context, accessor.Value, component);
                context.Failures.Add(failure);
            }

            // If there has been at least one failure, and our CascadeMode has been set to Stop
            // then don't continue to the next rule
            if (context.Failures.Count > totalFailures && cascade == CascadeMode.Stop) {
                break;
            }
        }

        if (context.Failures.Count <= totalFailures && DependentRules != null) {
            foreach (IValidationRuleInternal<T> dependentRule in DependentRules) {
                cancellation.ThrowIfCancellationRequested();
                await dependentRule.ValidateAsync(context, useAsync, cancellation);
            }
        }
    }

    void IValidationRuleInternal<T>.AddDependentRules(IEnumerable<IValidationRuleInternal<T>> rules) {
        if (DependentRules == null) {
            DependentRules = new List<IValidationRuleInternal<T>>();
        }

        DependentRules.AddRange(rules);
    }

    /// <summary>
    ///     Creates a new property rule from a lambda expression.
    /// </summary>
    public static PropertyRule<T, TProperty> Create(Expression<Func<T, TProperty>> expression,
        Func<CascadeMode> cascadeModeThunk, bool bypassCache = false) {
        MemberInfo member = expression.GetMember();
        Func<T, TProperty> compiled = AccessorCache<T>.GetCachedAccessor(member, expression, bypassCache);
        return new PropertyRule<T, TProperty>(member, x => compiled(x), expression, cascadeModeThunk,
            typeof(TProperty));
    }

    /// <summary>
    ///     Creates a new property rule from a lambda expression.
    /// </summary>
    internal static PropertyRule<T, TProperty> Create<TOld>(Expression<Func<T, TOld>> expression,
        Func<TOld, TProperty> transformer, Func<CascadeMode> cascadeModeThunk, bool bypassCache = false) {
        MemberInfo member = expression.GetMember();
        Func<T, TOld> compiled = AccessorCache<T>.GetCachedAccessor(member, expression, bypassCache);

        TProperty PropertyFunc(T instance) {
            return transformer(compiled(instance));
        }

        return new PropertyRule<T, TProperty>(member, PropertyFunc, expression, cascadeModeThunk, typeof(TOld));
    }

    /// <summary>
    ///     Creates a new property rule from a lambda expression.
    /// </summary>
    internal static PropertyRule<T, TProperty> Create<TOld>(Expression<Func<T, TOld>> expression,
        Func<T, TOld, TProperty> transformer, Func<CascadeMode> cascadeModeThunk, bool bypassCache = false) {
        MemberInfo member = expression.GetMember();
        Func<T, TOld> compiled = AccessorCache<T>.GetCachedAccessor(member, expression, bypassCache);

        TProperty PropertyFunc(T instance) {
            return transformer(instance, compiled(instance));
        }

        return new PropertyRule<T, TProperty>(member, PropertyFunc, expression, cascadeModeThunk, typeof(TOld));
    }
}