namespace DomainHelpers.Blazor.Store.Core;
public record RootStateChangedEventArgs {
    public required IStateChangedEventArgs<object, object> StateChangedEvent { get; init; }
    public required IStore<object, object> Store { get; init; }
    public required RootState RootState { get; init; }
}