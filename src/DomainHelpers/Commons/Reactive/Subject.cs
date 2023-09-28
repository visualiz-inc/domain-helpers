namespace DomainHelpers.Commons.Reactive;

public class Subject<T> : IObservable<T>, IObserver<T> {
    private bool _didComplete;
    private Exception? _exception;

    private readonly object _locker = new();
    private IObserver<T>[] _observers = [];

    public IDisposable Subscribe(IObserver<T> observer) {
        lock (_locker) {
            if (_exception is not null) {
                observer.OnError(_exception);
            }
            else if (_didComplete) {
                observer.OnCompleted();
            }
            else {
                if (!_observers.Contains(observer)) {
                    _observers = [.. _observers, observer];
                }
            }
        }

        return new ObservableSubscription(
            () => {
                lock (_locker) {
                    if (_observers.Contains(observer)) {
                        _observers = [.. _observers.Where(o => o != observer)];
                    }
                }
            }
        );
    }

    public void OnNext(T value) {
        if (_didComplete) {
            return;
        }

        foreach (var observer in _observers) {
            observer.OnNext(value);
        }
    }

    public void OnCompleted() {
        if (_didComplete) {
            return;
        }

        foreach (var observer in _observers) {
            observer.OnCompleted();
        }

        _didComplete = true;
    }

    public void OnError(Exception error) {
        if (_didComplete) {
            return;
        }

        foreach (var observer in _observers) {
            observer.OnError(error);
        }

        _exception = error;
        _didComplete = true;
    }

    public IDisposable Subscribe(Action<T> action) {
        return Subscribe(new Observer<T>(action, _ => { }, () => { }));
    }
}