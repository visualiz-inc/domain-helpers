using System.Reflection;

namespace DomainHelpers.Core.Validations.Validators;

public class GreaterThanValidator<T, TProperty> : AbstractComparisonValidator<T, TProperty>
    where TProperty : IComparable<TProperty>, IComparable {
    public GreaterThanValidator(TProperty value) : base(value) { }

    public GreaterThanValidator(Func<T, TProperty> valueToCompareFunc, MemberInfo member, string memberDisplayName)
        : base(valueToCompareFunc, member, memberDisplayName) { }

    public GreaterThanValidator(Func<T, (bool HasValue, TProperty Value)> valueToCompareFunc, MemberInfo member,
        string memberDisplayName)
        : base(valueToCompareFunc, member, memberDisplayName) { }

    public override string Name => "GreaterThanValidator";

    public override Comparison Comparison => Comparison.GreaterThan;

    public override bool IsValid(TProperty value, TProperty valueToCompare) {
        if (valueToCompare == null) {
            return false;
        }

        return value.CompareTo(valueToCompare) > 0;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}