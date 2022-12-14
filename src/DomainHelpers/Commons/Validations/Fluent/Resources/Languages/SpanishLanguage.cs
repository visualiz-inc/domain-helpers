#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class SpanishLanguage {
    public const string Culture = "es";

    public static string? GetTranslation(string key) {
        return key switch {
            "EmailValidator" => "'{PropertyName}' no es una dirección de correo electrónico válida.",
            "GreaterThanOrEqualValidator" => "'{PropertyName}' debe ser mayor o igual que '{ComparisonValue}'.",
            "GreaterThanValidator" => "'{PropertyName}' debe ser mayor que '{ComparisonValue}'.",
            "LengthValidator" =>
                "'{PropertyName}' debe tener entre {MinLength} y {MaxLength} caracter(es). Actualmente tiene {TotalLength} caracter(es).",
            "MinimumLengthValidator" =>
                "'{PropertyName}' debe ser mayor o igual que {MinLength} caracteres. Ingresó {TotalLength} caracter(es).",
            "MaximumLengthValidator" =>
                "'{PropertyName}' debe ser menor o igual que {MaxLength} caracteres. Ingresó {TotalLength} caracter(es).",
            "LessThanOrEqualValidator" => "'{PropertyName}' debe ser menor o igual que '{ComparisonValue}'.",
            "LessThanValidator" => "'{PropertyName}' debe ser menor que '{ComparisonValue}'.",
            "NotEmptyValidator" => "'{PropertyName}' no debería estar vacío.",
            "NotEqualValidator" => "'{PropertyName}' no debería ser igual a '{ComparisonValue}'.",
            "NotNullValidator" => "'{PropertyName}' no debe estar vacío.",
            "PredicateValidator" => "'{PropertyName}' no cumple con la condición especificada.",
            "AsyncPredicateValidator" => "'{PropertyName}' no cumple con la condición especificada.",
            "RegularExpressionValidator" => "'{PropertyName}' no tiene el formato correcto.",
            "EqualValidator" => "'{PropertyName}' debería ser igual a '{ComparisonValue}'.",
            "ExactLengthValidator" =>
                "'{PropertyName}' debe tener un largo de {MaxLength} caracteres. Actualmente tiene {TotalLength} caracter(es).",
            "ExclusiveBetweenValidator" =>
                "'{PropertyName}' debe estar entre {From} y {To} (exclusivo). Actualmente tiene un valor de {PropertyValue}.",
            "InclusiveBetweenValidator" =>
                "'{PropertyName}' debe estar entre {From} y {To}. Actualmente tiene un valor de {PropertyValue}.",
            "CreditCardValidator" => "'{PropertyName}' no es un número de tarjeta de crédito válido.",
            "ScalePrecisionValidator" =>
                "'{PropertyName}' no debe tener más de {ExpectedPrecision} dígitos en total, con margen para {ExpectedScale} decimales. Se encontraron {Digits} y {ActualScale} decimales.",
            "EmptyValidator" => "'{PropertyName}' debe estar vacío.",
            "NullValidator" => "'{PropertyName}' debe estar vacío.",
            "EnumValidator" => "'{PropertyName}' tiene un rango de valores que no incluye '{PropertyValue}'.",
            // Additional fallback messages used by clientside validation integration.
            "Length_Simple" => "'{PropertyName}' debe tener entre {MinLength} y {MaxLength} caracter(es).",
            "MinimumLength_Simple" => "'{PropertyName}' debe ser mayor o igual que {MinLength} caracteres.",
            "MaximumLength_Simple" => "'{PropertyName}' debe ser menor o igual que {MaxLength} caracteres.",
            "ExactLength_Simple" => "'{PropertyName}' debe tener un largo de {MaxLength} caracteres.",
            "InclusiveBetween_Simple" => "'{PropertyName}' debe estar entre {From} y {To}.",
            _ => null
        };
    }
}