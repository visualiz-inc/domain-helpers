namespace DomainHelpers.Blazor.Store.ReduxDevTools.Internals;
public interface IDevToolInteropHandler {
    Action<string>? MessageHandled { get; set; }

    Action? SyncRequested { get; set; }

    Task InitializeAsync(RootState state);

    Task SendAsync(object? command, HistoryStateContextJson context);

    void HandleMessage(string json);
}