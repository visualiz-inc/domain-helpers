using System.Reflection;

namespace DomainHelpers.Core.Validations.Validators {
    public class GreaterThanOrEqualValidator<T, TProperty> : AbstractComparisonValidator<T, TProperty>,
        IGreaterThanOrEqualValidator where TProperty : IComparable<TProperty>, IComparable {
        public GreaterThanOrEqualValidator(TProperty value) :
            base(value) { }

        public GreaterThanOrEqualValidator(Func<T, TProperty> valueToCompareFunc, MemberInfo member,
            string memberDisplayName)
            : base(valueToCompareFunc, member, memberDisplayName) { }

        public GreaterThanOrEqualValidator(Func<T, (bool HasValue, TProperty Value)> valueToCompareFunc,
            MemberInfo member, string memberDisplayName)
            : base(valueToCompareFunc, member, memberDisplayName) { }

        public override string Name => "GreaterThanOrEqualValidator";

        public override Comparison Comparison => Comparison.GreaterThanOrEqual;

        public override bool IsValid(TProperty value, TProperty valueToCompare) {
            if (valueToCompare == null) {
                return false;
            }

            return value.CompareTo(valueToCompare) >= 0;
        }

        protected override string GetDefaultMessageTemplate(string errorCode) {
            return Localized(errorCode, Name);
        }
    }

    public interface IGreaterThanOrEqualValidator : IComparisonValidator { }
}
