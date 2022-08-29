using System.ComponentModel.DataAnnotations;

namespace DomainHelpers.Commons.DataAnnotations;

public class ValidateNestedAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
        if (value is null) {
            return ValidationResult.Success;
        }

        var results = new List<ValidationResult>();
        var context = new ValidationContext(value, null, null);
        Validator.TryValidateObject(value, context, results, true);

        if (results is not []) {
            var fluttern = results.FluttenNesteds();
            var validationResults = fluttern.Select(x => new ValidationResult(
                    x.ErrorMessage,
                    x.MemberNames.Select(y => $"{validationContext.MemberName}.{y}")
                )
            );
            var compositeResults = new CompositeValidationResult("An nested error occured.");
            compositeResults.AddResults(validationResults);

            return compositeResults;
        }

        return ValidationResult.Success!;
    }
}

public class CompositeValidationResult : ValidationResult {
    readonly List<ValidationResult> results = new();

    public IReadOnlyCollection<ValidationResult> Results => this.results.AsReadOnly();

    public CompositeValidationResult(string errorMessage) : base(errorMessage) { }
    public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames) { }
    protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

    public void AddResult(ValidationResult validationResult) {
        this.results.Add(validationResult);
    }

    public void AddResults(IEnumerable<ValidationResult> validationResults) {
        this.results.AddRange(validationResults);
    }
}

public class PostalCodeAttribute : DataTypeAttribute {
    public PostalCodeAttribute() : base(DataType.PostalCode) {

    }
}