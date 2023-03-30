namespace DomainHelpers.Commons.Reactive;

public record ObservableSubscription : IDisposable {
    public ObservableSubscription(Action action) {
        Action = action;
    }

    private Action? Action { get; }

    public bool IsDisposed => Action is null;

    public void Dispose() {
        Action?.Invoke();
    }
}