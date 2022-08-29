

#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class IcelandicLanguage {
    public const string Culture = "is";

    public static string? GetTranslation(string key) => key switch {
        "EmailValidator" => "'{PropertyName}' er ekki gilt netfang.",
        "GreaterThanOrEqualValidator" => "'{PropertyName}' verður að vera meiri en eða jöfn '{ComparisonValue}'.",
        "GreaterThanValidator" => "'{PropertyName}' verður að vera meiri en '{ComparisonValue}'.",
        "LengthValidator" => "'{PropertyName}' verður að vera á milli {MinLength} og {MaxLength} stafir. Þú slóst inn {TotalLength} stafi.",
        "MinimumLengthValidator" => "Lengdin '{PropertyName}' verður að vera að minnsta kosti {MinLength} stafir. Þú slóst inn {TotalLength} stafi.",
        "MaximumLengthValidator" => "Lengd '{PropertyName}' verður að vera {MaxLength} stafir eða færri. Þú slóst inn {TotalLength} stafi.",
        "LessThanOrEqualValidator" => "'{PropertyName}' verður að vera minna en eða jafnt og '{ComparisonValue}'.",
        "LessThanValidator" => "'{PropertyName}' verður að vera minna en '{ComparisonValue}'.",
        "NotEmptyValidator" => "'{PropertyName} má ekki vera tómt.",
        "NotEqualValidator" => "'{PropertyName}' má ekki vera jafnt og '{ComparisonValue}'.",
        "NotNullValidator" => "'{PropertyName} má ekki vera tómt.",
        "PredicateValidator" => "Tilgreindu skilyrði var ekki uppfyllt fyrir '{PropertyName}'.",
        "AsyncPredicateValidator" => "Tilgreindu skilyrði var ekki uppfyllt fyrir '{PropertyName}'.",
        "RegularExpressionValidator" => "'{PropertyName}' er ekki með réttu sniði.",
        "EqualValidator" => "'{PropertyName}' verður að vera jafnt og '{ComparisonValue}'.",
        "ExactLengthValidator" => "'{PropertyName}' verður að vera {MaxLength} stafir að lengd. Þú slóst inn {TotalLength} stafi.",
        "InclusiveBetweenValidator" => "'{PropertyName}' verður að frá {From} til {To}. Þú slóst inn {PropertyValue}.",
        "ExclusiveBetweenValidator" => "'{PropertyName}' verður að vera á milli {From} og {To}. Þú slóst inn {PropertyValue}.",
        "CreditCardValidator" => "'{PropertyName}' er ekki gilt kreditkortanúmer.",
        "ScalePrecisionValidator" => "'{PropertyName}' má ekki vera meira en {ExpectedPrecision} tölustafir samtals, með heimild fyrir {ExpectedScale} aukastöfum. {Digits} tölustafir og {ActualScale} aukastafir fundust.",
        "EmptyValidator" => "'{PropertyName}' verður að vera tómt.",
        "NullValidator" => "'{PropertyName}' verður að vera tómt.",
        "EnumValidator" => "'{PropertyName}' hefur svið gilda sem innihalda ekki '{PropertyValue}'.",
        // Additional fallback messages used by clientside validation integration.
        "Length_Simple" => "'{PropertyName}' verður að vera á milli {MinLength} og {MaxLength} stafir.",
        "MinimumLength_Simple" => "Lengdin '{PropertyName}' verður að vera að minnsta kosti {MinLength} stafir.",
        "MaximumLength_Simple" => "Lengd '{PropertyName}' verður að vera {MaxLength} stafir eða færri.",
        "ExactLength_Simple" => "'{PropertyName}' verður að vera {MaxLength} stafir að lengd.",
        "InclusiveBetween_Simple" => "'{PropertyName}' verður að vera á milli {From} og {To}.",
        _ => null,
    };
}
