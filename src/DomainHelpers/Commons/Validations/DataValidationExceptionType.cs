using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DomainHelpers.Core.Commons.Validations;

public record DataValidationExceptionType : ExceptionType {
    public ImmutableArray<ValidationResult> ValidationResults { get; }

    public ImmutableDictionary<string, string?[]> MessageTree =>
        this.ValidationResults.BuildErrorMessageTree();

    public DataValidationExceptionType(ImmutableArray<ValidationResult> results) {
        this.ValidationResults = results;
    }
}