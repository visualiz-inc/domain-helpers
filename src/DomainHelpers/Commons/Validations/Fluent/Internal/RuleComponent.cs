using DomainHelpers.Core.Validations.Validators;

namespace DomainHelpers.Core.Validations.Internal;

/// <summary>
/// An individual component within a rule.
/// In a rule definition such as RuleFor(x => x.Name).NotNull().NotEqual("Foo")
/// the NotNull and the NotEqual are both rule components.
/// </summary>
public class RuleComponent<T, TProperty> : IRuleComponent<T, TProperty>
    where T : notnull {
    private readonly IAsyncPropertyValidator<T, TProperty>? _asyncPropertyValidator;
    private readonly IPropertyValidator<T, TProperty>? _propertyValidator;
    private Func<ValidationContext<T>, CancellationToken, Task<bool>>? _asyncCondition;
    private Func<ValidationContext<T>, bool>? _condition;
    private string? _errorMessage;
    private Func<ValidationContext<T>?, TProperty, string>? _errorMessageFactory;

    internal RuleComponent(IPropertyValidator<T, TProperty>? propertyValidator) {
        _propertyValidator = propertyValidator;
    }

    internal RuleComponent(IAsyncPropertyValidator<T, TProperty> asyncPropertyValidator,
        IPropertyValidator<T, TProperty> propertyValidator) {
        _asyncPropertyValidator = asyncPropertyValidator;
        _propertyValidator = propertyValidator;
    }

    private protected virtual bool SupportsAsynchronousValidation
        => _asyncPropertyValidator != null;

    private protected virtual bool SupportsSynchronousValidation
        => _propertyValidator != null;

    /// <inheritdoc />
    public bool HasCondition => _condition != null;

    /// <inheritdoc />
    public bool HasAsyncCondition => _asyncCondition != null;

    /// <inheritdoc />
    public virtual IPropertyValidator? Validator
        => (IPropertyValidator?)_propertyValidator ?? _asyncPropertyValidator;

    /// <summary>
    /// Adds a condition for this validator. If there's already a condition, they're combined together with an AND.
    /// </summary>
    /// <param name="condition"></param>
    public void ApplyCondition(Func<ValidationContext<T>, bool> condition) {
        if (_condition == null) {
            _condition = condition;
        }
        else {
            Func<ValidationContext<T>, bool> original = _condition;
            _condition = ctx => condition(ctx) && original(ctx);
        }
    }

    /// <summary>
    /// Adds a condition for this validator. If there's already a condition, they're combined together with an AND.
    /// </summary>
    /// <param name="condition"></param>
    public void ApplyAsyncCondition(Func<ValidationContext<T>, CancellationToken, Task<bool>> condition) {
        if (_asyncCondition == null) {
            _asyncCondition = condition;
        }
        else {
            Func<ValidationContext<T>, CancellationToken, Task<bool>> original = _asyncCondition;
            _asyncCondition = async (ctx, ct) => await condition(ctx, ct) && await original(ctx, ct);
        }
    }

    /// <summary>
    /// Function used to retrieve custom state for the validator
    /// </summary>
    public Func<ValidationContext<T>, TProperty, object>? CustomStateProvider { get; set; }

    /// <summary>
    /// Function used to retrieve the severity for the validator
    /// </summary>
    public Func<ValidationContext<T>, TProperty, Severity>? SeverityProvider { get; set; }

    /// <summary>
    /// Retrieves the error code.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Gets the raw unformatted error message. Placeholders will not have been rewritten.
    /// </summary>
    /// <returns></returns>
    public string GetUnformattedErrorMessage() {
        var message = _errorMessageFactory?.Invoke(null, default)
            ?? _errorMessage
            ?? Validator?.GetDefaultMessageTemplate(ErrorCode);

        return message;
    }

    /// <summary>
    /// Sets the overridden error message template for this validator.
    /// </summary>
    /// <param name="errorFactory">A function for retrieving the error message template.</param>
    public void SetErrorMessage(Func<ValidationContext<T>, TProperty, string> errorFactory) {
        _errorMessageFactory = errorFactory;
        _errorMessage = null;
    }

    /// <summary>
    /// Sets the overridden error message template for this validator.
    /// </summary>
    /// <param name="errorMessage">The error message to set</param>
    public void SetErrorMessage(string errorMessage) {
        _errorMessage = errorMessage;
        _errorMessageFactory = null;
    }

    internal async ValueTask<bool> ValidateAsync(ValidationContext<T> context, TProperty value, bool useAsync,
        CancellationToken cancellation) {
        if (useAsync) {
            // If ValidateAsync has been called on the root validator, then always prefer
            // the asynchronous property validator (if available).
            if (SupportsAsynchronousValidation) {
                return await InvokePropertyValidatorAsync(context, value, cancellation);
            }

            // If it doesn't support Async validation, then this means
            // the property validator is a Synchronous.
            // We don't need to explicitly check SupportsSynchronousValidation.
            return InvokePropertyValidator(context, value);
        }

        // If Validate has been called on the root validator, then always prefer
        // the synchronous property validator.
        if (SupportsSynchronousValidation) {
            return InvokePropertyValidator(context, value);
        }

        // Root Validator invoked synchronously, but the property validator
        // only supports asynchronous invocation.
        throw new AsyncValidatorInvokedSynchronouslyException();
    }

    private protected virtual bool InvokePropertyValidator(ValidationContext<T> context, TProperty value) {
        return _propertyValidator.IsValid(context, value);
    }

    private protected virtual Task<bool> InvokePropertyValidatorAsync(ValidationContext<T> context, TProperty value,
        CancellationToken cancellation) {
        return _asyncPropertyValidator.IsValidAsync(context, value, cancellation);
    }

    internal bool InvokeCondition(ValidationContext<T> context) {
        if (_condition != null) {
            return _condition(context);
        }

        return true;
    }

    internal async Task<bool> InvokeAsyncCondition(ValidationContext<T> context, CancellationToken token) {
        if (_asyncCondition != null) {
            return await _asyncCondition(context, token);
        }

        return true;
    }

    /// <summary>
    /// Gets the error message. If a context is supplied, it will be used to format the message if it has placeholders.
    /// If no context is supplied, the raw unformatted message will be returned, containing placeholders.
    /// </summary>
    /// <param name="context">The validation context.</param>
    /// <param name="value">The current property value.</param>
    /// <returns>Either the formatted or unformatted error message.</returns>
    public string GetErrorMessage(ValidationContext<T> context, TProperty value) {
        // Use a custom message if one has been specified.
        var rawTemplate = _errorMessageFactory?.Invoke(context, value)
            ?? _errorMessage
            ?? Validator?.GetDefaultMessageTemplate(ErrorCode);

        if (context == null) {
            return rawTemplate;
        }
        else {
            return context.MessageFormatter.BuildMessage(rawTemplate);
        }
    }
}