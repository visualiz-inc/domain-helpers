namespace System;

public record ObservableSubscription : IDisposable {
    private Action? Action { get; } = null;

    public bool IsDisposed => this.Action is null;

    public ObservableSubscription(Action action) {
        this.Action = action;
    }

    public void Dispose() {
        this.Action?.Invoke();
    }
}
