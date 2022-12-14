using DomainHelpers.Core.Validations.Internal;

namespace DomainHelpers.Core.Validations.Validators;

public class PredicateValidator<T, TProperty> : PropertyValidator<T, TProperty>, IPredicateValidator {
    public delegate bool Predicate(T instanceToValidate, TProperty propertyValue,
        ValidationContext<T> propertyValidatorContext);

    private readonly Predicate _predicate;

    public PredicateValidator(Predicate predicate) {
        predicate.Guard("A predicate must be specified.", nameof(predicate));
        _predicate = predicate;
    }

    public override string Name => "PredicateValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value) {
        if (!_predicate(context.InstanceToValidate, value, context)) {
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}

public interface IPredicateValidator : IPropertyValidator { }