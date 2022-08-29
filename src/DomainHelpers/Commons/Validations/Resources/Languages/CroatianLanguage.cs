

#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class CroatianLanguage {
    public const string Culture = "hr";

    public static string? GetTranslation(string key) => key switch {
        "EmailValidator" => "'{PropertyName}' nije ispravna e-mail adresa.",
        "GreaterThanOrEqualValidator" => "'{PropertyName}' mora biti veći ili jednak '{ComparisonValue}'.",
        "GreaterThanValidator" => "'{PropertyName}' mora biti veći od '{ComparisonValue}'.",
        "LengthValidator" => "'{PropertyName}' mora biti između {MinLength} i {MaxLength} znakova. Upisali ste {TotalLength} znakova.",
        "MinimumLengthValidator" => "'{PropertyName}' mora imati duljinu veću ili jednaku {MinLength}. Unijeli ste {TotalLength} znakova.",
        "MaximumLengthValidator" => "'{PropertyName}' mora imati duljinu manju ili jednaku {MaxLength}. Unijeli ste {TotalLength} znakova.",
        "LessThanOrEqualValidator" => "'{PropertyName}' mora biti manji ili jednak '{ComparisonValue}'.",
        "LessThanValidator" => "'{PropertyName}' mora biti manji od '{ComparisonValue}'.",
        "NotEmptyValidator" => "'{PropertyName}' ne smije biti prazan.",
        "NotEqualValidator" => "'{PropertyName}' ne smije biti jednak '{ComparisonValue}'.",
        "NotNullValidator" => "Niste upisali '{PropertyName}'",
        "PredicateValidator" => "'{PropertyName}' nije ispravan.",
        "AsyncPredicateValidator" => "'{PropertyName}' nije ispravan.",
        "RegularExpressionValidator" => "'{PropertyName}' nije u odgovarajućem formatu.",
        "EqualValidator" => "'{PropertyName}' mora biti jednak '{ComparisonValue}'.",
        "ExactLengthValidator" => "'{PropertyName}' mora sadržavati {MaxLength} znakova. Upisali ste {TotalLength} znakova.",
        "InclusiveBetweenValidator" => "'{PropertyName}' mora biti između {From} i {To}. Upisali ste {PropertyValue}.",
        "ExclusiveBetweenValidator" => "'{PropertyName}' mora biti između {From} i {To} (ne uključujući granice). Upisali ste {PropertyValue}.",
        "CreditCardValidator" => "'{PropertyName}' nije odgovarajuća kreditna kartica.",
        "ScalePrecisionValidator" => "'{PropertyName}' ne smije imati više od {ExpectedPrecision} znamenki, sa {ExpectedScale} decimalna mjesta. Upisali ste {Digits} znamenki i {ActualScale} decimalna mjesta.",
        "EmptyValidator" => "'{PropertyName}' mora biti prazan.",
        "NullValidator" => "'{PropertyName}' mora biti prazan.",
        "EnumValidator" => "'{PropertyName}' ima raspon vrijednosti koji ne uključuje '{PropertyValue}'.",
        // Additional fallback messages used by clientside validation integration.
        "Length_Simple" => "'{PropertyName}' mora biti između {MinLength} i {MaxLength} znakova.",
        "MinimumLength_Simple" => "'{PropertyName}' mora imati duljinu veću ili jednaku {MinLength}.",
        "MaximumLength_Simple" => "'{PropertyName}' mora imati duljinu manju ili jednaku {MaxLength}.",
        "ExactLength_Simple" => "'{PropertyName}' mora sadržavati {MaxLength} znakova.",
        "InclusiveBetween_Simple" => "'{PropertyName}' mora biti između {From} i {To}.",
        _ => null,
    };
}
