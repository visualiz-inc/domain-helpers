namespace System; 

public record ExceptionType;

/// <summary>
///     Represents the general error.
/// </summary>
public abstract class GeneralException : Exception {
    public GeneralException(
        string message,
        string? displayMessage = null,
        ExceptionType? exceptionType = null,
        Ulid? eventId = null,
        Exception? exception = null
    ) : base(
        message,
        exception) {
        ExceptionType = exceptionType;
        DisplayMessage = displayMessage;
        EventId = eventId;
    }

    public string? DisplayMessage { get; }

    public Ulid? EventId { get; }

    public ExceptionType? ExceptionType { get; }

    public static GeneralException WithDisplayMessage(
        string displayMessage,
        Ulid? eventId = null
    ) {
        return new GeneralException<ExceptionType>(
            null,
            displayMessage,
            displayMessage,
            eventId ?? Ulid.NewUlid()
        );
    }

    public static GeneralException<TExceptionType> WithDisplayMessage<TExceptionType>(
        TExceptionType exceptionType,
        string displayMessage,
        Ulid? eventId = null
    ) where TExceptionType : ExceptionType {
        return new(
            exceptionType,
            displayMessage,
            displayMessage,
            eventId ?? Ulid.NewUlid()
        );
    }

    public static GeneralException WithMessage(
        string message,
        string? displayMessage = null,
        Exception? ex = null,
        Ulid? eventId = null
    ) {
        return new GeneralException<ExceptionType>(
            null,
            message,
            displayMessage,
            eventId ?? Ulid.NewUlid(),
            ex!
        );
    }

    public static GeneralException<TExceptionType> WithMessage<TExceptionType>(
        TExceptionType exceptionType,
        string message,
        string? displayMessage = null,
        Exception? ex = null,
        Ulid? eventId = null
    ) where TExceptionType : ExceptionType {
        return new(
            exceptionType,
            message,
            displayMessage,
            eventId ?? Ulid.NewUlid(),
            ex!
        );
    }

    public static GeneralException WithException(
        Exception ex,
        string? message = null,
        string? displayMessage = null
    ) {
        return ex switch {
            GeneralException ge => WithChild(
                ge,
                ge.Message
            ),
            _ => WithMessage(
                message ?? ex.Message,
                displayMessage,
                ex
            )
        };
    }

    public static GeneralException WithChild(
        GeneralException ex,
        string message,
        string? displayMessage = null
    ) {
        return new GeneralException<ExceptionType>(
            null,
            message,
            displayMessage,
            ex.EventId,
            ex
        );
    }

    public static GeneralException<TExceptionType> WithChild<TExceptionType>(
        TExceptionType type,
        GeneralException ex,
        string message,
        string? displayMessage = null
    ) where TExceptionType : ExceptionType {
        return new(
            type,
            message,
            displayMessage,
            ex.EventId,
            ex
        );
    }
}

/// <summary>
///     Represents the general error.
/// </summary>
/// <typeparam name="TError">Error info.</typeparam>
public class GeneralException<TExceptionType> : GeneralException
    where TExceptionType : ExceptionType {
    public GeneralException(
        TExceptionType? exceptionType,
        string message,
        string? displayMessage = null,
        Ulid? eventId = null,
        Exception? error = null) : base(
        message,
        displayMessage,
        exceptionType,
        eventId,
        error) { }

    public new TExceptionType? ExceptionType
        => (TExceptionType?)base.ExceptionType;
}