namespace System;

public record ExceptionType;

/// <summary>
///     Represents the general error.
/// </summary>
public abstract class GeneralException : Exception {
    public GeneralException(
        string message,
        string? displayMessage = null,
        object? payload = null,
        Ulid? eventId = null,
        Exception? exception = null
    ) : base(
        message,
        exception) {
        Payload = payload;
        DisplayMessage = displayMessage;
        EventId = eventId;
    }

    public string? DisplayMessage { get; }

    public Ulid? EventId { get; }

    public object? Payload { get; }

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
        string? displayMessage,
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
        string? displayMessage,
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

    public static GeneralException<TPayload> WithChild<TPayload>(
        TPayload payload,
        GeneralException ex,
        string message,
        string? displayMessage = null
    ) {
        return new(
            payload,
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
public class GeneralException<TPayload> : GeneralException{
    public GeneralException(
        TPayload? payload,
        string message,
        string? displayMessage = null,
        Ulid? eventId = null,
        Exception? error = null
        ) : base(
        message,
        displayMessage,
        payload,
        eventId,
        error
            ) { }

    public new TPayload? Payload => (TPayload?)base.Payload;
}