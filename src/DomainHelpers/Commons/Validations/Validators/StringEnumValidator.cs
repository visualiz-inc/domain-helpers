namespace DomainHelpers.Core.Validations.Validators;

using DomainHelpers.Core.Validations.Internal;
using Resources;
using System;
using System.Linq;
using System.Reflection;

public class StringEnumValidator<T> : PropertyValidator<T, string> {
    private readonly Type _enumType;
    private readonly bool _caseSensitive;

    public override string Name => "StringEnumValidator";

    public StringEnumValidator(Type enumType, bool caseSensitive) {
        if (enumType == null) throw new ArgumentNullException(nameof(enumType));

        CheckTypeIsEnum(enumType);

        _enumType = enumType;
        _caseSensitive = caseSensitive;
    }

    public override bool IsValid(ValidationContext<T> context, string value) {
        if (value == null) return true;
        var comparison = _caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        return Enum.GetNames(_enumType).Any(n => n.Equals(value, comparison));
    }

    private void CheckTypeIsEnum(Type enumType) {
        if (!enumType.IsEnum) {
            string message = $"The type '{enumType.Name}' is not an enum and can't be used with IsEnumName.";
            throw new ArgumentOutOfRangeException(nameof(enumType), message);
        }
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        // Intentionally the same message as EnumValidator.
        return Localized(errorCode, "EnumValidator");
    }
}
