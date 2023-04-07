namespace DomainHelpers.Commons.Reactive; 
public class Subject<T> : IObservable<T>, IObserver<T> {
    private bool didComplete;
    private Exception? exception;

    private readonly object locker = new();
    private ImmutableArray<IObserver<T>> observers = ImmutableArray.Create<IObserver<T>>();

    public IDisposable Subscribe(IObserver<T> observer) {
        lock (locker) {
            if (exception is not null) {
                observer.OnError(exception);
            }
            else if (didComplete) {
                observer.OnCompleted();
            }
            else {
                if (!observers.Contains(observer)) {
                    observers = observers.Add(observer);
                }
            }
        }

        return new ObservableSubscription(
            () => {
                lock (locker) {
                    if (observers.Contains(observer)) {
                        observers = observers.Remove(observer);
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

    public void OnError(Exception error) {
        if (didComplete) {
            return;
        }

        foreach (var observer in observers) {
            observer.OnError(error);
        }

        exception = error;
        didComplete = true;
    }

    public IDisposable Subscribe(Action<T> aciton) {
        return Subscribe(new Observer<T>(aciton, _ => { }, () => { }));
    }
}