namespace DomainHelpers.Blazor.TransitionGroup.Internal; 
class Subscription(Action action) : IDisposable {
    private readonly Action Action = action;
    private bool IsDisposed;

    public void Dispose() {
        if (IsDisposed) {
            throw new ObjectDisposedException(
                nameof(Subscription),
                $"Attempt to call {nameof(Dispose)} twice on {nameof(Subscription)}."
            );
        }

        IsDisposed = true;
        GC.SuppressFinalize(this);
        Action();
    }

    ~Subscription() {
        if (!IsDisposed)
            throw new InvalidOperationException($"{nameof(Subscription)} was not disposed. ");
    }
}