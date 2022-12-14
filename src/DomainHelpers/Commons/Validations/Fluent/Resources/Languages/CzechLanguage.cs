#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class CzechLanguage {
    public const string Culture = "cs";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "Pole '{PropertyName}' musí obsahovat platnou emailovou adresu.",
            "GreaterThanOrEqualValidator" =>
                "Hodnota pole '{PropertyName}' musí být větší nebo rovna '{ComparisonValue}'.",
            "GreaterThanValidator" => "Hodnota pole '{PropertyName}' musí být větší než '{ComparisonValue}'.",
            "LengthValidator" =>
                "Délka pole '{PropertyName}' musí být v rozsahu {MinLength} až {MaxLength} znaků. Vámi zadaná délka je {TotalLength} znaků.",
            "MinimumLengthValidator" =>
                "Délka pole '{PropertyName}' musí být větší nebo rovna {MinLength} znakům. Vámi zadaná délka je {TotalLength} znaků.",
            "MaximumLengthValidator" =>
                "Délka pole '{PropertyName}' musí být menší nebo rovna {MaxLength} znakům. Vámi zadaná délka je {TotalLength} znaků.",
            "LessThanOrEqualValidator" =>
                "Hodnota pole '{PropertyName}' musí být menší nebo rovna '{ComparisonValue}'.",
            "LessThanValidator" => "Hodnota pole '{PropertyName}' musí být menší než '{ComparisonValue}'.",
            "NotEmptyValidator" => "Pole '{PropertyName}' nesmí být prázdné.",
            "NotEqualValidator" => "Pole '{PropertyName}' nesmí být rovno '{ComparisonValue}'.",
            "NotNullValidator" => "Pole '{PropertyName}' nesmí být prázdné.",
            "PredicateValidator" => "Nebyla splněna podmínka pro pole '{PropertyName}'.",
            "AsyncPredicateValidator" => "Nebyla splněna podmínka pro pole '{PropertyName}'.",
            "RegularExpressionValidator" => "Pole '{PropertyName}' nemá správný formát.",
            "EqualValidator" => "Hodnota pole '{PropertyName}' musí být rovna '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "Délka pole '{PropertyName}' musí být {MaxLength} znaků. Vámi zadaná délka je {TotalLength} znaků.",
            "InclusiveBetweenValidator" =>
                "Hodnota pole '{PropertyName}' musí být mezi {From} a {To} (včetně). Vámi zadaná hodnota je {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "Hodnota pole '{PropertyName}' musí být větší než {From} a menší než {To}. Vámi zadaná hodnota je {PropertyValue}.",
            "CreditCardValidator" => "Pole '{PropertyName}' musí obsahovat platné číslo platební karty.",
            "ScalePrecisionValidator" =>
                "Pole '{PropertyName}' nesmí mít víc než {ExpectedPrecision} číslic a {ExpectedScale} desetinných míst. Vámi bylo zadáno {Digits} číslic a {ActualScale} desetinných míst.",
            "EmptyValidator" => "Pole '{PropertyName}' musí být prázdné.",
            "NullValidator" => "Pole '{PropertyName}' musí být prázdné.",
            "EnumValidator" => "Pole '{PropertyName}' má rozsah hodnot, které neobsahují '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" => "Délka pole '{PropertyName}' musí být v rozsahu {MinLength} až {MaxLength} znaků.",
            "MinimumLength_Simple" => "Délka pole '{PropertyName}' musí být větší nebo rovna {MinLength} znakům.",
            "MaximumLength_Simple" => "Délka pole '{PropertyName}' musí být menší nebo rovna {MaxLength} znakům.",
            "ExactLength_Simple" => "Délka pole '{PropertyName}' musí být {MaxLength} znaků.",
            "InclusiveBetween_Simple" => "Hodnota pole '{PropertyName}' musí být mezi {From} a {To} (včetně).",
            _ => null
        };
    }
}