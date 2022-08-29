

#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class PortugueseBrazilLanguage {
    public const string Culture = "pt-BR";

    public static string? GetTranslation(string key) => key switch {
        "EmailValidator" => "'{PropertyName}' é um endereço de email inválido.",
        "GreaterThanOrEqualValidator" => "'{PropertyName}' deve ser superior ou igual a '{ComparisonValue}'.",
        "GreaterThanValidator" => "'{PropertyName}' deve ser superior a '{ComparisonValue}'.",
        "LengthValidator" => "'{PropertyName}' deve ter entre {MinLength} e {MaxLength} caracteres. Você digitou {TotalLength} caracteres.",
        "MinimumLengthValidator" => "'{PropertyName}' deve ser maior ou igual a {MinLength} caracteres. Você digitou {TotalLength} caracteres.",
        "MaximumLengthValidator" => "'{PropertyName}' deve ser menor ou igual a {MaxLength} caracteres. Você digitou {TotalLength} caracteres.",
        "LessThanOrEqualValidator" => "'{PropertyName}' deve ser inferior ou igual a '{ComparisonValue}'.",
        "LessThanValidator" => "'{PropertyName}' deve ser inferior a '{ComparisonValue}'.",
        "NotEmptyValidator" => "'{PropertyName}' deve ser informado.",
        "NotEqualValidator" => "'{PropertyName}' deve ser diferente de '{ComparisonValue}'.",
        "NotNullValidator" => "'{PropertyName}' não pode ser nulo.",
        "PredicateValidator" => "'{PropertyName}' não atende a condição definida.",
        "AsyncPredicateValidator" => "'{PropertyName}' não atende a condição definida.",
        "RegularExpressionValidator" => "'{PropertyName}' não está no formato correto.",
        "EqualValidator" => "'{PropertyName}' deve ser igual a '{ComparisonValue}'.",
        "ExactLengthValidator" => "'{PropertyName}' deve ter no máximo {MaxLength} caracteres. Você digitou {TotalLength} caracteres.",
        "ExclusiveBetweenValidator" => "'{PropertyName}' deve, exclusivamente, estar entre {From} e {To}. Você digitou {PropertyValue}.",
        "InclusiveBetweenValidator" => "'{PropertyName}' deve estar entre {From} e {To}. Você digitou {PropertyValue}.",
        "CreditCardValidator" => "'{PropertyName}' não é um número válido de cartão de crédito.",
        "ScalePrecisionValidator" => "'{PropertyName}' não pode ter mais do que {ExpectedPrecision} dígitos no total, com {ExpectedScale} dígitos decimais. {Digits} dígitos e {ActualScale} decimais foram informados.",
        "EmptyValidator" => "'{PropertyName}' deve estar vazio.",
        "NullValidator" => "'{PropertyName}' deve estar null.",
        "EnumValidator" => "'{PropertyName}' possui um intervalo de valores que não inclui '{PropertyValue}'.",
        // Additional fallback messages used by clientside validation integration.
        "Length_Simple" => "'{PropertyName}' deve ter entre {MinLength} e {MaxLength} caracteres.",
        "MinimumLength_Simple" => "'{PropertyName}' deve ser maior ou igual a {MinLength} caracteres.",
        "MaximumLength_Simple" => "'{PropertyName}' deve ser menor ou igual a {MaxLength} caracteres.",
        "ExactLength_Simple" => "'{PropertyName}' deve ter no máximo {MaxLength} caracteres.",
        "InclusiveBetween_Simple" => "'{PropertyName}' deve estar entre {From} e {To}.",
        _ => null,
    };
}
