#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class SlovenianLanguage {
    public const string Culture = "sl";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' ni veljaven e-poštni naslov.",
            "GreaterThanOrEqualValidator" => "'{PropertyName}' mora biti večji ali enak '{ComparisonValue}'.",
            "GreaterThanValidator" => "'{PropertyName}' mora biti večji od '{ComparisonValue}'.",
            "LengthValidator" =>
                "'{PropertyName}' mora imeti dolžino med {MinLength} in {MaxLength} znakov. Vnesli ste {TotalLength} znakov.",
            "MinimumLengthValidator" =>
                "'{PropertyName}' mora imeti dolžino večjo ali enako {MinLength}. Vnesli ste {TotalLength} znakov.",
            "MaximumLengthValidator" =>
                "'{PropertyName}' mora imeti dolžino manjšo ali enako {MaxLength}. Vnesli ste {TotalLength} znakov.",
            "LessThanOrEqualValidator" => "'{PropertyName}' mora biti manjši ali enak '{ComparisonValue}'.",
            "LessThanValidator" => "'{PropertyName}' mora biti manjši od '{ComparisonValue}'.",
            "NotEmptyValidator" => "'{PropertyName}' ne sme biti prazen.",
            "NotEqualValidator" => "'{PropertyName}' ne sme biti enak '{ComparisonValue}'.",
            "NotNullValidator" => "'{PropertyName}' ne sme biti prazen.",
            "PredicateValidator" => "Določen pogoj ni bil izpolnjen za '{PropertyName}'.",
            "AsyncPredicateValidator" => "Določen pogoj ni bil izpolnjen za '{PropertyName}'.",
            "RegularExpressionValidator" => "'{PropertyName}' ni v pravilni obliki.",
            "EqualValidator" => "'{PropertyName}' mora biti enak '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "'{PropertyName}' mora imeti {MaxLength} znakov. Vnesli ste {TotalLength} znakov.",
            "InclusiveBetweenValidator" =>
                "'{PropertyName}' mora biti med {From} in {To}. Vnesli ste {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "'{PropertyName}' mora biti med {From} in {To} (izključno). Vnesli ste {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' ni veljavna številka kreditne kartice.",
            "ScalePrecisionValidator" =>
                "'{PropertyName}' ne sme biti več kot {ExpectedPrecision} natančno števk in {ExpectedScale} decimalk. Vnesli ste {Digits} števk in {ActualScale} decimalk.",
            "EmptyValidator" => "'{PropertyName}' mora biti prazen.",
            "NullValidator" => "'{PropertyName}' mora biti prazen.",
            "EnumValidator" => "'{PropertyName}' ima obseg vrednosti, ki ne vključuje '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" => "'{PropertyName}' imeti dolžino med {MinLength} in {MaxLength} znakov. ",
            "MinimumLength_Simple" => "'{PropertyName}' mora imeti dolžino večjo ali enako {MinLength}.",
            "MaximumLength_Simple" => "'{PropertyName}' mora imeti dolžino manjšo ali enako {MaxLength}.",
            "ExactLength_Simple" => "'{PropertyName}' mora imeti {MaxLength} znakov.",
            "InclusiveBetween_Simple" => "'{PropertyName}' mora biti med {From} in {To}.",
            _ => null
        };
    }
}