using DomainHelpers.Core.Validations.Validators;

namespace DomainHelpers.Core.Validations.Internal;

internal class RuleComponentForNullableStruct<T, TProperty> : RuleComponent<T, TProperty?>
    where TProperty : struct {
    private readonly IAsyncPropertyValidator<T, TProperty>? _asyncPropertyValidator;
    private readonly IPropertyValidator<T, TProperty>? _propertyValidator;

    internal RuleComponentForNullableStruct(IPropertyValidator<T, TProperty>? propertyValidator)
        : base(null) {
        _propertyValidator = propertyValidator;
    }

    internal RuleComponentForNullableStruct(IAsyncPropertyValidator<T, TProperty>? asyncPropertyValidator,
        IPropertyValidator<T, TProperty>? propertyValidator)
        : base(null, null) {
        _asyncPropertyValidator = asyncPropertyValidator;
    }

    public override IPropertyValidator Validator
        => (IPropertyValidator)_propertyValidator ?? _asyncPropertyValidator;

    private protected override bool SupportsAsynchronousValidation
        => _asyncPropertyValidator != null;

    private protected override bool SupportsSynchronousValidation
        => _propertyValidator != null;

    private protected override bool InvokePropertyValidator(ValidationContext<T> context, TProperty? value) {
        if (!value.HasValue) {
            return true;
        }

        return _propertyValidator?.IsValid(context, value.Value) ?? false;
    }

    private protected override async Task<bool> InvokePropertyValidatorAsync(ValidationContext<T> context,
        TProperty? value, CancellationToken cancellation) {
        if (!value.HasValue) {
            return true;
        }

        return await _asyncPropertyValidator!.IsValidAsync(context, value.Value, cancellation);
    }
}