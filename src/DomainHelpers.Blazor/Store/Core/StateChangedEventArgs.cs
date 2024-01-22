namespace DomainHelpers.Blazor.Store.Core;

public enum StateChangeType {
    StateHasChanged,
    ForceReplaced,
    Restored,
}

public interface IStateChangedEventArgs<out TMessage>
    where TMessage : notnull {
    DateTime Timestamp { get; }

    TMessage? Message { get; }

    object? Sender { get; }
}

record StateChangedEventArgs<TMessage> : IStateChangedEventArgs<TMessage>
    where TMessage : notnull {
    public StateChangeType StateChangeType { get; init; }

    public required TMessage? Message { get; init; }

    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public required object? Sender { get; init; }
}

public interface IStateChangedEventArgs<out TState, out TMessage> : IStateChangedEventArgs<TMessage>
    where TState : class
    where TMessage : notnull {

    StateChangeType StateChangeType { get; }

    TState? LastState { get; }

    TState? State { get; }
}

record StateChangedEventArgs<TState, TMessage> : IStateChangedEventArgs<TState, TMessage>
    where TState : class
    where TMessage : notnull {
    public StateChangeType StateChangeType { get; init; }

    public required TMessage? Message { get; init; }

    public required TState? LastState { get; init; }

    public required TState? State { get; init; }

    public DateTime Timestamp { get; } = DateTime.UtcNow;

    public object? Sender { get; init; }
}