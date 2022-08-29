

#pragma warning disable 618

namespace DomainHelpers.Core.Validations.Resources;

internal class GreekLanguage {
    public const string Culture = "el";

    public static string? GetTranslation(string key) => key switch {
        "EmailValidator" => "Το πεδίο '{PropertyName}' δεν περιέχει μια έγκυρη διεύθυνση email.",
        "GreaterThanOrEqualValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μεγαλύτερη ή ίση με '{ComparisonValue}'.",
        "GreaterThanValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μεγαλύτερη από '{ComparisonValue}'.",
        "LengthValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει μήκος μεταξύ {MinLength} και {MaxLength} χαρακτήρες. Έχετε καταχωρίσει {TotalLength} χαρακτήρες.",
        "MinimumLengthValidator" => "Το μήκος του πεδίου '{PropertyName}' πρέπει να είναι τουλάχιστον {MinLength} χαρακτήρες. Έχετε καταχωρίσει {TotalLength} χαρακτήρες.",
        "MaximumLengthValidator" => "Το μήκος του πεδίου '{PropertyName}' πρέπει να είναι το πολύ {MaxLength} χαρακτήρες. Έχετε καταχωρίσει {TotalLength} χαρακτήρες.",
        "LessThanOrEqualValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μικρότερη ή ίση με '{ComparisonValue}'.",
        "LessThanValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μικρότερη από '{ComparisonValue}'.",
        "NotEmptyValidator" => "Το πεδίο '{PropertyName}' δεν πρέπει να είναι κενό.",
        "NotEqualValidator" => "Το πεδίο '{PropertyName}' δεν πρέπει να έχει τιμή ίση με '{ComparisonValue}'.",
        "NotNullValidator" => "Το πεδίο '{PropertyName}' δεν πρέπει να είναι κενό.",
        "PredicateValidator" => "Η ορισμένη συνθήκη δεν ικανοποιήθηκε για το πεδίο '{PropertyName}'.",
        "AsyncPredicateValidator" => "Η ορισμένη συνθήκη δεν ικανοποιήθηκε για το πεδίο '{PropertyName}'.",
        "RegularExpressionValidator" => "Η τιμή του πεδίου '{PropertyName}' δεν έχει αποδεκτή μορφή.",
        "EqualValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή ίση με '{ComparisonValue}'.",
        "ExactLengthValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει μήκος ίσο με {MaxLength} χαρακτήρες. Έχετε καταχωρίσει {TotalLength} χαρακτήρες.",
        "InclusiveBetweenValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μεταξύ {From} και {To}. Καταχωρίσατε την τιμή {PropertyValue}.",
        "ExclusiveBetweenValidator" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μεγαλύτερη από {From} και μικρότερη από {To}. Καταχωρίσατε την τιμή  {PropertyValue}.",
        "CreditCardValidator" => "Το πεδίο '{PropertyName}' δεν περιέχει αποδεκτό αριθμό πιστωτικής κάρτας.",
        "ScalePrecisionValidator" => "'Το πεδίο '{PropertyName}' δεν μπορεί να έχει περισσότερα από {ExpectedPrecision} ψηφία στο σύνολο, με μέγιστο επιτρεπόμενο αριθμό δεκαδικών τα {ExpectedScale} ψηφία. Έχετε καταχωρίσει {Digits} ψηφία συνολικά με {ActualScale} δεκαδικά.",
        "EmptyValidator" => "Το πεδίο '{PropertyName}' πρέπει να είναι κενό.",
        "NullValidator" => "Το πεδίο '{PropertyName}' πρέπει να είναι κενό.",
        "EnumValidator" => "Το πεδίο '{PropertyName}' επιτρέπει συγκεκριμένο εύρος τιμών που δεν περιλαμβάνουν την τιμή '{PropertyValue}' που καταχωρίσατε.",
        // Additional fallback messages used by clientside validation integration.
        "Length_Simple" => "Το πεδίο '{PropertyName}' πρέπει να έχει μήκος μεταξύ {MinLength} και {MaxLength} χαρακτήρες.",
        "MinimumLength_Simple" => "Το μήκος του πεδίου '{PropertyName}' πρέπει να είναι τουλάχιστον {MinLength} χαρακτήρες.",
        "MaximumLength_Simple" => "Το μήκος του πεδίου '{PropertyName}' πρέπει να είναι το πολύ {MaxLength} χαρακτήρες.",
        "ExactLength_Simple" => "Το πεδίο '{PropertyName}' πρέπει να έχει μήκος ίσο με {MaxLength} χαρακτήρες.",
        "InclusiveBetween_Simple" => "Το πεδίο '{PropertyName}' πρέπει να έχει τιμή μεταξύ {From} και {To}.",
        _ => null,
    };
}
