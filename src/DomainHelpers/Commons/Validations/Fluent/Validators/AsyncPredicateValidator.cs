using DomainHelpers.Core.Validations.Internal;

namespace DomainHelpers.Core.Validations.Validators;

/// <summary>
///     Asynchronous custom validator
/// </summary>
public class AsyncPredicateValidator<T, TProperty> : AsyncPropertyValidator<T, TProperty> {
    private readonly Func<T, TProperty, ValidationContext<T>, CancellationToken, Task<bool>> _predicate;

    /// <summary>
    ///     Creates a new AsyncPredicateValidator
    /// </summary>
    /// <param name="predicate"></param>
    public AsyncPredicateValidator(
        Func<T, TProperty, ValidationContext<T>, CancellationToken, Task<bool>> predicate) {
        predicate.Guard("A predicate must be specified.", nameof(predicate));
        _predicate = predicate;
    }

    public override string Name => "AsyncPredicateValidator";

    public override Task<bool> IsValidAsync(ValidationContext<T> context, TProperty value,
        CancellationToken cancellation) {
        return _predicate(context.InstanceToValidate, value, context, cancellation);
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}