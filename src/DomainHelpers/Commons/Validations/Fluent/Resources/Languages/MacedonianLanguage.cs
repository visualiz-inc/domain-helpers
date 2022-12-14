#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class MacedonianLanguage {
    public const string Culture = "mk";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' не е валидна емаил адреса.",
            "GreaterThanOrEqualValidator" =>
                "Вредноста на '{PropertyName}' мора да биде поголема или еднаква на '{ComparisonValue}'.",
            "GreaterThanValidator" => "Вредноста на '{PropertyName}' мора да биде поголема од '{ComparisonValue}'.",
            "LengthValidator" =>
                "Должината на '{PropertyName}' мора да биде помеѓу {MinLength} и {MaxLength} карактери. Имате внесено вкупно {TotalLength} карактери.",
            "MinimumLengthValidator" =>
                "Должината на '{PropertyName}' мора да биде поголема или еднаква на {MinLength} знаци. Внесовте {TotalLength} знаци.",
            "MaximumLengthValidator" =>
                "Должината на '{PropertyName}' мора да биде помала или еднаква на {MaxLength} знаци. Внесовте {TotalLength} знаци.",
            "LessThanOrEqualValidator" =>
                "Вредноста на '{PropertyName}' мора да биде помала или еднаква на '{ComparisonValue}'.",
            "LessThanValidator" => "Вредноста на '{PropertyName}' мора да биде помала од '{ComparisonValue}'.",
            "NotEmptyValidator" => "Вредноста на '{PropertyName}' не треба да биде празна.",
            "NotEqualValidator" =>
                "Вредноста на '{PropertyName}' би требало да биде еднаква на '{ComparisonValue}'.",
            "NotNullValidator" => "Вредноста на '{PropertyName}' не треба да биде празна.",
            "PredicateValidator" => "Специфичната состојба не беше најдена за  '{PropertyName}'.",
            "AsyncPredicateValidator" => "Специфичната состојба не беше најдена за  '{PropertyName}'.",
            "RegularExpressionValidator" => "'{PropertyName}' не е во правилниот формат.",
            "EqualValidator" => "Вредноста на '{PropertyName}' би требало да биде еднаква на '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "Должината на '{PropertyName}' мора да биде {MaxLength} карактери. Имате внесено вкупно {TotalLength} карактери.",
            "InclusiveBetweenValidator" =>
                "Вредноста на '{PropertyName}' мора да биде помеѓу {From} и {To}. Имате внесено {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "Вредноста на '{PropertyName}' мора да биде од {From} до {To} (исклучително). Имате внесено вредност {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' не е валиден бројот на кредитната картичка.",
            "ScalePrecisionValidator" =>
                "'{PropertyName}' не би требало да биде повеќе од  {ExpectedPrecision} цифри вкупно, со дозволени  {ExpectedScale} децимали. {Digits} цифри и {ActualScale} децимали беа најдени.",
            "EmptyValidator" => "'{PropertyName}' треба да биде празна.",
            "NullValidator" => "'{PropertyName}' треба да биде празна.",
            "EnumValidator" => "'{PropertyName}' има низа вредности кои не вклучуваат '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" =>
                "Должината на '{PropertyName}' мора да биде помеѓу {MinLength} и {MaxLength} карактери.",
            "MinimumLength_Simple" =>
                "Должината на '{PropertyName}' мора да биде поголема или еднаква на {MinLength} знаци.",
            "MaximumLength_Simple" =>
                "Должината на '{PropertyName}' мора да биде помала или еднаква на {MaxLength} знаци.",
            "ExactLength_Simple" => "Должината на '{PropertyName}' мора да биде {MaxLength} карактери.",
            "InclusiveBetween_Simple" => "Вредноста на '{PropertyName}' мора да биде помеѓу {From} и {To}.",
            _ => null
        };
    }
}