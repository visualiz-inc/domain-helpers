namespace System;

/// <summary>
///     Represents the general error.
/// </summary>
public class GeneralException : Exception {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="displayMessage"></param>
    /// <param name="exceptionType"></param>
    /// <param name="eventId"></param>
    /// <param name="exception"></param>
    public GeneralException(
        string message,
        string? displayMessage = null,
        object? exceptionType = null,
        Ulid? eventId = null,
        Exception? exception = null
    ) : base(
        message,
        exception
    ) {
        Payload = exceptionType;
        DisplayMessage = displayMessage;
        EventId = eventId;

        // Additional data
        Data.Add(nameof(EventId), eventId.ToString());
        Data.Add(nameof(DisplayMessage), displayMessage);
        Data.Add(nameof(Payload), exceptionType);
    }

    public string? DisplayMessage { get; }

    public Ulid? EventId { get; }

    public object? Payload { get; }

    public static GeneralException WithDisplayMessage(
        string displayMessage,
        Ulid? eventId = null
    ) {
        return new GeneralException<object>(
            null,
            displayMessage,
            displayMessage,
            eventId ?? Ulid.NewUlid()
        );
    }

    public static GeneralException<TPayload> WithDisplayMessage<TPayload>(
        TPayload exceptionType,
        string displayMessage,
        Ulid? eventId = null
    ) {
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
        return new GeneralException<object>(
            null,
            message,
            displayMessage,
            eventId ?? Ulid.NewUlid(),
            ex!
        );
    }

    public static GeneralException<TPayload> WithMessage<TPayload>(
        TPayload exceptionType,
        string message,
        string? displayMessage,
        Exception? ex = null,
        Ulid? eventId = null
    ) {
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
                ge.Message,
                ge.DisplayMessage
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
        return new GeneralException<object>(
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
    ) {
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
public class GeneralException<TExceptionType> : GeneralException {
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
        => (TExceptionType?)base.Payload;
}