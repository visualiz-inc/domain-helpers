namespace DomainHelpers;

public class Subject<T> : IObservable<T>, IObserver<T> {
    private bool didComplete = false;
    private System.Exception? exception;
    private ImmutableArray<IObserver<T>> observers = ImmutableArray.Create<IObserver<T>>();

    private object locker = new();

    public System.IDisposable Subscribe(IObserver<T> observer) {
        lock (this.locker) {
            if (exception is not null) {
                observer.OnError(exception);
            }
            else if (didComplete) {
                observer.OnCompleted();
            }
            else {
                if (!observers.Contains(observer)) {
                    this.observers = observers.Add(observer);
                }
            }
        }

        return new ObservableSubscription(
            () => {
                lock (this.locker) {
                    if (observers.Contains(observer)) {
                        this.observers = this.observers.Remove(observer);
                    }
                }
            }
        );
    }

    public void OnNext(T value) {
        if (didComplete) {
            return;
        }

        foreach (var observer in observers) {
            observer.OnNext(value);
        }
    }

    public void OnCompleted() {
        if (didComplete) {
            return;
        }

        foreach (var observer in observers) {
            observer.OnCompleted();
        }

        didComplete = true;
    }

    public void OnError(System.Exception error) {
        if (didComplete) {
            return;
        }

        foreach (var observer in observers) {
            observer.OnError(error);
        }

        exception = error;
        didComplete = true;
    }
}
