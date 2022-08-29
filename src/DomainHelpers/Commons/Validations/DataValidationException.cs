using DomainHelpers.Core.Commons.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainHelpers.Core.Commons;

public class DataValidationException : GeneralException<DataValidationExceptionType> {
    public new DataValidationExceptionType ExceptionType =>
        base.ExceptionType ?? throw new InvalidDataException("ExceptionType was null.");

    public DataValidationException(
        DataValidationExceptionType exceptionType,
        string message,
        string? displayMessage = null,
        Ulid? eventId = null,
        Exception? error = null
    ) : base(exceptionType, message, displayMessage, eventId, error) {
    }
}
