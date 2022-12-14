#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class SerbianLanguage {
    public const string Culture = "sr";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' nije validna email adresa.",
            "GreaterThanOrEqualValidator" => "'{PropertyName}' mora biti veće ili jednako '{ComparisonValue}'.",
            "GreaterThanValidator" => "'{PropertyName}' mora biti veće od '{ComparisonValue}'.",
            "LengthValidator" =>
                "'{PropertyName}' mora imati između {MinLength} i {MaxLength} karatkera. Uneseno je {TotalLength} karaktera.",
            "MinimumLengthValidator" =>
                "'{PropertyName}' mora imati najmanje {MinLength} karaktera. Uneseno je {TotalLength} karaktera.",
            "MaximumLengthValidator" =>
                "'{PropertyName}' ne sme imati više od {MaxLength} karaktera. Uneseno je {TotalLength} karaktera.",
            "LessThanOrEqualValidator" => "'{PropertyName}' mora biti manje ili jednako '{ComparisonValue}'.",
            "LessThanValidator" => "'{PropertyName}' mora biti manje od '{ComparisonValue}'.",
            "NotEmptyValidator" => "'{PropertyName}' ne sme biti prazan.",
            "NotEqualValidator" => "'{PropertyName}' ne sme biti jednak '{ComparisonValue}'.",
            "NotNullValidator" => "'{PropertyName}' ne sme biti prazan.",
            "PredicateValidator" => "Zadat uslov nije ispunjen za '{PropertyName}'.",
            "AsyncPredicateValidator" => "Zadat uslov nije ispunjen za '{PropertyName}'.",
            "RegularExpressionValidator" => "'{PropertyName}' nije u odgovarajućem formatu.",
            "EqualValidator" => "'{PropertyName}' mora biti jednak '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "'{PropertyName}' mora imati tačno {MaxLength} karaktera. Uneseno je {TotalLength} karaktera.",
            "InclusiveBetweenValidator" =>
                "'{PropertyName}' mora biti između {From} i {To}. Uneseno je {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "'{PropertyName}' mora biti između {From} i {To} (ekskluzivno). Uneseno je {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' nije validna kreditna kartica.",
            "ScalePrecisionValidator" =>
                "'{PropertyName}' ne sme imati više od {ExpectedPrecision} cifara, sa dozvoljenih {ExpectedScale} decimalnih mesta. Uneseno je {Digits} cifara i {ActualScale} decimalnih mesta.",
            "EmptyValidator" => "'{PropertyName}' mora biti prazno.",
            "NullValidator" => "'{PropertyName}' mora biti prazno.",
            "EnumValidator" => "'{PropertyName}' ima raspon vrednosti koji ne uključuje '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" => "'{PropertyName}' mora imati između {MinLength} i {MaxLength} karaktera.",
            "MinimumLength_Simple" => "'{PropertyName}' mora imati najmanje {MinLength} karaktera.",
            "MaximumLength_Simple" => "'{PropertyName}' ne sme imati više od {MaxLength} karaktera.",
            "ExactLength_Simple" => "'{PropertyName}' mora imati tačno {MaxLength} karaktera.",
            "InclusiveBetween_Simple" => "'{PropertyName}' mora biti između {From} i {To}.",
            _ => null
        };
    }
}