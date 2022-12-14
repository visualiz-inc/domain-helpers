using DomainHelpers.Core.Validations.Internal;
using DomainHelpers.Core.Validations.Results;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DomainHelpers.Core.Validations;

/// <summary>
///     Base class for object validators.
/// </summary>
/// <typeparam name="T">The type of the object being validated</typeparam>
public abstract class AbstractValidator<T> : IValidator<T>, IEnumerable<IValidationRule> {
    private Func<CascadeMode> _classLevelCascadeMode = () => ValidatorOptions.Global.DefaultClassLevelCascadeMode;
    private Func<CascadeMode> _ruleLevelCascadeMode = () => ValidatorOptions.Global.DefaultRuleLevelCascadeMode;
    internal TrackingCollection<IValidationRuleInternal<T>> Rules { get; } = new();

    /// <summary>
    ///     <para>
    ///         Sets the cascade behaviour <i>in between</i> rules in this validator.
    ///         This overrides the default value set in <see cref="ValidatorConfiguration.DefaultClassLevelCascadeMode" />.
    ///     </para>
    ///     <para>
    ///         If set to <see cref="DomainHelpers.Core.Validations.CascadeMode.Continue" /> then all rules in the class will
    ///         execute regardless of failures.
    ///     </para>
    ///     <para>
    ///         If set to <see cref="DomainHelpers.Core.Validations.CascadeMode.Stop" /> then execution of the validator will
    ///         stop after any rule fails.
    ///     </para>
    ///     <para>
    ///         Note that cascade behaviour <i>within</i> individual rules is controlled by
    ///         <see cref="AbstractValidator{T}.RuleLevelCascadeMode" />.
    ///     </para>
    ///     <para>
    ///         This cannot be set to the deprecated
    ///         <see cref="DomainHelpers.Core.Validations.CascadeMode.StopOnFirstFailure" />.
    ///         <see cref="DomainHelpers.Core.Validations.CascadeMode.StopOnFirstFailure" />. Attempting to do so it will
    ///         actually
    ///         result in <see cref="DomainHelpers.Core.Validations.CascadeMode.Stop" /> being used.
    ///     </para>
    /// </summary>
    public CascadeMode ClassLevelCascadeMode {
        get => _classLevelCascadeMode();
        set => _classLevelCascadeMode = () => value;
    }

    /// <summary>
    ///     <para>
    ///         Sets the default cascade behaviour <i>within</i> each rule in this validator.
    ///     </para>
    ///     <para>
    ///         This overrides the default value set in <see cref="ValidatorConfiguration.DefaultRuleLevelCascadeMode" />.
    ///     </para>
    ///     <para>
    ///         It can be further overridden for specific rules by calling
    ///         <see
    ///             cref="DefaultValidatorOptions.Cascade{T, TProperty}(IRuleBuilderInitial{T, TProperty}, DomainHelpers.Core.Validations.CascadeMode)" />
    ///         .
    ///         <seealso cref="RuleBase{T, TProperty, TValue}.CascadeMode" />.
    ///     </para>
    ///     <para>
    ///         Note that cascade behaviour <i>between</i> rules is controlled by
    ///         <see cref="AbstractValidator{T}.ClassLevelCascadeMode" />.
    ///     </para>
    ///     <para>
    ///         This cannot be set to the deprecated
    ///         <see cref="DomainHelpers.Core.Validations.CascadeMode.StopOnFirstFailure" />.
    ///         <see cref="DomainHelpers.Core.Validations.CascadeMode.StopOnFirstFailure" />. Attempting to do so it will
    ///         actually
    ///         result in <see cref="DomainHelpers.Core.Validations.CascadeMode.Stop" /> being used.
    ///     </para>
    /// </summary>
    public CascadeMode RuleLevelCascadeMode {
        get => _ruleLevelCascadeMode();
        set => _ruleLevelCascadeMode = () => value;
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the collection of validation rules.
    /// </summary>
    /// <returns>
    ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public IEnumerator<IValidationRule> GetEnumerator() {
        return Rules.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    ValidationResult IValidator.Validate(IValidationContext context) {
        context.Guard("Cannot pass null to Validate", nameof(context));
        return Validate(ValidationContext<T>.GetFromNonGenericContext(context));
    }

    Task<ValidationResult> IValidator.ValidateAsync(IValidationContext context, CancellationToken cancellation) {
        context.Guard("Cannot pass null to Validate", nameof(context));
        return ValidateAsync(ValidationContext<T>.GetFromNonGenericContext(context), cancellation);
    }

    /// <summary>
    ///     Validates the specified instance
    /// </summary>
    /// <param name="instance">The object to validate</param>
    /// <returns>A ValidationResult object containing any validation failures</returns>
    public ValidationResult Validate(T instance) {
        return Validate(new ValidationContext<T>(instance, new PropertyChain(),
            ValidatorOptions.Global.ValidatorSelectors.DefaultValidatorSelectorFactory()));
    }

    /// <summary>
    ///     Validates the specified instance asynchronously
    /// </summary>
    /// <param name="instance">The object to validate</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>A ValidationResult object containing any validation failures</returns>
    public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = new()) {
        return ValidateAsync(
            new ValidationContext<T>(instance, new PropertyChain(),
                ValidatorOptions.Global.ValidatorSelectors.DefaultValidatorSelectorFactory()), cancellation);
    }

    bool IValidator.CanValidateInstancesOfType(Type type) {
        if (type == null) {
            throw new ArgumentNullException(nameof(type));
        }

        return typeof(T).IsAssignableFrom(type);
    }

    /// <summary>
    ///     Validates the specified instance.
    /// </summary>
    /// <param name="context">Validation Context</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    public virtual ValidationResult Validate(ValidationContext<T> context) {
        if (context == null) {
            throw new ArgumentNullException(nameof(context));
        }

        // Note: Sync-over-async is OK in this scenario.
        // The use of the `useAsync` parameter ensures that no async code is
        // actually run, and we're using ValueTask
        // which is optimised for synchronous execution of tasks.
        // Unlike 'real' sync-over-async, we can never run into deadlocks as we're not actually invoking anything asynchronously.
        // See RuleComponent.ValidateAsync for the lowest level.
        // This technique is used by Microsoft within the .net runtime to avoid duplicate code paths for sync/async.
        // See https://www.thereformedprogrammer.net/using-valuetask-to-create-methods-that-can-work-as-sync-or-async/
        try {
            ValueTask<ValidationResult> completedValueTask
                = ValidateInternalAsync(context, false, default);

            // Sync tasks should always be completed.
            Debug.Assert(completedValueTask.IsCompleted);

            // GetResult() will also bubble up any exceptions correctly.
            return completedValueTask.GetAwaiter().GetResult();
        }
        catch (AsyncValidatorInvokedSynchronouslyException) {
            // If we attempted to execute an async validator, re-create the exception with more useful info.
            bool wasInvokedByMvc = context.RootContextData.ContainsKey("InvokedByMvc");
            throw new AsyncValidatorInvokedSynchronouslyException(GetType(), wasInvokedByMvc);
        }
    }

    /// <summary>
    ///     Validates the specified instance asynchronously.
    /// </summary>
    /// <param name="context">Validation Context</param>
    /// <param name="cancellation">Cancellation token</param>
    /// <returns>A ValidationResult object containing any validation failures.</returns>
    public virtual async Task<ValidationResult> ValidateAsync(ValidationContext<T> context,
        CancellationToken cancellation = new()) {
        if (context == null) {
            throw new ArgumentNullException(nameof(context));
        }

        context.IsAsync = true;
        return await ValidateInternalAsync(context, true, cancellation);
    }

    private async ValueTask<ValidationResult> ValidateInternalAsync(ValidationContext<T> context, bool useAsync,
        CancellationToken cancellation) {
        ValidationResult result = new ValidationResult(context.Failures);
        bool shouldContinue = PreValidate(context, result);

        if (!shouldContinue) {
            if (!result.IsValid && context.ThrowOnFailures) {
                RaiseValidationException(context, result);
            }

            return result;
        }

        EnsureInstanceNotNull(context.InstanceToValidate);

        foreach (IValidationRuleInternal<T> rule in Rules) {
            cancellation.ThrowIfCancellationRequested();
            await rule.ValidateAsync(context, useAsync, cancellation);

            if (ClassLevelCascadeMode == CascadeMode.Stop && result.Errors.Count > 0) {
                // Bail out if we're "failing-fast".
                // Check for > 0 rather than == 1 because a rule chain may have overridden the Stop behaviour to Continue
                // meaning that although the first rule failed, it actually generated 2 failures if there were 2 validators
                // in the chain.
                break;
            }
        }

        SetExecutedRuleSets(result, context);

        if (!result.IsValid && context.ThrowOnFailures) {
            RaiseValidationException(context, result);
        }

        return result;
    }

    private void SetExecutedRuleSets(ValidationResult result, ValidationContext<T> context) {
        HashSet<string> executed = context.RootContextData.GetOrAdd("_FV_RuleSetsExecuted",
            () => new HashSet<string> { RulesetValidatorSelector.DefaultRuleSetName });
        result.RuleSetsExecuted = executed.ToArray();
    }

    /// <summary>
    ///     Defines a validation rule for a specify property.
    /// </summary>
    /// <example>
    ///     RuleFor(x => x.Surname)...
    /// </example>
    /// <typeparam name="TProperty">The type of property being validated</typeparam>
    /// <param name="expression">The expression representing the property to validate</param>
    /// <returns>an IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitial<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression) {
        expression.Guard("Cannot pass null to RuleFor", nameof(expression));
        PropertyRule<T, TProperty> rule = PropertyRule<T, TProperty>.Create(expression, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TProperty>(rule, this);
    }

    /// <summary>
    ///     Defines a validation rule for a specify property and transform it to a different type.
    /// </summary>
    /// <example>
    ///     Transform(x => x.OrderNumber, to: orderNumber => orderNumber.ToString())...
    /// </example>
    /// <typeparam name="TProperty">The type of property being validated</typeparam>
    /// <typeparam name="TTransformed">The type after the transformer has been applied</typeparam>
    /// <param name="from">The expression representing the property to transform</param>
    /// <param name="to">Function to transform the property value into a different type</param>
    /// <returns>an IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitial<T, TTransformed> Transform<TProperty, TTransformed>(
        Expression<Func<T, TProperty>> from, Func<TProperty, TTransformed> to) {
        from.Guard("Cannot pass null to Transform", nameof(from));
        PropertyRule<T, TTransformed> rule =
            PropertyRule<T, TTransformed>.Create(from, to, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TTransformed>(rule, this);
    }

    /// <summary>
    ///     Defines a validation rule for a specify property and transform it to a different type.
    /// </summary>
    /// <example>
    ///     Transform(x => x.OrderNumber, to: orderNumber => orderNumber.ToString())...
    /// </example>
    /// <typeparam name="TProperty">The type of property being validated</typeparam>
    /// <typeparam name="TTransformed">The type after the transformer has been applied</typeparam>
    /// <param name="from">The expression representing the property to transform</param>
    /// <param name="to">Function to transform the property value into a different type</param>
    /// <returns>an IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitial<T, TTransformed> Transform<TProperty, TTransformed>(
        Expression<Func<T, TProperty>> from, Func<T, TProperty, TTransformed> to) {
        from.Guard("Cannot pass null to Transform", nameof(from));
        PropertyRule<T, TTransformed> rule =
            PropertyRule<T, TTransformed>.Create(from, to, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TTransformed>(rule, this);
    }


    /// <summary>
    ///     Invokes a rule for each item in the collection.
    /// </summary>
    /// <typeparam name="TElement">Type of property</typeparam>
    /// <param name="expression">Expression representing the collection to validate</param>
    /// <returns>An IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitialCollection<T, TElement> RuleForEach<TElement>(
        Expression<Func<T, IEnumerable<TElement>>> expression) {
        expression.Guard("Cannot pass null to RuleForEach", nameof(expression));
        CollectionPropertyRule<T, TElement> rule =
            CollectionPropertyRule<T, TElement>.Create(expression, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TElement>(rule, this);
    }

    /// <summary>
    ///     Invokes a rule for each item in the collection, transforming the element from one type to another.
    /// </summary>
    /// <typeparam name="TElement">Type of property</typeparam>
    /// <typeparam name="TTransformed">The type after the transformer has been applied</typeparam>
    /// <param name="expression">Expression representing the collection to validate</param>
    /// <param name="to">Function to transform the collection element into a different type</param>
    /// <returns>An IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitialCollection<T, TTransformed> TransformForEach<TElement, TTransformed>(
        Expression<Func<T, IEnumerable<TElement>>> expression, Func<TElement, TTransformed> to) {
        expression.Guard("Cannot pass null to RuleForEach", nameof(expression));
        CollectionPropertyRule<T, TTransformed> rule =
            CollectionPropertyRule<T, TTransformed>.CreateTransformed(expression, to, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TTransformed>(rule, this);
    }

    /// <summary>
    ///     Invokes a rule for each item in the collection, transforming the element from one type to another.
    /// </summary>
    /// <typeparam name="TElement">Type of property</typeparam>
    /// <typeparam name="TTransformed">The type after the transformer has been applied</typeparam>
    /// <param name="expression">Expression representing the collection to validate</param>
    /// <param name="to">Function to transform the collection element into a different type</param>
    /// <returns>An IRuleBuilder instance on which validators can be defined</returns>
    public IRuleBuilderInitialCollection<T, TTransformed> TransformForEach<TElement, TTransformed>(
        Expression<Func<T, IEnumerable<TElement>>> expression, Func<T, TElement, TTransformed> to) {
        expression.Guard("Cannot pass null to RuleForEach", nameof(expression));
        CollectionPropertyRule<T, TTransformed> rule =
            CollectionPropertyRule<T, TTransformed>.CreateTransformed(expression, to, () => RuleLevelCascadeMode);
        Rules.Add(rule);
        return new RuleBuilder<T, TTransformed>(rule, this);
    }

    /// <summary>
    ///     Defines a RuleSet that can be used to group together several validators.
    /// </summary>
    /// <param name="ruleSetName">The name of the ruleset.</param>
    /// <param name="action">Action that encapsulates the rules in the ruleset.</param>
    public void RuleSet(string ruleSetName, Action action) {
        ruleSetName.Guard("A name must be specified when calling RuleSet.", nameof(ruleSetName));
        action.Guard("A ruleset definition must be specified when calling RuleSet.", nameof(action));

        string[] ruleSetNames = ruleSetName.Split(',', ';')
            .Select(x => x.Trim())
            .ToArray();

        using (Rules.OnItemAdded(r => r.RuleSets = ruleSetNames)) {
            action();
        }
    }

    /// <summary>
    ///     Defines a condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder When(Func<T, bool> predicate, Action action) {
        return When((x, _) => predicate(x), action);
    }

    /// <summary>
    ///     Defines a condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder When(Func<T, ValidationContext<T>, bool> predicate, Action action) {
        return new ConditionBuilder<T>(Rules).When(predicate, action);
    }

    /// <summary>
    ///     Defines an inverse condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder Unless(Func<T, bool> predicate, Action action) {
        return Unless((x, _) => predicate(x), action);
    }

    /// <summary>
    ///     Defines an inverse condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder Unless(Func<T, ValidationContext<T>, bool> predicate, Action action) {
        return new ConditionBuilder<T>(Rules).Unless(predicate, action);
    }

    /// <summary>
    ///     Defines an asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder WhenAsync(Func<T, CancellationToken, Task<bool>> predicate, Action action) {
        return WhenAsync((x, _, cancel) => predicate(x, cancel), action);
    }

    /// <summary>
    ///     Defines an asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder WhenAsync(Func<T, ValidationContext<T>, CancellationToken, Task<bool>> predicate,
        Action action) {
        return new AsyncConditionBuilder<T>(Rules).WhenAsync(predicate, action);
    }

    /// <summary>
    ///     Defines an inverse asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder UnlessAsync(Func<T, CancellationToken, Task<bool>> predicate, Action action) {
        return UnlessAsync((x, _, cancel) => predicate(x, cancel), action);
    }

    /// <summary>
    ///     Defines an inverse asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder UnlessAsync(Func<T, ValidationContext<T>, CancellationToken, Task<bool>> predicate,
        Action action) {
        return new AsyncConditionBuilder<T>(Rules).UnlessAsync(predicate, action);
    }

    /// <summary>
    ///     Includes the rules from the specified validator
    /// </summary>
    public void Include(IValidator<T> rulesToInclude) {
        rulesToInclude.Guard("Cannot pass null to Include", nameof(rulesToInclude));
        IncludeRule<T> rule = IncludeRule<T>.Create(rulesToInclude, () => RuleLevelCascadeMode);
        Rules.Add(rule);
    }

    /// <summary>
    ///     Includes the rules from the specified validator
    /// </summary>
    public void Include<TValidator>(Func<T, TValidator> rulesToInclude) where TValidator : IValidator<T> {
        rulesToInclude.Guard("Cannot pass null to Include", nameof(rulesToInclude));
        IncludeRule<T> rule = IncludeRule<T>.Create(rulesToInclude, () => RuleLevelCascadeMode);
        Rules.Add(rule);
    }

    /// <summary>
    ///     Throws an exception if the instance being validated is null.
    /// </summary>
    /// <param name="instanceToValidate"></param>
    protected virtual void EnsureInstanceNotNull(object? instanceToValidate) {
        instanceToValidate?.Guard("Cannot pass null model to Validate.", nameof(instanceToValidate));
    }

    /// <summary>
    ///     Determines if validation should occtur and provides a means to modify the context and ValidationResult prior to
    ///     execution.
    ///     If this method returns false, then the ValidationResult is immediately returned from Validate/ValidateAsync.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    protected virtual bool PreValidate(ValidationContext<T> context, ValidationResult result) {
        return true;
    }

    /// <summary>
    ///     Throws a ValidationException. This method will only be called if the validator has been configured
    ///     to throw exceptions if validation fails. The default behaviour is not to throw an exception.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="result"></param>
    /// <exception cref="ValidationException"></exception>
    protected virtual void RaiseValidationException(ValidationContext<T> context, ValidationResult result) {
        throw new ValidationException(result.Errors);
    }
}