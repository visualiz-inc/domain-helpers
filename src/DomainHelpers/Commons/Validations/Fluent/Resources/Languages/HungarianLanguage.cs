#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources; 

internal class HungarianLanguage {
    public const string Culture = "hu";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' nem érvényes email cím.",
            "GreaterThanOrEqualValidator" =>
                "A(z) '{PropertyName}' nagyobb vagy egyenlő kell, hogy legyen, mint '{ComparisonValue}'.",
            "GreaterThanValidator" => "A(z) '{PropertyName}' nagyobb kell, hogy legyen, mint '{ComparisonValue}'.",
            "LengthValidator" =>
                "A(z) '{PropertyName}' legalább {MinLength}, de legfeljebb {MaxLength} karakter kell, hogy legyen. Ön {TotalLength} karaktert adott meg.",
            "MinimumLengthValidator" =>
                "A(z) '{PropertyName}' legalább {MinLength} karakter kell, hogy legyen. Ön {TotalLength} karaktert adott meg.",
            "MaximumLengthValidator" =>
                "A(z) '{PropertyName}' legfeljebb {MaxLength} karakter lehet csak. Ön {TotalLength} karaktert adott meg.",
            "LessThanOrEqualValidator" =>
                "A(z) '{PropertyName}' kisebb vagy egyenlő kell, hogy legyen, mint '{ComparisonValue}'.",
            "LessThanValidator" => "A(z) '{PropertyName}' kisebb kell, hogy legyen, mint '{ComparisonValue}'.",
            "NotEmptyValidator" => "A(z) '{PropertyName}' nem lehet üres.",
            "NotEqualValidator" => "A(z) '{PropertyName}' nem lehet egyenlő ezzel: '{ComparisonValue}'.",
            "NotNullValidator" => "A(z) '{PropertyName}' nem lehet üres.",
            "PredicateValidator" => "A megadott feltétel nem teljesült a(z) '{PropertyName}' mezőre.",
            "AsyncPredicateValidator" => "A megadott feltétel nem teljesült a(z) '{PropertyName}' mezőre.",
            "RegularExpressionValidator" => "A(z) '{PropertyName}' nem a megfelelő formátumban van.",
            "EqualValidator" => "A(z) '{PropertyName}' egyenlő kell, hogy legyen ezzel: '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "A(z) '{PropertyName}' pontosan {MaxLength} karakter kell, hogy legyen. Ön {TotalLength} karaktert adott meg.",
            "InclusiveBetweenValidator" =>
                "A(z) '{PropertyName}' nem lehet kisebb, mint {From} és nem lehet nagyobb, mint {To}. Ön ezt adta: {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "A(z) '{PropertyName}' nagyobb, mint {From} és kisebb, mint {To} kell, hogy legyen. Ön ezt adta: {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' nem érvényes bankkártyaszám.",
            "ScalePrecisionValidator" =>
                "A(z) '{PropertyName}' összesen nem lehet több {ExpectedPrecision} számjegynél, {ExpectedScale} tizedesjegy pontosság mellett. {Digits} számjegy és {ActualScale} tizedesjegy pontosság lett megadva.",
            "EmptyValidator" => "A(z) '{PropertyName}' üres kell, hogy legyen.",
            "NullValidator" => "A(z) '{PropertyName}' üres kell, hogy legyen.",
            "EnumValidator" =>
                "A(z) '{PropertyName}' csak olyan értékek közül választható, ami nem foglalja magába a(z) '{PropertyValue}' értéket.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" =>
                "A(z) '{PropertyName}' {MinLength} és {MaxLength} karakter között kell, hogy legyen.",
            "MinimumLength_Simple" =>
                "A(z) '{PropertyName}' hossza legalább {MinLength} karakter kell, hogy legyen.",
            "MaximumLength_Simple" => "A(z) '{PropertyName}' hossza legfeljebb {MaxLength} karakter lehet csak.",
            "ExactLength_Simple" => "A(z) '{PropertyName}' pontosan {MaxLength} karakter hosszú lehet csak.",
            "InclusiveBetween_Simple" =>
                "A(z) '{PropertyName}' {From} és {To} között kell, hogy legyen (befoglaló).",
            _ => null
        };
    }
}