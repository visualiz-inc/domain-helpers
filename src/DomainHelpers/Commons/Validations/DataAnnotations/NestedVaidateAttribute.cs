using System.ComponentModel.DataAnnotations;

namespace DomainHelpers.Commons.DataAnnotations; 

public class ValidateNestedAttribute : ValidationAttribute {
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext) {
        if (value is null) {
            return ValidationResult.Success;
        }

        List<ValidationResult> results = new List<ValidationResult>();
        ValidationContext context = new ValidationContext(value, null, null);
        Validator.TryValidateObject(value, context, results, true);

        if (results is not []) {
            ImmutableArray<ValidationResult> fluttern = results.FluttenNesteds();
            IEnumerable<ValidationResult> validationResults = fluttern.Select(x => new ValidationResult(
                    x.ErrorMessage,
                    x.MemberNames.Select(y => $"{validationContext.MemberName}.{y}")
                )
            );
            CompositeValidationResult compositeResults = new CompositeValidationResult("An nested error occured.");
            compositeResults.AddResults(validationResults);

            return compositeResults;
        }

        return ValidationResult.Success!;
    }
}

public class CompositeValidationResult : ValidationResult {
    private readonly List<ValidationResult> results = new();

    public CompositeValidationResult(string errorMessage) : base(errorMessage) { }

    public CompositeValidationResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage,
        memberNames) { }

    protected CompositeValidationResult(ValidationResult validationResult) : base(validationResult) { }

    public IReadOnlyCollection<ValidationResult> Results => results.AsReadOnly();

    public void AddResult(ValidationResult validationResult) {
        results.Add(validationResult);
    }

    public void AddResults(IEnumerable<ValidationResult> validationResults) {
        results.AddRange(validationResults);
    }
}

public class PostalCodeAttribute : DataTypeAttribute {
    public PostalCodeAttribute() : base(DataType.PostalCode) { }
}