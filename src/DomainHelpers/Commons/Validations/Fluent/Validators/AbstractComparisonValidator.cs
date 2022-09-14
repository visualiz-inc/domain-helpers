using DomainHelpers.Core.Validations.Internal;
using System.Reflection;

namespace DomainHelpers.Core.Validations.Validators; 

/// <summary>
///     Base class for all comparison validators
/// </summary>
public abstract class AbstractComparisonValidator<T, TProperty> : PropertyValidator<T, TProperty>,
    IComparisonValidator where TProperty : IComparable<TProperty>, IComparable {
    private readonly string _comparisonMemberDisplayName;
    private readonly Func<T, TProperty> _valueToCompareFunc;
    private readonly Func<T, (bool HasValue, TProperty Value)> _valueToCompareFuncForNullables;

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    protected AbstractComparisonValidator(TProperty value) {
        value.Guard("value must not be null.", nameof(value));
        ValueToCompare = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="valueToCompareFunc"></param>
    /// <param name="member"></param>
    /// <param name="memberDisplayName"></param>
    protected AbstractComparisonValidator(Func<T, (bool HasValue, TProperty Value)> valueToCompareFunc,
        MemberInfo member, string memberDisplayName) {
        _valueToCompareFuncForNullables = valueToCompareFunc;
        _comparisonMemberDisplayName = memberDisplayName;
        MemberToCompare = member;
    }

    /// <summary>
    /// </summary>
    /// <param name="valueToCompareFunc"></param>
    /// <param name="member"></param>
    /// <param name="memberDisplayName"></param>
    protected AbstractComparisonValidator(Func<T, TProperty> valueToCompareFunc, MemberInfo member,
        string memberDisplayName) {
        _valueToCompareFunc = valueToCompareFunc;
        _comparisonMemberDisplayName = memberDisplayName;
        MemberToCompare = member;
    }

    /// <summary>
    ///     The value being compared
    /// </summary>
    public TProperty ValueToCompare { get; }

    /// <summary>
    ///     Metadata- the comparison type
    /// </summary>
    public abstract Comparison Comparison { get; }

    /// <summary>
    ///     Metadata- the member being compared
    /// </summary>
    public MemberInfo MemberToCompare { get; }

    /// <summary>
    ///     Comparison value as non-generic for metadata.
    /// </summary>
    object IComparisonValidator.ValueToCompare =>
        // For clientside validation to work, we must return null if MemberToCompare or valueToCompareFunc is set.
        // We can't rely on ValueToCompare being null itself as it's generic, and will be initialized
        // as default(TProperty) which for non-nullable value types will emit the
        // default value for the type rather than null. See https://github.com/DomainHelpers.Core.Validations/DomainHelpers.Core.Validations/issues/1721
        MemberToCompare != null || _valueToCompareFunc != null ? null : ValueToCompare;

    /// <summary>
    ///     Performs the comparison
    /// </summary>
    /// <param name="context"></param>
    /// <param name="propertyValue"></param>
    /// <returns></returns>
    public sealed override bool IsValid(ValidationContext<T> context, TProperty propertyValue) {
        if (propertyValue == null) {
            // If we're working with a nullable type then this rule should not be applied.
            // If you want to ensure that it's never null then a NotNull rule should also be applied.
            return true;
        }

        (bool HasValue, TProperty Value) valueToCompare = GetComparisonValue(context);

        if (!valueToCompare.HasValue || !IsValid(propertyValue, valueToCompare.Value)) {
            context.MessageFormatter.AppendArgument("ComparisonValue",
                valueToCompare.HasValue ? valueToCompare.Value : "");
            context.MessageFormatter.AppendArgument("ComparisonProperty", _comparisonMemberDisplayName ?? "");
            return false;
        }

        return true;
    }

    public (bool HasValue, TProperty Value) GetComparisonValue(ValidationContext<T> context) {
        if (_valueToCompareFunc != null) {
            TProperty? value = _valueToCompareFunc(context.InstanceToValidate);
            return (value != null, value);
        }

        if (_valueToCompareFuncForNullables != null) {
            return _valueToCompareFuncForNullables(context.InstanceToValidate);
        }

        return (ValueToCompare != null, ValueToCompare);
    }

    /// <summary>
    ///     Override to perform the comparison
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueToCompare"></param>
    /// <returns></returns>
    public abstract bool IsValid(TProperty value, TProperty valueToCompare);
}

/// <summary>
///     Defines a comparison validator
/// </summary>
public interface IComparisonValidator : IPropertyValidator {
    /// <summary>
    ///     Metadata- the comparison type
    /// </summary>
    Comparison Comparison { get; }

    /// <summary>
    ///     Metadata- the member being compared
    /// </summary>
    MemberInfo MemberToCompare { get; }

    /// <summary>
    ///     Metadata- the value being compared
    /// </summary>
    object ValueToCompare { get; }
}

#pragma warning disable 1591
public enum Comparison {
    Equal,
    NotEqual,
    LessThan,
    GreaterThan,
    GreaterThanOrEqual,
    LessThanOrEqual
}
#pragma warning restore 1591