using System.ComponentModel.DataAnnotations;

namespace DomainHelpers.Core.Commons.Validations;

public record DataValidationExceptionType {
    public DataValidationExceptionType(ImmutableArray<ValidationResult> results) {
        ValidationResults = results;
    }

    public ImmutableArray<ValidationResult> ValidationResults { get; }

    public ImmutableDictionary<string, string?[]> MessageTree =>
        ValidationResults.BuildErrorMessageTree();
}