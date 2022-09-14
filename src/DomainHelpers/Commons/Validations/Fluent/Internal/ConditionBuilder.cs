namespace DomainHelpers.Core.Validations.Internal; 

internal class ConditionBuilder<T> {
    private readonly TrackingCollection<IValidationRuleInternal<T>> _rules;

    public ConditionBuilder(TrackingCollection<IValidationRuleInternal<T>> rules) {
        _rules = rules;
    }

    /// <summary>
    ///     Defines a condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder When(Func<T, ValidationContext<T>, bool> predicate, Action action) {
        List<IValidationRuleInternal<T>> propertyRules = new List<IValidationRuleInternal<T>>();

        using (_rules.OnItemAdded(propertyRules.Add)) {
            action();
        }

        // Generate unique ID for this shared condition.
        string? id = "_FV_Condition_" + Guid.NewGuid();

        bool Condition(IValidationContext context) {
            ValidationContext<T> actualContext = ValidationContext<T>.GetFromNonGenericContext(context);

            if (actualContext.InstanceToValidate != null) {
                if (actualContext.SharedConditionCache.TryGetValue(id, out Dictionary<T, bool>? cachedResults)) {
                    if (cachedResults.TryGetValue(actualContext.InstanceToValidate, out bool result)) {
                        return result;
                    }
                }
            }

            bool executionResult = predicate(actualContext.InstanceToValidate, actualContext);
            if (actualContext.InstanceToValidate != null) {
                if (actualContext.SharedConditionCache.TryGetValue(id, out Dictionary<T, bool>? cachedResults)) {
                    cachedResults.Add(actualContext.InstanceToValidate, executionResult);
                }
                else {
                    actualContext.SharedConditionCache.Add(id,
                        new Dictionary<T, bool> { { actualContext.InstanceToValidate, executionResult } });
                }
            }

            return executionResult;
        }

        // Must apply the predicate after the rule has been fully created to ensure any rules-specific conditions have already been applied.
        foreach (IValidationRuleInternal<T> rule in propertyRules) {
            rule.ApplySharedCondition(Condition);
        }

        return new ConditionOtherwiseBuilder<T>(_rules, Condition);
    }

    /// <summary>
    ///     Defines an inverse condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder Unless(Func<T, ValidationContext<T>, bool> predicate, Action action) {
        return When((x, context) => !predicate(x, context), action);
    }
}

internal class AsyncConditionBuilder<T> {
    private readonly TrackingCollection<IValidationRuleInternal<T>> _rules;

    public AsyncConditionBuilder(TrackingCollection<IValidationRuleInternal<T>> rules) {
        _rules = rules;
    }

    /// <summary>
    ///     Defines an asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should apply to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules.</param>
    /// <returns></returns>
    public IConditionBuilder WhenAsync(Func<T, ValidationContext<T>, CancellationToken, Task<bool>> predicate,
        Action action) {
        List<IValidationRuleInternal<T>> propertyRules = new List<IValidationRuleInternal<T>>();

        using (_rules.OnItemAdded(propertyRules.Add)) {
            action();
        }

        // Generate unique ID for this shared condition.
        string? id = "_FV_AsyncCondition_" + Guid.NewGuid();

        async Task<bool> Condition(IValidationContext context, CancellationToken ct) {
            ValidationContext<T> actualContext = ValidationContext<T>.GetFromNonGenericContext(context);

            if (actualContext.InstanceToValidate != null) {
                if (actualContext.SharedConditionCache.TryGetValue(id, out Dictionary<T, bool>? cachedResults)) {
                    if (cachedResults.TryGetValue(actualContext.InstanceToValidate, out bool result)) {
                        return result;
                    }
                }
            }

            bool executionResult = await predicate((T)context.InstanceToValidate,
                ValidationContext<T>.GetFromNonGenericContext(context), ct);
            if (actualContext.InstanceToValidate != null) {
                if (actualContext.SharedConditionCache.TryGetValue(id, out Dictionary<T, bool>? cachedResults)) {
                    cachedResults.Add(actualContext.InstanceToValidate, executionResult);
                }
                else {
                    actualContext.SharedConditionCache.Add(id,
                        new Dictionary<T, bool> { { actualContext.InstanceToValidate, executionResult } });
                }
            }

            return executionResult;
        }

        foreach (IValidationRuleInternal<T> rule in propertyRules) {
            rule.ApplySharedAsyncCondition(Condition);
        }

        return new AsyncConditionOtherwiseBuilder<T>(_rules, Condition);
    }

    /// <summary>
    ///     Defines an inverse asynchronous condition that applies to several rules
    /// </summary>
    /// <param name="predicate">The asynchronous condition that should be applied to multiple rules</param>
    /// <param name="action">Action that encapsulates the rules</param>
    public IConditionBuilder UnlessAsync(Func<T, ValidationContext<T>, CancellationToken, Task<bool>> predicate,
        Action action) {
        return WhenAsync(async (x, context, ct) => !await predicate(x, context, ct), action);
    }
}

internal class ConditionOtherwiseBuilder<T> : IConditionBuilder {
    private readonly Func<IValidationContext, bool> _condition;
    private readonly TrackingCollection<IValidationRuleInternal<T>> _rules;

    public ConditionOtherwiseBuilder(TrackingCollection<IValidationRuleInternal<T>> rules,
        Func<IValidationContext, bool> condition) {
        _rules = rules;
        _condition = condition;
    }

    public virtual void Otherwise(Action action) {
        List<IValidationRuleInternal<T>> propertyRules = new List<IValidationRuleInternal<T>>();

        Action<IValidationRuleInternal<T>> onRuleAdded = propertyRules.Add;

        using (_rules.OnItemAdded(onRuleAdded)) {
            action();
        }

        foreach (IValidationRuleInternal<T> rule in propertyRules) {
            rule.ApplySharedCondition(ctx => !_condition(ctx));
        }
    }
}

internal class AsyncConditionOtherwiseBuilder<T> : IConditionBuilder {
    private readonly Func<IValidationContext, CancellationToken, Task<bool>> _condition;
    private readonly TrackingCollection<IValidationRuleInternal<T>> _rules;

    public AsyncConditionOtherwiseBuilder(TrackingCollection<IValidationRuleInternal<T>> rules,
        Func<IValidationContext, CancellationToken, Task<bool>> condition) {
        _rules = rules;
        _condition = condition;
    }

    public virtual void Otherwise(Action action) {
        List<IValidationRuleInternal<T>> propertyRules = new List<IValidationRuleInternal<T>>();

        Action<IValidationRuleInternal<T>> onRuleAdded = propertyRules.Add;

        using (_rules.OnItemAdded(onRuleAdded)) {
            action();
        }

        foreach (IValidationRuleInternal<T> rule in propertyRules) {
            rule.ApplySharedAsyncCondition(async (ctx, ct) => !await _condition(ctx, ct));
        }
    }
}