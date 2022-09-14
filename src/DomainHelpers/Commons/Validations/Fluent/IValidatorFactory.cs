namespace DomainHelpers.Core.Validations; 

/// <summary>
///     Gets validators for a particular type.
/// </summary>
[Obsolete(
    "IValidatorFactory and its implementors are deprecated and will be removed in a future release. Please use the Service Provider directly (or a DI container). For details see https://github.com/DomainHelpers.Core.Validations/DomainHelpers.Core.Validations/issues/1961")]
public interface IValidatorFactory {
    /// <summary>
    ///     Gets the validator for the specified type.
    /// </summary>
    IValidator<T> GetValidator<T>();

    /// <summary>
    ///     Gets the validator for the specified type.
    /// </summary>
    IValidator GetValidator(Type type);
}