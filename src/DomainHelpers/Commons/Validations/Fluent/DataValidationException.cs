using DomainHelpers.Core.Commons.Validations;

namespace DomainHelpers.Core.Commons; 

public class DataValidationException : GeneralException<DataValidationExceptionType> {
    public DataValidationException(
        DataValidationExceptionType? exceptionType,
        string message,
        string? displayMessage = null,
        Ulid? eventId = null,
        Exception? error = null
    ) : base(exceptionType, message, displayMessage, eventId, error) { }

    public new DataValidationExceptionType ExceptionType =>
        base.Payload ?? throw new InvalidDataException("ExceptionType was null.");
}