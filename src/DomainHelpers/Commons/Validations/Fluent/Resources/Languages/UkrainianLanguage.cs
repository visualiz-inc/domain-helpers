#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class UkrainianLanguage {
    public const string Culture = "uk";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' не є email-адресою.",
            "GreaterThanOrEqualValidator" =>
                "'{PropertyName}' має бути більшим, або дорівнювати '{ComparisonValue}'.",
            "GreaterThanValidator" => "'{PropertyName}' має бути більшим за '{ComparisonValue}'.",
            "LengthValidator" =>
                "'{PropertyName}' має бути довжиною від {MinLength} до {MaxLength} символів. Ви ввели {TotalLength} символів.",
            "MinimumLengthValidator" =>
                "Довжина '{PropertyName}' має бути не меншою ніж {MinLength} символів. Ви ввели {TotalLength} символів.",
            "MaximumLengthValidator" =>
                "Довжина '{PropertyName}' має бути {MaxLength} символів, або менше. Ви ввели {TotalLength} символів.",
            "LessThanOrEqualValidator" => "'{PropertyName}' має бути меншим, або дорівнювати '{ComparisonValue}'.",
            "LessThanValidator" => "'{PropertyName}' має бути меншим за '{ComparisonValue}'.",
            "NotEmptyValidator" => "'{PropertyName}' не може бути порожнім.",
            "NotEqualValidator" => "'{PropertyName}' не може дорівнювати '{ComparisonValue}'.",
            "NotNullValidator" => "'{PropertyName}' не може бути порожнім.",
            "PredicateValidator" => "Вказана умова не є задовільною для '{PropertyName}'.",
            "AsyncPredicateValidator" => "Вказана умова не є задовільною для '{PropertyName}'.",
            "RegularExpressionValidator" => "'{PropertyName}' має неправильний формат.",
            "EqualValidator" => "'{PropertyName}' має дорівнювати '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "'{PropertyName}' має бути довжиною {MaxLength} символів. Ви ввели {TotalLength} символів.",
            "InclusiveBetweenValidator" =>
                "'{PropertyName}' має бути між {From} та {To} (включно). Ви ввели {PropertyValue}.",
            "ExclusiveBetweenValidator" =>
                "'{PropertyName}' має бути між {From} та {To}. Ви ввели {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' не є номером кредитної картки.",
            "ScalePrecisionValidator" =>
                "'{PropertyName}' не може мати більше за {ExpectedPrecision} цифр всього, з {ExpectedScale} десятковими знаками. {Digits} цифр та {ActualScale} десяткових знаків знайдено.",
            "EmptyValidator" => "'{PropertyName}' має бути порожнім.",
            "NullValidator" => "'{PropertyName}' має бути порожнім.",
            "EnumValidator" => "'{PropertyName}' має діапазон значень, який не включає '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" => "'{PropertyName}' має бути довжиною від {MinLength} до {MaxLength} символів.",
            "MinimumLength_Simple" => "Довжина '{PropertyName}' має бути не меншою ніж {MinLength} символів.",
            "MaximumLength_Simple" => "Довжина '{PropertyName}' має бути {MaxLength} символів, або менше.",
            "ExactLength_Simple" => "'{PropertyName}' має бути довжиною {MaxLength} символів.",
            "InclusiveBetween_Simple" => "'{PropertyName}' має бути між {From} та {To} (включно).",
            _ => null
        };
    }
}