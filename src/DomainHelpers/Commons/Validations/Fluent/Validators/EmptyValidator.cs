using System.Collections;

namespace DomainHelpers.Core.Validations.Validators;

public class EmptyValidator<T, TProperty> : PropertyValidator<T, TProperty> {
    public override string Name => "EmptyValidator";

    public override bool IsValid(ValidationContext<T> context, TProperty value) {
        switch (value) {
            case null:
            case string s when string.IsNullOrWhiteSpace(s):
            case ICollection { Count: 0 }:
            case Array { Length: 0 }:
            case IEnumerable e when !e.GetEnumerator().MoveNext():
                return true;
        }

        //TODO: Rewrite to avoid boxing
        if (Equals(value, default(TProperty))) {
            // Note: Code analysis indicates "Expression is always false" but this is incorrect.
            return true;
        }

        return false;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) {
        return Localized(errorCode, Name);
    }
}